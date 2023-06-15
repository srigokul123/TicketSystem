using ArcTicketSystem.API.Model;

namespace ArcTicketSystem.API.Repository
{
    public interface ITicketRepository
    {

        Task<IEnumerable<Ticket>> Get();

        Task<IEnumerable<Ticket>> GetAll(decimal UID); 

        Task<Ticket> Get(decimal Ticketid);

        Task<Ticket> Create(Ticket ticket);

        Task Update(Ticket ticket);

        Task Delete(decimal Ticketid);

        List<expticket> GetUserwiseReport(decimal UID);
    }


}

