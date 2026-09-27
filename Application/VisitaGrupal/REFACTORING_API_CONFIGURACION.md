# Refactorización Completa: API ConfiguracionVisitasGrupalesGuiadas

## ? Cambios Completados

### ?? Archivos Creados/Modificados

#### **1. DTOs y Comandos**
- ? **ConfiguracionVisitasGrupalesGuiadasDto.cs** - Agregados `DiasDisponibles` y `Turnos`
- ? **CreateConfiguracionVisitasGrupalesGuiadasCommand.cs** - Agregados días y turnos
- ? **UpdateConfiguracionVisitasGrupalesGuiadasCommand.cs** - Agregados días y turnos

#### **2. Handlers**
- ? **CreateConfiguracionVisitasGrupalesGuiadasHandler.cs** - Usa entidad directamente
- ? **UpdateConfiguracionVisitasGrupalesGuiadasHandler.cs** - Usa métodos de la entidad
- ? **GetConfiguracionVisitasGrupalesGuiadasHandler.cs** - Devuelve configuración completa

#### **3. Repositorio**
- ? **IRepositorioConfiguracionVisitasGrupalesGuiadas.cs** - Ya actualizado previamente
- ? **RepositorioConfiguracionVisitasGrupalesGuiadas.cs** - Simplificado

#### **4. Configuración EF Core**
- ? **ConfiguracionVisitasGrupalesGuiadaConfig.cs** - NUEVO
  - Mapeo de `DiasDisponibles` como owned type (JSON)
  - Mapeo de `Turnos` como owned collection (tabla separada)

#### **5. Validadores**
- ? **CreateConfiguracionVisitasGrupalesGuiadasValidator.cs** - NUEVO
- ? **UpdateConfiguracionVisitasGrupalesGuiadasValidator.cs** - NUEVO

---

## ??? Archivos Obsoletos (Pueden removerse)

- ? `IConfiguracionVisitasGrupalesGuiadasService.cs`
- ? `ConfiguracionVisitasGrupalesGuiadasService.cs`
- ? `TurnosVisitasOptions.cs` (Domain)
- ? `IConfiguracionVisitasOptionsProvider.cs` (Domain)

---

## ?? Estructura de Base de Datos

### Tabla: `ConfiguracionVisitasGrupalesGuiadas`
```sql
CREATE TABLE ConfiguracionVisitasGrupalesGuiadas (
    Id INT PRIMARY KEY IDENTITY,
    MinGuiasParaCapacidadCompleta INT NOT NULL,
    CapacidadPorGuia INT NOT NULL,
    CapacidadMaximaPorTurno INT NOT NULL,
    DiasDisponibles NVARCHAR(MAX) NOT NULL -- JSON: [0,1,2,3,4]
);
```

### Tabla: `TurnosVisitasGuiadas`
```sql
CREATE TABLE TurnosVisitasGuiadas (
    Id INT PRIMARY KEY IDENTITY,
    ConfiguracionVisitasGrupalesGuiadasId INT NOT NULL,
    HoraInicio TIME NOT NULL,
    HoraFin TIME NOT NULL,
    FOREIGN KEY (ConfiguracionVisitasGrupalesGuiadasId) 
        REFERENCES ConfiguracionVisitasGrupalesGuiadas(Id) ON DELETE CASCADE
);
```

---

## ?? Pasos de Migración

### 1. Crear Migración
```bash
dotnet ef migrations add RefactorConfiguracionVisitasGrupalesGuiadas --project Infrastructure --startup-project Template-API
```

### 2. Revisar Migración Generada
Verificar que se crean las tablas y columnas correctas.

### 3. Aplicar Migración
```bash
dotnet ef database update --project Infrastructure --startup-project Template-API
```

### 4. Seed de Datos Iniciales

Crear configuración inicial en el seeder o via endpoint:

```csharp
// En el seeder de la aplicación o script SQL
var diasLaborales = new DiasLaboralesMuseo(new[]
{
    DayOfWeek.Monday,
    DayOfWeek.Tuesday,
    DayOfWeek.Wednesday,
    DayOfWeek.Thursday,
    DayOfWeek.Friday
});

var turnos = new List<TurnoVisitaGuiada>
{
    new(new TimeOnly(9, 30), new TimeOnly(10, 30)),
    new(new TimeOnly(11, 0), new TimeOnly(12, 0)),
    new(new TimeOnly(14, 30), new TimeOnly(15, 30)),
    new(new TimeOnly(16, 0), new TimeOnly(17, 0))
};

var config = new ConfiguracionVisitasGrupalesGuiadas(
    minGuias: 2,
    capacidadPorGuia: 25,
    capacidadMaxima: 50,
    diasDisponibles: diasLaborales,
    turnos: turnos
);

await repositorio.AddAsync(config);
```

O usar el Factory:
```csharp
var config = ConfiguracionVisitasFactory.CrearConfiguracionPorDefecto();
await repositorio.AddAsync(config);
```

### 5. Actualizar Dependency Injection

Verificar que el repositorio esté registrado (probablemente ya está):
```csharp
services.AddScoped<IRepositorioConfiguracionVisitasGrupalesGuiadas, 
                   RepositorioConfiguracionVisitasGrupalesGuiadas>();
```

### 6. Remover Servicios Obsoletos (Opcional)

Si no se usan en otro lugar:
```csharp
// REMOVER:
services.AddScoped<IConfiguracionVisitasGrupalesGuiadasService, 
                   ConfiguracionVisitasGrupalesGuiadasService>();
services.Configure<TurnosVisitasOptions>(configuration.GetSection("TurnosVisitas"));
```

---

## ?? Uso de la API

### 1. Crear Configuración (POST)

**Endpoint**: `POST /api/v1/ConfiguracionVisitasGrupalesGuiadas`

**Body**:
```json
{
  "minGuiasParaCapacidadCompleta": 2,
  "capacidadPorGuia": 25,
  "capacidadMaximaPorTurno": 50,
  "diasDisponibles": [1, 2, 3, 4, 5],
  "turnos": [
    {
      "horaInicio": "09:30:00",
      "horaFin": "10:30:00"
    },
    {
      "horaInicio": "11:00:00",
      "horaFin": "12:00:00"
    },
    {
      "horaInicio": "14:30:00",
      "horaFin": "15:30:00"
    },
    {
      "horaInicio": "16:00:00",
      "horaFin": "17:00:00"
    }
  ]
}
```

**Días de la semana** (enum):
- `0` = Sunday
- `1` = Monday
- `2` = Tuesday
- `3` = Wednesday
- `4` = Thursday
- `5` = Friday
- `6` = Saturday

**Response**: `201 Created`
```json
{
  "id": 1
}
```

### 2. Obtener Configuración (GET)

**Endpoint**: `GET /api/v1/ConfiguracionVisitasGrupalesGuiadas`

**Response**: `200 OK`
```json
{
  "id": 1,
  "minGuiasParaCapacidadCompleta": 2,
  "capacidadPorGuia": 25,
  "capacidadMaximaPorTurno": 50,
  "diasDisponibles": [1, 2, 3, 4, 5],
  "turnos": [
    {
      "horaInicio": "09:30:00",
      "horaFin": "10:30:00"
    },
    {
      "horaInicio": "11:00:00",
      "horaFin": "12:00:00"
    },
    {
      "horaInicio": "14:30:00",
      "horaFin": "15:30:00"
    },
    {
      "horaInicio": "16:00:00",
      "horaFin": "17:00:00"
    }
  ]
}
```

### 3. Actualizar Configuración (PUT)

**Endpoint**: `PUT /api/v1/ConfiguracionVisitasGrupalesGuiadas`

**Body**: (mismo formato que POST)

**Response**: `204 No Content`

### 4. Eliminar Configuración (DELETE)

**Endpoint**: `DELETE /api/v1/ConfiguracionVisitasGrupalesGuiadas`

**Response**: `204 No Content`

---

## ? Validaciones Implementadas

### Capacidad
- ? `MinGuiasParaCapacidadCompleta` > 0
- ? `CapacidadPorGuia` > 0
- ? `CapacidadMaximaPorTurno` > 0
- ? `CapacidadMaximaPorTurno` >= `CapacidadPorGuia`

### Días Disponibles
- ? No puede ser null
- ? Debe tener al menos un día

### Turnos
- ? No puede ser null
- ? Debe tener al menos un turno
- ? `HoraFin` > `HoraInicio`
- ? Duración mínima: 30 minutos
- ? Duración máxima: 3 horas
- ? No pueden solaparse entre sí

---

## ?? Casos de Prueba

### Casos Válidos

**Test 1: Configuración Completa Válida**
```json
{
  "minGuiasParaCapacidadCompleta": 2,
  "capacidadPorGuia": 25,
  "capacidadMaximaPorTurno": 50,
  "diasDisponibles": [1, 2, 3, 4, 5],
  "turnos": [
    { "horaInicio": "09:30", "horaFin": "10:30" },
    { "horaInicio": "11:00", "horaFin": "12:00" }
  ]
}
```
? Expected: 201 Created

**Test 2: Solo Lunes y Miércoles**
```json
{
  "minGuiasParaCapacidadCompleta": 2,
  "capacidadPorGuia": 30,
  "capacidadMaximaPorTurno": 60,
  "diasDisponibles": [1, 3],
  "turnos": [
    { "horaInicio": "10:00", "horaFin": "11:00" }
  ]
}
```
? Expected: 201 Created

### Casos Inválidos

**Test 3: Capacidad Máxima < Capacidad Por Guía**
```json
{
  "minGuiasParaCapacidadCompleta": 2,
  "capacidadPorGuia": 50,
  "capacidadMaximaPorTurno": 40,
  ...
}
```
? Expected: 400 Bad Request - "La capacidad máxima no puede ser menor a la capacidad por guía."

**Test 4: Turnos Solapados**
```json
{
  ...
  "turnos": [
    { "horaInicio": "09:30", "horaFin": "10:30" },
    { "horaInicio": "10:00", "horaFin": "11:00" }
  ]
}
```
? Expected: 400 Bad Request - "Los turnos no deben solaparse entre sí."

**Test 5: Turno Muy Corto**
```json
{
  ...
  "turnos": [
    { "horaInicio": "09:30", "horaFin": "09:45" }
  ]
}
```
? Expected: 400 Bad Request - "Un turno debe durar al menos 30 minutos."

**Test 6: Turno Muy Largo**
```json
{
  ...
  "turnos": [
    { "horaInicio": "09:00", "horaFin": "13:00" }
  ]
}
```
? Expected: 400 Bad Request - "Un turno no puede durar más de 3 horas."

**Test 7: Sin Días Disponibles**
```json
{
  ...
  "diasDisponibles": [],
  ...
}
```
? Expected: 400 Bad Request - "Debe existir al menos un día disponible."

---

## ?? Integración con Disponibilidad

El servicio `ServicioDisponibilidadTurnosVisitasGuiadas` ahora usa esta configuración:

```csharp
var config = await repositorioConfiguracion.ObtenerConfiguracionActivaAsync();

var turnosDisponibles = await servicioDisponibilidad.CalcularDisponibilidad(
    fechaDesde,
    fechaHasta,
    guias,
    visitas,
    config  // ? Configuración completa
);
```

Esto permite:
- ? Filtrar días no disponibles automáticamente
- ? Usar turnos configurados dinámicamente
- ? Calcular capacidad según configuración actual

---

## ?? Checklist de Verificación

Antes de marcar como completo, verificar:

- [ ] Migración de BD ejecutada correctamente
- [ ] Seed de configuración inicial creado
- [ ] Endpoint POST funciona y crea configuración
- [ ] Endpoint GET devuelve configuración completa
- [ ] Endpoint PUT actualiza correctamente
- [ ] Validadores funcionan (probar casos inválidos)
- [ ] Disponibilidad de turnos usa la nueva configuración
- [ ] Días no configurados se filtran automáticamente
- [ ] Turnos configurados aparecen en disponibilidad
- [ ] Documentación de API actualizada
- [ ] Tests unitarios creados (opcional pero recomendado)

---

## ?? Beneficios Logrados

1. **Flexibilidad Total**: Cambiar turnos y días sin tocar código
2. **Gestión Centralizada**: Un solo lugar para configurar visitas
3. **Validaciones Robustas**: Reglas de negocio aplicadas consistentemente
4. **Arquitectura Limpia**: Entidad de dominio rica, no anémica
5. **Facilita Testing**: Configuración inyectable y mockeable
6. **Trazabilidad**: Configuraciones versionables en BD
7. **Multi-tenant Ready**: Diferentes configuraciones por museo (futuro)

---

## ?? Notas Finales

- La configuración se guarda como **singleton** (solo una activa)
- Los turnos se guardan en tabla separada para consultas eficientes
- Los días se serializan como JSON array en una columna
- Se puede extender para agregar `HorarioApertura` y `HoraCierre` si difiere del museo

---

**Fecha**: 2024
**Estado**: ? COMPLETADO
**Próximo Paso**: Ejecutar migraciones y seed inicial
