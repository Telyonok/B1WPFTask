using B1WPFTask.Models.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace B1WPFTask.Models;

public class InputFile : IEntity
{
    [Key]
    public Guid Id { get; set; }
    [Required]
    public string FileName { get; set; }
    public List<BankAccountClass> AccountClasses { get; set; }

    public override string ToString() => FileName;
}