using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebGoatCore.Data;
using WebGoatCore.Models;
using WebGoatCore.ViewModels;

namespace WebGoat.NET.Controllers
{
    public class OrderController : Controller
    {
        private readonly OrderRepository _orderRepository;

        // Constructor injection
        public OrderController(OrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Manage()
        {
            return View(_orderRepository.GetAllOrders());
        }
        // GET: OrderController
        public ActionResult Index()
        {
            return View();
        }

        // GET: OrderController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: OrderController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: OrderController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }


        [Authorize(Roles = "Admin")]
        [HttpGet("Order/Edit/{id}")]
        public IActionResult Edit(int id)
        {

            // Create a view model to pass data to the view
            var viewModel = new OrderEditViewModel
            {
                Order = _orderRepository.GetOrderById(id),
            };

            return View(viewModel);
        }

        // POST: OrderController/Edit/5
        [Authorize(Roles = "Admin")]
        [HttpPost("Order/Edit/{id}")]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, OrderEditViewModel model)
        {
            if (ModelState.IsValid)
            {
                // Update order in the repository
                _orderRepository.UpdateOrder(model.Order);

                // Redirect to Manage view after successful update
                return RedirectToAction("Manage");
            }

            // If validation fails, return the same view with the model
            return View(model);
        }

        // GET: OrderController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: OrderController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
