using B1WPFTask.Models.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace B1WPFTask.Models;
public class RandomRow : IEntity, IEquatable<RandomRow?>
{
    [Key]
    public Guid Id { get; set; }
    [Required]
    public DateTime Date { get; set; }
    [Required]
    public string LatinString { get; set; }
    [Required]
    public string RussianString { get; set; }
    [Required]
    public int PositiveInt { get; set; }
    [Required]
    public double PositiveDouble { get; set; } 

    public RandomRow()
    {
    }

    public RandomRow(DateTime date, string latinString, string russianString, int positiveInt, double positiveDouble)
    {
        Date = date;
        LatinString = latinString;
        RussianString = russianString;
        PositiveInt = positiveInt;
        PositiveDouble = positiveDouble;
    }

    public override string ToString() =>
        $"{Date:dd.MM.yyyy}||{LatinString}||{RussianString}||{PositiveInt}||{PositiveDouble:F8}||";

    public override bool Equals(object? obj) =>
        obj is RandomRow rowData && Equals(rowData);

    public bool Equals(RandomRow? other) =>
        other is not null && Id.Equals(other.Id);

    public override int GetHashCode() => Id.GetHashCode();

    public static bool operator ==(RandomRow? left, RandomRow? right) => left?.Equals(right) ?? false;

    public static bool operator !=(RandomRow? left, RandomRow? right) => !(left == right);

    public static RandomRow Create(DateTime date, string latinString, string russianString, int positiveEvenNumber, double positiveNumber) =>
    new(date, latinString, russianString, positiveEvenNumber, positiveNumber)
    {
        Id = Guid.NewGuid()
    };

    public static RandomRow? CreateFromLine(string line)
    {
        var props = line.Split("||");
        if (props.Length != 6)
        {
            return null;
        }

        bool isDate = DateTime.TryParse(props[0], out var date);
        bool isInt = int.TryParse(props[3], out var evenNumber);
        bool isDouble = double.TryParse(props[4], out var positiveDouble);
        if (!isDate || !isInt || !isDouble)
        {
            return null;
        }

        return Create(date, props[1], props[2], evenNumber, positiveDouble);
    }
}
