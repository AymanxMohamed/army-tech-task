using ArmyTechTask.DataAccess.IRepository;
using ArmyTechTask.Models;
using Microsoft.AspNetCore.Mvc;
using System;
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
            return await Submit("City Created Successfully");
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();
            return View(await _unitOfWork.CitiesRepository.Get(x => x.Id == id));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Cities city)
        {
            if (!ModelState.IsValid)
                return View(city);
            _unitOfWork.CitiesRepository.Update(city);
            return await Submit("City Updated Successfully");
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return NotFound();
            return View(await _unitOfWork.CitiesRepository.Get(x => x.Id == id));
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeletePost(int id)
        {
            await _unitOfWork.CitiesRepository.Delete(id);
            try
            {
                await _unitOfWork.Save();
                TempData["Success"] = "Branch deleted Successfully";
            } 
            catch (Exception)
            {
                TempData["Error"] = "Cann't Delete This city Because it has Dependencies";
            }
            return RedirectToAction("Index");
        }
        private async Task<IActionResult> Submit(string successMessage)
        {
            await _unitOfWork.Save();
            TempData["Success"] = successMessage;
            return RedirectToAction("Index");
        }
    }
}
