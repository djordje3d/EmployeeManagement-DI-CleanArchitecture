namespace Zaposleni_Clean_MVC_API.Models
{
    public class AssignRolesDto
    {
        public string UserId { get; set; }
        public List<string> Roles { get; set; } = new List<string>();
    }

}
