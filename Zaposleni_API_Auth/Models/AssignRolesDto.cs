namespace Zaposleni_API_Auth.Models
{
    public class AssignRolesDto
    {
        public string UserId { get; set; }
        public List<string> Roles { get; set; } = new List<string>();
    }

}
