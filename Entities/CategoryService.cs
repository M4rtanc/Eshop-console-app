using HW02.BussinessContext.FileDatabase;
using HW02.Events;
using HW02.IO;
using HW02.IO.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HW02.Entities
{
    public class CategoryService
    {
        private List<Category> _categoryList;
        private int _lastId;
        private UserIO _userOutput;
        private CategoryDBContext _DBContext;

        public CategoryService(UserIO userOutput, CategoryDBContext catDBContext)
        {
            _categoryList = new List<Category>();
            _lastId = -1;
            _userOutput = userOutput;
            _DBContext = catDBContext;
        }

        public bool IsIdValid(int? id)
        {
            foreach (Category product in _categoryList)
            {
                if (product.Id == id)
                {
                    return true;
                }
            }
            return false;
        }

        private void addCategory(Command command)
        {
            Category newCategory = new Category(_lastId + 1, command.Name);
            _lastId += 1;
            _categoryList.Add(newCategory);
            command.EntityId = _lastId;
            command.Result = ResultType.Success;
        }

        private void deleteCategory(Command command)
        {
            if (!IsIdValid(command.EntityId))
            {
                command.Result = ResultType.Failure;
                command.FailureMessage = "Invalid CategoryId";
                _userOutput.writeHelp(command.FailureMessage);
                return;
            }
            foreach (Category category in _categoryList)
            {
                if (category.Id == command.EntityId)
                {
                    command.Name = category.Name;
                }
            }
            _categoryList.RemoveAll(x => x.Id == command.EntityId);
            command.Result = ResultType.Success;
        }

        private void listCategory(Command command)
        {
            _userOutput.WriteCategories(_categoryList);
            command.Result = ResultType.Success;
        }

        public void ExecuteCommand(object sender, CommandEvent e)
        {
            if (e.Command.Entity != EntityType.Category)
            {
                return;
            }

            switch (e.Command.Operation)
            {
                case OperationType.Add:
                    addCategory(e.Command);
                    break;
                case OperationType.Delete:
                    deleteCategory(e.Command);
                    break;
                case OperationType.Get:
                    listCategory(e.Command);
                    break;
                default:
                    break;
            }
            try
            {
                _DBContext.SaveCategories(_categoryList);
            }
            catch (IOException ex)
            {
                e.Command.Result = ResultType.Failure;
                e.Command.FailureMessage = ex.Message;
                _userOutput.writeHelp(e.Command.FailureMessage);
                _categoryList = _DBContext.ReadCategories();
            }
            OnExecution(e);
        }

        public virtual void OnExecution(CommandEvent e)
        {
            EventHandler<CommandEvent> handler = ExecutedCommand;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        public event EventHandler<CommandEvent> ExecutedCommand;
    }
}
