namespace AuthenticationService.Options
{
    public class AuthenticationOptions
    {
        public static string OptionsKey { get; } = nameof(AuthenticationOptions);
        public TimeSpan AccessTokenLifeTime { get; set; }
        public string AuthenticationUrl { get; set; }
        public string AuthenticationSetupUrl { get; set; }
    }
}
