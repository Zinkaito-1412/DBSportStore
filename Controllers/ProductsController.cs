using System;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using DBSportStore.Models;

namespace DBSportStore.Controllers
{
    public class ProductsController : Controller
    {
        private DBSportStoreEntities db = new DBSportStoreEntities();

        public ActionResult Index()
        {
            var products = db.Products.Include(p => p.Category);
            return View(products.ToList());
        }

        public ActionResult Details(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            Product product = db.Products.Find(id);

            if (product == null)
                return HttpNotFound();

            return View(product);
        }

        public ActionResult Create()
        {
            ViewBag.CategoryID = new SelectList(
                db.Categories, "CategoryID", "CategoryName");

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(
            [Bind(Include = "ProductID,ProductName,Description,CategoryID,Price,StockQuantity,IsActive")] Product product,
            HttpPostedFileBase imageFile)
        {
            if (imageFile == null || imageFile.ContentLength == 0)
            {
                ModelState.AddModelError("ImageUrl", "Vui lòng chọn hình ảnh.");
            }
            else
            {
                string extension = Path.GetExtension(imageFile.FileName).ToLowerInvariant();

                if (extension != ".jpg" &&
                    extension != ".jpeg" &&
                    extension != ".png")
                {
                    ModelState.AddModelError(
                        "ImageUrl",
                        "Chỉ chấp nhận ảnh JPG, JPEG hoặc PNG.");
                }
                else if (imageFile.ContentLength > 2 * 1024 * 1024)
                {
                    ModelState.AddModelError(
                        "ImageUrl",
                        "Dung lượng ảnh tối đa là 2 MB.");
                }
            }

            if (ModelState.IsValid)
            {
                string folder = Server.MapPath("~/Content/Images");

                if (!Directory.Exists(folder))
                    Directory.CreateDirectory(folder);

                string fileName = Path.GetFileName(imageFile.FileName);
                string name = Path.GetFileNameWithoutExtension(fileName);
                string extensionName = Path.GetExtension(fileName);
                string filePath = Path.Combine(folder, fileName);
                int count = 1;

                while (System.IO.File.Exists(filePath))
                {
                    fileName = name + "-" + count + extensionName;
                    filePath = Path.Combine(folder, fileName);
                    count++;
                }

                imageFile.SaveAs(filePath);
                product.ImageUrl = fileName;

                db.Products.Add(product);
                db.SaveChanges();

                return RedirectToAction("Index");
            }

            ViewBag.CategoryID = new SelectList(
                db.Categories,
                "CategoryID",
                "CategoryName",
                product.CategoryID);

            return View(product);
        }

        public ActionResult Edit(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            Product product = db.Products.Find(id);

            if (product == null)
                return HttpNotFound();

            ViewBag.CategoryID = new SelectList(
                db.Categories,
                "CategoryID",
                "CategoryName",
                product.CategoryID);

            return View(product);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(
            [Bind(Include = "ProductID,ProductName,Description,CategoryID,Price,StockQuantity,IsActive")] Product product,
            HttpPostedFileBase imageFile)
        {
            Product existingProduct = db.Products.Find(product.ProductID);

            if (existingProduct == null)
                return HttpNotFound();

            if (imageFile != null && imageFile.ContentLength > 0)
            {
                string extension = Path.GetExtension(imageFile.FileName).ToLowerInvariant();

                if (extension != ".jpg" &&
                    extension != ".jpeg" &&
                    extension != ".png")
                {
                    ModelState.AddModelError(
                        "ImageUrl",
                        "Chỉ chấp nhận ảnh JPG, JPEG hoặc PNG.");
                }
                else if (imageFile.ContentLength > 2 * 1024 * 1024)
                {
                    ModelState.AddModelError(
                        "ImageUrl",
                        "Dung lượng ảnh tối đa là 2 MB.");
                }
            }

            if (ModelState.IsValid)
            {
                existingProduct.ProductName = product.ProductName;
                existingProduct.Description = product.Description;
                existingProduct.CategoryID = product.CategoryID;
                existingProduct.Price = product.Price;
                existingProduct.StockQuantity = product.StockQuantity;
                existingProduct.IsActive = product.IsActive;

                if (imageFile != null && imageFile.ContentLength > 0)
                {
                    string folder = Server.MapPath("~/Content/Images");

                    if (!Directory.Exists(folder))
                        Directory.CreateDirectory(folder);

                    string fileName = Path.GetFileName(imageFile.FileName);
                    string name = Path.GetFileNameWithoutExtension(fileName);
                    string extensionName = Path.GetExtension(fileName);
                    string filePath = Path.Combine(folder, fileName);
                    int count = 1;

                    while (System.IO.File.Exists(filePath))
                    {
                        fileName = name + "-" + count + extensionName;
                        filePath = Path.Combine(folder, fileName);
                        count++;
                    }

                    imageFile.SaveAs(filePath);
                    existingProduct.ImageUrl = fileName;
                }

                db.SaveChanges();
                return RedirectToAction("Index");
            }

            product.ImageUrl = existingProduct.ImageUrl;

            ViewBag.CategoryID = new SelectList(
                db.Categories,
                "CategoryID",
                "CategoryName",
                product.CategoryID);

            return View(product);
        }

        public ActionResult Delete(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            Product product = db.Products.Find(id);

            if (product == null)
                return HttpNotFound();

            return View(product);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Product product = db.Products.Find(id);

            if (product != null)
            {
                db.Products.Remove(product);
                db.SaveChanges();
            }

            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
                db.Dispose();

            base.Dispose(disposing);
        }
    }
}