using System;
using System.ComponentModel.DataAnnotations;
using System.Windows.Input;
using MakCraft.Behaviors.Interfaces;
using MakCraft.ViewModels;
using TransitionTestApp.ViewModels.Container;

namespace TransitionTestApp.ViewModels
{
    class TransitionWindow1ViewModel : TransitionViewModelBase
    {
        public TransitionWindow1ViewModel()
        {
        }

        private string _trans1Text;
        [Required(ErrorMessage = "この項目は必須項目です。")]
        public string Trans1Text
        {
            get { return _trans1Text; }
            set
            {
                // NotifyObject.SetProperty メソッドを利用してプロパティの変更・変更通知を行う例
                // SetProperty メソッドの呼び出しは、propertyName の指定を省略しています
                base.SetProperty(ref _trans1Text, value);
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
                // プロパティの実装コードでプロパティの変更を行い、NotifyObject.RaisePropertyChanged メソッドを
                // 利用してプロパティ変更通知を行う例(プロパティ名の取得に nameof 演算子を利用)
                _modelessKick = value;
                base.RaisePropertyChanged(nameof(ModelessKick));
            }
        }

        private void ShowTransitionWindow2Execute()
        {
            base.DialogType = typeof(TransitionWindow2);
            // コンテナに前画面のビューモデルを記録
            ((TransitionContainer)base.Container).PreviousViewModel = this;
            ((TransitionContainer)base.Container).Transition1Text = _trans1Text;
            base.CommunicationDialog = base.Container;
            ModelessKick = new object();
            base.DisplayMode = WindowAction.Hide;
        }
        private bool ShowTransitionWindow2CanExecute(object param)
        {
            return base.IsValid;
        }
        private ICommand _showTransitionWindow2Command;
        public ICommand ShowTransitionWindow2Command
        {
            get
            {
                if (_showTransitionWindow2Command == null)
                    _showTransitionWindow2Command = new RelayCommand(ShowTransitionWindow2Execute, ShowTransitionWindow2CanExecute);
                return _showTransitionWindow2Command;
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

        protected override void Dispose(bool disposing)
        {
            System.Diagnostics.Debug.WriteLine("** TransitionWindowViewModel1 disposed.");
            base.Dispose(disposing);
        }
    }
}
