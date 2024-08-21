using B1WPFTask.Commands.Base;
using B1WPFTask.Extensions;
using B1WPFTask.Models;
using System.IO;

namespace B1WPFTask.Commands;
internal class GenerateFilesCommand : Command.WithParams<GenerateFilesCommand.ExecuteParams>
{
    public static Action<string> OnGenerated;

    protected override void Execute(ExecuteParams parameters)
    {
        var random = new Random();
        var currentFileNumber = 1;
        for (int i = 0; i < parameters.FileCount; i++)
        {
            using var streamWriter = new StreamWriter(File.OpenWrite($"RandomRowFile_{currentFileNumber++}.txt"));
            for (int j = 0; j < parameters.RowsPerFileCount; j++)
            {
                var randomRowData = GetRandomRowData(random);
                streamWriter.WriteLine(randomRowData);
            }
        }

        OnGenerated.Invoke("Generated");
    }

    protected override bool ValidateParams(ExecuteParams parameters) =>
        parameters.RowsPerFileCount > 0 &&
        parameters.FileCount > 0;

    private static RandomRow GetRandomRowData(Random random)
    {
        var date = random.DateInLastYears(5);
        var latinString = random.LatinString(10);
        var russianString = random.RussianString(10);
        var intNumber = random.PositiveIntInRange(2..100_000_000);
        var doubleNumber = random.PositiveDoubleInRange(1..20, 8);

        var randomRowData = new RandomRow(date, latinString, russianString, intNumber, doubleNumber);
        return randomRowData;
    }

    public record ExecuteParams(int FileCount, int RowsPerFileCount);
}
