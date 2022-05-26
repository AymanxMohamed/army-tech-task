using ArmyTechTask.DataAccess.IRepository;
using ArmyTechTask.Models;
using ArmyTechTask.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ArmyTechTask.Areas.Customer.Controllers
{
    public class BranchesController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public BranchesController(IUnitOfWork unitOfWork) => _unitOfWork = unitOfWork;

        public async Task<IActionResult> Index() => View(
            await _unitOfWork.BranchesRepository.GetAll(include: q => q.Include(x => x.City))
            );

        public async Task<IActionResult> Create()
        {
            IEnumerable<Cities> cities = await _unitOfWork.CitiesRepository.GetAll();

            var citiesList = cities.Select(i => new SelectListItem
            {
                Text = i.CityName,
                Value = i.Id.ToString()
            });
            BranchesViewModel branchesVM = new()
            {
                CitiesList = citiesList
            };
            return View(branchesVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(BranchesViewModel branchVM)
        {
            if (!ModelState.IsValid)
                return View(branchVM);
            await _unitOfWork.BranchesRepository.Insert(branchVM.Branches);
            return await Submit("Branch Created Successfully");
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();

            IEnumerable<Cities> cities = await _unitOfWork.CitiesRepository.GetAll();

            var citiesList = cities.Select(i => new SelectListItem
            {
                Text = i.CityName,
                Value = i.Id.ToString()
            });
            BranchesViewModel cashierVm = new()
            {
                Branches = await _unitOfWork.BranchesRepository.Get(x => x.Id == id),
                CitiesList = citiesList
            };
            return View(cashierVm);


        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(BranchesViewModel branchVM)
        {
            if (!ModelState.IsValid)
                return View(branchVM);
            _unitOfWork.BranchesRepository.Update(branchVM.Branches);
            return await Submit("Branch Updated Successfully");
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return NotFound();
            return View(await _unitOfWork.BranchesRepository.Get(x => x.Id == id, include: q => q.Include(x => x.City)));
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeletePost(int id)
        {
            await _unitOfWork.BranchesRepository.Delete(id);
            try
            {
                await _unitOfWork.Save();
                TempData["Success"] = "Branch deleted Successfully";
            }
            catch (Exception)
            {
                TempData["Error"] = "Cann't Delete This Branch Because he has Dependencies";
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
