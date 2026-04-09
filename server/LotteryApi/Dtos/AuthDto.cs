namespace LotteryApi.Dtos
{
    public class AuthDto
    {
        public class LoginRequestDto
        {
            public string Email { get; set; } = string.Empty;
            public string Password { get; set; } = string.Empty;
        }

        public class LoginResponseDto
        {
            public string Token { get; set; } = string.Empty;
            public string TokenType { get; set; } = "Bearer";
            public int ExpiresIn { get; set; }
            public UserDto User { get; set; } = null!;
        }
    }
}
