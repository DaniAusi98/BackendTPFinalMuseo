namespace Application.Usuario.ApplicationServices.ApplicationServiceInterfaces
{
    public class ConfirmEmailServiceResponse : IConfirmEmailService
    {
        private readonly IIdentityService _identityService;

        public ConfirmEmailServiceResponse(IIdentityService identityService)
        {
            _identityService = identityService;
        }

        public async Task<bool> ExecuteAsync(
            string userId,
            string token,
            CancellationToken cancellationToken = default)
        {
            return await _identityService.ConfirmEmailAsync(
            userId,
            token);
        }
    }
}
