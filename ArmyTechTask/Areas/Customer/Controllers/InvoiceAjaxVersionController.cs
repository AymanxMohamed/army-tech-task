using ArmyTechTask.DataAccess.IRepository;
using ArmyTechTask.Models;
using ArmyTechTask.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ArmyTechTask.Areas.Customer.Controllers
{
    public class InvoiceAjaxVersionController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public InvoiceAjaxVersionController(IUnitOfWork unitOfWork) => _unitOfWork = unitOfWork;

        public async Task<IActionResult>Index() => View(
            await _unitOfWork.InvoiceHeaderRepository.GetAll(include: q =>
            q.Include(x => x.Cashier)
            .Include(x => x.Branch)
            .Include(x => x.InvoiceDetails)
            ));
     

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateInvoiceHeader()
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
            return PartialView("_CreateInvoiceHeader", invoiceHeaderVM);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> StoreInvoiceHeader(InvoiceHeaderViewModel invoiceHeaderVM)
        {
            var cashier = await _unitOfWork.CashaiersRepository.Get(x => x.Id == invoiceHeaderVM.InvoiceHeader.CashierId);

            invoiceHeaderVM.InvoiceHeader.BranchId = cashier.BranchId;
            await _unitOfWork.InvoiceHeaderRepository.Insert(invoiceHeaderVM.InvoiceHeader);
            await _unitOfWork.Save();

            invoiceHeaderVM.InvoiceDetails.InvoiceHeaderId = invoiceHeaderVM.InvoiceHeader.Id;
            await _unitOfWork.InvoiceDetailsRepository.Insert(invoiceHeaderVM.InvoiceDetails);
            await _unitOfWork.Save();

            return PartialView();
        }
        
    }
}
