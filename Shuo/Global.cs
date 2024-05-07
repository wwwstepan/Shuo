using System.Reflection;
using System.Text.Json;

namespace Shuo;

public static class Global
{
    private static List<LearnWord> _allWords;
    public static List<LearnWord> AllWords { get => _allWords ??= ReadWords(); }
    public static int QuantityWords;
    public static LearnMode LearnMode { get; set; }

    public static List<LearnWord> ReadWords()
    {
        var assembly = Assembly.GetExecutingAssembly();

        const string resourceName = "Shuo.Resources.words.json";
        using Stream stream = assembly.GetManifestResourceStream(resourceName);
        if (stream is null)
            throw new Exception($"{resourceName} not found");
        using StreamReader reader = new(stream);
        if (reader is null)
            throw new Exception($"Can't read {resourceName}");

        string result = reader.ReadToEnd();
        var wordsRaw = JsonSerializer.Deserialize<List<List<string>>>(result);

        if (wordsRaw is null || wordsRaw.Count < 1)
            throw new Exception($"Error in {resourceName}");

        var allWords = new List<LearnWord>(wordsRaw.Count);

        foreach (var pair in wordsRaw)
        {
            if (pair.Count == 2 && !string.IsNullOrWhiteSpace(pair[0]) && !string.IsNullOrWhiteSpace(pair[1]))
                allWords.Add(new LearnWord { Ru = pair[0], Ch = pair[1] });
        }

        return allWords;
    }
}
