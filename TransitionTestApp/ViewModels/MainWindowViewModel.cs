using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;
using MakCraft.ViewModels;
using MakCraft.Behaviors;
using MakCraft.Behaviors.Interfaces;
using TransitionTestApp.ViewModels.Container;

namespace TransitionTestApp.ViewModels
{
    class MainWindowViewModel : TransitionViewModelBase
    {
        public MainWindowViewModel() { }

        private string _trans1Text;
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

        private string _trans2Text;
        public string Trans2Text
        {
            get { return _trans2Text; }
            set
            {
                // プロパティの実装コードでプロパティの変更を行い、NotifyObject.RaisePropertyChanged メソッドを
                // 利用してプロパティ変更通知を行う例(プロパティ名の取得に nameof 演算子を利用)
                _trans2Text = value;
                base.RaisePropertyChanged(nameof(Trans2Text));
            }
        }

        private string _trans3Text;
        public string Trans3Text
        {
            get { return _trans3Text; }
            set
            {
                // プロパティの実装コードでプロパティの変更を行い、NotifyObject.RaisePropertyChanged メソッドを
                // 利用してプロパティ変更通知を行う例(プロパティ名の取得に PropertyHelper.GetName メソッド を利用)
                _trans3Text = value;
                base.RaisePropertyChanged(nameof(Trans3Text));
            }
        }

        private string _selectedBook;
        public string SelectedBook
        {
            get { return _selectedBook; }
            set
            {
                base.SetProperty(ref _selectedBook, value);
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

        private object _modalKick;
        /// <summary>
        /// モーダルダイアログの表示のキック用
        /// </summary>
        public object ModalKick
        {
            get { return _modalKick; }
            set
            {
                base.SetProperty(ref _modalKick, value);
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

        private void ShowTransitionWindowExecute()
        {
            Trans1Text = string.Empty;
            Trans2Text = string.Empty;
            Trans3Text = string.Empty;
            // 遷移を行う画面を表示
            base.DialogType = typeof(TransitionWindow1);
            var container = new TransitionContainer(TransitionName.trans.ToString(), this);
            base.CommunicationDialog = container;
            ModelessKick = new object();
            base.DisplayMode = WindowAction.Hide;
        }
        private ICommand _showTransitionWindowCommand;
        public ICommand ShowTransitionWindowCommand
        {
            get
            {
                if (_showTransitionWindowCommand == null)
                    _showTransitionWindowCommand = new RelayCommand(ShowTransitionWindowExecute);
                return _showTransitionWindowCommand;
            }
        }

        private void ShowTransAfterSelectedExecute()
        {
            // 遷移を行う画面を表示
            // A -> B 完了 -> A を経由して C 表示のパターン
            // C の表示は OnFinished メソッド側で行っている
            base.DialogType = typeof(SelectWindow);
            var container = new SelectContainer(TransitionName.select.ToString(), this);
            base.CommunicationDialog = container;
            ModelessKick = new object();
            base.DisplayMode = WindowAction.Hide;
        }
        private ICommand _showTransAfterSelectedCommand;
        public ICommand ShowTransAfterSelectedCommand
        {
            get
            {
                if (_showTransAfterSelectedCommand == null)
                    _showTransAfterSelectedCommand = new RelayCommand(ShowTransAfterSelectedExecute);
                return _showTransAfterSelectedCommand;
            }
        }

        private void ShowMultipleWindowExecute()
        {
            base.DialogType = typeof(MultipleWindow);
            base.CommunicationDialog = null;
            ModelessKick = new object();
        }
        private ICommand _showMultipleWindowCommand;
        public ICommand ShowMultipleWindowCommand
        {
            get
            {
                if (_showMultipleWindowCommand == null)
                    _showMultipleWindowCommand = new RelayCommand(ShowMultipleWindowExecute);
                return _showMultipleWindowCommand;
            }
        }

        private void ShowModalWindowExecute()
        {
            SelectedBook = string.Empty;
            base.DialogType = typeof(ModalWindow1);
            base.CommunicationDialog = null;
            base.DialogActionCallback = dialogResult =>
            {
                if (dialogResult.HasValue && dialogResult.Value)
                {
                    var viewModel = base.ResultViewModel as ModalWindow1ViewModel;
                    SelectedBook = viewModel.SelectedBook.Title;
                }
            };
            ModalKick = new object();
        }
        private ICommand _showModalWindowCommand;
        public ICommand ShowModalWindowCommand
        {
            get
            {
                if (_showModalWindowCommand == null)
                    _showModalWindowCommand = new RelayCommand(ShowModalWindowExecute);
                return _showModalWindowCommand;
            }
        }

        private void ShowModalWindow2Execute()
        {
            SelectedBook = string.Empty;
            base.DialogType = typeof(ModalWindow2);
            base.CommunicationDialog = null;
            base.DialogActionCallback = dialogResult =>
            {
                if (dialogResult.HasValue && dialogResult.Value)
                {
                    var viewModel = base.ResultViewModel as ModalWindow2ViewModel;
                    SelectedBook = viewModel.SelectedBook.Title;
                }
            };
            ModalKick = new object();
        }
        private ICommand _showModalWindow2Command;
        public ICommand ShowModalWindow2Command
        {
            get
            {
                if (_showModalWindow2Command == null)
                    _showModalWindow2Command = new RelayCommand(ShowModalWindow2Execute);
                return _showModalWindow2Command;
            }
        }

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
        // 画面遷移の一連の手順の完了時に行う処理
        public override void OnFinished(ITransContainer container)
        {
            switch ((TransitionName)Enum.Parse(typeof(TransitionName), container.Key))
            {
                case TransitionName.trans:
                    var trans = container as TransitionContainer;
                    Trans1Text = trans.Transition1Text;
                    Trans2Text = trans.Transition2Text;
                    Trans3Text = trans.Transition3Text;
                    break;
                case TransitionName.select:
                    // 遷移を行う画面を表示
                    base.DialogType = typeof(AfterSelectWindow);
                    var afterContainer = new SelectContainer(TransitionName.after.ToString(), this);
                    afterContainer.ItemName = ((SelectContainer)container).ItemName;
                    base.CommunicationDialog = afterContainer;
                    ModelessKick = new object();
                    base.DisplayMode = WindowAction.Hide;
                    break;
                case TransitionName.after:
                    break;
            }

            base.OnFinished(container);
        }

        private bool _isClosable = true;
        protected override bool WindowCloseCanExecute(object param)
        {
            return _isClosable;
        }

        public override void WindowClose()
        {
            try
            {
                ViewModelUtility.CloseViewModels(typeof(MultipleWindowViewModel));
            }
            catch (WindowPendingProcessException)
            {
                // MessageDialogAction ビヘイビアを使用してメッセージボックスを出す
                base.MessageDialogActionParam = new MessageDialogActionParameter("閉じることのできないウィンドウがあります。",
                                            "クローズできません");
                base.RaisePropertyChanged(nameof(MessageDialogActionParam));
                return;
            }

            base.WindowClose();
        }

        private void CheckCanCloseExecute(EventArgs e)
        {
            if (!(e is CancelEventArgs cancelEventArgs))
            {
                return;
            }

            // 閉じることのできないウィンドウがあるかを確認
            if (!ViewModelUtility.IsReadyCloseAllWindows)
            {
                base.MessageDialogActionCallback = result =>
                {
                    if (result == MessageBoxResult.No)
                    {
                        // 'いいえ' が選択されていたら Window のクローズのキャンセルをセット
                        cancelEventArgs.Cancel = true;
                    }
                };
                // メッセージボックスを表示
                base.MessageDialogActionParam = new MessageDialogActionParameter(
                    "処理途中のウィンドウがあります。強制終了しますか?",
                    "終了しますか?",
                    System.Windows.MessageBoxButton.YesNo,
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

        private enum TransitionName
        {
            trans,
            select,
            after,
        }
    }
}
