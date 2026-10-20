using System.Linq;
using System.Web.Mvc;
using DBSportStore.Models;

namespace DBSportStore.Controllers
{
    public class ProductController : Controller
    {
        private DBSportStoreEntities db = new DBSportStoreEntities();

        public ActionResult Index()
        {
            var products = db.Products.ToList();
            return View(products);
        }
    }
}