using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;
using WeakEventViewModelBaseTestApp.Models;
using WeakEventViewModelBaseTestApp.Services;
using MakCraft.ViewModels;

namespace WeakEventViewModelBaseTestApp.ViewModels
{
    internal class MainWindowViewModel : WeakEventViewModelBase
    {
        private readonly IItemService _itemService;

        public MainWindowViewModel() : this(ItemService.GetInstance()) { }
        public MainWindowViewModel(IItemService itemService)
        {
            _itemService = itemService;
            PropertyChangedEventManager.AddListener(_itemService, this, nameof(IItemService.Item01));
            PropertyChangedEventManager.AddListener(_itemService, this, nameof(IItemService.Item02));
        }

        private string _item01;
        public string Item01
        {
            get => _item01;
            set => base.SetProperty(ref _item01, value);
        }

        private string _item02;
        public string Item02
        {
            get => _item02;
            set => SetProperty(ref _item02, value);
        }

        public ObservableCollection<SampleModel> SampleModels
        {
            get => _itemService.SampleModels;
        }

        protected override void OnReceivedPropertyChangeNotification(Type managerType, object sender, EventArgs e)
        {
            // PropertyChangedEventManager からのイベント通知であることを確認
            if (managerType != typeof(PropertyChangedEventManager))
            {
                return;
            }

            // イベントソースが IItemService であることを確認
            if (!(sender is IItemService service))
            {
                return;
            }

            // PropertyChangedEventArgs であることを確認
            if (!(e is PropertyChangedEventArgs eventArgs))
            {
                return;
            }
            string newValue;
            switch (eventArgs.PropertyName)
            {
                case nameof(IItemService.Item01):
                    newValue = service.Item01;
                    if (_item01 != newValue)
                    {
                        Item01 = newValue;
                    }
                    break;
                case nameof(IItemService.Item02):
                    newValue = service.Item02;
                    if (_item02 != newValue)
                    {
                        Item02 = newValue;
                    }
                    break;
                default:
                    throw new NotImplementedException(eventArgs.PropertyName);
            }
        }

        private void ShowEditWindowExecute()
        {
            var window = new ModalWindow01();
            window.ShowDialog();
        }
        private ICommand _showEditWindowCommand;
        public ICommand ShowEditWindowCommand
        {
            get
            {
                if (_showEditWindowCommand == null)
                {
                    _showEditWindowCommand = new RelayCommand(ShowEditWindowExecute);
                }
                return _showEditWindowCommand;
            }
        }

        private void GcExecute()
        {
            Mouse.OverrideCursor = Cursors.Wait;
            System.GC.Collect();
            System.GC.WaitForPendingFinalizers();
            System.GC.Collect();
            Mouse.OverrideCursor = null;
        }
        private ICommand _gcCommand;
        public ICommand GcCommand
        {
            get
            {
                if (_gcCommand == null)
                {
                    _gcCommand = new RelayCommand(GcExecute);
                }
                return _gcCommand;
            }
        }

        ~MainWindowViewModel()
        {
            System.Diagnostics.Debug.WriteLine($"[{DateTime.Now}] Destruct MainWindowViewModel.");
        }
    }
}
