using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Reflection;
using System.Text.Json;

namespace Shuo;

public partial class AppModel : ObservableObject
{
    [ObservableProperty]
    private AppState _appState;

    private LearnMode _learnMode;
    public LearnMode LearnMode
    {
        get => _learnMode;
        set
        {
            SetProperty(ref _learnMode, value);
            OnPropertyChanged(nameof(ButtonLearnModeText));
        }
    }

    private LearnVolume _learnVolume;
    public LearnVolume LearnVolume
    {
        get => _learnVolume;
        set {
            SetProperty(ref _learnVolume, value);
            OnPropertyChanged(nameof(ButtonLearnVolumeText));
        }
    }

    public string ButtonLearnModeText => LearnMode == LearnMode.ChinaFirst ? "Китайский\n->\nРусский" : "Русский\n->\nКитайский";
    public string ButtonLearnVolumeText => $"{(int)LearnVolume} слов";

    public AppModel()
    {
        AppState = AppState.Customize;
        LearnMode = LearnMode.RussianFirst;
        LearnVolume = LearnVolume.Words10;
    }

    [RelayCommand]
    public async Task StartLearning()
    {
        Global.LearnMode = LearnMode;
        Global.QuantityWords = (int)LearnVolume;
        AppState = AppState.Learn;
        await Shell.Current.GoToAsync(nameof(LearnPage));
    }

    [RelayCommand]
    public void NextLearnMode() => LearnMode = LearnMode.NextEnum();

    [RelayCommand]
    public void NextLearnVolume() => LearnVolume = LearnVolume.NextEnum();

    public class WordsJsonModel
    {
        public List<List<string>>? Words { get; set; }
    }
}
