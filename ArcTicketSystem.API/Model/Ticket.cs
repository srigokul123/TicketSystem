using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace ArcTicketSystem.API.Model
{
    public class Ticket
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]

        public decimal Ticketid { get; set; }
        public string Ticketname { get; set; }
        public string Ticketcategory { get; set; }
        public string Ticketdescription { get; set; }
        public string Ticketstatus { get; set; }
        public decimal Createdby { get; set; }
        public DateTime Createdon { get; set; }
        public decimal Modifiedby { get; set; }
        public DateTime Modifiedon { get; set; }

    }
    public class expticket {
        public string Ticketname { get; set; }
        public string Ticketcategory { get; set; }
        public string Ticketdescription { get; set; }
        public string Ticketstatus { get; set; }
        public DateTime Createdon { get; set; }
    }
}


