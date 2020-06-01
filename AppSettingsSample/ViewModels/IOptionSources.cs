using System.ComponentModel;

namespace AppSettingsSample.ViewModels
{
    interface IOptionSources : INotifyPropertyChanged
    {
        double FontSize { get; set; }
        double FontSizeMagnification { get; set; }
    }
}
