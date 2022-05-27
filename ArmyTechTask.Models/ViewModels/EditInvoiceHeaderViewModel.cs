using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ArmyTechTask.Models.ViewModels
{
    public class EditInvoiceHeaderViewModel
    {
        public InvoiceHeader InvoiceHeader { get; set; }
        public ICollection<InvoiceDetails> InvoiceDetails { get; set; }
        public IEnumerable<SelectListItem> CashierList { get; set; }
    }
}
