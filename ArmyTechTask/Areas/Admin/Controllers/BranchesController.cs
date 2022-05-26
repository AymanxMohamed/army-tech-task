using ArmyTechTask.DataAccess.IRepository;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace ArmyTechTask.Areas.Customer.Controllers
{
    public class BranchesController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public BranchesController(IUnitOfWork unitOfWork) => _unitOfWork = unitOfWork;

        public async Task<IActionResult>Index() => View(await _unitOfWork.CitiesRepository.GetAll());
    }
}
