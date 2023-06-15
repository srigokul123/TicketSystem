using ArcTicketSystem.API.Model;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace ArcTicketSystem.API.Data
{
    public class TicketContext : DbContext
    {
        public TicketContext(DbContextOptions<TicketContext> options) : base(options)
        {

        }
        public DbSet<Ticket> Ticket { get; set; }

        public DbSet<User> Users { get; set; }
    }
}
