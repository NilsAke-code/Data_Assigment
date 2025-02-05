using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;


namespace Data.Entities
{
    public class ProjectEntity
    {
        [Key]
        public int Id { get; set; }
        public string Title { get; set; } = null!;
        public string? Description { get; set; }

        [Column(TypeName = "date")]
        public DateTime StartDate { get; set; }

        [Column(TypeName = "date")]
        public DateTime EndDate { get; set; }

        public int CustomerId { get; set; }
        public CustomerEntity Customer { get; set; } = null!;

        public int ServiceId { get; set; }
        public ServiceEntity Service { get; set; } = null!;

        public int UserId { get; set; }
        public UserEntity User { get; set; } = null!;

        public int StatusId { get; set; }
        public StatusTypeEntity Status { get; set; } = null!;

    }
}
