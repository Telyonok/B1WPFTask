﻿using B1WPFTask.Models.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace B1WPFTask.Models;

public class BankAccount : IEntity
{
    [Key]
    public Guid Id { get; set; }
    [Required]
    public int Number { get; set; }
    [Required]
    public BankAccountClass Class { get; set; }
    [Required]
    public InputBalance InputBalance { get; set; }
    [Required]
    public OutputBalance OutputBalance { get; set; }
    [Required]
    public Turnover Turnover { get; set; }
}
