using Application.VisitaGrupal.UseCases.Commands.UpdateConfiguracionVisitasGrupalesGuiadas;
using FluentValidation;

namespace Application.VisitaGrupal.Validators
{
    public class UpdateConfiguracionVisitasGrupalesGuiadasValidator : AbstractValidator<UpdateConfiguracionVisitasGrupalesGuiadasCommand>
    {
        public UpdateConfiguracionVisitasGrupalesGuiadasValidator()
        {
            RuleFor(x => x.MinGuiasParaCapacidadCompleta)
                .GreaterThan(0)
                .WithMessage("El número mínimo de guías debe ser mayor a 0.");

            RuleFor(x => x.CapacidadPorGuia)
                .GreaterThan(0)
                .WithMessage("La capacidad por guía debe ser mayor a 0.");

            RuleFor(x => x.CapacidadMaximaPorTurno)
                .GreaterThan(0)
                .WithMessage("La capacidad máxima por turno debe ser mayor a 0.")
                .GreaterThanOrEqualTo(x => x.CapacidadPorGuia)
                .WithMessage("La capacidad máxima no puede ser menor a la capacidad por guía.");

            RuleFor(x => x.DiasDisponibles)
                .NotNull()
                .WithMessage("Debe especificar los días disponibles.")
                .Must(dias => dias != null && dias.Any())
                .WithMessage("Debe existir al menos un día disponible.");

            RuleFor(x => x.Turnos)
                .NotNull()
                .WithMessage("Debe especificar los turnos.")
                .Must(turnos => turnos != null && turnos.Any())
                .WithMessage("Debe existir al menos un turno disponible.");

            RuleForEach(x => x.Turnos)
                .ChildRules(turno =>
                {
                    turno.RuleFor(t => t.HoraFin)
                        .GreaterThan(t => t.HoraInicio)
                        .WithMessage("La hora de fin debe ser posterior a la hora de inicio.");

                    turno.RuleFor(t => t)
                        .Must(t =>
                        {
                            var duracion = t.HoraFin.ToTimeSpan() - t.HoraInicio.ToTimeSpan();
                            return duracion.TotalMinutes >= 30;
                        })
                        .WithMessage("Un turno debe durar al menos 30 minutos.");

                    turno.RuleFor(t => t)
                        .Must(t =>
                        {
                            var duracion = t.HoraFin.ToTimeSpan() - t.HoraInicio.ToTimeSpan();
                            return duracion.TotalHours <= 3;
                        })
                        .WithMessage("Un turno no puede durar más de 3 horas.");
                });

            RuleFor(x => x.Turnos)
                .Must(turnos =>
                {
                    if (turnos == null || turnos.Count <= 1) return true;

                    for (int i = 0; i < turnos.Count; i++)
                    {
                        for (int j = i + 1; j < turnos.Count; j++)
                        {
                            var turno1 = turnos[i];
                            var turno2 = turnos[j];

                            if (turno1.HoraInicio < turno2.HoraFin && turno1.HoraFin > turno2.HoraInicio)
                            {
                                return false;
                            }
                        }
                    }
                    return true;
                })
                .WithMessage("Los turnos no deben solaparse entre sí.");
        }
    }
}
