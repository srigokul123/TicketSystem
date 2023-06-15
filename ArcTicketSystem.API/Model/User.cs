using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace ArcTicketSystem.API.Model
{
    public class User
    {
        [Key]
        [NotNull]
        public decimal Userid { get; set; }
        [StringLength(256)]
        [NotNull]
        public string Username { get; set; }
        [StringLength(50)]
        [NotNull]
        public string Emailid { get; set; }
        [StringLength(50)]
        [NotNull]
        public string Password { get; set; }
        [StringLength(50)]
        [NotNull]
        public string Userrole { get; set; }
        [StringLength(256)]
        [NotNull]
        public string Status { get; set; }
        [NotNull]
        public DateTime Createdon { get; set; }
    }
}
