using CoffeeShop.Models;
using CoffeeShop.Services;
using Microsoft.AspNetCore.Mvc;

namespace CoffeeShop.Controllers
{
    public class ContactController : Controller
    {
        private readonly EmailService _emailService;

        public ContactController(EmailService emailService)
        {
            _emailService = emailService;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Index(ContactModel model)
        {
            if (ModelState.IsValid)
            {
                await _emailService.SendEmailAsync(model);
                ViewBag.Message = "Email sent successfully!";
            }
            else
            {
                ViewBag.Message = "Please correct the errors in the form.";
            }

            return View(model);
        }
    }
}

