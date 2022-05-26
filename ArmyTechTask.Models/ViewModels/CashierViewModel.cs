using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ArmyTechTask.Models.ViewModels
{
    public class BranchesViewModel
    {
        public Branches Branches { get; set; }
        public IEnumerable<SelectListItem> CitiesList { get; set; }
    }
}
