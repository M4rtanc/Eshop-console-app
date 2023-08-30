using HW02.BussinessContext;
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
    public class ProductService
    {
        private List<Product> _productList;
        private int _lastId;
        private UserIO _userOutput;
        private ProductDBContext _DBContext;
        private CategoryService _categories;

        public ProductService(UserIO userOutput, ProductDBContext prodDBContext, CategoryService categories)
        {
            _productList = new List<Product>();
            _lastId = -1;
            _userOutput = userOutput;
            _DBContext = prodDBContext;
            _categories = categories;
        }

        private bool IsIdValid(int id)
        {
            foreach (Product product in _productList)
            {
                if (product.Id == id)
                {
                    return true;
                }
            }
            return false;
        }

        private bool IsCategoryIdValid(int id)
        {
            return _categories.IsIdValid(id);
        }

        private void AddProduct(Command command)
        {
            if (!IsCategoryIdValid(command.CategoryId))
            {
                command.Result = ResultType.Failure;
                command.FailureMessage = "Invalid CategoryId";
                _userOutput.writeHelp(command.FailureMessage);
                return;
            }
            Product newProduct = new Product(_lastId + 1, command.Name, command.CategoryId, command.Price);
            _productList.Add(newProduct);
            command.EntityId = _lastId;
            command.Result = ResultType.Success;
            _lastId += 1;
        }

        private void DeleteProduct(Command command)
        {
            if (!IsIdValid(command.EntityId))
            {
                command.Result = ResultType.Failure;
                command.FailureMessage = "Invalid ProductId";
                _userOutput.writeHelp(command.FailureMessage);
                return;
            }
            foreach (Product product in _productList)
            {
                if (product.Id == command.EntityId)
                {
                    command.Name = product.Name;
                    command.CategoryId = product.CategoryId;
                }
            }
            _productList.RemoveAll(x => x.Id == command.EntityId);
            command.Result = ResultType.Success;
        }

        private void CascadeDelete(int categoryId)
        {
            _productList.RemoveAll(x => x.CategoryId == categoryId);
        }

        private void getProducts(Command command)
        {
            if (command.CategoryId != -1 && !IsCategoryIdValid(command.CategoryId))
            {
                command.Result = ResultType.Failure;
                command.FailureMessage = "Invalid CategoryId";
                _userOutput.writeHelp(command.FailureMessage);
                return;
            }
            _userOutput.WriteProducts(_productList, command.CategoryId);
            command.Result = ResultType.Success;
        }

        public void ExecuteCommand(object sender, CommandEvent e)
        {
            if (e.Command.Entity != EntityType.Product)
            {
                if (e.Command.Operation == OperationType.Delete)
                {
                    CascadeDelete(e.Command.EntityId);
                }
                return;
            }

            switch (e.Command.Operation)
            {
                case OperationType.Add:
                    AddProduct(e.Command);
                    e.Command.EntityId = _lastId;
                    break;
                case OperationType.Delete:
                    DeleteProduct(e.Command);
                    break;
                case OperationType.Get:
                    getProducts(e.Command);
                    break;
                default:
                    break;
            }
            try
            {
                _DBContext.SaveProducts(_productList);
            }
            catch (IOException ex)
            {
                e.Command.Result = ResultType.Failure;
                e.Command.FailureMessage = ex.Message;
                _userOutput.writeHelp(e.Command.FailureMessage);
                _productList = _DBContext.ReadProducts();
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
