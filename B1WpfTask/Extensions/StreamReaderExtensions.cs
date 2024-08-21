using System.IO;

namespace B1WPFTask.Extensions;
public static class StreamReaderExtensions
{
    public static int RemoveAllLinesContaining(this StreamReader reader, string subString, string originalPath, Action<string> onWriting = null)
    {
        var deletedLines = 0;
        var tempFilePath = $"Tmp_{Guid.NewGuid()}.txt";
        using (var writer = new StreamWriter(tempFilePath))
        {
            while (!reader.EndOfStream)
            {
                var line = reader.ReadLine();
                if (line.Contains(subString))
                {
                    deletedLines++;
                    continue;
                }

                writer.WriteLine(line);
                onWriting.Invoke(line);
            }
        }

        reader.Close();

        File.Delete(originalPath);
        File.Move(tempFilePath, originalPath);

        return deletedLines;
    }

    public static void ReadLinesWithAction(this StreamReader reader, Action<string> onLineReaded = null)
    {
        while (!reader.EndOfStream)
        {
            var line = reader.ReadLine();
            onLineReaded.Invoke(line);
        }
    }
}
