namespace Application.PersonalMuseo.DataTrasnferObjets
{
    public class LoginPersonalResponseDto
    {
        public string Token { get; set; } = string.Empty;
        public UserMuseoDto User { get; set; } = default!;
        public DateTime ExpiresAt { get; set; }
    }
}
