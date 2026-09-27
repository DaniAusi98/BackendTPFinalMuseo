# ?? Eliminación de GuidedTourRule - COMPLETADO

## Estado: ? ELIMINADO

La clase `GuidedTourRule` fue eliminada porque era **completamente redundante** con el flujo del Producer.

## Flujo Correcto del Sistema

### Arquitectura de Validación en Capas

```
????????????????????????????????????????????????????????????????
? CAPA 1: Servicio de Dominio (Visitas Guiadas)               ?
? ServicioDisponibilidadTurnosVisitasGuiadas                  ?
?                                                              ?
? Responsabilidad: Validar disponibilidad INTERNA de guiadas  ?
? ? Valida: Calendario del museo                             ?
? ? Valida: Configuración de visitas guiadas                 ?
? ? Valida: Disponibilidad de guías                          ?
? ? Calcula: Capacidad según visitas existentes              ?
? ?? Retorna: List<TurnoDisponible> con estados               ?
????????????????????????????????????????????????????????????????
                          ?
????????????????????????????????????????????????????????????????
? CAPA 2: Producer (Traductor)                                ?
? GuidedAvailabilityProducerService                           ?
?                                                              ?
? Responsabilidad: Traducir turnos ? candidatos para engine   ?
? ?? Traduce: TurnoDisponible ? VisitaGrupalGuiada (ficticia) ?
? ?? Filtra: Solo turnos con estado Disponible                ?
? ?? Prepara: AvailabilityContext con datos preconsultados    ?
? ?? Invoca: AvailabilityEngine.CheckManyAsync()              ?
????????????????????????????????????????????????????????????????
                          ?
????????????????????????????????????????????????????????????????
? CAPA 3: Motor de Reglas (Conflictos Entre Actividades)      ?
? AvailabilityEngine + Rules                                  ?
?                                                              ?
? Responsabilidad: Validar convivencia EXTERNA                ?
? ? NoConcurrentGuidedWithAutoguidedRule                     ?
?    ? Evita solapamiento guiadas vs autoguiadas             ?
? ? NoGroupIfHallEventOrEducationalRule                      ?
?    ? Evita visitas grupales durante eventos en hall        ?
? ? CalendarAndRoomBlockRule                                 ?
?    ? Valida bloqueos de sala (NO calendario para guiadas)  ?
? ? NoGroupIfExhibitInMountingOrDisassemblyRule              ?
?    ? Evita visitas durante montaje de muestras             ?
?                                                              ?
? ? NO valida calendario para guiadas (ya validado)          ?
????????????????????????????????????????????????????????????????
```

## ¿Por Qué Se Eliminó GuidedTourRule?

### Problema Original
```csharp
// ? GuidedTourRule (ELIMINADO)
public class GuidedTourRule : IAvailabilityRule
{
    public async Task<AvailabilityResult> CheckAsync(AvailabilityContext ctx, ActividadMuseo candidate)
    {
        // 1. Volvía a llamar al ServicioDisponibilidadTurnosVisitasGuiadas
        var turnos = await _servicioDisponibilidad.CalcularDisponibilidad(...);

        // 2. Validaba si existe al menos 1 turno disponible
        var anyAvailable = turnos.Any(t => t.EstadoTurno == Disponible);

        // 3. Retornaba Ok/Fail
        // ? PROBLEMA: El candidato YA ES un turno disponible!
    }
}
```

### ¿Por qué era circular?
1. **Producer** llamaba a `CalcularDisponibilidad()` ? obtenía turnos disponibles
2. **Producer** creaba candidatos desde esos turnos
3. **Engine** ejecutaba `GuidedTourRule`
4. **GuidedTourRule** volvía a llamar a `CalcularDisponibilidad()` ? ?? CIRCULAR
5. **GuidedTourRule** validaba "¿existe un turno disponible?" cuando **el candidato YA ES un turno disponible**

### Impacto de Eliminación
- ? **Eliminadas 3 consultas redundantes** por cada turno
- ? **Eliminada lógica circular** innecesaria
- ? **Mejor performance** en validación batch
- ? **Código más claro** y mantenible

## Validaciones Actuales

### Responsabilidad de Cada Componente

| Componente | Valida Calendario | Valida Conflictos | Valida Guías | Valida Capacidad |
|------------|-------------------|-------------------|--------------|------------------|
| **ServicioDisponibilidadTurnosVisitasGuiadas** | ? Sí | ? No (solo guiadas) | ? Sí | ? Sí |
| **GuidedAvailabilityProducerService** | ? No (ya validado) | ? No (delega al engine) | ? No (ya validado) | ? No (ya validado) |
| **AvailabilityEngine** | ? No para guiadas* | ? Sí (entre tipos) | ? No | ? No |

\* `CalendarAndRoomBlockRule` **excluye** validación de calendario para `VisitaGrupalGuiada` porque ya fue validado en el servicio de dominio.

## Mejoras Implementadas

### 1. Producer Optimizado
```csharp
// ? CORRECTO: Solo filtra por estado
var candidatosFiltrados = candidatosTurnos
    .Where(turno => turno.EstadoTurno == EstadoTurno.Disponible 
                 && turno.CapacidadDisponible > 0);
// No re-valida calendario (ya validado por el servicio)
```

### 2. CalendarAndRoomBlockRule Excluye Guiadas
```csharp
// ? CORRECTO: No valida calendario para guiadas
if (candidate is not VisitaGrupalGuiada)
{
    // Solo valida calendario para otras actividades
    if (ctx.DiasCierre != null && ...)
        return Fail("Museo cerrado");
}
```

### 3. Contexto Limpio
```csharp
// ? CORRECTO: No incluye DiasCierre (ya validado)
var baseCtx = new AvailabilityContext
{
    ExistingActivities = actividadesEnRango,
    DiasCierre = null, // Ya validado por servicio de dominio
    // Solo datos para validar conflictos externos
};
```

## Validaciones Eliminadas vs Mantenidas

### ? Eliminadas (Redundantes)
- `GuidedTourRule` - Volvía a calcular disponibilidad
- Filtro por `diasCierre` en producer - Ya validado en servicio
- Validación de calendario en engine para guiadas

### ? Mantenidas (Necesarias)
- `ServicioDisponibilidadTurnosVisitasGuiadas` - Validación INTERNA
- `NoConcurrentGuidedWithAutoguidedRule` - Validación EXTERNA
- `NoGroupIfHallEventOrEducationalRule` - Validación EXTERNA
- `CalendarAndRoomBlockRule` (para otras actividades) - Validación EXTERNA
- `NoGroupIfExhibitInMountingOrDisassemblyRule` - Validación EXTERNA

## Conclusión

El sistema ahora tiene una **separación clara de responsabilidades**:

1. **Dominio** ? Valida reglas de negocio específicas de cada tipo de actividad
2. **Producer** ? Traduce entre contextos (dominio ? application)
3. **Engine** ? Valida conflictos entre diferentes tipos de actividades

Esta arquitectura es:
- ? **Más eficiente** (sin consultas redundantes)
- ? **Más clara** (responsabilidades bien definidas)
- ? **Más mantenible** (cada capa tiene un propósito único)
- ? **Escalable** (fácil agregar nuevos tipos de actividad con su propio servicio + producer)

---

**Fecha de eliminación**: 2024  
**Estado**: ? Completado  
**Archivos eliminados**: 
- `Application\Availability\Rules\GuidedTourRule.cs`
- `Application\Availability\Providers\GuidedRuleProvider.cs`

