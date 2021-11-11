using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace Products.Api.Models
{
    public class Products
    {
        public IEnumerable<Product> Items { get; private set; }

        public Products()
        {
            LoadProducts(null);
        }

        public Products(string name)
        {
            LoadProducts($"where lower(name) like '%{name.ToLower()}%'");
        }

        public Products(IEnumerable<Product> items)
        {
            Items = items;
        }

        private void LoadProducts(string where)
        {
            /*Items = new List<Product>();
            var conn = Helpers.NewConnection();
            conn.Open();
            var cmd = conn.CreateCommand();
            cmd.CommandText = $"select id from Products {where}";

            var rdr = cmd.ExecuteReader();
            while (rdr.Read())
            {
                var id = Guid.Parse(rdr.GetString(0));
                Items.Add(new Product(id));
            }*/
            throw new NotImplementedException();
        }
    }

    public class ProductOptions
    {
        public List<ProductOption> Items { get; private set; }

        public ProductOptions(IEnumerable<ProductOption> items)
        {
            Items = items?.ToList() ?? new List<ProductOption>();
        }

        public ProductOptions(Guid productId)
        {
            LoadProductOptions($"where productid = '{productId}' collate nocase");

            throw new NotImplementedException("remove this");
        }

        private void LoadProductOptions(string where)
        {
            throw new NotImplementedException("remove this");
        }
    }

    public class ProductOption
    {
        public Guid Id { get; set; }

        public Guid ProductId { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public ProductOption(Guid id, Guid productId, string name, string description)
        {
            Id = id;
            ProductId = productId;
            Name = name;
            Description = description;
        }

        public ProductOption(Guid id)
        {
           
        }

        public void Save()
        {
            /*var conn = Helpers.NewConnection();
            conn.Open();
            var cmd = conn.CreateCommand();

            cmd.CommandText = IsNew
                ? $"insert into productoptions (id, productid, name, description) values ('{Id}', '{ProductId}', '{Name}', '{Description}')"
                : $"update productoptions set name = '{Name}', description = '{Description}' where id = '{Id}' collate nocase";

            cmd.ExecuteNonQuery();*/

            throw new NotImplementedException();
        }

        public void Delete()
        {
            var conn = Helpers.NewConnection();
            conn.Open();
            var cmd = conn.CreateCommand();
            cmd.CommandText = $"delete from productoptions where id = '{Id}' collate nocase";
            cmd.ExecuteReader();
        }
    }
}