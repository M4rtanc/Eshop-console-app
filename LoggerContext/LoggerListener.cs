using HW02.Events;
using HW02.LoggerContext.DB;

namespace HW02
{
    public class LoggerListener
    {
        private LoggerDBContext _DBContext;
        public LoggerListener(LoggerDBContext loggerDBContext)
        {
            _DBContext = loggerDBContext;
        }

        public void CatchCommand(object sender, CommandEvent e)
        {
            try
            {
               _DBContext.WriteLog(e.Command);
            }
            catch (IOException ex)
            {
                throw ex;
            }
        }
    }
}
