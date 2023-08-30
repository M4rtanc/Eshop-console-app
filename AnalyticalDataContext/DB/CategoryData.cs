using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HW02.AnalyticalDataContext.DB
{
    public class CategoryData
    {
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public int CategoryCount { get; set; }
        public CategoryData(int categoryId, string categoryName)
        {
            CategoryId = categoryId;
            CategoryName = categoryName;
            CategoryCount = 0;
        }
    }
}
