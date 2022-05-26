using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ArmyTechTask.Models.ViewModels
{
    public class InvoiceHeaderViewModel
    {
        public InvoiceHeader InvoiceHeader { get; set; }
        public InvoiceDetails InvoiceDetails { get; set; }
        public IEnumerable<SelectListItem> CashierList { get; set; }
    }
}
