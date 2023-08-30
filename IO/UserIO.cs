using HW02.Entities;
using HW02.Events;
using System;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace HW02.IO
{
    public class UserIO
    {
        public UserIO() { }

        public void writeHelp(string message = "")
        {
            if (message != "")
            {
                Console.WriteLine(message);
            }
            Console.WriteLine("USAGE:\n" +
                "add-product [Name] [CategoryId] [Price]\n" +
                "delete-product [ProductId]\n" +
                "list-products\n" +
                "add-category [Name]\n" +
                "delete-category [CategoryId]\n" +
                "list-categories\n" +
                "get-products-by-category [CategoryId]\n");
        }

        private bool formatCheck(string line)
        {
            Regex addProduct = new Regex(@"^\s*(add-product)\s+[a-žA-Ž]+\d*\s+\d+\s+(\d+|\d+\.\d+)\s*$");
            Regex deleteProduct = new Regex(@"^\s*(delete-product)\s+\d+\s*$");
            Regex listProducts = new Regex(@"^\s*(list-products)\s*$");
            Regex addCategory = new Regex(@"^\s*(add-category)\s+[a-žA-Ž]+\d*\s*$");
            Regex deleteCategory = new Regex(@"^\s*(delete-category)\s+\d+\s*$");
            Regex listCategories = new Regex(@"^\s*(list-categories)\s*$");
            Regex getProductsByCat = new Regex(@"^\s*(get-products-by-category)\s+\d+\s*$");

            return addProduct.IsMatch(line) || deleteProduct.IsMatch(line) || listProducts.IsMatch(line) || addCategory.IsMatch(line) ||
                deleteCategory.IsMatch(line) || listCategories.IsMatch(line) || getProductsByCat.IsMatch(line);
        }
        
        public void ReadInput()
        {
            string inp;
            while ((inp = Console.ReadLine()) != null)
            {
                if (!this.formatCheck(inp))
                {
                    this.writeHelp("Invalid format.");
                    continue;
                }
                Command command = new Command(inp);
                CommandEvent commandEvent = new CommandEvent();
                commandEvent.Command = command;
                this.OnCommand(commandEvent);
            }
        }

        public void WriteProducts(List<Product> products, int categoryId)
        {
            int idLength = 2;
            int nameLength = 4;
            int categoryIdLength = 10;
            int priceLength = 5;

            foreach (Product product in products)
            {
                if (categoryId == -1 || categoryId == product.CategoryId)
                {
                    idLength = Math.Max(idLength, product.Id.ToString().Length);
                    nameLength = Math.Max(nameLength, product.Name.Length);
                    categoryIdLength = Math.Max(categoryIdLength, product.CategoryId.ToString().Length);
                    priceLength = Math.Max(priceLength, product.Price.ToString().Length);
                }
            }

            Console.WriteLine("{0} | {1} | {2} | {3}", "Id".PadRight(idLength), "Name".PadRight(nameLength), "CategoryId".PadRight(categoryIdLength), "Price".PadRight(priceLength));
            Console.WriteLine("".PadRight(idLength + nameLength + categoryIdLength + priceLength + 12, '-'));

            foreach (Product product in products)
            {
                if (categoryId == -1 || categoryId == product.CategoryId)
                {
                    Console.WriteLine("{0} | {1} | {2} | {3}", product.Id.ToString().PadRight(idLength), product.Name.PadRight(nameLength), product.CategoryId.ToString().PadRight(categoryIdLength), product.Price.ToString().PadRight(priceLength));
                }
            }
        }

        public void WriteCategories(List<Category> categories)
        {
            int idLength = 2;
            int nameLength = 4;

            foreach (Category category in categories)
            {
                idLength = Math.Max(idLength, category.Id.ToString().Length);
                nameLength = Math.Max(nameLength, category.Name.Length);
            }

            Console.WriteLine("{0} | {1}", "Id".PadRight(idLength), "Name".PadRight(nameLength));
            Console.WriteLine("".PadRight(idLength + nameLength + 5, '-'));

            foreach (Category category in categories)
            {
                Console.WriteLine("{0} | {1}", category.Id.ToString().PadRight(idLength), category.Name.PadRight(nameLength));
            }
        }

        public virtual void OnCommand(CommandEvent e)
        {
            EventHandler<CommandEvent> handler = Command;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        public event EventHandler<CommandEvent> Command;
    }
}
