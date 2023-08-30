using HW02.AnalyticalDataContext.DB;
using HW02.Events;
using HW02.IO;
using HW02.IO.Types;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http.Headers;
using System.Xml.Linq;

namespace HW02.AnalyticalDataContext
{
    public class AnalyticalDataListener
    {
        private List<CategoryData> _categoryDataList { get; set; }
        private AnalyticalDBContext _DBContext;
        public AnalyticalDataListener(AnalyticalDBContext analyticalDBContext)
        {
            _categoryDataList = new List<CategoryData>();
            _DBContext = analyticalDBContext;
        }

        private void executeCategoryCommand(Command command)
        {
            switch (command.Operation)
            {
                case OperationType.Add:
                    _categoryDataList.Add(new CategoryData(command.EntityId, command.Name));
                    break;
                case OperationType.Delete:
                    _categoryDataList.RemoveAll(x => x.CategoryId == command.EntityId);
                    break;
                default:
                    break;
            }
        }

        private void executeProductCommand(Command command)
        {
            switch (command.Operation)
            {
                case OperationType.Add:
                    _categoryDataList.Find(c => c.CategoryId == command.CategoryId).CategoryCount++;
                    break;
                case OperationType.Delete:
                    _categoryDataList.Find(c => c.CategoryId == command.CategoryId).CategoryCount--;
                    break;
                default:
                    break;
            }
        }

        public void CatchCommand(object sender, CommandEvent e)
        {
            if (e.Command.Result == ResultType.Failure)
            {
                return;
            }
            switch (e.Command.Entity)
            {
                case EntityType.Category:
                    executeCategoryCommand(e.Command);
                    break;
                case EntityType.Product:
                    executeProductCommand(e.Command);
                    break;
                default:
                    break;
            }
            try
            {
                _DBContext.SaveAnalyticalData(_categoryDataList);
            }
            catch (IOException ex)
            {
                throw ex;
            }
        }
    }
}
