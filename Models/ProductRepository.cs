using System.Collections.Generic;
using System.Linq;

namespace DBSportStore.Models
{
    public class ProductRepository
    {
        public List<Product> GetAllProducts()
        {
            using (var db = new DBSportStoreEntities())
            {
                return db.Products
                         .Where(p => p.IsActive)
                         .ToList();
            }
        }
    }
}