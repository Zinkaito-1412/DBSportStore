using DBSportStore.Models;
using System.Collections.Generic;
using System.Web.Mvc;

namespace DBSportStore.Controllers
{
    public class StoreController : Controller
    {
        public ActionResult Index()
        {
            var products = new List<ProductCardViewModel>
            {
                new ProductCardViewModel
                {
                    ProductID = 1,
                    NamePro = "Nike Air Zoom",
                    CategoryName = "Giày thể thao",
                    Price = 1290000,
                    ImagePro = "shoe-01.jpg"
                },
                new ProductCardViewModel
                {
                    ProductID = 2,
                    NamePro = "Adidas Training Tee",
                    CategoryName = "Áo quần thi đấu",
                    Price = 590000,
                    ImagePro = "shirt-01.jpg",
                    ShortDescription = "Giày thể thao nhẹ, phù hợp cho chạy bộ."
                },
                new ProductCardViewModel
                {
                    ProductID = 3,
                    NamePro = "Gym Performance Set",
                    CategoryName = "Dụng cụ thể thao",
                    Price = 890000,
                    ImagePro = "gym-01.jpg",
                    ShortDescription = "Chất lượng cao"
                },
                new ProductCardViewModel
                {
                    ProductID = 4,
                    NamePro = "Running Shoes Pro",
                    CategoryName = "Giày thể thao",
                    Price = 1490000,
                    ImagePro = "shoe-02.jpg",
                    ShortDescription = "Giày thể thao nhẹ, phù hợp cho chạy bộ."
                },
                new ProductCardViewModel
                {
                 ProductID = 5,
                    NamePro = "Đồ tập gym nam",
                    CategoryName = "Training Sports Shorts",
                    Price = 200000,
                    ImagePro = "Gym outfit.jpg",
                    ShortDescription = "Thoải mái, co giãn tốt, phù hợp khi tập gym."
                },
                new ProductCardViewModel
                {
                    ProductID = 6,
                    NamePro = "Giày Sneaker Nam",
                    CategoryName = "Gym Training Shoes",
                    Price = 990000,
                    ImagePro = "shoe-03.jpg",
                    ShortDescription = "Giày thể thao nhẹ, phù hợp cho chạy bộ."
                },
                new ProductCardViewModel
                {
                    ProductID = 7,
                    NamePro = "Bình nước thể thao",
                    CategoryName = "Phụ kiện thể thao",
                    Price = 120000,
                    ImagePro = "gym-02.jpg",
                    ShortDescription = "Bình nước tiện lợi khi tập luyện thể thao."
                }

            };


            return View(products);
        }
        public ActionResult AboutStore()
        {
            return View();
        }
    }
}