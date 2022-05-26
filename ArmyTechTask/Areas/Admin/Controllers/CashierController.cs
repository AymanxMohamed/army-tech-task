using ArmyTechTask.DataAccess.IRepository;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace ArmyTechTask.Areas.Customer.Controllers
{
    public class CashierController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public CashierController(IUnitOfWork unitOfWork) => _unitOfWork = unitOfWork;

        public async Task<IActionResult>Index() => View(await _unitOfWork.CitiesRepository.GetAll());
    }
}
