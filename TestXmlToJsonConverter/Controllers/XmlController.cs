using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Text;
using System.Xml.Linq;
using TestXmlToJsonConverter.XmlHandler;

namespace TestXmlToJsonConverter.Controllers
{
    [Route("api/ToJson")]
    [ApiController]
    public class XmlController : ControllerBase
    {
        private IXmlHandler _xmlHandler;
        public XmlController(IXmlHandler xmlHandler) => _xmlHandler = xmlHandler;

        [HttpPost]
        [Consumes("application/xml")]
        public async Task<IActionResult> convertXmlToJson()
        {
            using var reader = new StreamReader(Request.Body, Encoding.UTF8);
            string xmlString = await reader.ReadToEndAsync();
            var result = await _xmlHandler.ProceedXmlConvert(xmlString);
            if (result.isSuccess)
                return Ok(new { Message = result.message, Json = result.jsonMessage });
            else
                return BadRequest(new { Message = result.message});

        }

    }
}
