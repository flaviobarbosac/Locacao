using Locacao.Domain.Model;
using Locacao.Repository.Interface;
using Microsoft.EntityFrameworkCore;

namespace Locacao.Repository
{
    public class MotorcycleRepository : BaseRepository<Motorcycle>, IMotorcycleRepository
    {
        private readonly Infraestructure.LocacaoDbContext _context;
        private readonly DbSet<Motorcycle> _dbSet;
        public MotorcycleRepository(Infraestructure.LocacaoDbContext context) : base(context)
        {
            _context = context;
            _dbSet = context.Set<Motorcycle>();
        }

        public async Task AddAsync(Motorcycle motorcycle)
        {
            _context.Motorcycles.Add(motorcycle);
            await _context.SaveChangesAsync();
        }

        public async Task SaveMotorcycleEventAsync(Motorcycle motorcycle)
        {
            var eventEntity = new MotorcycleRegistrationEvent
            {
                MotorcycleId = motorcycle.Id,                
                Id = motorcycle.Id,
                Message= "MotorcycleRegistered",                
                RegistrationDate= DateTime.UtcNow,
                CreationDate = DateTime.UtcNow,
                ModificationDate = DateTime.UtcNow
            };

            _context.MotorcycleRegistrationEvents.Add(eventEntity);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Motorcycle>> GetMotorcyclesAsync(string licensePlate = null)
        {
            IQueryable<Motorcycle> query = _context.Motorcycles;

            if (!string.IsNullOrWhiteSpace(licensePlate))
            {
                query = query.Where(m => m.LicensePlate.Contains(licensePlate));
            }

            return await query.ToListAsync();
        }

        public async Task<bool> LicensePlateExistsAsync(string licensePlate)
        {
            return await _context.Motorcycles.AnyAsync(m => m.LicensePlate == licensePlate);
        }


    }
}