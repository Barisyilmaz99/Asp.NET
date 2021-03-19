using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Shopapp.Bussiness.Abstract;
using Shopapp.Entities;
using Shopapp.WebUI.Models;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Shopapp.WebUI.Controllers
{
    [Authorize(Roles ="admin")]
    public class AdminController : Controller
    {
        
        private IProductService _productService;
        private ICategoryService _categoryService;
        public AdminController(IProductService productService, ICategoryService categoryService)
        {
            _productService = productService;
            _categoryService = categoryService;
        }
        public IActionResult ProductList()
        {
            return View(new ProductListModel()
            {
                Products = _productService.GetAll()
            }
                );
        }
        public IActionResult CategoryList()
        {
            return View(new CategoryListModel()
            {
                Categories = _categoryService.GetAll()

            });
        }
        public IActionResult DeleteCategory(int categoryId)
        {
            var entity = _categoryService.GetById(categoryId);
            if (entity != null)
            {
                _categoryService.Delete(entity);
            }
            return Redirect("CategoryList");
        }
        public IActionResult EditProduct(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var entity = _productService.GetByIdwithCategories((int)id);
            if (entity == null)
            {
                return NotFound();
            }
            var model = new ProductModel()
            {
                ID = entity.ID,
                Name = entity.Name,
                Price = entity.Price,
                Description = entity.Description,
                ImageURL = entity.ImageURL,
                SelectedCategories = entity.ProductCategories.Select(i=>i.Category).ToList()
            };
            ViewBag.Categories = _categoryService.GetAll();
            return View(model);
        }
        public IActionResult EditCategory(int id)
        {
            var entity = _categoryService.GetByIdWithProducts(id);
            var model = new CategoryModel()
            {
                Name = entity.Name,
                ID = entity.ID,
                Products = entity.ProductCategories.Select(p => p.Product).ToList()
            };
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> EditProduct(ProductModel model,int[] categoryIds,IFormFile file)
        {
            if (ModelState.IsValid)
            {

            
                var entity = _productService.GetById(model.ID);
                if (entity == null)
                {
                    return NotFound();
                }
                entity.Name = model.Name;
                entity.Description = model.Description;
                entity.Price = model.Price;
                if (file!=null)
                {
                    entity.ImageURL = file.FileName;
                    var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\img", file.FileName);
                    using (var stream = new FileStream(path,FileMode.Create))
                    {
                        await file.CopyToAsync(stream);
                    }
                }
                _productService.Update(entity,categoryIds);
                return Redirect("Products");
            }
                ViewBag.Categories = _categoryService.GetAll();
                return View(model);
        }
       
        [HttpPost]
        public IActionResult EditCategory(CategoryModel model)
        {
            var entity = _categoryService.GetById(model.ID);
            if (entity == null)
            {
                return NotFound();
            }
            entity.Name = model.Name;
            _categoryService.Update(entity);

            return Redirect("CategoryList");
        }
        [HttpPost]
        public IActionResult DeleteProduct(int productId)
        {
            var entity = _productService.GetById(productId);
            if (entity != null)
            {
                _productService.Delete(entity);
            }
            return Redirect("Products");
        }
        [HttpGet]
        public IActionResult CreateCategory()
        {
            return View();
        }
        [HttpPost]
        public IActionResult CreateCategory(CategoryModel model)
        {
            var entity = new Category()
            {
                Name = model.Name
            };
            _categoryService.Create(entity);
            return Redirect("CategoryList");
        }
        [HttpGet]
        public IActionResult CreateProduct()
        {
            return View(new ProductModel());
        }
        [HttpPost]
        public IActionResult CreateProduct(ProductModel model) {
            if (ModelState.IsValid)
            {

            
            var entity = new Product()
            { Name = model.Name,
                Price = model.Price,
                Description = model.Description,
                ImageURL = model.ImageURL
            };

            _productService.Create(entity);
            return Redirect("Products");
            }
            return View(model);
        }

        [HttpPost]
        public IActionResult DeleteFromCategory(int categoryID,int productID)
        {
            _categoryService.DeleteFromCategory(categoryID, productID);
            return Redirect("/admin/editcategory/"+categoryID);
        }
    }

}