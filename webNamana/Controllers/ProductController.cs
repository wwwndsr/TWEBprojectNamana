using System;
using System.Data.Entity.Validation;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using webNamana.Domain.Entities.Product;
using webNamana.Helpers;
using webNamana.BusinessLogic;
using webNamana.BusinessLogic.Interfaces;
using webNamana.Models;
using System.Collections.Generic;
using System.IO;
using webNamana.BusinessLogic.DBModel;
using webNamana.BusinessLogic.Core;

namespace webNamana.Web.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductService _product;
        private readonly ProductApi _productApi = new ProductApi();

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
        public ActionResult Details(int? id)
        {
            // Проверяем, передан ли параметр id
            if (!id.HasValue)
            {
                // Возвращаем ошибку 400 - неправильный запрос, если id не передан
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest, "Product ID is required");
            }

            // Получаем продукт из бизнес-логики по id
            var product = _product.GetProductById(id.Value);

            // Если продукт не найден, возвращаем ошибку 404 - не найдено
            if (product == null)
            {
                return HttpNotFound($"Product with ID {id.Value} not found");
            }

            // Возвращаем представление с моделью продукта
            return View(product);
        }


        [HttpGet]
        public ActionResult Create()
        {
            return View(new ProductEntity());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ProductEntity model, HttpPostedFileBase ProductImageFile)
        {
            if (ProductImageFile != null && ProductImageFile.ContentLength > 0)
            {
                var fileName = Path.GetFileName(ProductImageFile.FileName);
                var uniqueFileName = Guid.NewGuid() + "_" + fileName;
                var path = Server.MapPath("~/Uploads/Products/");

                if (!Directory.Exists(path))
                    Directory.CreateDirectory(path);

                var fullPath = Path.Combine(path, uniqueFileName);
                ProductImageFile.SaveAs(fullPath);

                model.ProductImage = "/Uploads/Products/" + uniqueFileName;
            }
            else
            {
                ModelState.AddModelError("ProductImage", "Необходимо выбрать изображение.");
                return View(model);
            }

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            bool added = _productApi.AddProduct(model);
            if (added)
            {
                TempData["Message"] = "Товар успешно добавлен.";
                return RedirectToAction("Create");
            }
            else
            {
                ModelState.AddModelError("", "Ошибка при добавлении товара.");
                return View(model);
            }
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


        // GET: /Product/ProductPage
        public ActionResult ProductPage()
        {
            var productEntities = _product.GetAllProducts();

            if (productEntities == null || !productEntities.Any())
            {
                // Пока нет товаров — можно вернуть пустую модель или ViewBag сообщение
                ViewBag.Message = "There is no products";
                return View(new List<ProductViewModel>());
            }

            var productViewModels = productEntities.Select(p => new ProductViewModel
            {
                ProductId = p.Id,
                ProductName = p.ProductName,
                Description = p.Description,
                Price = p.Price,
                ProductImage = p.ProductImage
            }).ToList();

            return View(productViewModels);
        }



    }
}
