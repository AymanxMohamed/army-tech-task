using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ArmyTechTask.Models.ViewModels
{
    public class CashierViewModel
    {
        public Cashier Cashier { get; set; }
        public IEnumerable<SelectListItem> BranchesList { get; set; }
    }
}
