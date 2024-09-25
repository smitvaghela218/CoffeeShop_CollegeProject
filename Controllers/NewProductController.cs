using CoffeeShop.Models;
using Microsoft.AspNetCore.Mvc;

namespace CoffeeShop.Controllers
{
    public class NewProductController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Save(NewProductModel newProductModel)
        {
            if (ModelState.IsValid)
            {
                return RedirectToAction("Index");
            }
            else
            {
                return View("ProductAddEdit", newProductModel);
            }
        }
    }
}
