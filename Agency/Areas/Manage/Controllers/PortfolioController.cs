using Business.Exceptions;
using Business.Services.Abstacts;
using Core.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace Agency.Areas.Manage.Controllers
{
    [Area("Manage")]
    [Authorize(Roles = "Admin")]
    public class PortfolioController : Controller
    {
        IPortfolioService _portfolioService;

        public PortfolioController(IPortfolioService portfolioService)
        {
            _portfolioService = portfolioService;
        }

        public IActionResult Index()
        {
            return View(_portfolioService.GetAllPortfolio());
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Portfolio portfolio)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            try
            {
                _portfolioService.CrearePortfolio(portfolio);
            }catch(NotFoundFileException ex)
            {
                ModelState.AddModelError(ex.PropertyName, ex.Message);
                return View();
            }catch(ContentTypeException ex)
            {
                ModelState.AddModelError(ex.PropertyName, ex.Message);
                return View();
            }catch(Exception ex)
            {
                return View("Error");
            }
            return RedirectToAction("Index");
        }


        public IActionResult Delete(int id)
        {
            if (!ModelState.IsValid)
            {
                return View("Error");
            }
            try
            {
                _portfolioService.DeletePortfolio(id);
            }catch(NotFoundFileException ex)
            {
                ModelState.AddModelError(ex.PropertyName, ex.Message);
                return View("Error");
            }catch(Exception ex)
            {
                return View("Error");
            }
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Update(int id)
        {
            Portfolio port = _portfolioService.GetPortfolio(x=>x.Id == id);
            if(port == null)
            {
                return View("Error");
            }
            return View(port);
        }

        [HttpPost]
        public IActionResult Update(Portfolio portfolio)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            try
            {
                _portfolioService.UpdatePortfolio(portfolio.Id, portfolio);
            }catch(NotFoundFileException ex)
            {
                ModelState.AddModelError(ex.PropertyName, ex.Message);
                return View();
            }catch(ContentTypeException ex)
            {
                ModelState.AddModelError(ex.PropertyName, ex.Message);
                return View();
            }catch(Exception ex)
            {
                return View("Error");
            }
            return RedirectToAction("Index");
        }
    }
}
