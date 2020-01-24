using System.Windows;
using MakCraft.Behaviors.Interfaces;

namespace MakCraft.Behaviors
{
    /// <summary>
    /// <see cref="Behaviors.MessageDialogAction"/> へ渡すパラメーター。
    /// <see cref="IsDialog"/> が false のときには Button の設定は反映されません。
    /// </summary>
    public class MessageDialogActionParameter : IMessageDialogActionParameter
    {
        /// <summary>
        /// MessageBoxに表示するメッセージ。
        /// </summary>
        public string Message { get; protected set; }
        /// <summary>
        /// MessageBox に表示するタイトル。
        /// </summary>
        public string Caption { get; protected set; }
        /// <summary>
        /// MessageBox に表示するボタン。
        /// </summary>
        public MessageBoxButton Button { get; protected set; }
        /// <summary>
        /// 表示するウィンドウの種別を取得します。
        /// true:ダイアログ(ユーザ応答を処理する)、false:メッセージ。
        /// </summary>
        public bool IsDialog { get; protected set; }

        /// <summary>
        /// <see cref="Behaviors.MessageDialogAction"/> へ渡すパラメーター(メッセージ表示用)。
        /// ボタン表示は OK のみ。
        /// </summary>
        /// <param name="message"></param>
        /// <param name="caption"></param>
        public MessageDialogActionParameter(string message, string caption)
            : this(message, caption, MessageBoxButton.OK, false) { }
        /// <summary>
        /// <see cref="Behaviors.MessageDialogAction"/> へ渡すパラメーター(ダイアログ表示用)。
        /// </summary>
        /// <param name="message"></param>
        /// <param name="caption"></param>
        /// <param name="button"></param>
        public MessageDialogActionParameter(string message, string caption, MessageBoxButton button)
            : this(message, caption, button, true) { }
        /// <summary>
        /// <see cref="Behaviors.MessageDialogAction"/> へ渡すパラメーター。
        /// </summary>
        /// <param name="message"></param>
        /// <param name="caption"></param>
        /// <param name="button"></param>
        /// <param name="isDialog"></param>
        public MessageDialogActionParameter(string message, string caption, MessageBoxButton button, bool isDialog)
        {
            Message = message;
            Caption = caption;
            Button = button;
            IsDialog = isDialog;
        }
    }
}
