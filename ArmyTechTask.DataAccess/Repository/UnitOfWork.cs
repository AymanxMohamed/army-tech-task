using ArmyTechTask.DataAccess.IRepository;
using ArmyTechTask.Models;
using System;
using System.Threading.Tasks;

namespace ArmyTechTask.DataAccess.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _context;
        public IGenericRepository<Branches> BranchesRepository { get; private set; }
        public IGenericRepository<Cashier> CashaiersRepository { get; private set; }
        public IGenericRepository<Cities> CitiesRepository { get; private set; }
        public IGenericRepository<InvoiceDetails> InvoiceDetailsRepository { get; private set; }
        public IGenericRepository<InvoiceHeader> InvoiceHeaderRepository { get; private set; }

        public UnitOfWork(AppDbContext context)
        {
            _context = context;
            BranchesRepository = new GenericRepository<Branches>(_context);
            CashaiersRepository = new GenericRepository<Cashier>(_context);
            CitiesRepository = new GenericRepository<Cities>(_context);
            InvoiceDetailsRepository = new GenericRepository<InvoiceDetails>(_context);
            InvoiceHeaderRepository = new GenericRepository<InvoiceHeader>(_context);
        }

        public async Task Save() => await _context.SaveChangesAsync();
    }
}
