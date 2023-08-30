using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HW02.Entities
{
    public struct Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int CategoryId { get; set; }
        public decimal Price { get; set; }

        public Product(int id, string name, int categoryId, decimal price)
        {
            Id = id;
            Name = name;
            CategoryId = categoryId;
            Price = price;
        }
    }
}
