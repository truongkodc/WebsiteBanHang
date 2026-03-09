using System.Reflection.Metadata.Ecma335;

namespace WebsiteBanHang.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using WebsiteBanHang.Models;
    using WebsiteBanHang.Repositories;
    public class ProductController : Controller
    {
        private readonly IProductRepository _productRepository;
        private readonly ICategoryRepository _categoryRepository;
        public ProductController(IProductRepository productRepository, ICategoryRepository categoryRepository)
        {
            _productRepository = productRepository;
            _categoryRepository = categoryRepository;
        }
        public IActionResult Add()
        {
            var categories = _categoryRepository.GetAllCategories();
            ViewBag.Categories = new SelectList(categories, "Id", "Name");
            return View();
        }
        
        [HttpPost]
        public IActionResult Add(Product product)
        {
            if (ModelState.IsValid)
            {
                _productRepository.Add(product);
                return RedirectToAction("Index");
            }
            return View(product);
        }
        public IActionResult Index()
        {
            var products = _productRepository.GetAll();
            return View(products);
        }

        public IActionResult Display(int id)
        {
            var product = _productRepository.GetByID(id);
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }

        public IActionResult Update(int id)
        {
            var product = _productRepository.GetByID(id);
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }

        [HttpPost]
        public IActionResult Update(Product product)
        {
            if (ModelState.IsValid)
            {
                _productRepository.Update(product);
                return RedirectToAction("Index");
            }
            return View(product);
        }

        public IActionResult Delete(int id)
        {
            var product = _productRepository.GetByID(id);
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }

        [HttpPost, ActionName("Delete")]
        public IActionResult DeleteConfirmed(int id)
        {
            _productRepository.Delete(id);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public  async Task<IActionResult> Add(Product product, IFormFile imageUrl, List<IFormFile> imageUrls)
        {
            if (ModelState.IsValid)
            {
                if(imageUrl != null)
                {
                product.ImageUrl= await SaveImage(imageUrl);
                }
            if(imageUrls != null)
            {
                product.ImageUrls = new List<string>();
                foreach(var file in imageUrls)
                {
                    product.ImageUrls.Add(await SaveImage(file));//Lưu các hình ảnh khác
                }
            }
            _productRepository.Add(product);
            return RedirectToAction("Index");
            }
            return View(product);
        }
        private async Task<string> SaveImage(IFormFile image)
        {
            var savePath = Path.Combine("wwwroot/images", image.FileName);//Thay đổi đường dẫn theo cấu hình 

            using (var fileSteam = new FileStream (savePath, FileMode.Create))
            {
                await image.CopyToAsync(fileSteam);
            }
            return "/image/" + image.FileName; // Trả về đường dẫn tương đối
        }
            }
        }
    

