using DBSportStore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace DBSportStore.Controllers
{
    public class StoreController : Controller
    {
        public ActionResult Index(
            int? categoryId,
            string keyword,
            decimal? minPrice,
            decimal? maxPrice,
            string sortOrder,
            int page = 1)
        {
            int pageSize = 6;

            if (page < 1)
            {
                page = 1;
            }

            ProductRepository repository = new ProductRepository();
            List<Product> productsFromDatabase =
                repository.GetAllProducts();

            using (DBSportStoreEntities db = new DBSportStoreEntities())
            {
                var categories = db.Categories.ToList();

                ViewBag.Categories = categories;
                ViewBag.SelectedCategoryId = categoryId;
                ViewBag.Keyword = keyword;
                ViewBag.MinPrice = minPrice;
                ViewBag.MaxPrice = maxPrice;
                ViewBag.SortOrder = sortOrder;

                // Lọc theo danh mục
                if (categoryId.HasValue)
                {
                    productsFromDatabase = productsFromDatabase
                        .Where(p => p.CategoryID == categoryId.Value)
                        .ToList();
                }

                // Tìm theo tên hoặc mô tả
                if (!string.IsNullOrWhiteSpace(keyword))
                {
                    string searchKeyword = keyword.Trim();

                    productsFromDatabase = productsFromDatabase
                        .Where(p =>
                            (!string.IsNullOrWhiteSpace(p.ProductName) &&
                             p.ProductName.IndexOf(
                                 searchKeyword,
                                 StringComparison.OrdinalIgnoreCase) >= 0)
                            ||
                            (!string.IsNullOrWhiteSpace(p.Description) &&
                             p.Description.IndexOf(
                                 searchKeyword,
                                 StringComparison.OrdinalIgnoreCase) >= 0))
                        .ToList();
                }

                // Lọc theo giá thấp nhất
                if (minPrice.HasValue)
                {
                    productsFromDatabase = productsFromDatabase
                        .Where(p => p.Price >= minPrice.Value)
                        .ToList();
                }

                // Lọc theo giá cao nhất
                if (maxPrice.HasValue)
                {
                    productsFromDatabase = productsFromDatabase
                        .Where(p => p.Price <= maxPrice.Value)
                        .ToList();
                }

                // Bài 2: Sắp xếp theo giá
                if (sortOrder == "price_asc")
                {
                    productsFromDatabase = productsFromDatabase
                        .OrderBy(p => p.Price)
                        .ToList();
                }
                else if (sortOrder == "price_desc")
                {
                    productsFromDatabase = productsFromDatabase
                        .OrderByDescending(p => p.Price)
                        .ToList();
                }

                // Đếm sản phẩm sau khi lọc
                int totalProducts = productsFromDatabase.Count;

                int totalPages = (int)Math.Ceiling(
                    totalProducts / (double)pageSize);

                if (totalPages == 0)
                {
                    totalPages = 1;
                }

                if (page > totalPages)
                {
                    page = totalPages;
                }

                ViewBag.CurrentPage = page;
                ViewBag.TotalPages = totalPages;
                ViewBag.TotalProducts = totalProducts;

                // Lấy sản phẩm của trang hiện tại
                List<Product> productsOnPage = productsFromDatabase
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .ToList();

                // Chuyển sang ViewModel
                List<ProductCardViewModel> products =
                    productsOnPage.Select(p =>
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