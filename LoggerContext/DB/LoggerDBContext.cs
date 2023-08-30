using HW02.Helpers;
using HW02.IO;

namespace HW02.LoggerContext.DB
{
    public class LoggerDBContext
    {

        private readonly string[] _paths = { "..", "..", "..", "LoggerContext", "DB", "Storage", "Log.txt" };
        private readonly string _filePath;

        public LoggerDBContext()
        {
            _filePath = Path.Combine(_paths);
            FileHelper.CreateFile(_filePath);
        }


        public void WriteLog(Command log)
        {
            using StreamWriter sw = File.AppendText(_filePath);
            sw.WriteLine(log.ToLogString());
        }
    }
}
