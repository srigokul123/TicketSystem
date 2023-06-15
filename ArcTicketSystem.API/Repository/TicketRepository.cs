using ArcTicketSystem.API.Data;
using ArcTicketSystem.API.Model;
using ArcTicketSystem.API.Repository;
using Microsoft.EntityFrameworkCore;

namespace ArcTicketSystem.API.Repository
{
    public class TicketRepository : ITicketRepository
    {
        private readonly TicketContext _context;
        public TicketRepository(TicketContext context)
        {
            _context = context;
        }

        public async Task<Ticket> Create(Ticket ticket)
        {
            ticket.Createdon = DateTime.UtcNow;
            ticket.Modifiedon = DateTime.UtcNow;
            _context.Ticket.Add(ticket);
            await _context.SaveChangesAsync();
            return ticket;
        }

        public async Task Delete(decimal Ticketid)
        {
            var ticketToDelete = await _context.Ticket.FindAsync(Ticketid);
            _context.Ticket.Remove(ticketToDelete);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Ticket>> Get()
        {

            return await _context.Ticket.ToListAsync();

        }
        
       public async Task<IEnumerable<Ticket>> GetAll(decimal UID)
        {
            return await _context.Ticket.Where(o=>o.Createdby == UID).ToListAsync();
        }


        public async Task<Ticket> Get(decimal Ticketid)
        {

            return await _context.Ticket.FindAsync(Ticketid);

        }

        public async Task Update(Ticket ticket)

        {
            var extkt = _context.Ticket.FirstOrDefault(t => t.Ticketid == ticket.Ticketid);
            if (extkt != null)
            {
                ticket.Createdon = extkt.Createdon;

                extkt.Ticketname = ticket.Ticketname;
                extkt.Ticketcategory = ticket.Ticketcategory;
                extkt.Ticketdescription = ticket.Ticketdescription;
                extkt.Ticketstatus = ticket.Ticketstatus;
                extkt.Createdby = ticket.Createdby;
                extkt.Modifiedon = DateTime.Now;
                extkt.Modifiedby = ticket.Modifiedby;
                await _context.SaveChangesAsync();

            }

            //_context.Entry(ticket).State = EntityState.Modified;
            //await _context.SaveChangesAsync();
        }

        public List<expticket> GetUserwiseReport(decimal UID)
        {
            try
            {
                var listoftickets = (from expTic in _context.Ticket.Where(o => o.Createdby == UID).AsNoTracking()
                                   select new expticket()
                                   {
                                       Ticketname = expTic.Ticketname,
                                       Ticketcategory = expTic.Ticketcategory,
                                       Ticketdescription = expTic.Ticketdescription,
                                       Ticketstatus = expTic.Ticketstatus,
                                       Createdon = expTic.Createdon,                                       
                                   }).ToList();

                return listoftickets;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

    }

}