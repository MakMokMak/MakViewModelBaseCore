using System.Collections.Generic;
using System.Windows.Input;
using MakCraft.ViewModels;
using TransitionTestApp.ViewModels.Container;

namespace TransitionTestApp.ViewModels
{
    class SelectWindowViewModel : TransitionViewModelBase
    {
        public SelectWindowViewModel()
        {
            Items = InitList();
        }

        private List<string> _items;
        public List<string> Items
        {
            get { return _items; }
            set
            {
                // NotifyObject.SetProperty メソッドを利用してプロパティの変更・変更通知を行う例
                // SetProperty メソッドの呼び出しは、propertyName の指定を省略しています
                base.SetProperty(ref _items, value);
            }
        }

        private string _selectedItem;
        public string SelectedItem
        {
            get { return _selectedItem; }
            set
            {
                // プロパティの実装コードでプロパティの変更を行い、NotifyObject.RaisePropertyChanged メソッドを
                // 利用してプロパティ変更通知を行う例(プロパティ名の取得に nameof 演算子を利用)
                _selectedItem = value;
                base.RaisePropertyChanged(nameof(SelectedItem));
            }
        }

        protected override void TransitionComplete()
        {
            ((SelectContainer)base.Container).ItemName = SelectedItem;

            base.TransitionComplete();
        }

        private void CompleteExecute()
        {
            TransitionComplete();
        }
        private bool CompleteCanExecute(object param)
        {
            return !string.IsNullOrEmpty(_selectedItem);
        }
        private ICommand _completeCommand;
        public ICommand CompleteCommand
        {
            get
            {
                if (_completeCommand == null)
                    _completeCommand = new RelayCommand(CompleteExecute, CompleteCanExecute);
                return _completeCommand;
            }
        }

        protected override void Dispose(bool disposing)
        {
            System.Diagnostics.Debug.WriteLine("** SelectWindowViewModel disposed.");
            base.Dispose(disposing);
        }

        private List<string> InitList()
        {
            return new List<string> {
                "りんご", "柿", "ぶどう", "みかん"
            };
        }
    }
}
