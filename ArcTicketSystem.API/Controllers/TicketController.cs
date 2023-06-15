using ArcTicketSystem.API.Model;using ArcTicketSystem.API.Repository;using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;using Microsoft.AspNetCore.Mvc;using Newtonsoft.Json;
using OfficeOpenXml;
using OfficeOpenXml.Table;

namespace ArcTicketSystem.API.Controllers{    [Route("api/[controller]")]    [ApiController]    [Authorize]    public class TicketController : ControllerBase    {        private readonly ITicketRepository _ticketRepository;        public TicketController(ITicketRepository ticketRepository)        {            _ticketRepository = ticketRepository;        }        [HttpGet]      // [Authorize(Policy = "Admin")]        public async Task<IEnumerable<Ticket>> GetTicket()        {            return await _ticketRepository.Get();        }        [HttpGet("{UID}")]        //[Route("api/Ticket/{UID}")]        public async Task<IEnumerable<Ticket>> GetAll(decimal UID)
        {
            return await _ticketRepository.GetAll(UID);
        }       // [HttpGet("{Ticketid}")]       //// [Authorize(Policy = "User")]       // public async Task<ActionResult<Ticket>> GetTicket(int Ticketid)       // {       //     return await _ticketRepository.Get(Ticketid);       // }        [HttpPost]        public async Task<ActionResult<Ticket>> PostTicket([FromBody] Ticket ticket)        {            var newTicket = await _ticketRepository.Create(ticket);            return CreatedAtAction(nameof(GetTicket), new { Ticketid = newTicket.Ticketid }, newTicket);        }        [HttpPut("{Ticketid}")]      //  [Authorize(Policy = "Admin")]        public async Task<ActionResult> PutTicket(int Ticketid, [FromBody] Ticket ticket)        {            if (Ticketid != ticket.Ticketid)            {                return BadRequest();            }            await _ticketRepository.Update(ticket);            return NoContent();        }        [HttpDelete("{Ticketid}")]      //  [Authorize(Policy = "Admin")]        public async Task<ActionResult> Delete(int Ticketid)        {            var ticketToDelete = await _ticketRepository.Get(Ticketid);            if (ticketToDelete == null)            {                return NotFound();            }            await _ticketRepository.Delete(ticketToDelete.Ticketid);            return NoContent();        }        [HttpGet]
        [Route("api/Ticket/DownloadReport")]
        public IActionResult DownloadReport()
        {
            return Ok();
        }

        [HttpPost]
        [Route("DownloadReport")]
        public IActionResult DownloadReport([FromBody] decimal UID)
        {
            string reportname = $"User_Wise_{Guid.NewGuid():N}.xlsx";
            var list = _ticketRepository.GetUserwiseReport(UID);
            if (list.Count > 0)
            {
                //var exportbytes = ExporttoExcel<expticket>(list, reportname);
                //return File(exportbytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", reportname);
                var exportbytes = ExporttoExcel<expticket>(list, reportname);                var jsonObj = new                {                    content = exportbytes,                    type = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",                    filename = reportname

                };                var JsonResult = JsonConvert.SerializeObject(jsonObj);
                //return File(exportbytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", reportname);
                return Ok(JsonResult);



            }
            else
            {
                //TempData["Message"] = "No Data to Export";
                //return View();
                return BadRequest();
            }
            
        }

        private byte[] ExporttoExcel<T>(List<T> table, string filename)
        {
            using ExcelPackage pack = new ExcelPackage();
            ExcelWorksheet ws = pack.Workbook.Worksheets.Add(filename);
            ws.Cells["A1"].LoadFromCollection(table, true, TableStyles.Light1);
            return pack.GetAsByteArray();
        }    }}