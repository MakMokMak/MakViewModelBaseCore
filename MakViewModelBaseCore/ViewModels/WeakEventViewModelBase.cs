using System.ComponentModel;
using System.Windows;

namespace MakCraft.ViewModels
{
    /// <summary>
    /// 弱いイベントパターンを用いたリスナー登録機能を持つビューモデルベースです。
    /// </summary>
    public abstract class WeakEventViewModelBase : ViewModelBase
    {
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public WeakEventViewModelBase() { }

        /// <summary>
        /// PropertyChangedEventManager へ弱いイベントのリスナーを登録します。
        /// </summary>
        /// <param name="notifyObject">WeakPropertyChanged を発火するオブジェクト</param>
        /// <param name="weakEventListener">弱いイベントの発火を待ち受けるオブジェクト</param>
        public void AddListener(INotifyPropertyChanged notifyObject, IWeakEventListener weakEventListener)
        {
            PropertyChangedEventManager.AddListener(
                notifyObject,
                weakEventListener,
                string.Empty);
        }

        /// <summary>
        /// PropertyChangedEventManager から弱いイベントのリスナーを削除します。
        /// (明示的に削除を行わなくてもメモリーリークは発生しません)
        /// </summary>
        /// <param name="notifyObject">WeakPropertyChanged を発火するオブジェクト</param>
        /// <param name="weakEventListener">弱いイベントの発火を待ち受けるオブジェクト</param>
        public void RemoveListener(INotifyPropertyChanged notifyObject, IWeakEventListener weakEventListener)
        {
            PropertyChangedEventManager.RemoveListener(
                notifyObject,
                weakEventListener,
                string.Empty);
        }
    }
}
