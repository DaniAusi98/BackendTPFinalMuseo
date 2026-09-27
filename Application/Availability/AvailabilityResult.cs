namespace Application.Availability
{
    public record AvailabilityResult(bool IsOk, string Message = "")
    {
        public static AvailabilityResult Ok() => new(true, string.Empty);
        public static AvailabilityResult Fail(string message) => new(false, message);
    }
}
