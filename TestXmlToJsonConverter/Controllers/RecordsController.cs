using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TestXmlToJsonConverter.Dtos;
using TestXmlToJsonConverter.RecordsHandler;

namespace TestXmlToJsonConverter.Controllers
{
    [Route("api/Records")]
    [ApiController]
    public class RecordsController : ControllerBase
    {
        private IRecordsHandler _recordsHandler { get; set; }
        public RecordsController(IRecordsHandler recordsHandler) =>
            _recordsHandler = recordsHandler;

        [HttpPost]
        public IActionResult HandleRecords([FromBody]RecordsRequest recordsfile)
        {
            try
            {
                _recordsHandler.HandleRecords(recordsfile.recordsDirectory);
                return Ok(new { message = "Записи обработаны" });
            }
            catch (Exception ex)
            {
                {
                    return StatusCode(500, new { message = $"Ошибка при обработке записей {ex.Message}" });
                }
            }
        }
    }
}
