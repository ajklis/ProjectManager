namespace ProjectManager.Infrastructure.Options
{
    public class AuthenticationOptions
    {
        public static string OptionsKey { get; } = nameof(AuthenticationOptions);
        public TimeSpan AccessTokenLifeTime { get; set; }
    }
}
