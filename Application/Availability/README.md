Availability engine - diseño y uso

Resumen
-------
Este módulo implementa un motor de reglas para validar la disponibilidad de actividades en el museo.
**Actualizado** con logging completo, optimización de consultas y mejores prácticas de observabilidad.

Conceptos clave
---------------
- AvailabilityEngine: orquestador que ejecuta reglas aplicables a una candidata (ahora con logging).
- IRuleProvider: decide si aplica a una candidata y crea las reglas necesarias (puede inyectar repos/servicios).
- IAvailabilityRule: unidad de validación que recibe un `AvailabilityContext` y la `ActividadMuseo` candidata y devuelve `AvailabilityResult`.
- AvailabilityContext: datos preparados por la capa de aplicación (rango fecha, `ExistingActivities` solapadas, metadata opcional).

Flujo de ejecución
------------------
1. El handler/servicio de aplicación crea la instancia candidata (`ActividadMuseo`, p. ej. `VisitaGrupalGuiada`) y calcula el rango `start/end` desde sus `TimeSlots`.
2. La capa de aplicación preconsulta repositorios para traer solo las `ExistingActivities` que se solapan en tiempo/sala y arma un `AvailabilityContext`.
3. Llama `await availabilityEngine.CheckAsync(ctx, candidate)`.
4. `AvailabilityEngine` pregunta a la `IRuleFactory` (implementada por `CompositeRuleFactory`) las reglas aplicables: la fábrica pregunta a todos los `IRuleProvider` registrados; cada provider ejecuta `CanHandle(candidate)` y, si devuelve true, `CreateRules(candidate)`.
5. El motor ejecuta las reglas devueltas en orden; corta al primer `Fail` y devuelve el resultado al handler.
6. **NUEVO**: El motor ahora registra logs detallados de cada regla ejecutada, fallos y éxitos.

Registro en DI
--------------
La extensión de registro está en `Application/Registrations/ApplicationServicesRegistration.cs`.
Se registró mediante:

```csharp
// Motor y fábrica
services.AddScoped<AvailabilityEngine>();
services.AddScoped<IRuleFactory, CompositeRuleFactory>();

// Providers (auto-scan por namespace)
services.Scan(scan => scan
    .FromAssemblyOf<AvailabilityEngine>()
    .AddClasses(classes => classes.InNamespaces("Application.Availability.Providers"))
    .AsImplementedInterfaces()
    .WithScopedLifetime());

// Rules (auto-scan por namespace) - NUEVO
services.Scan(scan => scan
    .FromAssemblyOf<AvailabilityEngine>()
    .AddClasses(classes => classes.InNamespaces("Application.Availability.Rules"))
    .AsImplementedInterfaces()
    .WithScopedLifetime());
```

Notas de implementación
-----------------------
- **NUEVO**: Las reglas ahora inyectan `ILogger<T>` para observabilidad y debugging.
- **OPTIMIZADO**: Las reglas deben priorizar usar datos preconsultados del `AvailabilityContext.Metadata` antes de hacer consultas directas.
- **ADVERTENCIA**: Cuando una regla no encuentra datos en el contexto, registra un warning y hace consulta directa (evitar en producción).
- Las reglas deben inyectar solo los repositorios/servicios que necesitan y realizar sus propias consultas cuando corresponda.
- La capa de aplicación debe prefetchear datos pesados que sean requeridos por múltiples reglas y colocarlos en `AvailabilityContext.Metadata` para evitar consultas duplicadas.
- Si necesitás controlar orden/prioridad, modificar `IRuleProvider` para exponer `Priority` y que `CompositeRuleFactory` ordene las reglas antes de devolverlas.

Logging y Observabilidad (NUEVO)
---------------------------------
El motor ahora registra:

**Nivel Information**:
- Inicio de validación con tipo de actividad y cantidad de reglas
- Éxito de validación completa
- Fallos de reglas específicas con motivo

**Nivel Debug**:
- Ejecución de cada regla individual
- Uso de datos preconsultados vs consultas directas
- Detalles de validaciones específicas (solapamientos, bloqueos)

**Nivel Warning**:
- Consultas directas a repositorios (debería usarse contexto)
- Datos no preconsultados en contexto

**Ejemplo de logs**:
```
[INF] Checking availability for VisitaGrupalGuiada (Id: 123) with 4 rules
[DBG] Executing rule: CalendarAndRoomBlockRule
[DBG] No calendar or room blocks found for candidate 123
[DBG] Executing rule: NoConcurrentGuidedWithAutoguidedRule
[WRN] Rule GuidedTourRule failed for VisitaGrupalGuiada (Id: 123): No hay turno disponible
```

Optimización de Consultas (NUEVO)
----------------------------------
Las reglas ahora siguen este patrón para evitar consultas redundantes:

```csharp
// ? BIEN: Usar datos preconsultados del contexto
if (ctx.Metadata?.TryGetValue(AvailabilityMetadataKeys.GuidesPrefetched, out var guiasObj) 
    && guiasObj is IEnumerable<Guia> guias)
{
    // Usar guias del contexto
    _logger.LogDebug("Using prefetched guides from context");
}
else
{
    // ? Fallback: consultar repositorio con warning
    guias = await _repositorioGuia.ObtenerGuiasConDisponibilidadAsync();
    _logger.LogWarning("Guides not prefetched, fetching from repository");
}
```

Ejemplo de handler (actualizado)
---------------------------------
```csharp
public async Task Handle(CreateVisitaGuiadaCommand cmd)
{
    var candidate = new VisitaGrupalGuiada(/* datos */);
    var start = candidate.TimeSlots.Min(ts => ts.Inicio);
    var end = candidate.TimeSlots.Max(ts => ts.Fin);

    // Prefetch actividades solapadas
    var existentes = await _repoActividadMuseo.FindAllAsync(start, end);
    var diasCierre = await _repositorioDiasCierre.FindAllAsync();
    var bloqueos = await _repositorioBloqueos.FindAllAsync(start, end);

    var ctx = new AvailabilityContext
    {
        Start = start,
        End = end,
        ExistingActivities = existentes,
        DiasCierre = diasCierre,
        BloqueosSala = bloqueos,
        Metadata = new Dictionary<string, object>
        {
            { AvailabilityMetadataKeys.DiasCierre, diasCierre },
            { AvailabilityMetadataKeys.BloqueosSala, bloqueos }
            // Agregar más datos si las reglas los necesitan
        }
    };

    var result = await _availabilityEngine.CheckAsync(ctx, candidate);

    if (!result.IsOk)
    {
        _logger.LogWarning("Availability check failed: {Message}", result.Message);
        throw new BusinessException(result.Message);
    }

    // persistir actividad...
}
```

Dónde extender
---------------
- Agregar nuevas reglas en `Application/Availability/Rules` y proveedores en `Application/Availability/Providers`.
- Las reglas se auto-registran por namespace scan (no es necesario registro manual).
- **IMPORTANTE**: Todas las reglas deben inyectar `ILogger<T>` para mantener observabilidad.
- Proveedores adicionales se registran automáticamente en el namespace `Application.Availability.Providers`.

Reglas Implementadas
--------------------
1. **CalendarAndRoomBlockRule**: Valida bloqueos de sala. NO valida calendario para `VisitaGrupalGuiada` (ya validado en dominio)
2. **NoGroupIfHallEventOrEducationalRule**: Bloquea visitas grupales en hall durante eventos
3. **NoConcurrentGuidedWithAutoguidedRule**: Exclusión mutua guiadas/autoguiadas
4. **NoConcurrentGuidedIfGroupRule**: Validación de solapamientos grupales
5. **NoGroupIfExhibitInMountingOrDisassemblyRule**: Restricción durante montaje de muestras

? **GuidedTourRule Eliminado**
-------------------------------
Esta regla fue eliminada porque era redundante. Ver `Rules/GUIDED_RULE_DEPRECATION.md` para detalles.

**Flujo correcto para visitas guiadas**:
```
ServicioDisponibilidadTurnosVisitasGuiadas (Dominio)
  ? Valida: Calendario, Config, Guías, Capacidad
  ? Retorna: List<TurnoDisponible>
GuidedAvailabilityProducerService (Application)
  ? Filtra: Solo turnos disponibles
  ? Traduce: TurnoDisponible ? Candidato
  ? Invoca: AvailabilityEngine
AvailabilityEngine
  ? Valida: Conflictos con OTRAS actividades
  ? Retorna: Turnos sin conflictos externos
```

Mejores Prácticas
-----------------
1. **Siempre preconsultar datos compartidos** en el handler/producer antes de llamar al engine
2. **Usar `AvailabilityContext.Metadata`** para pasar datos reutilizables entre reglas
3. **Inyectar `ILogger<T>`** en todas las reglas para debugging
4. **Registrar warnings** cuando se hacen consultas directas en reglas
5. **Preferir datos del contexto** sobre consultas a repositorios en reglas
6. **Documentar dependencias de metadata** en comentarios XML de cada regla

Troubleshooting
---------------
**Problema**: Reglas hacen muchas consultas a BD
- Verificar que el handler preconsulte todos los datos necesarios
- Revisar logs de nivel Warning para detectar consultas directas
- Agregar datos faltantes a `AvailabilityContext.Metadata`

**Problema**: No se ejecutan todas las reglas esperadas
- Verificar que el `IRuleProvider.CanHandle()` retorna `true` para el tipo de candidato
- Revisar logs de nivel Information para ver qué reglas se ejecutan
- Verificar que las reglas estén en el namespace correcto para auto-registro

**Problema**: Performance lento con muchos candidatos
- Usar `CheckManyAsync()` en lugar de múltiples `CheckAsync()`
- Verificar que se estén preconsultando datos en el contexto base
- Revisar logs para detectar N+1 queries

Contacto
-------
Archivo actualizado con mejoras de logging y optimización de consultas.
Ver commits para historial de cambios.

