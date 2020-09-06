using System;
using System.Windows;

namespace MakCraft.ViewModels
{
    /// <summary>
    /// IWeakEventListener インターフェイスを実装したビューモデルベースです。
    /// </summary>
    public abstract class WeakEventViewModelBase : ViewModelBase, IWeakEventListener
    {
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public WeakEventViewModelBase() { }

        #region IWeakEventListener

        /// <summary>
        /// イベント マネージャーからイベントを受信します。
        /// </summary>
        /// <param name="managerType"></param>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <returns></returns>
        public bool ReceiveWeakEvent(Type managerType, object sender, EventArgs e)
        {
            // イベント マネージャーからイベントを受信したときの処理を行う。
            OnReceiveWeakEventNotification(managerType, sender, e);
            // PropertyChangedEvent を受信したときの処理を行う。(旧型式。将来削除(2020/09/06))
            OnReceivedPropertyChangeNotification(managerType, sender, e);

            return true;
        }

        /// <summary>
        /// PropertyChangedEvent を受信したときに実行する仮想メソッドです。
        /// </summary>
        /// <param name="managerType"></param>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        [Obsolete("OnReceiveWeakEventNotification(Type managerType, object sender, EventArgs e) 仮想メソッドを使用してください。")]
        protected virtual void OnReceivedPropertyChangeNotification(Type managerType, object sender, EventArgs e) { }

        /// <summary>
        /// イベント マネージャーからイベントを受信したときに実行する仮想メソッドです。
        /// </summary>
        /// <param name="managerType"></param>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected virtual void OnReceiveWeakEventNotification(Type managerType, object sender, EventArgs e) { }

        #endregion
    }
}
