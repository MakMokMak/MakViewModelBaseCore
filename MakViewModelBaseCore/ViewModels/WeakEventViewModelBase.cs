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
            // PropertyChangedEvent を受信したときの処理を行う。
            OnReceivedPropertyChangeNotification(managerType, sender, e);

            return true;
        }

        /// <summary>
        /// PropertyChangedEvent を受信したときに実行する仮想メソッドです。
        /// </summary>
        /// <param name="managerType"></param>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected virtual void OnReceivedPropertyChangeNotification(Type managerType, object sender, EventArgs e) { }

        #endregion
    }
}
