using Grocery.Core.Interfaces.Repositories;
using Grocery.Core.Models;
using Microsoft.Data.Sqlite;

namespace Grocery.Core.Data.Repositories
{
    public class GroceryListItemsRepository : DatabaseConnection, IGroceryListItemsRepository
    {
        private readonly List<GroceryListItem> groceryListItems = [];

        public GroceryListItemsRepository()
        {
            //ISO 8601 format: date.ToString("o", CultureInfo.InvariantCulture)
            CreateTable(@"
CREATE TABLE IF NOT EXISTS GroceryListItem
(
    [Id] INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT,
    [GroceryListId] INTEGER NOT NULL,
    [ProductId] INTEGER NOT NULL,
    [Quantity] INTEGER NOT NULL,
    FOREIGN KEY (GroceryListId) REFERENCES GroceryList(Id),
    FOREIGN KEY (ProductId) REFERENCES Product(Id)
)
");
            List<string> insertQueries = [
                "INSERT OR IGNORE INTO GroceryListItem(GroceryListId, ProductId, Quantity) VALUES(1, 1, 3)",
                "INSERT OR IGNORE INTO GroceryListItem(GroceryListId, ProductId, Quantity) VALUES(1, 2, 1)",
                "INSERT OR IGNORE INTO GroceryListItem(GroceryListId, ProductId, Quantity) VALUES(1, 3, 4)",
                "INSERT OR IGNORE INTO GroceryListItem(GroceryListId, ProductId, Quantity) VALUES(2, 1, 2)",
                "INSERT OR IGNORE INTO GroceryListItem(GroceryListId, ProductId, Quantity) VALUES(2, 2, 5)"
            ];
            InsertMultipleWithTransaction(insertQueries);
            groceryListItems = GetAll();
        }

        public List<GroceryListItem> GetAll()
        {
            groceryListItems.Clear();
            var selectQuery = @"
SELECT Id, GroceryListId, ProductId, Quantity FROM GroceryListItem";
            OpenConnection();
            using (var command = new SqliteCommand(selectQuery, Connection))
            {
                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    var id = reader.GetInt32(0);
                    var groceryListId = reader.GetInt32(1);
                    var productId = reader.GetInt32(2);
                    var quantity = reader.GetInt32(3);
                    groceryListItems.Add(new GroceryListItem(id, groceryListId, productId, quantity));
                }
            }
            CloseConnection();
            return groceryListItems;
        }

        public List<GroceryListItem> GetAllOnGroceryListId(int id)
        {
            return groceryListItems.Where(g => g.GroceryListId == id).ToList();
        }

        public GroceryListItem Add(GroceryListItem item)
        {
            var newId = groceryListItems.Count > 0 ? groceryListItems.Max(g => g.Id) + 1 : 1;
            item.Id = newId;
            groceryListItems.Add(item);
            return Get(item.Id);
        }

        public GroceryListItem? Delete(GroceryListItem item)
        {
            throw new NotImplementedException();
        }

        public GroceryListItem? Get(int id)
        {
            return groceryListItems.FirstOrDefault(g => g.Id == id);
        }

        public GroceryListItem? Update(GroceryListItem item)
        {
            GroceryListItem? listItem = groceryListItems.FirstOrDefault(i => i.Id == item.Id);
            listItem = item;
            return listItem;
        }
    }
}
