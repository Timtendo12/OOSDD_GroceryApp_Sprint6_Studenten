using Grocery.Core.Interfaces.Repositories;
using Grocery.Core.Models;
using Grocery.Core.Services;

namespace Grocery.Core.Data.Repositories
{
    public class ProductRepository : DatabaseConnection,IProductRepository
    {
        private readonly List<Product> products = [];
        public ProductRepository()
        {
            CreateTable(@"
CREATE TABLE IF NOT EXISTS Product
(
    [Id] INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT,
    [Name] TEXT NOT NULL,
    [Stock] INTEGER NOT NULL,
    [ShelfLife] TEXT NOT NULL,
    [Price] REAL NOT NULL
)");
            List<string> insertQueries = [
                "INSERT OR IGNORE INTO Product(Name, Stock, ShelfLife, Price) VALUES('Melk', 300, '2025-09-25', 0.95)",
                "INSERT OR IGNORE INTO Product(Name, Stock, ShelfLife, Price) VALUES('Kaas', 100, '2025-09-30', 7.98)",
                "INSERT OR IGNORE INTO Product(Name, Stock, ShelfLife, Price) VALUES('Brood', 400, '2025-09-12', 2.19)",
                "INSERT OR IGNORE INTO Product(Name, Stock, ShelfLife, Price) VALUES('Cornflakes', 0, '2025-12-31', 1.48)"
            ];

            InsertMultipleWithTransaction(insertQueries);

            GetAll();
        }
        public List<Product> GetAll()
        {
            products.Clear();
            var selectQuery = @"SELECT Id, Name, Stock, ShelfLife, Price FROM Product";
            OpenConnection();
            using (var command = new Microsoft.Data.Sqlite.SqliteCommand(selectQuery, Connection))
            {
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        products.Add(new Product(0, "", 0)
                        {
                            Id = reader.GetInt32(0),
                            Name = reader.GetString(1),
                            Stock = reader.GetInt32(2),
                            ShelfLife = DateOnly.Parse(reader.GetString(3)),
                            Price = reader.GetDecimal(4)
                        });
                    }
                }
            }
            return products;
        }

        public Product? Get(int id)
        {
            return products.FirstOrDefault(p => p.Id == id);
        }

        public Product Add(Product item)
        {
            var newId = products.Count > 0 ? products.Max(p => p.Id) + 1 : 1;
            item.Id = newId;
            products.Add(item);
            return Get(item.Id);
        }

        public Product? Delete(Product item)
        {
            throw new NotImplementedException();
        }

        public Product? Update(Product item)
        {
            Product? product = products.FirstOrDefault(p => p.Id == item.Id);
            if (product == null) return null;
            product.Id = item.Id;
            return product;
        }
    }
}
