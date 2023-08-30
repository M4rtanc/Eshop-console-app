using HW02.IO.Types;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace HW02.IO
{
    public class Command
    {
        public OperationType Operation;
        public EntityType Entity;
        public int EntityId;
        public string Name;
        public int CategoryId;
        public int Price;
        public string Timestamp;
        public ResultType Result;
        public string FailureMessage;

        public Command(string line)
        {
            Regex r = new Regex(@"^\s*(add-product|delete-product|list-products|add-category|delete-category|list-categories|get-products-by-category)\s*");

            string command = r.Matches(line)[0].Groups[1].Value;
            
            switch (command)
            {
                case "add-product":
                    Operation = OperationType.Add;
                    Entity = EntityType.Product;

                    Regex addProduct = new Regex(@"^\s*(add-product)\s+([a-žA-Ž]+\d*)\s+(\d+)\s+(\d+)\s*$");
                    GroupCollection parameters = addProduct.Matches(line)[0].Groups;

                    Name = parameters[2].Value;
                    CategoryId = Convert.ToInt32(parameters[3].Value);
                    EntityId = -1;
                    Price = Convert.ToInt32(parameters[4].Value);
                    break;

                case "delete-product":
                    Operation = OperationType.Delete;
                    Entity = EntityType.Product;

                    Regex deleteProduct = new Regex(@"^\s*(delete-product)\s+(\d+)\s*$");
                    EntityId = Convert.ToInt32(deleteProduct.Matches(line)[0].Groups[2].Value);

                    Name = "";
                    CategoryId = -1;
                    Price = -1;
                    break;

                case "list-products":
                    Operation = OperationType.Get;
                    Entity = EntityType.Product;
                    Name = "";
                    CategoryId = -1;
                    EntityId = -1;
                    Price = -1;
                    break;      

                case "add-category":
                    Operation = OperationType.Add;
                    Entity = EntityType.Category;

                    Regex addCategory = new Regex(@"^\s*(add-category)\s+([a-žA-Ž]+)\d*\s*$");
                    Name = addCategory.Matches(line)[0].Groups[2].Value;

                    CategoryId = -1;
                    EntityId = -1;
                    Price = -1;
                    break;

                case "delete-category":
                    Operation = OperationType.Delete;
                    Entity = EntityType.Category;
                    Name = "";

                    Regex deleteCategory = new Regex(@"^\s*(delete-category)\s+(\d+)\s*$");
                    EntityId = Convert.ToInt32(deleteCategory.Matches(line)[0].Groups[2].Value);
                    CategoryId = -1;

                    Price = -1;
                    break;

                case "list-categories":
                    Operation = OperationType.Get;
                    Entity = EntityType.Category;
                    Name = "";
                    CategoryId = -1;
                    EntityId = -1;
                    Price = -1;
                    break;

                case "get-products-by-category":
                    Operation = OperationType.Get;
                    Entity = EntityType.Product;
                    Name = "";

                    Regex getProductsByCat = new Regex(@"^\s*(get-products-by-category)\s+(\d+)\s*$");
                    CategoryId = Convert.ToInt32(getProductsByCat.Matches(line)[0].Groups[2].Value);

                    EntityId = -1;
                    Price = -1;
                    break;

                default:
                    break;
            }
            Timestamp = DateTime.Now.ToString(CultureInfo.CreateSpecificCulture("fr-FR"));
            Result = ResultType.Failure;
            FailureMessage = "";
        }

        public string ToLogString()
        {
            if (this.Result == ResultType.Failure)
            {
                return String.Format("[{0}] {1}; {2}; {3}; {4}", this.Timestamp, this.Operation, this.Entity, this.Result, this.FailureMessage);
            }
            else if (this.Operation == OperationType.Get)
            {
                return String.Format("[{0}] {1}; {2}; {3}", this.Timestamp, this.Operation, this.Entity, this.Result);
            }
            else if (this.Entity == EntityType.Category)
            {
                return String.Format("[{0}] {1}; {2}; {3}; {4}; {5}", this.Timestamp, this.Operation, this.Entity, this.Result, this.EntityId, this.Name);
            }
            else
            {
                return String.Format("[{0}] {1}; {2}; {3}; {4}; {5}; {6}", this.Timestamp, this.Operation, this.Entity, this.Result, this.EntityId, this.Name, this.CategoryId);
            }
        }
    }
}
