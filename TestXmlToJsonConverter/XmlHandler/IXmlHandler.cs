using System.Xml.Linq;
using TestXmlToJsonConverter.Dtos;

namespace TestXmlToJsonConverter.XmlHandler
{
    public interface IXmlHandler
    {
        Task<ConvertedResponse> ProceedXmlConvert(string xmlData);
    }
}
