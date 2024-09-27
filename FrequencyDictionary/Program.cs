var frequencyDictionaryLogic = new FrequencyDictionaryLogic();
await frequencyDictionaryLogic.RunAsync(args);


public class FrequencyDictionaryLogic
{
    /// <summary>
    /// Запустить программу для вычисления частотного словаря для заданного текстового файла
    /// </summary>
    internal async Task RunAsync(string[] args, CancellationToken cancellationToken = default)
    {
        try
        {
        Start:
            string? pathInput = null;
            if (args.Length == 0)
            {
                 pathInput = GetInputPath();
            }
            else
            {
                if (ValidatePath(args[0]))
                {
                    pathInput = args[0];
                }
            }

            if (pathInput == null)
            {
                goto Start;
            }

        Output:

            string? pathOutput = null;
            if (args.Length <= 1)
            {
                pathOutput = GetOutputPath();
            }
            else
            {
                if (ValidatePath(args[1]))
                {
                    pathOutput = args[1];
                }
            }

            if (pathOutput == null)
            {
                goto Output;
            }

            var text = await File.ReadAllTextAsync(pathInput, cancellationToken);
            var dict = Calculate(text);
            await WriteToFileAsync(pathOutput, dict, cancellationToken);
            Console.ReadLine();

        }
        catch (Exception e)
        {
            Console.BackgroundColor = ConsoleColor.Red;
            Console.WriteLine($"Произошла ошибка при выполнении программы - {e.Message}");
            Console.ResetColor();
        }
    }

    /// <summary>
    /// Получить путь к файлу для разбора 
    /// </summary>
    private static string? GetInputPath()
    {
        Console.WriteLine("Введите путь к файлу с текстом ");
        var pathInput = Console.ReadLine();
        if (!ValidatePath(pathInput))
        {
            return null;
        }
        if (!Path.Exists(pathInput))
        {
            Console.BackgroundColor = ConsoleColor.Yellow;
            Console.WriteLine("Не существует указанного файла");
            Console.ResetColor();
            return null;
        }

        return pathInput;
    }

    /// <summary>
    /// Валидация пути к файлу
    /// </summary>
    public static bool ValidatePath(string? path)
    {
        if (string.IsNullOrEmpty(path))
        {
            Console.BackgroundColor = ConsoleColor.Yellow;
            Console.WriteLine("Путь к файлу не может быть пустым");
            Console.ResetColor();
            return false;
        }
        if (Path.GetExtension(path) != ".txt")
        {
            Console.BackgroundColor = ConsoleColor.Yellow;
            Console.WriteLine("Расширение файла должно быть .txt");
            Console.ResetColor();
            return false;
        }
        return true;
    }

    /// <summary>
    /// Получить путь к файлу для записи 
    /// </summary>
    private static string? GetOutputPath()
    {
        Console.WriteLine("Введите путь для сохранения результата ");
        var pathOutput = Console.ReadLine();
        if (!ValidatePath(pathOutput))
        {
            return null;
        }
        return pathOutput;
    }

    /// <summary>
    /// Записать в указанный файл
    /// </summary>
    private static async Task WriteToFileAsync(string pathOutput, Dictionary<string, int> dictionary,  CancellationToken cancellationToken = default)
    {
        await using (StreamWriter writer = File.CreateText(pathOutput))
        {

            foreach (KeyValuePair<string, int> pair in dictionary.OrderByDescending(x => x.Value))
            {
                var result = $"{pair.Key} : {pair.Value}";
                await writer.WriteLineAsync(result.ToCharArray(), cancellationToken);
            }
        }

        Console.WriteLine("Запись завершена");
    }

    /// <summary>
    /// Рассчитать вхождение слов
    /// </summary>
    public static Dictionary<string, int> Calculate(string text)
    {
        var textReplace = text.Replace("\r\n", " ");
        var words = textReplace.Split(' ', StringSplitOptions.RemoveEmptyEntries);
        Dictionary<string, int> dictionary = [];
        foreach (string word in words)
        {
            var str = word.ToLower();
            if (dictionary.ContainsKey(str))
            {
                dictionary[str]++;
            }
            else
            {
                dictionary.Add(str, 1);
            }
        }
        Console.WriteLine("Всего слов : {0} ", dictionary.Count);
        return dictionary;

    }
}