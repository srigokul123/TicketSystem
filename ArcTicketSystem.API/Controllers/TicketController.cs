﻿using ArcTicketSystem.API.Model;
using Microsoft.AspNetCore.Http;
using OfficeOpenXml;
using OfficeOpenXml.Table;

namespace ArcTicketSystem.API.Controllers
        {
            return await _ticketRepository.GetAll(UID);
        }
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
                var exportbytes = ExporttoExcel<expticket>(list, reportname);

                };
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
        }