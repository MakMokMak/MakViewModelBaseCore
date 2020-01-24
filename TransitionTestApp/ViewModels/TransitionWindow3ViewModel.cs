using System;
using System.Windows.Input;
using MakCraft.ViewModels;
using TransitionTestApp.ViewModels.Container;

namespace TransitionTestApp.ViewModels
{
    class TransitionWindow3ViewModel : TransitionViewModelBase
    {
        public TransitionWindow3ViewModel()
        {
        }

        private string _trans1Text;
        public string Trans1Text
        {
            get { return _trans1Text; }
            set
            {
                base.SetProperty(ref _trans1Text, value);
            }
        }

        private string _trans2Text;
        public string Trans2Text
        {
            get { return _trans2Text; }
            set
            {
                base.SetProperty(ref _trans2Text, value);
            }
        }

        private string _trans3Text;
        public string Trans3Text
        {
            get { return _trans3Text; }
            set
            {
                base.SetProperty(ref _trans3Text, value);
            }
        }

        // 完了時の処理
        protected override void TransitionComplete()
        {
            ((TransitionContainer)base.Container).Transition3Text = _trans3Text;

            base.TransitionComplete();
        }

        // 完了コマンド
        private void CompleteExecute()
        {
            TransitionComplete();
        }
        private bool CompleteCanExecute(object param)
        {
            return !string.IsNullOrEmpty(Trans3Text);
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

        // Transition1ViewModelBase から渡されてきたデータをセット
        protected override void OnContainerReceived(object container)
        {
            var tContainer = container as TransitionContainer;
            Trans1Text = tContainer.Transition1Text;
            Trans2Text = tContainer.Transition2Text;

            base.OnContainerReceived(container);
        }

        protected override void Dispose(bool disposing)
        {
            System.Diagnostics.Debug.WriteLine("** TransitionWindowViewModel3 disposed.");
            base.Dispose(disposing);
        }
    }
}
