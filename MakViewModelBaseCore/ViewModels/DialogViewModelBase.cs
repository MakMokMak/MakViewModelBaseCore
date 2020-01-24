using System;
using System.Windows;
using MakCraft.Behaviors.Interfaces;

namespace MakCraft.ViewModels
{
    /// <summary>
    /// データ検証とダイアログ表示の基本機能を提供するビューモデルの基底クラスです。
    /// </summary>
    public abstract class DialogViewModelBase : ValidationViewModelBase, IDialogTransferContainer
    {
        /// <summary>
        /// コンストラクタ。
        /// </summary>
        public DialogViewModelBase() { }

        /// <summary>
        /// 作成する Dialog に渡すデータを取得・設定します。
        /// View 側で <see cref="MakCraft.Behaviors.DialogTransferDataAction"/> の
        /// Parameter にバインドしてください。
        /// </summary>
        public object CommunicationDialog { get; protected set; }

        /// <summary>
        /// 表示するカスタムダイアログボックスの型の情報。
        /// View 側で <see cref="MakCraft.Behaviors.DialogTransferDataAction"/> の
        /// DialogType にバインドしてください。
        /// </summary>
        public Type DialogType { get; protected set; }

        #region IDialogTransferContainer
        private object _container;
        /// <summary>
        /// ウィンドウ作成元から渡されたデータの受取用。
        /// </summary>
        public virtual object Container
        {
            get { return _container; }
            set
            {
                _container = value;
                OnContainerReceived(_container);
            }
        }
        #endregion IDialogTransferContainer

        #region DialogTransferContainer
        /// <summary>
        /// ダイアログが閉じられた後に実行するコールバックを取得・設定します。
        /// View 側で <see cref="MakCraft.Behaviors.DialogTransferDataAction"/> の
        /// ActionCallBack にバインドしてください。
        /// </summary>
        public Action<bool?> DialogActionCallback { get; protected set; }

        /// <summary>
        /// ダイアログ表示で生成されたダイアログのビューモデルへの参照を取得・設定します
        /// (ダイアログで設定された値の参照用)。
        /// View 側で <see cref="MakCraft.Behaviors.DialogTransferDataAction"/> の
        /// ResultViewModel にバインドしてください。
        /// </summary>
        public object ResultViewModel { get; set; }

        /// <summary>
        /// ウィンドウ作成元からのデータを受け取った際に行う処理。
        /// </summary>
        /// <param name="container"></param>
        protected virtual void OnContainerReceived(object container)
        {
        }
        #endregion DialogTransferContainer

        #region MessageDialogAction
        private IMessageDialogActionParameter _messageDialogActionParam;
        /// <summary>
        /// <see cref="MakCraft.Behaviors.MessageDialogAction"/> に渡すパラメーター。
        /// View 側で <see cref="Microsoft.Xaml.Behaviors.Core.PropertyChangedTrigger"/> の Binding と
        /// <see cref="MakCraft.Behaviors.MessageDialogAction"/> の
        /// Parameter にバインドしてください。
        /// </summary>
        public IMessageDialogActionParameter MessageDialogActionParam
        {
            get { return _messageDialogActionParam; }
            set { base.SetProperty(ref _messageDialogActionParam, value); }
        }

        /// <summary>
        /// <see cref="MakCraft.Behaviors.MessageDialogAction"/> の実行後に呼ばれるCallBack。
        /// View 側で <see cref="MakCraft.Behaviors.MessageDialogAction"/> の
        /// ActionCallBack にバインドしてください。
        /// </summary>
        public Action<MessageBoxResult> MessageDialogActionCallback { get; set; }
        #endregion MessageDialogAction
    }
}
