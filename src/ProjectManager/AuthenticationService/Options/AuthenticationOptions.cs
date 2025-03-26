namespace AuthenticationService.Options
{
    public class AuthenticationOptions
    {
        public static string OptionsKey { get; } = nameof(AuthenticationOptions);
        public TimeSpan AccessTokenLifeTime { get; set; }
        public string AuthenticationChannel { get; set; }
    }
}
