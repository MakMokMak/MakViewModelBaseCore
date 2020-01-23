using System.Windows;

namespace MakCraft.ViewModels
{
    /// <summary>
    /// 弱いイベントパターンを用いたプロパティ変更通知のイベントハンドラを持つ WeakEventListner のインターフェイス。
    /// </summary>
    public interface IPropertyChangedWeakEventListener : IWeakEventListener, INotifyWeakPropertyChanged
    {
    }
}
