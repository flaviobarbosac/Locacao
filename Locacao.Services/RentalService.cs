using Locacao.Domain.Enum;
using Locacao.Domain.Model;
using Locacao.Repository.Interface;
using Locacao.Services.Interface;

namespace Locacao.Services
{
    public class RentalService : BaseServices<Rental>, IRentalService
    {
        private readonly IBaseRepository<Rental> _rentalRepository;
        private IDeliverymanService _deliverymanService;

        public RentalService(IBaseRepository<Rental> rentalRepository, IDeliverymanService deliverymanService)
             : base(rentalRepository)
        {
            _rentalRepository = rentalRepository;
            _deliverymanService = deliverymanService;
        }

        public decimal CalculateTotalCost(Rental rental, DateTime actualReturnDate)
        {
            var plannedDays = (rental.ExpectedEndDate - rental.StartDate).Days;
            var actualDays = (actualReturnDate - rental.StartDate).Days;
            var dailyRate = GetDailyRate(rental.RentalPlan);

            if (actualReturnDate <= rental.ExpectedEndDate)
            {
                // Devolução antecipada ou no prazo
                var usedDays = Math.Min(plannedDays, actualDays);
                var unusedDays = plannedDays - usedDays;
                var baseCost = usedDays * dailyRate;
                var penaltyCost = CalculateEarlyReturnPenalty(rental.RentalPlan, unusedDays, dailyRate);
                return baseCost + penaltyCost;
            }
            else
            {
                // Devolução tardia
                var baseCost = plannedDays * dailyRate;
                var extraDays = actualDays - plannedDays;
                var lateFee = extraDays * 50m; // R$50,00 por dia adicional
                return baseCost + lateFee;
            }
        }

        private decimal GetDailyRate(RentalPlan plan)
        {
            return plan switch
            {
                RentalPlan.SevenDays => 30m,
                RentalPlan.FifteenDays => 28m,
                RentalPlan.ThirtyDays => 22m,
                RentalPlan.FortyFiveDays => 20m,
                RentalPlan.FiftyDays => 18m,
                _ => throw new ArgumentException("Invalid rental plan")
            };
        }

        private decimal CalculateEarlyReturnPenalty(RentalPlan plan, int unusedDays, decimal dailyRate)
        {
            var penaltyRate = plan switch
            {
                RentalPlan.SevenDays => 0.20m,
                RentalPlan.FifteenDays => 0.40m,
                _ => 0m
            };

            return unusedDays * dailyRate * penaltyRate;
        }

        public Rental CreateRental(Guid motorcycleId, Guid deliverymanId, RentalPlan rentalPlan)
        {
            var deliveryman = GetDeliveryman(deliverymanId); // Implemente este método
            if (!_deliverymanService.IsEligibleForRental(deliveryman.Result))
            {
                throw new InvalidOperationException("Deliveryman is not eligible for rental.");
            }

            var startDate = DateTime.UtcNow.Date.AddDays(1);
            var endDate = CalculateEndDate(startDate, rentalPlan);

            var rental = new Rental
            {
                MotorcycleId = motorcycleId,
                DeliverymanId = deliverymanId,
                StartDate = startDate,
                EndDate = endDate,
                ExpectedEndDate = endDate,
                RentalPlan = rentalPlan,
                TotalCost = CalculateInitialTotalCost(rentalPlan, startDate, endDate)
            };

            // Salve a locação no banco de dados
            SaveRental(rental);

            return rental;
        }

        private DateTime CalculateEndDate(DateTime startDate, RentalPlan plan)
        {
            return plan switch
            {
                RentalPlan.SevenDays => startDate.AddDays(7),
                RentalPlan.FifteenDays => startDate.AddDays(15),
                RentalPlan.ThirtyDays => startDate.AddDays(30),
                RentalPlan.FortyFiveDays => startDate.AddDays(45),
                RentalPlan.FiftyDays => startDate.AddDays(50),
                _ => throw new ArgumentException("Invalid rental plan")
            };
        }

        private decimal CalculateInitialTotalCost(RentalPlan plan, DateTime startDate, DateTime endDate)
        {
            var days = (endDate - startDate).Days;
            var dailyRate = GetDailyRate(plan);
            return days * dailyRate;
        }
        
        private async Task<Deliveryman> GetDeliveryman(Guid deliverymanId)
        {
            return await _deliverymanService.GetByIdAsync(deliverymanId);
        }

        private async Task SaveRental(Rental rental)
        {
            await _rentalRepository.AddAsync(rental);            
        }
    }
}