using System.ComponentModel.DataAnnotations;

namespace Locacao.Domain.Model.Base
{
    public class ModelBase
    {
        [Key]
        public Guid Id { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime ModificationDate { get; set; }
    }
}
