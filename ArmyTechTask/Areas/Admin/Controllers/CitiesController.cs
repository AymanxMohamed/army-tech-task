using ArmyTechTask.DataAccess.IRepository;
using ArmyTechTask.Models;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace ArmyTechTask.Areas.Customer.Controllers
{
    public class CitiesController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public CitiesController(IUnitOfWork unitOfWork) => _unitOfWork = unitOfWork;

        public async Task<IActionResult> Index() => View(await _unitOfWork.CitiesRepository.GetAll());
        public IActionResult Create() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Cities city)
        {
            if (!ModelState.IsValid)
                return View(city);
            await _unitOfWork.CitiesRepository.Insert(city);
            await _unitOfWork.Save();
            TempData["Success"] = "City Created Successfully";
            return RedirectToAction("Index");
        }

    }
}
