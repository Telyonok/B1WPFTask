using B1WPFTask.Commands.Base;
using B1WPFTask.Data.Readers.Base;
using B1WPFTask.Data.Repositories.Interfaces;
using B1WPFTask.Models;
using System.IO;

namespace B1WPFTask.Commands;
internal class ImportExcelToDbCommand : Command.WithParams<ImportExcelToDbCommand.ExecuteParams>
{
    public static Action<string> OnImportFinished;

    private readonly IExcelReader<BankAccountClass> _excelReader;
    private readonly IRepository<InputFile> _repository;

    public ImportExcelToDbCommand(IExcelReader<BankAccountClass> excelReader, IRepository<InputFile> repository)
    {
        _excelReader = excelReader;
        _repository = repository;
    }

    protected override async void Execute(ExecuteParams parameters)
    {
        var accountClasses = _excelReader.Read(parameters.Path);
        var inputFile = new InputFile
        {
            Id = Guid.NewGuid(),
            FileName = Path.GetFileName(parameters.Path),
            AccountClasses = accountClasses.ToList()
        };
        await _repository.AddAsync(inputFile);
        OnImportFinished.Invoke("Finished import");
    }

    protected override bool ValidateParams(ExecuteParams parameters) =>
        !string.IsNullOrWhiteSpace(parameters.Path);

    public record ExecuteParams(string Path);
}
