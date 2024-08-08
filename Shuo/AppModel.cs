using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace Shuo;

public partial class AppModel : ObservableObject
{
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

    public string ButtonLearnModeText => LearnMode == LearnMode.ChinaFirst
        ? "Китайский → Русский" : "Русский → Китайский";

    public string ButtonLearnVolumeText => $"{(int)LearnVolume} слов";

    public AppModel()
    {
        LearnMode = LearnMode.ChinaFirst;
        LearnVolume = LearnVolume.Words10;
    }

    [RelayCommand]
    public async Task StartLearning()
    {
        Global.LearnMode = LearnMode;
        Global.QuantityWords = (int)LearnVolume;
        await Shell.Current.GoToAsync(nameof(LearnPage));
    }

    [RelayCommand]
    public void NextLearnMode() => LearnMode = LearnMode.NextEnum();

    [RelayCommand]
    public void NextLearnVolume() => LearnVolume = LearnVolume.NextEnum();
}
