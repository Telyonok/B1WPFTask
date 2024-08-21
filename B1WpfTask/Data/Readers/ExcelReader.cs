using B1WPFTask.Data.Readers.Base;
using B1WPFTask.Models;
using OfficeOpenXml;
using System.Text.RegularExpressions;

namespace B1WPFTask.Data.Readers;
internal class ExcelReader : BaseExcelReader<BankAccountClass>
{
    private const string _classPattern = @"(?<Number>\d+)\s*(?<Name>.*)";

    protected override IEnumerable<BankAccountClass> Read(ExcelWorksheet worksheet, int rowCount, int colCount)
    {
        var accountClasses = new List<BankAccountClass>();
        for (int row = 9; row < rowCount - 3; row++)
        {
            var isBold = worksheet.Cells[row, 1].IsBold();
            if (isBold)
            {
                var isTotalOrSubTotal = worksheet.Cells[row, 1].IsTotalOrSubTotal();
                if (isTotalOrSubTotal) continue;

                var cellValue = worksheet.Cells[row, 1].Text;
                var match = Regex.Match(cellValue, _classPattern);
                if (match.Success)
                {
                    var classNumber = int.Parse(match.Groups["Number"].Value);
                    var className = match.Groups["Name"].Value.Trim();
                    accountClasses.Add(BankAccountClass.Create(classNumber, className));
                }
                continue;
            }

            var account = ParseRow(worksheet, row, accountClasses[^1]);
            accountClasses[^1].BankAccounts.Add(account);
        }

        return accountClasses;
    }

    private static BankAccount ParseRow(ExcelWorksheet worksheet, int row, BankAccountClass bankAccountClass)
    {
        var inputBalance = new InputBalance
        {
            Id = Guid.NewGuid(),
            Active = decimal.Parse(worksheet.Cells[row, 2].Text),
            Passive = decimal.Parse(worksheet.Cells[row, 3].Text)
        };
        var turnover = new Turnover
        {
            Id = Guid.NewGuid(),
            Debit = decimal.Parse(worksheet.Cells[row, 4].Text),
            Credit = decimal.Parse(worksheet.Cells[row, 5].Text)
        };
        var outputBalance = new OutputBalance
        {
            Id = Guid.NewGuid(),
            Active = decimal.Parse(worksheet.Cells[row, 6].Text),
            Passive = decimal.Parse(worksheet.Cells[row, 7].Text)
        };

        var accountNumber = int.Parse(worksheet.Cells[row, 1].Text);

        return new BankAccount
        {
            Id = Guid.NewGuid(),
            Number = accountNumber,
            InputBalance = inputBalance,
            Turnover = turnover,
            OutputBalance = outputBalance,
            Class = bankAccountClass
        };
    }
}

file static class ExcelRangeExtensions
{
    public static bool IsBold(this ExcelRange range) =>
        range.Style.Font.Bold;

    public static bool IsTotalOrSubTotal(this ExcelRange range) =>
        range.Text.Length == 2 && int.TryParse(range.Text, out var _) ||
        range.Text.Contains("ПО КЛАССУ") ||
        range.Text.Contains("БАЛАНС");
}