using B1WPFTask.Commands;
using B1WPFTask.Data.Readers.Base;
using B1WPFTask.Data.Repositories.Interfaces;
using B1WPFTask.Models;
using B1WPFTask.ViewModels.Base;
using Microsoft.EntityFrameworkCore;
using Microsoft.Win32;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace B1WPFTask.ViewModels;
internal class MainWindowViewModel : BaseViewModel
{

    #region Parameters
    /// <summary>
    /// Window title
    /// </summary>
    private string _Title = "Main window";

    /// <summary>
    /// Window title
    /// </summary>
    public string Title
    {
        get => _Title;
        set => Set(ref _Title, value);
    }

    /// <summary>
    /// File count to generate
    /// </summary>
    private int _FileCount = 100;

    /// <summary>
    /// File count to generate
    /// </summary>
    public int FileCount
    {
        get => _FileCount;
        set
        {
            if (Set(ref _FileCount, value))
            {
                OnPropertyChanged(nameof(GenerateFilesCommandParams));
            }
        }
    }

    /// <summary>
    /// Row count per file
    /// </summary>
    private int _RowsPerFileCount = 100;

    /// <summary>
    /// Row coount per file
    /// </summary>
    public int RowsPerFileCount
    {
        get => _RowsPerFileCount;
        set
        {
            if (Set(ref _RowsPerFileCount, value))
            {
                OnPropertyChanged(nameof(GenerateFilesCommandParams));
            }
        }
    }

    /// <summary>
    /// Generate files command params
    /// </summary>
    public GenerateFilesCommand.ExecuteParams GenerateFilesCommandParams =>
        new(FileCount, RowsPerFileCount);

    /// <summary>
    /// Contains value for removing lines
    /// </summary>
    private string _ContainsValue = "";

    /// <summary>
    /// Contains value for removing lines
    /// </summary>
    public string ContainsValue
    {
        get => _ContainsValue;
        set
        {
            if (Set(ref _ContainsValue, value))
            {
                OnPropertyChanged(nameof(CombineFilesCommandParams));
            }
        }
    }

    /// <summary>
    /// Flag to remove / not remove lines with value
    /// </summary>
    private bool _RemoveLinesWithCharacters = true;

    /// <summary>
    /// Flag to remove / not remove lines with value
    /// </summary>
    public bool RemoveLinesWithCharacters
    {
        get => _RemoveLinesWithCharacters;
        set
        {
            if (Set(ref _RemoveLinesWithCharacters, value))
            {
                OnPropertyChanged(nameof(CombineFilesCommandParams));
            }
        }
    }

    /// <summary>
    /// Params to execute combine files command
    /// </summary>
    public CombineFilesCommand.ExecuteParams CombineFilesCommandParams =>
        new(ContainsValue, RemoveLinesWithCharacters);

    /// <summary>
    /// Params to execute import from file to db command
    /// </summary>
    public ImportFromFileToDbCommand.ExecuteParams ImportFromFileToDbCommandParams =>
        new();

    /// <summary>
    /// Excel file path
    /// </summary>
    private string _ExcelFilePath;

    /// <summary>
    /// Excel file path
    /// </summary>
    public string ExcelFilePath
    {
        get => _ExcelFilePath;
        set
        {
            if (Set(ref _ExcelFilePath, value))
            {
                OnPropertyChanged(nameof(ImportExcelToDbCommandParams));
            }
        }
    }

    /// <summary>
    /// Params to execute excel import to db command
    /// </summary>
    public ImportExcelToDbCommand.ExecuteParams ImportExcelToDbCommandParams =>
        new(ExcelFilePath);

    /// <summary>
    /// Loaded files
    /// </summary>
    private List<InputFile> _InputFiles;

    /// <summary>
    /// Loaded files
    /// </summary>
    public List<InputFile> InputFiles
    {
        get => _InputFiles;
        set => Set(ref _InputFiles, value);
    }
    
    /// <summary>
    /// Selected input file
    /// </summary>
    private InputFile _SelectedInputFile;

    /// <summary>
    /// Selected input file
    /// </summary>
    public InputFile SelectedInputFile
    {
        get => _SelectedInputFile;
        set => Set(ref _SelectedInputFile, value);
    }

    /// <summary>
    /// Selected tab item index
    /// </summary>
    private int _SelectedTabItemIndex;

    /// <summary>
    /// Selected tab item index
    /// </summary>
    public int SelectedTabItemIndex
    {
        get => _SelectedTabItemIndex;
        set => Set(ref _SelectedTabItemIndex, value);
    }

    /// <summary>
    /// Selected subtab item index
    /// </summary>
    private int _SelectedSubTabItemIndex;

    /// <summary>
    /// Selected subtab item index
    /// </summary>
    public int SelectedSubTabItemIndex
    {
        get => _SelectedSubTabItemIndex;
        set => Set(ref _SelectedSubTabItemIndex, value);
    }

    /// <summary>
    /// Selected input file
    /// </summary>
    private InputFile _ReadingInputFile;

    /// <summary>
    /// Selected input file
    /// </summary>
    public InputFile ReadingInputFile
    {
        get => _ReadingInputFile;
        set => Set(ref _ReadingInputFile, value);
    }

    /// <summary>
    /// Selected bank account class of the selected input file
    /// </summary>
    private BankAccountClass _SelectedBankAccountClass;

    /// <summary>
    /// Selected bank account class of the selected input file
    /// </summary>
    public BankAccountClass SelectedBankAccountClass
    {
        get => _SelectedBankAccountClass;
        set => Set(ref _SelectedBankAccountClass, value);
    }

    /// <summary>
    /// Selected class accounts
    /// </summary>
    private ObservableCollection<BankAccount> _SelectedClassAccounts = new();

    /// <summary>
    /// Selected class accounts
    /// </summary>
    public ObservableCollection<BankAccount> SelectedClassAccounts
    {
        get => _SelectedClassAccounts;
        set => Set(ref _SelectedClassAccounts, value);
    }

    #endregion

    private readonly IRepository<RandomRow> _randomRowRepository;
    private readonly IRepository<BankAccount> _bankAccountRepository;
    private readonly IRepository<InputFile> _inputFileRepository;
    private readonly IExcelReader<BankAccountClass> _excelReader;

    public MainWindowViewModel(IRepository<RandomRow> randomRowRepository, IExcelReader<BankAccountClass> excelReader, IRepository<InputFile> inputFileRepository, IRepository<BankAccount> bankAccountRepository)
    {
        _randomRowRepository = randomRowRepository;
        _bankAccountRepository = bankAccountRepository;
        _inputFileRepository = inputFileRepository;
        _excelReader = excelReader;
        SubscribeOnEvents();
        GetLoadedFilesCommand.Execute(null);
    }

    #region Commands
    public ICommand GenerateFilesCommand => new GenerateFilesCommand();

    public ICommand CombineFilesCommand => new CombineFilesCommand();

    public ICommand ImportFromFileToDbCommand => new ImportFromFileToDbCommand(_randomRowRepository);

    public ICommand ImportExcelToDbCommand => new ImportExcelToDbCommand(_excelReader, _inputFileRepository);


    public ICommand GetFileDataCommand => new LambdaCommand(OnGetFileDataCommandExecutedAsync);

    public async void OnGetFileDataCommandExecutedAsync(object p)
    {
        ReadingInputFile = await _inputFileRepository.Get(file => file.Id == SelectedInputFile.Id)
            .Include(file => file.AccountClasses)
            .FirstOrDefaultAsync();

        SelectedTabItemIndex = 1;
    }

    public ICommand GetAccountByClassCommand => new LambdaCommand(OnGetAccountByClassCommandExecuted, CanGetAccountByClassCommandExecute);

    public bool CanGetAccountByClassCommandExecute(object p) => SelectedBankAccountClass is not null;

    public async void OnGetAccountByClassCommandExecuted(object p)
    {
        var selectedClassAccounts = await _bankAccountRepository
            .Get(account => account.Class == SelectedBankAccountClass)
            .OrderBy(account => account.Number)
            .Include(account => account.InputBalance)
            .Include(account => account.OutputBalance)
            .Include(account => account.Turnover)
            .ToListAsync();

        SelectedClassAccounts = new ObservableCollection<BankAccount>(selectedClassAccounts);
    }

    public ICommand ChooseExcelFileCommand => new LambdaCommand(OnChooseExcelFileCommandExecuted);

    public void OnChooseExcelFileCommandExecuted(object p)
    {
        var openFileDialog = new OpenFileDialog
        {
            Filter = "Excel Files|*.xls;*.xlsx|All Files|*.*",
            Title = "Select an Excel File"
        };

        if (!openFileDialog.ShowDialog() ?? false)
        {
            return;
        }

        ExcelFilePath = openFileDialog.FileName;
    }

    public ICommand GetLoadedFilesCommand => new LambdaCommand(OnGetLoadedFilesCommandExecuted);

    public void OnGetLoadedFilesCommandExecuted(object p)
    {
        InputFiles = _inputFileRepository.GetAll().OrderBy(file => file.FileName).ToList();
    }

    #endregion

    private void SetStatusToTitle(string title)
    {
        Title = title;
    }

    private void SubscribeOnEvents()
    {
        Commands.GenerateFilesCommand.OnGenerated += SetStatusToTitle;
        Commands.CombineFilesCommand.OnCombined += SetStatusToTitle;
        Commands.ImportFromFileToDbCommand.OnRowImporting += SetStatusToTitle;
        Commands.ImportExcelToDbCommand.OnImportFinished += SetStatusToTitle;
    }
}
