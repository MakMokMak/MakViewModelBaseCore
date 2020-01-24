using System;
using System.Windows.Input;
using MakCraft.ViewModels;
using TransitionTestApp.ViewModels.Container;

namespace TransitionTestApp.ViewModels
{
    class AfterSelectWindowViewModel : TransitionViewModelBase
    {
        public AfterSelectWindowViewModel()
        {
        }

        private string _selectedItem;
        public string SelectedItem
        {
            get { return _selectedItem; }
            set
            {
                base.SetProperty(ref _selectedItem, value);
            }
        }

        private void CompleteExecute()
        {
            base.TransitionComplete();
        }
        private ICommand _completeCommand;
        public ICommand CompleteCommand
        {
            get
            {
                if (_completeCommand == null)
                    _completeCommand = new RelayCommand(CompleteExecute);
                return _completeCommand;
            }
        }

        private void GcExecute()
        {
            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();
        }
        private ICommand _gcCommand;
        public ICommand GcCoomand
        {
            get
            {
                if (_gcCommand == null)
                    _gcCommand = new RelayCommand(GcExecute);
                return _gcCommand;
            }
        }

        // MainWindowViewModel から渡されてきたデータをセット
        protected override void OnContainerReceived(object container)
        {
            SelectedItem = ((SelectContainer)container).ItemName;

            base.OnContainerReceived(container);
        }

        protected override void Dispose(bool disposing)
        {
            System.Diagnostics.Debug.WriteLine("** AfterSelectWindowViewModel disposed.");
            base.Dispose(disposing);
        }

    }
}
