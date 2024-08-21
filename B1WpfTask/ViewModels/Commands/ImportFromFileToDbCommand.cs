using B1WPFTask.Commands.Base;
using B1WPFTask.Data.Repositories.Interfaces;
using B1WPFTask.Models;
using System.Diagnostics;
using System.IO;

namespace B1WPFTask.Commands;
internal class ImportFromFileToDbCommand : Command.WithParams<ImportFromFileToDbCommand.ExecuteParams>
{
    public static Action<string> OnRowImporting;

    private const int _trackAfterCount = 1000;

    private readonly IRepository<RandomRow> _repository;

    public ImportFromFileToDbCommand(IRepository<RandomRow> repository) =>
        _repository = repository;

    protected override async void Execute(ExecuteParams parameters)
    {
        var elapsedSeconds = await RunWithTimeCounter(async () =>
        {
            var rows = new List<RandomRow>();
            var lineCount = File.ReadLines("CombinedFile").Count();
            var currentLine = 0;
            var lines = File.ReadLines("CombinedFile");
            foreach (var line in lines)
            {
                OnRowImporting.Invoke(StatusMessageForUser(currentLine++, lineCount));

                var dataRow = RandomRow.CreateFromLine(line);
                if (dataRow is null) continue;

                rows.Add(dataRow);
                if (rows.Count == _trackAfterCount)
                {
                    await _repository.AddRangeAsync(rows);
                    rows.Clear();
                }
            }

            await _repository.AddRangeAsync(rows);
        });

        OnRowImporting.Invoke($"Finished in {elapsedSeconds:F3}s");
    }

    protected override bool ValidateParams(ExecuteParams parameters) =>
        true;

    private static string StatusMessageForUser(int currentLine, int countLines)
    {
        double percentage = Math.Round((double)currentLine / countLines * 100, 3);
        return $"{currentLine} / {countLines} -> {percentage}%";
    }

    private static async Task<double> RunWithTimeCounter(Func<Task> func)
    {
        var stopwatch = Stopwatch.StartNew();

        await func?.Invoke();

        stopwatch.Stop();

        return stopwatch.Elapsed.TotalSeconds;
    }

    public record ExecuteParams();
}
