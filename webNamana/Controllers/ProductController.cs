using System;
using System.Data.Entity.Validation;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using webNamana.Domain.Entities.Product;
using webNamana.Helpers;
using webNamana.BusinessLogic;
using webNamana.BusinessLogic.Interfaces;

namespace webNamana.Web.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductService _product;

        public ProductController()
        {
            var bl = new BusinessLogic.BusinessLogic();
            _product = bl.GetProductService(); // инициализация бизнес-логики
        }

        // GET: /Product
        public ActionResult Index()
        {
            var products = _product.GetAllProducts();
            return View(products);
        }

        // GET: /Product/Details/5
        public ActionResult Details(int id)
        {
            var product = _product.GetProductById(id);
            if (product == null)
                return HttpNotFound();

            return View(product);
        }

        // GET: /Product/Create
        public ActionResult Create()
        {
            return View(new ProductEntity());
        }

        // POST: /Product/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ProductEntity model)
        {
            // Получаем загруженный файл
            var file = Request.Files["ProductImageFile"];

            if (file != null && file.ContentLength > 0)
            {
                // Генерируем уникальное имя файла и сохраняем
                var fileName = System.IO.Path.GetFileName(file.FileName);
                var uniqueFileName = Guid.NewGuid().ToString() + "_" + fileName;
                var path = Server.MapPath("~/Uploads/Products/");

                // Убедимся, что папка существует
                if (!System.IO.Directory.Exists(path))
                    System.IO.Directory.CreateDirectory(path);

                var fullPath = System.IO.Path.Combine(path, uniqueFileName);
                file.SaveAs(fullPath);

                // Сохраняем относительный путь в модель (зависит от структуры твоего продукта)
                model.ProductImage = "/Uploads/Products/" + uniqueFileName;
            }

            if (!ModelState.IsValid)
                return View(model);

            bool created = _product.AddProduct(model);
            if (!created)
            {
                ModelState.AddModelError("", "Ошибка при добавлении продукта.");
                return View(model);
            }

            return RedirectToAction("Index");
        }


        // GET: /Product/Edit/5
        public ActionResult Edit(int id)
        {
            var product = _product.GetProductById(id);
            if (product == null)
                return HttpNotFound();

            return View(product);
        }

        // POST: /Product/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, ProductEntity model)
        {
            if (!ModelState.IsValid)
                return View(model);

            bool updated = _product.UpdateProduct(id, model);
            if (!updated)
            {
                ModelState.AddModelError("", "Ошибка при обновлении продукта.");
                return View(model);
            }

            return RedirectToAction("Index");
        }

        // GET: /Product/Delete/5
        public ActionResult Delete(int id)
        {
            var product = _product.GetProductById(id);
            if (product == null)
                return HttpNotFound();

            return View(product);
        }

        // POST: /Product/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            bool deleted = _product.DeleteProduct(id);
            if (!deleted)
            {
                ModelState.AddModelError("", "Ошибка при удалении продукта.");
                return View(_product.GetProductById(id));
            }

            return RedirectToAction("Index");
        }
    }
}
