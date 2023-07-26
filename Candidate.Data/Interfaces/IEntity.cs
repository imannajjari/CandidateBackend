using System.ComponentModel.DataAnnotations;

namespace Candidate.Data.Interfaces;

public interface IEntity
{
    [Key]
    [Required]
    int ID { get; set; }

    [Required]
    bool IsActive { get; set; }

    [Required]
    bool IsDeleted { get; set; }

    DateTime CreationDateTime { get; set; }

    DateTime? ModificationDateTime { get; set; }
}