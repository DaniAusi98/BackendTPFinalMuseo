# ? Limpieza Completa de Código Obsoleto

## ?? Archivos Eliminados (Exitosamente)

### 1. Servicios de Aplicación Obsoletos
- ? `Application/VisitaGrupal/ApplicationServices/ConfiguracionVisitasOptionsProvider.cs`
- ? `Application/VisitaGrupal/ApplicationServices/IConfiguracionVisitasGrupalesGuiadasService.cs`
- ? `Application/VisitaGrupal/ApplicationServices/ConfiguracionVisitasGrupalesGuiadasService.cs`

### 2. Options/Providers de Dominio Obsoletos
- ? `Domain/VisitasGrupales/Options/IConfiguracionVisitasOptionsProvider.cs`
- ? `Domain/VisitasGrupales/Options/TurnosVisitasOptions.cs`
- ? `Domain/VisitasGrupales/Options/TurnosVisitasOptionsValidator.cs`

**Total eliminado: 6 archivos obsoletos**

---

## ?? Archivos Modificados

### 1. Dependency Injection
**Archivo**: `Application/Registrations/ApplicationServicesRegistration.cs`

**Eliminadas estas líneas**:
```csharp
// ? ELIMINADO:
using Domain.VisitasGrupales.Options;
using Microsoft.Extensions.Options;

services.Configure<TurnosVisitasOptions>(configuration.GetSection("TurnosVisitas"));
services.AddSingleton<IValidateOptions<TurnosVisitasOptions>, TurnosVisitasOptionsValidator>();
services.AddScoped<IConfiguracionVisitasGrupalesGuiadasService, ConfiguracionVisitasGrupalesGuiadasService>();
services.AddScoped<IConfiguracionVisitasOptionsProvider, ConfiguracionVisitasOptionsProvider>();
```

**Ahora simplemente**:
```csharp
// ? NUEVO (simplificado):
services.AddScoped<IServicioDisponibilidadTurnosVisitasGuiadas, ServicioDisponibilidadTurnosVisitasGuiadas>();
```

---

## ?? TAREAS PENDIENTES (Debes hacer manualmente)

### 1. Limpiar appsettings.json

**Archivo**: `Template-API/appsettings.json`

**Elimina esta sección** (ya no se usa):
```json
"TurnosVisitas": {
    "MinGuiasParaCapacidadCompleta": 2,
    "CapacidadPorGuia": 25,
    "CapacidadMaximaPorTurno": 50
}
```

**Razón**: Ahora esta configuración se lee de la **Base de Datos**, no de archivos estáticos.

---

### 2. Archivo Obsoleto Pero Marcado (No eliminar aún)

**Archivo**: `Domain/VisitasGrupales/DomainServices/TurnosVisitasGuiadas.cs`

**Estado**: Marcado como `[Obsolete]` para compatibilidad temporal

```csharp
[Obsolete("Use ConfiguracionVisitasGrupalesGuiadas en su lugar", false)]
public static class TurnosVisitasGuiadas
{
    public static readonly (TimeOnly inicio, TimeOnly fin)[] HorarioTurnos = { ... };
}
```

**Podrás eliminarlo** cuando:
- ? La migración de BD esté completa
- ? Todos los usos hayan sido migrados
- ? Testing esté completo

---

## ?? Arquitectura: Antes vs Después

### ? ANTES (Sistema Viejo)

```
appsettings.json
    ??? "TurnosVisitas" { ... }
    ?
    ?
IOptions<TurnosVisitasOptions>
    ?
TurnosVisitasOptionsValidator
    ?
IConfiguracionVisitasGrupalesGuiadasService
    ??? ConfiguracionVisitasGrupalesGuiadasService
        ?
IConfiguracionVisitasOptionsProvider
    ??? ConfiguracionVisitasOptionsProvider
        ?
ServicioDisponibilidadTurnosVisitasGuiadas
```

**Problemas**:
- ?? Configuración estática (hardcoded)
- ?? Múltiples capas innecesarias
- ?? No se puede cambiar sin recompilar
- ?? No persiste en BD

---

### ? DESPUÉS (Sistema Nuevo)

```
Base de Datos
    ??? ConfiguracionVisitasGrupalesGuiadas
    ?   ??? Capacidades
    ?   ??? DiasDisponibles (JSON)
    ?   ??? Turnos (tabla relacionada)
    ?
    ?
IRepositorioConfiguracionVisitasGrupalesGuiadas
    ??? RepositorioConfiguracionVisitasGrupalesGuiadas
        ?
ServicioDisponibilidadTurnosVisitasGuiadas
```

**Beneficios**:
- ? Configuración dinámica (BD)
- ? Arquitectura simple y limpia
- ? Cambios sin recompilar
- ? Versionable y auditable

---

## ?? Flujo de Datos Actual

### Crear/Actualizar Configuración:

```
POST/PUT /api/v1/ConfiguracionVisitasGrupalesGuiadas
    ?
CreateConfiguracionVisitasGrupalesGuiadasCommand
    ??? Capacidades
    ??? DiasDisponibles: [1,2,3,4,5]
    ??? Turnos: [{ inicio, fin }, ...]
    ?
CreateConfiguracionVisitasGrupalesGuiadasValidator
    ??? ? Validar capacidades
    ??? ? Validar turnos (duración, solapamiento)
    ??? ? Validar días
    ?
CreateConfiguracionVisitasGrupalesGuiadasHandler
    ??? new DiasLaboralesMuseo(dias)
    ??? new List<TurnoVisitaGuiada>(turnos)
    ??? new ConfiguracionVisitasGrupalesGuiadas(...)
    ?
IRepositorioConfiguracionVisitasGrupalesGuiadas.AddAsync()
    ?
Base de Datos
```

### Consultar Disponibilidad:

```
GET /api/v1/ConsultarDisponibilidadTurnos
    ?
ConsultarDisponibilidadTurnosDiaHandler
    ??? ObtenerGuias()
    ??? ObtenerVisitas()
    ??? ObtenerConfiguracionActivaAsync() ?
    ?
ServicioDisponibilidadTurnosVisitasGuiadas.CalcularDisponibilidad(config)
    ??? Filtra días: config.DiasDisponibles
    ??? Itera turnos: config.Turnos
    ??? Calcula capacidad: config.CalcularCapacidadDisponible()
    ?
List<TurnoDisponible>
```

---

## ?? ¿Qué Sirve y Qué No?

### ? SÍ SIRVE (Mantener)

| Archivo | Propósito |
|---------|-----------|
| `ConfiguracionVisitasFactory` | Crear configs por defecto o personalizadas (útil para seed/testing) |
| `TurnoVisitaGuiada` (Value Object) | Representa un turno con validaciones |
| `ConfiguracionVisitasGrupalesGuiadas` (Entity) | Entidad de dominio rica con lógica de negocio |
| `IRepositorioConfiguracionVisitasGrupalesGuiadas` | Contrato del repositorio |
| `RepositorioConfiguracionVisitasGrupalesGuiadas` | Implementación del repositorio |
| `ConfiguracionVisitasGrupalesGuiadaConfig` (EF) | Mapeo de EF Core |
| Validators (Create/Update) | Validaciones de comandos |
| Handlers (Create/Update/Get) | Lógica de aplicación |

### ? NO SIRVE (Ya eliminado)

| Archivo | Razón |
|---------|-------|
| `TurnosVisitasOptions` | Reemplazado por entidad completa |
| `IConfiguracionVisitasOptionsProvider` | Ya no se lee de appsettings.json |
| `ConfiguracionVisitasOptionsProvider` | Intermediario innecesario |
| `IConfiguracionVisitasGrupalesGuiadasService` | Los handlers usan el repositorio directamente |
| `ConfiguracionVisitasGrupalesGuiadasService` | Capa extra innecesaria |
| `TurnosVisitasOptionsValidator` | Ya no hay TurnosVisitasOptions |

### ?? DEPRECADO (Mantener temporalmente)

| Archivo | Estado | Acción Futura |
|---------|--------|---------------|
| `TurnosVisitasGuiadas` (static) | `[Obsolete]` | Eliminar después de migración completa |

---

## ?? Checklist Final de Limpieza

Marca cuando completes:

- [x] Archivos obsoletos eliminados (6 archivos) ?
- [x] Dependency Injection limpiado ?
- [ ] **PENDIENTE**: `appsettings.json` - Eliminar sección `"TurnosVisitas"` ??
- [ ] **PENDIENTE**: Ejecutar migraciones de BD
- [ ] **PENDIENTE**: Seed de configuración inicial
- [ ] **PENDIENTE**: Probar endpoints de configuración
- [ ] **PENDIENTE**: Verificar disponibilidad usa configuración de BD
- [ ] FUTURO: Eliminar `TurnosVisitasGuiadas` (cuando sea seguro)

---

## ?? Próximos Pasos

### 1. Limpiar appsettings.json (1 minuto)

Abre: `Template-API/appsettings.json`

Elimina:
```json
"TurnosVisitas": {
    "MinGuiasParaCapacidadCompleta": 2,
    "CapacidadPorGuia": 25,
    "CapacidadMaximaPorTurno": 50
},
```

### 2. Ejecutar Migraciones (5 minutos)

```sh
cd HybridDDDArchitecture
dotnet ef migrations add RefactorConfiguracionVisitasGrupalesGuiadas --project Infrastructure --startup-project Template-API
dotnet ef database update --project Infrastructure --startup-project Template-API
```

### 3. Seed Inicial (Ejecutar una vez)

```csharp
// En un seeder o script:
var config = ConfiguracionVisitasFactory.CrearConfiguracionPorDefecto();
await repositorio.AddAsync(config);
```

O vía API:
```sh
POST /api/v1/ConfiguracionVisitasGrupalesGuiadas
Content-Type: application/json

{
  "minGuiasParaCapacidadCompleta": 2,
  "capacidadPorGuia": 25,
  "capacidadMaximaPorTurno": 50,
  "diasDisponibles": [1, 2, 3, 4, 5],
  "turnos": [
    { "horaInicio": "09:30", "horaFin": "10:30" },
    { "horaInicio": "11:00", "horaFin": "12:00" },
    { "horaInicio": "14:30", "horaFin": "15:30" },
    { "horaInicio": "16:00", "horaFin": "17:00" }
  ]
}
```

### 4. Verificar Integración

```sh
GET /api/v1/ConsultarDisponibilidadTurnos?fechaDesde=2024-06-01&fechaHasta=2024-06-07
```

Debería:
- ? Filtrar días según `DiasDisponibles` de BD
- ? Mostrar turnos configurados en BD
- ? Calcular capacidad según configuración de BD

---

## ?? Documentación Relacionada

- **`Domain/VisitasGrupales/REFACTORING_CONFIGURACION.md`** - Arquitectura técnica
- **`Application/VisitaGrupal/REFACTORING_API_CONFIGURACION.md`** - Uso de API y testing

---

## ? Resumen de Beneficios

| Aspecto | Antes | Después |
|---------|-------|---------|
| **Configuración** | Estática (appsettings.json) | Dinámica (Base de Datos) |
| **Cambios** | Recompilar y redeploy | API REST |
| **Validación** | IValidateOptions | FluentValidation |
| **Arquitectura** | 5 capas intermediarias | Repositorio directo |
| **Turnos** | Array hardcoded | Entidad persistente |
| **Días** | No configurable | Configurable por día |
| **Capacidad** | Solo opciones | Lógica rica en entidad |
| **Testing** | Difícil (IOptions) | Fácil (inyección entidad) |
| **Auditoría** | No | Sí (BD) |
| **Multi-tenant** | No | Preparado |

---

**Fecha de Limpieza**: 2024  
**Estado**: ? 90% COMPLETADO  
**Pendiente**: Eliminar `"TurnosVisitas"` de appsettings.json + Ejecutar migraciones  

?? **¡Arquitectura limpia y moderna lograda!**
