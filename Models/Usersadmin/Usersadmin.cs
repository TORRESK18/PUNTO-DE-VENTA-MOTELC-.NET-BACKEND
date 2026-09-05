using System.ComponentModel.DataAnnotations;

namespace MTLCRISTALVK18BACK.Models.Usersadmin
{
    public class Usersadmin
    {

        [Key]
        public int IdUs { get; set; }

        [Required]
        public int NtrabajadorUs { get; set; }
        public string? NombreUs { get; set; }
        public string? PuestoUs { get; set; }
        public string? DepartamentoUs { get; set; }
        public string? UsernameUs  { get; set; }

        [EmailAddress]
        public string? EmailUs { get; set; }
        public string? PasswordUs { get; set; }
    }
}
