using System;
using System.Windows.Input;
using MakCraft.Behaviors.Interfaces;
using MakCraft.ViewModels;
using TransitionTestApp.ViewModels.Container;

namespace TransitionTestApp.ViewModels
{
    class TransitionWindow2ViewModel : TransitionViewModelBase
    {
        public TransitionWindow2ViewModel()
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

        private object _modelessKick;
        /// <summary>
        /// モードレスダイアログの表示のキック用
        /// </summary>
        public object ModelessKick
        {
            get { return _modelessKick; }
            set
            {
                base.SetProperty(ref _modelessKick, value);
            }
        }

        private void ShowTransitionWindow3Execute()
        {
            base.DialogType = typeof(TransitionWindow3);
            // コンテナに前画面のビューモデルを記録
            ((TransitionContainer)base.Container).PreviousViewModel = this;
            ((TransitionContainer)base.Container).Transition2Text = _trans2Text;
            base.CommunicationDialog = base.Container;
            ModelessKick = new object();
            base.DisplayMode = WindowAction.Hide;
        }
        private bool ShowTransitionWindow3CanExecute(object param)
        {
            return !string.IsNullOrEmpty(Trans2Text);
        }
        private ICommand _showTransitionWindow3Command;
        public ICommand ShowTransitionWindow3Command
        {
            get
            {
                if (_showTransitionWindow3Command == null)
                    _showTransitionWindow3Command = new RelayCommand(
                        ShowTransitionWindow3Execute,
                        ShowTransitionWindow3CanExecute);
                return _showTransitionWindow3Command;
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
            Trans1Text = ((TransitionContainer)container).Transition1Text;

            base.OnContainerReceived(container);
        }

        protected override void Dispose(bool disposing)
        {
            System.Diagnostics.Debug.WriteLine("** TransitionWindowViewModel2 disposed.");
            base.Dispose(disposing);
        }
    }
}
