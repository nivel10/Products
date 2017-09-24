using System.Collections.Generic;

namespace Products.Models
{
    public class Category
    {
		public int CategoryId { get; set; }

		public string Description { get; set; }

        public List<Product> Products { get; set; }

        public override string ToString()
        {
            return Description;
        }
    }
}
