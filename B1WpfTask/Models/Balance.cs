using B1WPFTask.Models.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace B1WPFTask.Models;

public class Balance : IEntity
{
    [Key]
    public Guid Id { get; set; }
    [Required]
    public decimal Active { get; set; }
    [Required]
    public decimal Passive { get; set; }
}