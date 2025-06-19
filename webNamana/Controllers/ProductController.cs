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
        private readonly IProductBL _product;
        private readonly ProductApi _productApi = new ProductApi();

        public ProductController()
        {
            var bl = new BusinessLogic.BusinessLogic();
            _product = bl.GetProductBL(); // инициализация бизнес-логики
        }

        // GET: /AdminProductList
        public ActionResult AdminProductList()
        {
            var products = _product.GetAllProducts();
            var model = new List<ProductListViewModel>();

            foreach (var p in products)
            {
                model.Add(new ProductListViewModel
                {
                    ProductId = p.Id,
                    ProductName = p.ProductName,
                    Price = p.Price,
                    ProductImage = p.ProductImage
                });
            }

            return View(model);
        }

        // GET: /Product/Details
        public ActionResult Details(int? id)
        {
            if (!id.HasValue)
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest, "Product ID is required");

            var productEntity = _product.GetProductById(id.Value);
            if (productEntity == null)
                return HttpNotFound($"Product with ID {id.Value} not found");

            // Создаем ViewModel из ProductEntity
            var model = new ProductListViewModel
            {
                ProductId = productEntity.Id,
                ProductName = productEntity.ProductName,
                Description = productEntity.Description,
                Price = productEntity.Price,
                ProductImage = productEntity.ProductImage
            };

            return View(model);
        }


        // GET: /Product/Create
        [HttpGet]
        public ActionResult Create()
        {
            return View(new ProductCreateViewModel());
        }

        // POST: /Product/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ProductCreateViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            if (model.ProductImageFile == null || model.ProductImageFile.ContentLength == 0)
            {
                ModelState.AddModelError("ProductImageFile", "Please upload a product image.");
                return View(model);
            }

            string uniqueFileName = Guid.NewGuid() + "_" + Path.GetFileName(model.ProductImageFile.FileName);
            string uploadDir = Server.MapPath("~/Uploads/Products/");
            if (!Directory.Exists(uploadDir))
                Directory.CreateDirectory(uploadDir);

            string fullPath = Path.Combine(uploadDir, uniqueFileName);
            model.ProductImageFile.SaveAs(fullPath);

            var productEntity = new ProductEntity
            {
                ProductName = model.ProductName,
                Description = model.Description,
                Price = model.Price,
                ProductImage = "/Uploads/Products/" + uniqueFileName
            };

            bool added = _product.AddProduct(productEntity);
            if (added)
            {
                TempData["Message"] = "Product successfully added!";
                return RedirectToAction("Create");
            }

            ModelState.AddModelError("", "Failed to add product.");
            return View(model);
        }


        // GET: /Product/Edit
        public ActionResult Edit(int id)
        {
            var product = _product.GetProductById(id);
            if (product == null)
                return HttpNotFound();

            return View(product);
        }

        // POST: /Product/Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, ProductEntity model)
        {
            if (!ModelState.IsValid)
                return View(model);

            bool updated = _product.UpdateProduct(id, model);
            if (!updated)
            {
                ModelState.AddModelError("", "Failed to update product.");
                return View(model);
            }

            return RedirectToAction("AdminProductList");
        }

        // GET: /Product/Delete
        public ActionResult Delete(int id)
        {
            var product = _product.GetProductById(id);
            if (product == null)
                return HttpNotFound();

            return View(product);
        }

        // POST: /Product/Delete
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            bool deleted = _product.DeleteProduct(id);
            if (!deleted)
            {
                ModelState.AddModelError("", "Failed to delete product.");
                return View(_product.GetProductById(id));
            }

            return RedirectToAction("AdminProductList");
        }


        // GET: /Product/ProductPage
        public ActionResult ProductPage()
        {
            var productEntities = _product.GetAllProducts();

            if (productEntities == null || !productEntities.Any())
            {
                // Пока нет товаров — можно вернуть пустую модель или ViewBag сообщение
                ViewBag.Message = "There is no products";
                return View(new List<ProductListViewModel>());
            }

            var productViewModels = productEntities.Select(p => new ProductListViewModel
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
