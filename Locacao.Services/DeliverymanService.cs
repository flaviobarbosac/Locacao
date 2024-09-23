using Locacao.Domain.Enum;
using Locacao.Domain.Model;
using Locacao.Repository.Interface;
using Locacao.Services.Interface;

namespace Locacao.Services
{
    public class DeliveryManService : BaseServices<Deliveryman>, IDeliverymanService
    {
        private readonly IBaseRepository<Deliveryman> _rentalRepository;

        public DeliveryManService(IBaseRepository<Deliveryman> rentalRepository)
             : base(rentalRepository)
        {
            _rentalRepository = rentalRepository;
        }

        public bool IsEligibleForRental(Deliveryman deliveryman)
        {
            return deliveryman.DriversLicenseType == DriversLicenseType.A || deliveryman.DriversLicenseType == DriversLicenseType.AB;
        }

    }
}
