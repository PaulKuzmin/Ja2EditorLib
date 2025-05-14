using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;

namespace Ja2StracSaveEditor;

public class SettingsModel : INotifyPropertyChanged
{
    private string _ja2StracPath;
    private string _ja2DataPath;

    [Display(Name = "Ja2Stracciatella directory")]
    public string Ja2StracPath
    {
        get => _ja2StracPath;
        set
        {
            if (value == _ja2StracPath) return;
            _ja2StracPath = value;
            OnPropertyChanged();
        }
    }

    [Display(Name = "Ja2 data directory")]
    public string Ja2DataPath
    {
        get => _ja2DataPath;
        set
        {
            if (value == _ja2DataPath) return;
            _ja2DataPath = value;
            OnPropertyChanged();
        }
    }

    public event PropertyChangedEventHandler PropertyChanged;

    protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}