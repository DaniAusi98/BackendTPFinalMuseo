namespace Application.ConfiguracionSistema.DataTransferObjets
{
    public class MuseumHoursDto
    {
        public List<HorarioAperturaDto> HorarioAperturaList { get; private set; }

        public List<DiaCierreDto> DiaCierreDtos { get; private set; }
        public MuseumHoursDto(
            List<HorarioAperturaDto> horarioAperturaList,
            List<DiaCierreDto> diaCierreDtos)
        {
            HorarioAperturaList = horarioAperturaList;
            DiaCierreDtos = diaCierreDtos;
        }
    }
}
