using System;
using System.Windows.Input;
using MakCraft.Behaviors.Interfaces;

namespace MakCraft.ViewModels
{
    /// <summary>
    /// データ検証と画面遷移及び表示状態設定機能を持つビューモデルの基底クラスです。
    /// </summary>
    public abstract class TransitionViewModelBase : DialogViewModelBase, IWindowCloseCommand, IViewModelStatus, IReceiveFinished
    {
        /// <summary>
        /// コンストラクタ。
        /// </summary>
        public TransitionViewModelBase() { }

        private WindowAction _windowAction;
        /// <summary>
        /// ウィンドウの表示状態を取得・設定します。
        /// View 側で <see cref="Microsoft.Xaml.Behaviors.Core.PropertyChangedTrigger"/> の Binding と
        /// <see cref="MakCraft.Behaviors.DisplayModeAction"/> の DisplayMode にバインドしてください。
        /// </summary>
        public WindowAction DisplayMode
        {
            get { return _windowAction; }
            protected set
            {
                base.SetProperty(ref _windowAction, value);
            }
        }

        /// <summary>
        /// 前画面のビューモデルを取得します。
        /// </summary>
        protected TransitionViewModelBase PreviousViewModel { get; private set; }

        /// <summary>
        /// ウィンドウ作成元からのデータ受け取り用プロパティ。
        /// </summary>
        public override object Container
        {
            get
            {
                return base.Container;
            }
            set
            {
                if (!(value is ITransContainer container))
                {
                    throw new InvalidCastException(
                        "渡されたコンテナが ITransContainer インターフェイスを実装していません。");
                }

                if (container.PreviousViewModel == null)
                {
                    throw new InvalidOperationException(
                        "渡されたコンテナに前画面のビューモデルが設定されていません。");
                }

                // 前画面のビューモデルを保存
                PreviousViewModel = container.PreviousViewModel;

                base.Container = value;
            }
        }

        /// <summary>
        /// ウィンドウが閉じることの出来る状態かどうかを返します。
        /// 仮想メソッドは常に 'true' を返します。制御が必要な場合はオーバーライドしてください。
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        protected virtual bool WindowCloseCanExecute(object param)
        {
            return true;
        }
        private ICommand _windowCloseCommand;
        /// <summary>
        /// ウィンドウを閉じるコマンド。
        /// ボタンの有効・無効は <see cref="WindowCloseCanExecute(object)"/>、
        /// コマンド処理の実体は <see cref="WindowClose"/> を設定します。
        /// </summary>
        public ICommand WindowCloseCommand
        {
            get
            {
                if (_windowCloseCommand == null)
                    _windowCloseCommand = new RelayCommand(WindowClose, WindowCloseCanExecute);
                return _windowCloseCommand;
            }
        }

        /// <summary>
        /// ウィンドウがクローズされた際の操作。
        /// </summary>
        protected virtual void OnWindowClosed()
        {
            if (Container != null || Container as ITransContainer == null)
            {
                var container = Container as ITransContainer;

                if (CurrentStatus == ViewModelStatus.Completed)
                {
                    if (container.TransStartViewModel != PreviousViewModel &&
                        PreviousViewModel != ViewModelUtility.GetMainWindowViewModel())
                    {   // 直前の画面が遷移の開始元でなくなおかつ 直前の画面が MainWindow でなければ閉じる
                        PreviousViewModel.DisplayMode = WindowAction.Close;
                    }
                    else
                    {   // 直前の画面が遷移の開始元 または 直前の画面が MainWindow のときは直前の画面を表示する
                        PreviousViewModel.DisplayMode = WindowAction.Show;
                        // 遷移の開始元の画面遷移完了時の処理をキック
                        container.TransStartViewModel.OnFinished(container);
                    }
                }
                else
                {
                    PreviousViewModel.DisplayMode = WindowAction.Show;
                }
            }
        }
        private ICommand _windowClosedCommand;
        /// <summary>
        /// ウィンドウがクローズされた際の操作コマンド。
        /// ウィンドウの Closed イベントが発生した際に呼び出されるようにしてください。
        /// コマンド処理の実体は <see cref="OnWindowClosed"/> を設定します。
        /// </summary>
        public ICommand WindowClosedCommand
        {
            get
            {
                if (_windowClosedCommand == null)
                    _windowClosedCommand = new RelayCommand(OnWindowClosed);
                return _windowClosedCommand;
            }
        }

        /// <summary>
        /// 一連の画面遷移の完了を通知します。
        /// </summary>
        protected virtual void TransitionComplete()
        {
            CurrentStatus = ViewModelStatus.Completed;
            WindowClose();
        }

        #region IWindowCloseCommand

        /// <summary>
        /// ウィンドウを閉じることが可能かを取得します。
        /// </summary>
        public bool CanCloseWindow
        {
            get { return WindowCloseCanExecute(null); }
        }

        /// <summary>
        /// ビューモデルからウィンドウへ Close を通知するメソッドです。
        /// </summary>
        public virtual void WindowClose()
        {
            // コンテナの前画面情報をクリア
            if (Container != null)
            {
                var container = Container as ITransContainer;
                container.PreviousViewModel = null;
            }
            // Window へ Close を通知する。
            DisplayMode = WindowAction.Close;
        }

        #endregion IWindowCloseCommand

        #region IViewModelStatus
        // ステータスの初期値を Halfway にしておく(Window 作成・表示は ModelessDialogTransferDataAction が行うため)
        // 状態変更時の操作は DisplayModeAction が行う
        private ViewModelStatus _modelStatus = ViewModelStatus.Halfway;
        /// <summary>
        /// ビューモデルの処理状態を取得・設定します。
        /// </summary>
        public ViewModelStatus CurrentStatus
        {
            get { return _modelStatus; }
            set { _modelStatus = value; }
        }
        #endregion IViewModelStatus

        #region IReceiveFinished
        /// <summary>
        /// 画面遷移完了時に実行する処理です。
        /// </summary>
        public virtual void OnFinished(ITransContainer container)
        {
        }
        #endregion IReceiveFinished
    }
}
