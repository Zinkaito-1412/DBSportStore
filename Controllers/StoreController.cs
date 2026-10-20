using DBSportStore.Models;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace DBSportStore.Controllers
{
    public class StoreController : Controller
    {
        public ActionResult Index(int? categoryId)
        {
            ProductRepository repository = new ProductRepository();
            List<Product> productsFromDatabase = repository.GetAllProducts();

            using (DBSportStoreEntities db = new DBSportStoreEntities())
            {
                var categories = db.Categories.ToList();

                ViewBag.Categories = categories;
                ViewBag.SelectedCategoryId = categoryId;

                if (categoryId.HasValue)
                {
                    productsFromDatabase = productsFromDatabase
                        .Where(p => p.CategoryID == categoryId.Value)
                        .ToList();
                }

                List<ProductCardViewModel> products =
                    productsFromDatabase.Select(p =>
                    {
                        var category = categories.FirstOrDefault(
                            c => c.CategoryID == p.CategoryID);

                        return new ProductCardViewModel
                        {
                            ProductID = p.ProductID,
                            NamePro = p.ProductName,
                            CategoryName = category != null
                                ? category.CategoryName
                                : "Chưa phân loại",
                            Price = p.Price,
                            ImagePro = p.ImageUrl,
                            ShortDescription = p.Description
                        };
                    }).ToList();

                return View(products);
            }
        }

        public ActionResult AboutStore()
        {
            return View();
        }
    }
}