namespace Locacao.Domain.Model
{
    public class Motorcycle : Base.ModelBase
    {        
        public int Year { get; set; }
        public string Model { get; set; }
        public string LicensePlate { get; set; }
        public List<Rental> Rentals { get; set; }
    }
}
