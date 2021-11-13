using System.Collections.Generic;
using System.Linq;

namespace Products.Api.Models
{
    public class ProductOptions
    {
        public List<ProductOption> Items { get; private set; }

        public ProductOptions(IEnumerable<ProductOption> items)
        {
            Items = items?.ToList() ?? new List<ProductOption>();
        }
    }
}