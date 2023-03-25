
namespace SharedKernell.Helpers
{
    using DocumentFormat.OpenXml.Packaging;
    using IFilterTextReader;
    using iTextSharp.text.pdf;
    using iTextSharp.text.pdf.parser;
    using Microsoft.AspNetCore.Http;
    using MimeTypes;
    using System.Dynamic;
    using System.Globalization;
    using System.Security.Cryptography;
    using System.Text;
    using System.Text.RegularExpressions;

    public static class CoreHelper
    {
        public static string GetSha256(string str)
        {
            var sha256 = SHA256.Create();
            var enconding = new ASCIIEncoding();
            var sb = new StringBuilder();
            var stream = sha256.ComputeHash(enconding.GetBytes(str));
            foreach (var item in stream) sb.AppendFormat("{0:x2}", item);

            return sb.ToString();
        }

        public static bool IsMailValid(string mail)
        {
            return Regex.IsMatch(mail, @"^[^@\s]+@[^@\s]+\.[^@\s]+$", RegexOptions.IgnoreCase);
        }

        public static DateTime GetDatePeru(this DateTime currentDate)
        {
            DateTime currentDatePeru = TimeZoneInfo.ConvertTime(currentDate, GetHourZonePeru());
            return currentDatePeru.Date;
        }

        public static DateTime GetDateTimePeru(this DateTime currentDate)
        {
            DateTime currentDatePeru = TimeZoneInfo.ConvertTime(currentDate, GetHourZonePeru());
            return currentDatePeru;
        }

        public static bool RangeDateBetween( this DateTime currentDate, DateTime startDate, DateTime endDate)
        {
            return (currentDate >= startDate && currentDate <= endDate);
        }

        public static TimeZoneInfo GetHourZonePeru()
        {
            string displayName = "(GMT-05:00) Peru Time";
            string standardName = "Peru Time";
            TimeSpan offset = new TimeSpan(-5, 0, 0);
            return TimeZoneInfo.CreateCustomTimeZone(standardName, offset, displayName, standardName);
        }

        public static string Base64Encode(string plainText)
        {
            var plainTextBytes = Encoding.UTF8.GetBytes(plainText);
            return Convert.ToBase64String(plainTextBytes);
        }

        public static string Base64Decode(string base64EncodedData)
        {
            var base64EncodedBytes = Convert.FromBase64String(base64EncodedData);
            return Encoding.UTF8.GetString(base64EncodedBytes);
        }

        public static DateTime GetNextWeekend(int day)
        {
            DateTime result = DateTime.Now.GetDatePeru().AddDays(1);
            while ((int)result.DayOfWeek != day)
                result = result.AddDays(1);
            return result;
        }

        public static DateTime GetNextWeekend(DateTime date, int day)
        {
            while ((int)date.DayOfWeek != day)
                date = date.AddDays(1);
            return date;
        }

        public static IEnumerable<(string Key, string Name)> GetNameMonths(DateTime desde, DateTime hasta)
        {
            return Enumerable.Range(0, 11)
                     .Select(p => desde.AddMonths(p))
                     .TakeWhile(p => p <= hasta)
                     .Select(p => (
                         Key: p.ToString("MM/yyyy"),
                         Name: p.GetNameMonth()
                     ))
                     .ToList();
        }

        public static bool IsPdf(this string base64String)
        {
            if (string.IsNullOrEmpty(base64String))
                return false;

            return string.Equals(base64String.Substring(0, 5), "JVBER", StringComparison.OrdinalIgnoreCase);
        }

        public static string ToCaseInvariantTitle(this string self)
        {
            if (string.IsNullOrWhiteSpace(self))
                return self;

            return CultureInfo.InvariantCulture.TextInfo.ToTitleCase(self);
        }

        public static string GetNameMonth(this DateTime self)
        {
            return self.ToString("MMMM", CultureInfo.CreateSpecificCulture("es")).ToCaseInvariantTitle();
        }

        public static void CreateDirectory(string path)
        {
            if (!Directory.Exists(path)) Directory.CreateDirectory(path);
        }

        public static async Task SaveFile(string folderPath, string fileName, string fileBase64)
        {
            var fullPathFile = System.IO.Path.Combine(folderPath, fileName);
            CreateDirectory(folderPath);

            byte[] fileBytes = Convert.FromBase64String(fileBase64);
            await File.WriteAllBytesAsync(fullPathFile, fileBytes);
        }

        public static string ToUniqueName(this string self)
        {
            if (string.IsNullOrEmpty(self))
                return self;

            int extensionIndex = self.LastIndexOf('.');
            extensionIndex = extensionIndex != -1 ? extensionIndex : self.Length;
            return self.Insert(extensionIndex, $"_{DateTime.Now:yyyyMMddHHmmss}");
        }

        public static string GetHeader(this HttpRequest request, string key)
        {
            return request.Headers.FirstOrDefault(x => x.Key == key).Value.FirstOrDefault();
        }

        public static string ExtractTextFromWord(string path)
        {
            using WordprocessingDocument wordDocument = WordprocessingDocument.Open(path, false);
            MainDocumentPart mainPart = wordDocument.MainDocumentPart;
            string text = mainPart.Document.InnerText;

            return text;
        }

        public static string ExtractText(string path)
        {
            string text;

            TextReader reader = new FilterReader(path);
            using (reader)
                text = reader.ReadToEnd();

            return text;
        }

        public static string ExtractText(string fileBase64, string extension)
        {
            string text;
            byte[] fileBytes = Convert.FromBase64String(fileBase64);
            using MemoryStream stream = new MemoryStream(fileBytes);

            using (TextReader reader = new FilterReader(stream, extension, null))
            {
                text = reader.ReadToEnd();
            }

            return text;
        }

        public static string ExtractTextFromPdf(string path)
        {
            string prevPage = string.Empty;
            var text = string.Empty;
            using PdfReader reader = new PdfReader(new Uri(path));

            for (int page = 1; page <= reader.NumberOfPages; page++)
            {
                ITextExtractionStrategy its2 = new SimpleTextExtractionStrategy();
                var pageText = PdfTextExtractor.GetTextFromPage(reader, page, its2);

                if (prevPage != pageText) text = string.Join(" ", pageText);
                prevPage = pageText;
            }
            reader.Close();

            return text;
        }

        public static string RemoveSpaces(this string text)
        {
            return Regex.Replace(text, @"\s+|\t|\n|\r", " ").Trim();
        }

        public static dynamic ToExpandedObject(object value)
        {
            IDictionary<string, object> dapperRowProperties = value as IDictionary<string, object>;

            IDictionary<string, object> expando = new ExpandoObject();

            foreach (KeyValuePair<string, object> property in dapperRowProperties)
                expando.Add(property.Key, property.Value);

            return (ExpandoObject) expando;
        }

        public static DateTime UnixTimeStampToDateTime(double unixTimeStamp)
        {

            System.DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
            dtDateTime = dtDateTime.AddSeconds(unixTimeStamp).ToUniversalTime();
            return dtDateTime;
        }

    }
}