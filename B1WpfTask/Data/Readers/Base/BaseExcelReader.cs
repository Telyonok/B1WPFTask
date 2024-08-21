using OfficeOpenXml;
using System.IO;

namespace B1WPFTask.Data.Readers.Base;

public abstract class BaseExcelReader<T> : IExcelReader<T> where T : class
{
    public IEnumerable<T> Read(string path)
    {
        var fileInfo = new FileInfo(path);
        if (!fileInfo.Exists)
        {
            throw new FileNotFoundException();
        }

        ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
        using var package = new ExcelPackage(fileInfo);
        var worksheet = package.Workbook.Worksheets[0] ?? 
            throw new InvalidOperationException("File doesn't have any worksheet");

        int rowCount = worksheet.Dimension.Rows;
        int colCount = worksheet.Dimension.Columns;

        return Read(worksheet, rowCount, colCount);
    }

    protected abstract IEnumerable<T> Read(ExcelWorksheet worksheet, int rowCount, int colCount);
}
