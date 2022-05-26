using ArmyTechTask.DataAccess.IRepository;
using ArmyTechTask.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
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
       
    }
}
