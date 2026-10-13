namespace Application.Usuario.DataTransferObjets
{
    public class LoginResponseDto
    {
        public string Token { get; set; } = string.Empty;
        public UserDto User { get; set; } = default!;
        public DateTime ExpiresAt { get; set; }
        public LoginResponseDto() { }

    }
}
