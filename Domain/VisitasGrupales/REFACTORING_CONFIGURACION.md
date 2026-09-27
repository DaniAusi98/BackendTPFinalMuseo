# Refactorización: Configuración de Visitas Grupales Guiadas

## ?? Resumen de Cambios

Se ha refactorizado la configuración de visitas grupales guiadas para hacerla **dinámica y persistente en base de datos**, eliminando dependencias de valores hardcodeados.

---

## ?? Objetivos Logrados

### ? 1. Configuración Unificada
- **Antes**: Configuración dispersa entre `TurnosVisitasOptions`, `TurnosVisitasGuiadas` (estática)
- **Ahora**: Todo centralizado en `ConfiguracionVisitasGrupalesGuiadas` (entidad persistente)

### ? 2. Gestión Dinámica de Turnos
- **Antes**: Turnos hardcodeados en `TurnosVisitasGuiadas.HorarioTurnos`
- **Ahora**: Turnos configurables mediante `TurnoVisitaGuiada` (Value Object)

### ? 3. Días Disponibles Configurables
- **Nuevo**: Agregado `DiasLaboralesMuseo` para especificar qué días se ofrecen visitas guiadas

---

## ?? Archivos Nuevos

### 1. `Domain/VisitasGrupales/ValueObjects/TurnoVisitaGuiada.cs`
Value Object que representa un turno de visita guiada.

**Características**:
- Validación de duración (30 min - 3 horas)
- Métodos para detectar solapamientos
- Validación de rango con horarios del museo

```csharp
var turno = new TurnoVisitaGuiada(
    new TimeOnly(9, 30), 
    new TimeOnly(10, 30)
);
```

### 2. `Domain/VisitasGrupales/Factories/ConfiguracionVisitasFactory.cs`
Factory para crear configuraciones predeterminadas o personalizadas.

**Métodos**:
- `CrearConfiguracionPorDefecto()`: Crea config con valores actuales
- `CrearConfiguracionPersonalizada()`: Permite personalización completa

```csharp
var config = ConfiguracionVisitasFactory.CrearConfiguracionPorDefecto();
```

---

## ?? Archivos Modificados

### 1. `Domain/VisitasGrupales/Entities/ConfiguracionVisitasGrupalesGuiadas.cs`

#### Propiedades Agregadas:
```csharp
public DiasLaboralesMuseo DiasDisponibles { get; private set; }
public IReadOnlyCollection<TurnoVisitaGuiada> Turnos { get; }
```

#### Métodos Nuevos:
- `ActualizarDiasDisponibles()`: Cambiar días disponibles
- `AgregarTurno()`: Agregar nuevo turno con validación de solapamiento
- `RemoverTurno()`: Eliminar turno existente
- `ActualizarTurnos()`: Reemplazar todos los turnos
- `EsDiaDisponible()`: Verificar si un día está disponible
- `CalcularCapacidadDisponible()`: Calcular capacidad según guías

### 2. `Application/VisitaGrupal/Repositories/IRepositorioConfiguracionVisitasGrupalesGuiadas.cs`

Ahora extiende `IRepository<T>` y agrega:
```csharp
Task<ConfiguracionVisitasGrupalesGuiadas?> ObtenerConfiguracionActivaAsync();
```

### 3. `Domain/VisitasGrupales/DomainServices/ServicioDisponibilidadTurnosVisitasGuiadas.cs`

#### Cambios:
- ? Removida dependencia de `IConfiguracionVisitasOptionsProvider`
- ? Ahora recibe `ConfiguracionVisitasGrupalesGuiadas` como parámetro
- ? Itera sobre `configuracion.Turnos` en lugar de array estático
- ? Verifica `configuracion.EsDiaDisponible()` para filtrar días

### 4. `Application/VisitaGrupal/UseCases/.../ConsultarDisponibilidadTurnosDiaHandler.cs`

#### Cambios:
- ? Agregado `IRepositorioConfiguracionVisitasGrupalesGuiadas`
- ? Obtiene configuración de BD antes de calcular disponibilidad
- ? Pasa configuración al servicio de disponibilidad

### 5. `Domain/VisitasGrupales/DomainServices/TurnosVisitasGuiadas.cs`

?? **DEPRECADO**: Marcado con `[Obsolete]` para migración gradual.

```csharp
[Obsolete("Use ConfiguracionVisitasGrupalesGuiadas en su lugar")]
public static class TurnosVisitasGuiadas { ... }
```

---

## ?? Ejemplo de Uso

### Crear Configuración

```csharp
// Configuración por defecto
var config = ConfiguracionVisitasFactory.CrearConfiguracionPorDefecto();

// O personalizada
var diasLaborales = new DiasLaboralesMuseo(new[]
{
    DayOfWeek.Monday,
    DayOfWeek.Tuesday,
    DayOfWeek.Thursday,
    DayOfWeek.Friday
});

var turnos = new List<TurnoVisitaGuiada>
{
    new(new TimeOnly(10, 0), new TimeOnly(11, 0)),
    new(new TimeOnly(15, 0), new TimeOnly(16, 0))
};

var config = new ConfiguracionVisitasGrupalesGuiadas(
    minGuias: 2,
    capacidadPorGuia: 30,
    capacidadMaxima: 60,
    diasDisponibles: diasLaborales,
    turnos: turnos
);
```

### Modificar Configuración

```csharp
// Agregar nuevo turno
var nuevoTurno = new TurnoVisitaGuiada(
    new TimeOnly(12, 0), 
    new TimeOnly(13, 0)
);
config.AgregarTurno(nuevoTurno);

// Actualizar capacidades
config.ActualizarCapacidad(
    minGuias: 3,
    capacidadPorGuia: 25,
    capacidadMaxima: 75
);

// Cambiar días disponibles
var nuevosDias = new DiasLaboralesMuseo(new[]
{
    DayOfWeek.Monday,
    DayOfWeek.Wednesday,
    DayOfWeek.Friday
});
config.ActualizarDiasDisponibles(nuevosDias);
```

### Consultar Disponibilidad

```csharp
// Verificar si un día está disponible
bool disponible = config.EsDiaDisponible(DayOfWeek.Monday); // true

// Calcular capacidad según guías disponibles
int capacidad = config.CalcularCapacidadDisponible(guiasDisponibles: 3); // 60
```

---

## ?? Migración Requerida

### 1. Implementar Repositorio en Infrastructure

```csharp
// Infrastructure/Repositories/Sql/RepositorioConfiguracionVisitasGrupalesGuiadas.cs
public class RepositorioConfiguracionVisitasGrupalesGuiadas 
    : BaseRepository<ConfiguracionVisitasGrupalesGuiadas>, 
      IRepositorioConfiguracionVisitasGrupalesGuiadas
{
    public async Task<ConfiguracionVisitasGrupalesGuiadas?> ObtenerConfiguracionActivaAsync()
    {
        // Implementar lógica para obtener la configuración activa
        // Por ejemplo: return await _dbSet.FirstOrDefaultAsync();
    }
}
```

### 2. Configurar EF Core Mapping

```csharp
// Infrastructure/Configurations/ConfiguracionVisitasGrupalesGuiadaConfig.cs
public class ConfiguracionVisitasGrupalesGuiadaConfig 
    : IEntityTypeConfiguration<ConfiguracionVisitasGrupalesGuiadas>
{
    public void Configure(EntityTypeBuilder<ConfiguracionVisitasGrupalesGuiadas> builder)
    {
        builder.ToTable("ConfiguracionVisitasGrupalesGuiadas");

        // Configurar DiasLaborales como owned type
        builder.OwnsOne(c => c.DiasDisponibles, dias =>
        {
            dias.Property(d => d.Dias)
                .HasConversion(
                    v => string.Join(',', v),
                    v => v.Split(',', StringSplitOptions.RemoveEmptyEntries)
                          .Select(x => Enum.Parse<DayOfWeek>(x))
                          .ToList()
                );
        });

        // Configurar Turnos como owned collection
        builder.OwnsMany(c => c.Turnos, turnos =>
        {
            turnos.WithOwner();
            turnos.Property(t => t.HoraInicio).IsRequired();
            turnos.Property(t => t.HoraFin).IsRequired();
        });
    }
}
```

### 3. Crear Migración de Base de Datos

```bash
dotnet ef migrations add AgregarConfiguracionVisitasGrupalesGuiadas
dotnet ef database update
```

### 4. Seed Inicial

```csharp
// Agregar en el seeder de la aplicación
var configDefault = ConfiguracionVisitasFactory.CrearConfiguracionPorDefecto();
await repositorioConfiguracion.AddAsync(configDefault);
```

---

## ? Validaciones Implementadas

### En `TurnoVisitaGuiada`:
- ? Hora fin > hora inicio
- ? Duración mínima: 30 minutos
- ? Duración máxima: 3 horas

### En `ConfiguracionVisitasGrupalesGuiadas`:
- ? MinGuias > 0
- ? CapacidadPorGuia > 0
- ? CapacidadMaxima >= CapacidadPorGuia
- ? Turnos no se solapen entre sí
- ? Al menos un día laboral
- ? Al menos un turno disponible

---

## ?? Beneficios

1. **Flexibilidad**: Cambiar turnos sin recompilar
2. **Trazabilidad**: Historial de cambios en configuración
3. **Multi-tenant**: Diferentes configuraciones por sucursal/museo
4. **Testing**: Más fácil mockear configuraciones
5. **Mantenibilidad**: Código más limpio y SOLID

---

## ?? Breaking Changes

### Para Desarrolladores:
- `TurnosVisitasGuiadas.HorarioTurnos` está **obsoleto**
- `IConfiguracionVisitasOptionsProvider` puede ser removido
- `TurnosVisitasOptions` puede ser removido
- `IServicioDisponibilidadTurnosVisitasGuiadas.CalcularDisponibilidad()` ahora requiere `ConfiguracionVisitasGrupalesGuiadas`

### Plan de Migración:
1. ? Implementar repositorio en Infrastructure
2. ? Configurar EF Core mappings
3. ? Ejecutar migraciones
4. ? Seed de configuración inicial
5. ? Actualizar dependency injection
6. ? Remover clases obsoletas (opcional)

---

## ?? Testing

```csharp
[Fact]
public void DeberiaCalcularCapacidadCorrectamente()
{
    // Arrange
    var config = ConfiguracionVisitasFactory.CrearConfiguracionPorDefecto();

    // Act
    var capacidad1Guia = config.CalcularCapacidadDisponible(1);
    var capacidad2Guias = config.CalcularCapacidadDisponible(2);

    // Assert
    capacidad1Guia.Should().Be(25);
    capacidad2Guias.Should().Be(50);
}

[Fact]
public void NoDeberiaPermitirTurnosSolapados()
{
    // Arrange
    var config = ConfiguracionVisitasFactory.CrearConfiguracionPorDefecto();
    var turnoSolapado = new TurnoVisitaGuiada(
        new TimeOnly(9, 45), 
        new TimeOnly(10, 45)
    );

    // Act & Assert
    Assert.Throws<DomainException>(() => config.AgregarTurno(turnoSolapado));
}
```

---

## ?? Notas Finales

- La clase `TurnosVisitasGuiadas` se mantiene temporalmente para compatibilidad
- Se recomienda removerla en la próxima versión mayor
- La configuración debe ser única y activa por museo/sistema
- Considerar agregar versionamiento de configuraciones para auditoría

---

**Fecha de Refactorización**: 2024
**Versión**: 2.0
**Autor**: Sistema de Refactorización DDD
