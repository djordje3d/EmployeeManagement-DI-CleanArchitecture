namespace Zaposleni_Clean_MVC_API.Models
{
    public class UserDto
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }

        public List<string> Roles { get; set; } = new List<string>(); // Dodata lista uloga
    }
}
