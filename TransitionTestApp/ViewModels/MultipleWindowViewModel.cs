using System;
using System.ComponentModel;
using System.Windows.Input;
using MakCraft.Behaviors;
using MakCraft.ViewModels;

namespace TransitionTestApp.ViewModels
{
    class MultipleWindowViewModel : TransitionViewModelBase
    {
        public MultipleWindowViewModel() { }

        private void ChangeClosableExecute()
        {
            _isClosable = !_isClosable;
        }
        private ICommand _changeClosableCommand;
        public ICommand ChangeClosableCommand
        {
            get
            {
                if (_changeClosableCommand == null)
                    _changeClosableCommand = new RelayCommand(ChangeClosableExecute);
                return _changeClosableCommand;
            }
        }

        private void CheckCanCloseExecute(EventArgs e)
        {
            if (!(e is CancelEventArgs cancelEventArgs))
            {
                return;
            }

            if (!base.CanCloseWindow)
            {
                // コールバックに Window のクローズのキャンセルをセット
                // 問い合わせへの回答で制御を変える場合の例は MainWindowsViewModel を参照
                base.MessageDialogActionCallback = result => cancelEventArgs.Cancel = true;
                // メッセージボックスを表示
                base.MessageDialogActionParam = new MessageDialogActionParameter(
                    "現在、終了できる状態ではありません",
                    "終了することはできません",
                    System.Windows.MessageBoxButton.OK,
                    true);
            }
        }
        private RelayCommand<EventArgs> _checkCanCloseCommand;
        public ICommand CheckCanCloseCommand
        {
            get
            {
                if (_checkCanCloseCommand == null)
                    _checkCanCloseCommand = new RelayCommand<EventArgs>(CheckCanCloseExecute);
                return _checkCanCloseCommand;
            }
        }

        protected override void Dispose(bool disposing)
        {
            Console.WriteLine("** MultipleWindowViewModel disposed.");
            base.Dispose(disposing);
        }

        private bool _isClosable = false;
        protected override bool WindowCloseCanExecute(object param)
        {
            return _isClosable;
        }
    }
}
