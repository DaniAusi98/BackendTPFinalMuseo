# CONTRIBUTING

## Propósito
Este repositorio implementa reglas de dominio del museo. Las contribuciones deben respetar el proceso operativo real de la institución.

## Estándares de dominio

### Flujo de eventos institucionales
- La aprobación/rechazo inicial de propuestas de evento se gestiona **fuera del sistema** con Dirección.
- La entidad/formulario `Evento` se crea **solo después** de esa aprobación previa.
- En esta etapa del sistema, `Evento` **no** debe modelar:
  - una entidad intermedia de solicitud a Dirección,
  - ni un flujo de rechazo automático por email al organizador.
- Cualquier cambio que introduzca esos comportamientos requiere validación funcional explícita con el área usuaria.

## Buenas prácticas para cambios
- Evitar agregar estados o entidades que no representen el proceso institucional vigente.
- Priorizar trazabilidad simple (fechas/observaciones) antes que nuevos flujos de negocio.
- Mantener consistencia con el modelo actual de `ActividadMuseo` para estados operativos.