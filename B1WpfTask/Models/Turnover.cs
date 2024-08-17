using B1WPFTask.Models.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace B1WPFTask.Models;

public class Turnover : IEntity
{
    [Key]
    public Guid Id { get; set; }
    [Required]
    public decimal Debit { get; set; }
    [Required]
    public decimal Credit { get; set; }
}