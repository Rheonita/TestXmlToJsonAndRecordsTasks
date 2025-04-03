using Newtonsoft.Json;

namespace TestXmlToJsonConverter.RecordsHandler
{
    public class RecordsHandler : IRecordsHandler
    {
        private const string StateFile = "state.json"; // Файл состояния
        private const int RecordsPerFile = 100; // Количество записей в одном файле

        public void HandleRecords(string inputFile)
        {
            // Загрузка состояния (если оно есть)
            var state = LoadState();

            // Чтение всех записей из исходного файла
            var lines = File.ReadAllLines(inputFile);

            int currentRecord = state.LastProcessedRecordIndex == 0 ? 1 : state.LastProcessedRecordIndex;
            int currentFileNumber = state.LastProcessedFileNumber;

            List<string> currentBatch = new List<string>();

            // Обработка всех записей
            for (int i = currentRecord; i < lines.Length; i++)
            {
                currentBatch.Add(lines[i]);

                // Если собрали 100 записей, записываем их в новый файл
                if (currentBatch.Count >= RecordsPerFile)
                {
                    currentFileNumber++;
                    string newFileName = $"{Path.GetFileNameWithoutExtension(inputFile)}-{currentFileNumber:D4}.log";
                    File.WriteAllLines(newFileName, currentBatch);
                    currentBatch.Clear(); // Очищаем текущую партию для следующего файла
                }

                // Сохраняем состояние на каждом шаге
                SaveState(i, currentFileNumber);
            }

            // Если есть не записанные данные (меньше 100 записей)
            if (currentBatch.Count > 0)
            {
                currentFileNumber++;
                string newFileName = $"{Path.GetFileNameWithoutExtension(inputFile)}-{currentFileNumber:D4}.log";
                File.WriteAllLines(newFileName, currentBatch);
            }
        }

        // Сохраняем информацию о последнем обработанном файле и записи
        private void SaveState(int lastProcessedRecordIndex, int lastProcessedFileNumber)
        {
            var state = new ProcessingState
            {
                LastProcessedRecordIndex = lastProcessedRecordIndex,
                LastProcessedFileNumber = lastProcessedFileNumber
            };

            File.WriteAllText(StateFile, JsonConvert.SerializeObject(state));
        }

        // Загружаем информацию о последнем состоянии обработки
        private ProcessingState LoadState()
        {
            if (File.Exists(StateFile))
            {
                var json = File.ReadAllText(StateFile);
                return JsonConvert.DeserializeObject<ProcessingState>(json);
            }
            return new ProcessingState(); // Если файла нет, начинаем с начала
        }
    }

    public class ProcessingState
    {
        public int LastProcessedRecordIndex { get; set; }
        public int LastProcessedFileNumber { get; set; }
    }
}
