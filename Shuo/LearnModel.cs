using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Timer = System.Timers.Timer;

namespace Shuo;

public partial class LearnModel : ObservableObject
{
    public LearnMode LearnMode;
    public List<LearnWord> learnWords = new();

    private readonly Random rnd = new();
    private readonly Timer timer;

    private bool isFirstWord;

    [ObservableProperty]
    private int _currenWordIndex;

    [ObservableProperty]
    private int _learnTime;

    [ObservableProperty]
    private string _currenWord = string.Empty;

    public string NextButtonText => isFirstWord ? "Показать\nперевод" : "Следующее\nслово";

    public bool NextButtonIsVisible => !(CurrenWordIndex >= learnWords.Count - 1 && !isFirstWord);

    public LearnModel()
    {
        timer = new Timer(1000);
        timer.Elapsed += TimerElapsed;
        StartLearning();
    }

    private void TimerElapsed(object? sender, System.Timers.ElapsedEventArgs e)
    {
        LearnTime++;
    }

    public async Task GoHome() => await Shell.Current.GoToAsync("../../route");

    [RelayCommand]
    public void NextWord()
    {
        isFirstWord = !isFirstWord;

        CurrenWord = LearnMode == LearnMode.ChinaFirst
            ? (isFirstWord
                ? learnWords[CurrenWordIndex].Ch
                : learnWords[CurrenWordIndex].Ru)
            : (isFirstWord
                ? learnWords[CurrenWordIndex].Ru
                : learnWords[CurrenWordIndex].Ch);

        if (!isFirstWord)
            CurrenWordIndex++;

        if (CurrenWordIndex >= learnWords.Count - 1 && !isFirstWord)
            timer.Stop();

        OnPropertyChanged(nameof(NextButtonText));
        OnPropertyChanged(nameof(NextButtonIsVisible));
    }

    private void StartLearning()
    {
        SetLearnWords();

        LearnMode = Global.LearnMode;
        LearnTime = 0;
        CurrenWordIndex = 0;
        isFirstWord = false;

        NextWord();
        
        timer.Start();
    }

    [RelayCommand]
    public async Task StopLearning()
    {
        timer.Stop();
        await GoHome();
    }

    private void SetLearnWords()
    {
        if (Global.AllWords.Count < Global.QuantityWords)
            throw new Exception("В словаре слишком мало слов");

        var idxs = new int[Global.QuantityWords];

        if (learnWords is not null && learnWords.Capacity == Global.QuantityWords)
            learnWords.Clear();
        else
            learnWords = new List<LearnWord>(Global.QuantityWords);

        for (int i = 0; i < Global.QuantityWords; i++)
        {
            int idx;
            do
            {
                idx = rnd.Next(0, Global.AllWords.Count - 1);
            } while (idxs.Contains(idx));
            idxs[i] = idx;
            learnWords.Add(Global.AllWords[idx]);
        }
    }
}
