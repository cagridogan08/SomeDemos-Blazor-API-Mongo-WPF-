

namespace WpfAppWithRedisCache
{
    public class UserLoginDto
    {
        public static UserLoginDto GetDefaultUser() => new UserLoginDto
        {
            Username = "admin",
            Password = "Iotech+2015"
        };

        public string Password { get; set; }

        public string Username { get; set; }
    }
}
