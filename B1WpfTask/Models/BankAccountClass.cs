using B1WPFTask.Models.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace B1WPFTask.Models;
public class BankAccountClass : IEntity
{
    [Key]
    public Guid Id { get; set; }
    [Required]
    public int Number { get; set; }
    [Required]
    public string Title { get; set; }
    public List<BankAccount> BankAccounts { get; set; }

    public BankAccountClass()
    {
    }

    public BankAccountClass(Guid id, int number, string title, List<BankAccount> bankAccounts)
    {
        Id = id;
        Number = number;
        Title = title;
        BankAccounts = bankAccounts;
    }

    public override string ToString() => Title;

    public static BankAccountClass Create(int number, string title) =>
        new(Guid.NewGuid(), number, title, bankAccounts: []);
}
