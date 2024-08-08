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

    [ObservableProperty]
    private bool _showTranslate;

    [ObservableProperty]
    private int _currenWordIndex;

    [ObservableProperty]
    private int _learnTime;

    [ObservableProperty]
    private string _currenWord = string.Empty;

    [ObservableProperty]
    private string _currenTranslate = string.Empty;

    public string NextButtonText => ShowTranslate ? "Следующее слово" : "Показать перевод";

    [ObservableProperty]
    private bool _nextButtonIsVisible;

    public LearnModel()
    {
        timer = new Timer(1000);
        timer.Elapsed += TimerElapsed;
        StartLearning();
    }

    [RelayCommand]
    public void NextWord()
    {
        if (!ShowTranslate && CurrenWordIndex >= learnWords.Count - 1)
        {
            timer.Stop();
            ShowTranslate = true;
            NextButtonIsVisible = false;
            return;
        }

        if (ShowTranslate)
        {
            CurrenWordIndex++;
            GetWord();
        }

        ShowTranslate = !ShowTranslate;

        OnPropertyChanged(nameof(NextButtonText));
    }

    private void StartLearning()
    {
        SetLearnWords();

        LearnMode = Global.LearnMode;
        LearnTime = 0;
        CurrenWordIndex = 0;
        NextButtonIsVisible = true;
        ShowTranslate = false;

        GetWord();
        
        timer.Start();
    }

    [RelayCommand]
    public async Task StopLearning()
    {
        timer.Stop();
        await GoHome();
    }

    private void GetWord()
    {
        if (LearnMode == LearnMode.ChinaFirst)
        {
            CurrenWord = learnWords[CurrenWordIndex].Ch;
            CurrenTranslate = learnWords[CurrenWordIndex].Ru;
        }
        else
        {
            CurrenWord = learnWords[CurrenWordIndex].Ru;
            CurrenTranslate = learnWords[CurrenWordIndex].Ch;
        }
    }

    private void TimerElapsed(object? sender, System.Timers.ElapsedEventArgs e)
    {
        LearnTime++;
    }

    public async Task GoHome() => await Shell.Current.GoToAsync("../../route");

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
