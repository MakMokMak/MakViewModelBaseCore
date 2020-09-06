using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows.Input;
using WeakEventViewModelBaseTestApp.Models;
using WeakEventViewModelBaseTestApp.Services;
using MakCraft.ViewModels;

namespace WeakEventViewModelBaseTestApp.ViewModels
{
    internal class ModalWindow01ViewModel : WeakEventViewModelBase
    {
        private readonly IItemService _itemService;

        public ModalWindow01ViewModel() : this(ItemService.GetInstance()) { }
        public ModalWindow01ViewModel(IItemService itemService)
        {
            _itemService = itemService;
            PropertyChangedEventManager.AddListener(_itemService, this, nameof(IItemService.Item01));
            PropertyChangedEventManager.AddListener(_itemService, this, nameof(IItemService.Item02));
            Item01 = _itemService.Item01;
            Item02 = _itemService.Item02;
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

        public ObservableCollection<SampleModel> SampleModels => _itemService.SampleModels;

        protected override bool OnReceiveWeakEventNotification(Type managerType, object sender, EventArgs e)
        {
            // PropertyChangedEventManager からのイベント通知であることを確認
            if (managerType != typeof(PropertyChangedEventManager))
            {
                return false;
            }

            // イベントソースが IItemService であることを確認
            if (!(sender is IItemService service))
            {
                return false;
            }

            // PropertyChangedEventArgs であることを確認
            if (!(e is PropertyChangedEventArgs eventArgs))
            {
                return false;
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

            return true;
        }

        #region Edit Item

        private string _editItem01;
        public string EditItem01
        {
            get => _editItem01;
            set => base.SetProperty(ref _editItem01, value);
        }

        private string _editItem02;
        public string EditItem02
        {
            get => _editItem02;
            set => base.SetProperty(ref _editItem02, value);
        }

        private void EditItemExecute()
        {
            if (_editItem01 != _item01)
            {
                _itemService.Item01 = _editItem01;
            }
            if (_editItem02 != _item02)
            {
                _itemService.Item02 = _editItem02;
            }
        }
        private ICommand _editItemCommand;
        public ICommand EditItemCommand
        {
            get
            {
                if (_editItemCommand == null)
                {
                    _editItemCommand = new RelayCommand(EditItemExecute);
                }
                return _editItemCommand;
            }
        }

        #endregion

        #region Edit SampleModel

        private void EditSampleModelExecute()
        {
            var targetKey = 0;
            var target = _itemService.SampleModels.FirstOrDefault(p => p.Id == targetKey);
            if (target == null)
            {
                throw new KeyNotFoundException($"Key: {targetKey}");
            }
            target.Created = DateTime.Now;
            target.Text = $"{target.Text} modified.";
        }

        private ICommand _editSampleCommand;
        public ICommand EditSampleCommand
        {
            get
            {
                if (_editSampleCommand == null)
                {
                    _editSampleCommand = new RelayCommand(EditSampleModelExecute);
                }
                return _editSampleCommand;
            }
        }

        #endregion

        #region Add SampleModel

        private void AddSampleModelExecute()
        {
            var dt = DateTime.Now;
            var model = new SampleModel
            {
                Created = dt,
                Text = $"SampleModel{dt}",
            };
            _itemService.Add(model);
        }
        private ICommand _addSampleCommand;
        public ICommand AddSampleCommand
        {
            get
            {
                if (_addSampleCommand == null)
                {
                    _addSampleCommand = new RelayCommand(AddSampleModelExecute);
                }
                return _addSampleCommand;
            }
        }

        #endregion

        ~ModalWindow01ViewModel()
        {
            System.Diagnostics.Debug.WriteLine($"[{DateTime.Now}] Destruct ModalWindow01ViewModel.");
        }
    }
}
