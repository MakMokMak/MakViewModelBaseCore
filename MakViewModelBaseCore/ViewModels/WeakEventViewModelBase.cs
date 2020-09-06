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
            // PropertyChangedEvent を受信したときの処理を行う。(旧型式。将来削除(2020/09/06))
            OnReceivedPropertyChangeNotification(managerType, sender, e);
            // イベント マネージャーからイベントを受信したときの処理を行う。
            return OnReceiveWeakEventNotification(managerType, sender, e);
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
        /// <returns>
        /// リスナーがイベントを処理した場合は <c>true</c>。リスナーが認識または処理しないイベントを受信した場合は、
        /// このメソッドは <c>false</c> を返します。
        /// (旧型式 <see cref="OnReceivedPropertyChangeNotification(Type, object, EventArgs)"/> との互換性のため、旧型式が有効な間は、
        /// デフォルトで <c>true</c> を返します。)
        /// </returns>
        protected virtual bool OnReceiveWeakEventNotification(Type managerType, object sender, EventArgs e)
        {
            return true;
        }

        #endregion
    }
}
