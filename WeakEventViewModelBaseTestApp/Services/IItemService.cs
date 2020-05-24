using System.Collections.ObjectModel;
using System.ComponentModel;
using WeakEventViewModelBaseTestApp.Models;

namespace WeakEventViewModelBaseTestApp.Services
{
    interface IItemService : INotifyPropertyChanged
    {
        string Item01 { get; set; }
        string Item02 { get; set; }
        ObservableCollection<SampleModel> SampleModels { get; }

        void Add(SampleModel sampleModel);
        void Update(SampleModel sampleModel);
    }
}
