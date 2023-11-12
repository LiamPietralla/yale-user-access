namespace YaleAccess.Models.Options
{
    public class AuthenticationOptions
    {
        public const string Authentication = "Authentication";

        public string Password { get; set; } = string.Empty;
    }
}
