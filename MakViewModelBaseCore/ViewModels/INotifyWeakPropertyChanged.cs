using System.ComponentModel;

namespace MakCraft.ViewModels
{
    /// <summary>
    /// 弱いイベントパターンを用いたプロパティ変更通知のイベントハンドラを持つ NotifyWeakPropertyChanged のインターフェイス。
    /// </summary>
    public interface INotifyWeakPropertyChanged
    {
        /// <summary>
        /// 弱いイベントパターンを用いたプロパティ変更通知のイベントハンドラ。
        /// </summary>
        event PropertyChangedEventHandler WeakPropertyChanged;
    }
}
