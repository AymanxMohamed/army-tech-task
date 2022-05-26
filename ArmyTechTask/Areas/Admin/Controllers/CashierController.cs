using ArmyTechTask.DataAccess.IRepository;
using ArmyTechTask.Models;
using ArmyTechTask.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace ArmyTechTask.Areas.Customer.Controllers
{
    public class CashierController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public CashierController(IUnitOfWork unitOfWork) => _unitOfWork = unitOfWork;

        public async Task<IActionResult> Index() => View(
            await _unitOfWork.CashaiersRepository.GetAll(include: q => q.Include(x => x.Branch))
            );

        public async Task<IActionResult> Create()
        {
            IEnumerable<Branches> branches = await _unitOfWork.BranchesRepository.GetAll();
     
            var branchesList = branches.Select(i => new SelectListItem
            {
                Text = i.BranchName,
                Value = i.Id.ToString()
            });
            CashierViewModel cashierVm = new()
            {
                BranchesList = branchesList
            };
            return View(cashierVm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Cashier cashier)
        {
            if (!ModelState.IsValid)
                return View(cashier);
            await _unitOfWork.CashaiersRepository.Insert(cashier);
            return await Submit("Cashier Created Successfully");
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();

            IEnumerable<Branches> branches = await _unitOfWork.BranchesRepository.GetAll();

            var branchesList = branches.Select(i => new SelectListItem
            {
                Text = i.BranchName,
                Value = i.Id.ToString()
            });
            CashierViewModel cashierVm = new()
            {
                Cashier = await _unitOfWork.CashaiersRepository.Get(x => x.Id == id),
                BranchesList = branchesList
            };
            return View(cashierVm);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Cashier cashier)
        {
            if (!ModelState.IsValid)
                return View(cashier);
            _unitOfWork.CashaiersRepository.Update(cashier);
            return await Submit("Cashier Updated Successfully");
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return NotFound();
            return View(await _unitOfWork.CashaiersRepository.Get(x => x.Id == id, include: q => q.Include(x => x.Branch)));
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeletePost(int id)
        {
            await _unitOfWork.CashaiersRepository.Delete(id);
            try
            {
                await _unitOfWork.Save();
                TempData["Success"] = "Cashier deleted Successfully";
            }
            catch (Exception)
            {
                TempData["Error"] = "Cann't Delete This Cashier Because he has Dependencies";
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
