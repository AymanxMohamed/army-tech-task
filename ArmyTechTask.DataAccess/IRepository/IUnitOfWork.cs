using ArmyTechTask.Models;
using System;
using System.Threading.Tasks;

namespace ArmyTechTask.DataAccess.IRepository
{
    public interface IUnitOfWork : IDisposable
    {
        IGenericRepository<Branches> BranchesRepository { get; }
        IGenericRepository<Cashier> CashaiersRepository { get; }
        IGenericRepository<Cities> CitiesRepository { get; }
        IGenericRepository<InvoiceDetails> InvoiceDetailsRepository { get; }
        IGenericRepository<InvoiceHeader> InvoiceHeaderRepository { get; }
        Task Save();
    }
}
