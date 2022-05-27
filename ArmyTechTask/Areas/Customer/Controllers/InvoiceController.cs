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
    public class InvoiceController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public InvoiceController(IUnitOfWork unitOfWork) => _unitOfWork = unitOfWork;

        public async Task<IActionResult> Index() => View(
            await _unitOfWork.InvoiceHeaderRepository.GetAll(include: q =>
            q.Include(x => x.Cashier)
            .Include(x => x.Branch)
            .Include(x => x.InvoiceDetails)
            ));

        public async Task<IActionResult> Create()
        {
            IEnumerable<Cashier> cashiers = await _unitOfWork.CashaiersRepository.GetAll();

            var cashairList = cashiers.Select(i => new SelectListItem
            {
                Text = i.CashierName,
                Value = i.Id.ToString()
            });
            InvoiceHeaderViewModel invoiceHeaderVM = new()
            {
                CashierList = cashairList
            };
            return View(invoiceHeaderVM);
        }

        public IActionResult AddInvoiceDetail(long id) => View(new InvoiceDetails { InvoiceHeaderId = id });

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddInvoiceDetail(InvoiceDetails item)
        {
            item.Id = 0;
            await _unitOfWork.InvoiceDetailsRepository.Insert(item);
            await _unitOfWork.Save();
            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(InvoiceHeaderViewModel invoiceHeaderVM)
        {
            var cashier = await _unitOfWork.CashaiersRepository.Get(x => x.Id == invoiceHeaderVM.InvoiceHeader.CashierId);

            invoiceHeaderVM.InvoiceHeader.BranchId = cashier.BranchId;
            await _unitOfWork.InvoiceHeaderRepository.Insert(invoiceHeaderVM.InvoiceHeader);
            await _unitOfWork.Save();

            invoiceHeaderVM.InvoiceDetails.InvoiceHeaderId = invoiceHeaderVM.InvoiceHeader.Id;
            await _unitOfWork.InvoiceDetailsRepository.Insert(invoiceHeaderVM.InvoiceDetails);
            await _unitOfWork.Save();
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();
           var invoiceHeader = await _unitOfWork.InvoiceHeaderRepository.Get(x => x.Id == id, include: q =>
            q.Include(x => x.Cashier)
            .Include(x => x.Branch)
            .Include(x => x.InvoiceDetails)
            );

            IEnumerable<Cashier> cashiers = await _unitOfWork.CashaiersRepository.GetAll();

            var cashairList = cashiers.Select(i => new SelectListItem
            {
                Text = i.CashierName,
                Value = i.Id.ToString()
            });
            EditInvoiceHeaderViewModel invoiceHeaderVM = new()
            {
                InvoiceDetails = invoiceHeader.InvoiceDetails,
                InvoiceHeader = invoiceHeader,
                CashierList = cashairList
            };
            return View(invoiceHeaderVM);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(EditInvoiceHeaderViewModel invoiceHeaderVM)
        {
            if (!ModelState.IsValid)
                return View(invoiceHeaderVM);
            _unitOfWork.InvoiceHeaderRepository.Update(invoiceHeaderVM.InvoiceHeader);
            _unitOfWork.InvoiceDetailsRepository.UpdateRange(invoiceHeaderVM.InvoiceDetails);
            return await Submit("Invoice Updated Successfully");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateItem(InvoiceDetails item)
        {
            if (!ModelState.IsValid)
                return View(item);
            _unitOfWork.InvoiceDetailsRepository.Update(item);
            return await Submit("Invoice Updated Successfully");
        }
        

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return NotFound();
            return View(await _unitOfWork.InvoiceHeaderRepository.Get(x => x.Id == id, include: q =>
            q.Include(x => x.Cashier)
            .Include(x => x.Branch)
            .Include(x => x.InvoiceDetails)
            ));
        }
        public async Task<IActionResult> DeleteItem(long id)
        {
            var item = await _unitOfWork.InvoiceDetailsRepository.Get(x => x.Id == id);
            await _unitOfWork.InvoiceDetailsRepository.Delete(id);
            await _unitOfWork.Save();
            return RedirectToAction("Index");
        }

        

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeletePost(int id)
        {
            var invoiceHeader = await _unitOfWork.InvoiceHeaderRepository.Get(x => x.Id == id, include: q =>
            q.Include(x => x.InvoiceDetails));
            _unitOfWork.InvoiceDetailsRepository.DeleteRange(invoiceHeader.InvoiceDetails);
            await _unitOfWork.Save();
            await _unitOfWork.InvoiceHeaderRepository.Delete(invoiceHeader.Id);
            await _unitOfWork.Save();
            TempData["Success"] = "Invoice deleted Successfully";
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
