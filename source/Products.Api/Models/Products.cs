using System.Collections.Generic;
using Newtonsoft.Json;

namespace Products.Api.Models
{
    public class Products
    {
        public IEnumerable<Product> Items { get;}

        public Products(IEnumerable<Product> items)
        {
            Items = items;
        }
    }
}