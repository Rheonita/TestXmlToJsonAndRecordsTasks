using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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
        public IActionResult HandleRecords(string recordsfile)
        {
            try
            {
                _recordsHandler.HandleRecords(recordsfile);
                return Ok(new { message = "Log processing started successfully." });
            }
            catch (Exception ex)
            {
                {
                    return StatusCode(500, new { message = $"Error processing logs: {ex.Message}" });
                }
            }
        }
    }
}
