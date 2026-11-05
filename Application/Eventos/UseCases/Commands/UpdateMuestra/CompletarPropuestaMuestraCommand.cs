using Application.Eventos.DataTransferObjets;
using Core.Application;

namespace Application.Eventos.UseCases.Commands.UpdateMuestra
{
    public class CompletarPropuestaMuestraCommand : IRequestCommand<MuestraDto>
    {
        public string MuestraId { get; set; }
        public string NombreResponsable { get; set; }
        public string TelefonoResponsable { get; set; }
        public string EmailResponsable { get; set; }
        public string TituloPropuesta { get; set; }
        public LogisticaMuestraTemporalDto LogisticaMuestraTemporal { get; set; }
        public string Publico { get; set; }
        public bool RequiereDifusion { get; set; }
        public string DescripcionMuestra { get; set; }
        public string FundamentacionMuestra { get; set; }
        public string? Institucion { get; set; }
        public string? EmailInstitucion { get; set; }
        public List<string> UrlImagenes { get; set; } = [];
        public List<ArchivoNuevoDto>? ImagenesNuevas { get; set; }
        public bool SolicitarAsistenciaDifusion { get; set; }
        public List<string> SalasIds { get; set; } = [];


        public CompletarPropuestaMuestraCommand()
        {

        }

    }


}
