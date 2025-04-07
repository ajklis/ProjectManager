namespace AuthenticationService.Models
{
    public class LoginRequest
    {
        public string Email { get; set; }
        public string HashedPassword { get; set; }
    }
}
