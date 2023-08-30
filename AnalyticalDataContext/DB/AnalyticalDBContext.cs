using HW02.Helpers;
using System.Collections.Generic;
using System.Text.Json;

namespace HW02.AnalyticalDataContext.DB
{
    public class AnalyticalDBContext
    {
        private readonly string[] _paths = { "..", "..", "..", "a.xml" };
        private readonly string _filePath;

        public AnalyticalDBContext()
        {
            _filePath = Path.Combine(_paths);
            FileHelper.CreateFile(_filePath);
        }

        public void SaveAnalyticalData(List<CategoryData> log)
        {
            string jsonString = JsonSerializer.Serialize(log);
            using StreamWriter outputFile = new StreamWriter(_filePath);
            outputFile.WriteLine(jsonString);
        }

        public List<object> ReadAnalyticalData()
        {
            string? line;
            using (StreamReader inputFile = new StreamReader(_filePath))
            {
                line = inputFile.ReadLine();
            }

            if (line == null)
            {
                return new List<object>();
            }

            var model = JsonSerializer.Deserialize<List<object>>(line);
            return model;
        }
    }
}
