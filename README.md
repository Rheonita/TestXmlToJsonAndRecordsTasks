## Функционал API 🚀
## Приложение состоит из двух частей:
### 🔄 1.Конвертация XML в JSON и логирование по типу
Приложение принимает XML-документ, конвертирует его в JSON, записывает в лог-файлы, разделяя данные по типам (Type). При этом для каждого типа создается отдельный файл с именем, состоящим из типа и даты.

POST http://127.0.0.1:9091/api/ToJson

Приложение ожидает получить XML-документ следующего формата:
```xml
<?xml version="1.0" encoding="UTF-8" ?>
<Data>
    <Method>
        <Name>Order</Name>
        <Type>Services</Type>
        <Assembly>ServiceRepository, Version=1.0.0.1, Culture=neutral, PublicKeyToken=null</Assembly>
    </Method>
    <Process>
        <Name>scheduler.exe</Name>
        <Id>185232</Id>
        <Start>
            <Epoch>1464709722277</Epoch>
            <Date>2016-05-31T12:07:42.2771759+03:00</Date>
        </Start>
    </Process>
    <Layer>DailyScheduler</Layer>
    <Creation>
        <Epoch>1464709728500</Epoch>
        <Date>2016-05-31T07:48:21.5007982+03:00</Date>
    </Creation>
    <Type>Information</Type>
</Data>
```

Далее сконвертированный файл вы можете получить в директории которую указали в appsetting.json переменной Directory

---

### 🧾 2.Перенос данных в новые файлы через 100 записей
Вторая часть приложения обрабатывает лог-файлы, содержащие информацию по типам, читая их построчно и записывая каждые 100 записей в отдельные файлы, используя нумерацию файлов (например, Information-2016-05-31-0001.log). Программа сохраняет состояние обработки и продолжает с последнего обработанного файла при перезапуске.

POST http://127.0.0.1:9091/api/Records
Необходимо отправить json тело
```json
{
  "recordsDirectory": "E:\\Logs\\Information-2016-05-31.log"
}
```

Ответ
```json
{
    "message": "Записи обработаны"
}
```

Далее откройте корневую папку с проектом, либо через обозреватель решений вы найдете нужные вам результаты в виде файлов формата .log

![image](https://github.com/user-attachments/assets/6903c49a-ed27-4890-9450-0ac2ae1ce3b5)

Перед повторным запуском необходимо очистить либо удалить файл state.json


---

Технологии ⚙️
ASP.NET Core 8.0
Newtonsoft.Json
