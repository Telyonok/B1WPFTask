using B1WPFTask.Commands.Base;
using B1WPFTask.Extensions;
using System.IO;

namespace B1WPFTask.Commands;
internal class CombineFilesCommand : Command.WithParams<CombineFilesCommand.ExecuteParams>
{
    public static Action<string> OnCombined;

    protected override void Execute(ExecuteParams parameters)
    {
        var deletedLines = 0;
        using var commonFileWriter = new StreamWriter("CombinedFile");
        var filesToCombine = GetFilesToCombine();
        foreach (var file in filesToCombine)
        {
            using var reader = new StreamReader(file);
            if (parameters.RemoveLinesWithCharacters)
            {
                deletedLines += reader.RemoveAllLinesContaining(parameters.ContainsValue, originalPath: file, onWriting: commonFileWriter.WriteLine);
            }
            else
            {
                reader.ReadLinesWithAction(commonFileWriter.WriteLine);
            }
        }

        OnCombined.Invoke($"Combined. Deleted {deletedLines} rows");
    }

    protected override bool ValidateParams(ExecuteParams parameters) =>
        true;

    private static IEnumerable<string> GetFilesToCombine() =>
        Directory.EnumerateFiles(Directory.GetCurrentDirectory())
            .Select(pathString => Path.GetFileName(pathString))
            .Where(fileName => fileName.StartsWith("RandomRowFile_") &&
                               fileName.EndsWith(".txt"));

    public record ExecuteParams(string ContainsValue, bool RemoveLinesWithCharacters);
}
