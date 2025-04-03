using System.Text;
using System.Text.Json;
using System.Xml.Linq;
using TestXmlToJsonConverter.Dtos;

namespace TestXmlToJsonConverter.XmlHandler
{
    public class XmlHandler : IXmlHandler
    {
        private string LogDirectory = Environment.GetEnvironmentVariable("Directory");
        public async Task<ConvertedResponse> ProceedXmlConvert(string xmlData)
        {
            try
            {
                /*
                Нижняя проверка нужна т.к. в изначальном xmle не было Root элемента на случай если будет несколько элементов Data чтобы можно было идентифицировать каждый data элемент по type,
                json указанный в документации по логике указывает на то что такой root нужен
                */
                // Проверка на наличие корневого элемента
                if (!xmlData.Trim().Contains("<Root>"))
                {
                    // Если <Root> еще нет, добавляем его после XML декларации
                    xmlData = xmlData.Insert(xmlData.IndexOf("?>") + 2, "<Root>") + "</Root>";
                }

                var xml = XDocument.Parse(xmlData);

                // Перебираем все элементы <Data>
                var dataElements = xml.Root?.Elements("Data") ?? Enumerable.Empty<XElement>();

                foreach (var data in dataElements)
                {
                    string type = data.Element("Type")?.Value ?? "Unknown";
                    string date = data.Element("Creation")?.Element("Date")?.Value.Split('T')[0] ?? "Unknown";

                    var dict = new Dictionary<string, object>();

                    foreach (var element in data.Elements())
                    {
                        if (element.HasElements)
                        {
                            var subDict = new Dictionary<string, object>();
                            foreach (var subElement in element.Elements())
                            {
                                subDict[subElement.Name.LocalName] = subElement.Value;
                            }
                            dict[element.Name.LocalName] = subDict;
                        }
                        else
                        {
                            dict[element.Name.LocalName] = element.Value;
                        }
                    }

                    string json = JsonSerializer.Serialize(dict, new JsonSerializerOptions { WriteIndented = false });

                    // Сохраняем логи для текущего типа
                    SaveLog(type, date, json);
                }

                return new ConvertedResponse { isSuccess = true, jsonMessage = "Записи сохранены в файлы", message = "Успешная конвертация" };
            }
            catch (Exception ex)
            {
                return new ConvertedResponse { isSuccess = false, jsonMessage = null, message = "Ошибка при конвертации" };
            }
        }

        private void SaveLog(string type, string date, string json)
        {
            string filePath = Path.Combine(LogDirectory, $"{type}-{date}.log");

            if (!Directory.Exists(LogDirectory))
                Directory.CreateDirectory(LogDirectory);

            int recordCount = 0;
            if (File.Exists(filePath))
            {
                string[] lines = File.ReadAllLines(filePath);
                if (lines.Length > 0 && lines[0].StartsWith("Records:"))
                {
                    int.TryParse(lines[0].Split(':')[1].Trim(), out recordCount);
                }
            }

            recordCount++;
            string[] updatedLines = new[] { $"Records: {recordCount}" }
                .Concat(File.Exists(filePath) ? File.ReadLines(filePath).Skip(1) : Enumerable.Empty<string>())
                .Concat(new[] { json })
                .ToArray();

            File.WriteAllLines(filePath, updatedLines, Encoding.UTF8);
        }

    }
}
