namespace B1WPFTask.Data.Readers.Base;
public interface IExcelReader<T> where T : class
{
    IEnumerable<T> Read(string path);
}
