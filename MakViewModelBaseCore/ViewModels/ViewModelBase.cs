using System;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Windows.Input;
using System.Windows.Threading;

namespace MakCraft.ViewModels
{
    /// <summary>
    /// プロパティ変更通知を実装したビューモデルの基底クラス。
    /// </summary>
    public abstract class ViewModelBase : NotifyObject, IDisposable
    {
        /// <summary>
        /// コンストラクタ。
        /// </summary>
        public ViewModelBase()
        {
            // インスタンスが作られたときの Diapatcher を取得
            _uiDispatcher = Dispatcher.CurrentDispatcher;
        }


        private readonly Dispatcher _uiDispatcher;
        /// <summary>
        /// UI スレッドのディスパッチャ。
        /// </summary>
        protected Dispatcher UiDispatcher
        {
            get { return _uiDispatcher; }
        }

        /// <summary>
        /// UI スレッドからのアクセスかどうかを判定する。
        /// </summary>
        /// <returns></returns>
        protected bool IsUiThread()
        {
            // 今のスレッドと UI スレッドを比較
            return UiDispatcher.Thread == Thread.CurrentThread;
        }

        /// <summary>
        /// PropertyChanged イベントを発火します(呼び出し元のスレッドが UI スレッドでない場合には、UI スレッドにて実行を行います)。
        /// propertyName が省略された場合、呼び出し元のメソッドまたはプロパティの名前を用います。
        /// </summary>
        /// <param name="propertyName"></param>
        protected override void RaisePropertyChanged([CallerMemberName] string propertyName = null)
        {
            InvokeOnUiThread(new Action(() => base.RaisePropertyChanged(propertyName)));
        }

        /// <summary>
        /// CommandManager.RequerySuggested イベントを強制的に発火させます(呼び出し元のスレッドが UI スレッドでない場合には、UI スレッドにて実行を行います)。
        /// </summary>
        protected void InvalidateRequerySuggested()
        {
            InvokeOnUiThread(CommandManager.InvalidateRequerySuggested);
        }

        // UI スレッドで actoin を呼び出す
        private void InvokeOnUiThread(Action action)
        {
            if (IsUiThread())
            {   // UI スレッドなら、そのまま実行
                action();
            }
            else
            {   // UI スレッドじゃなかったら、Dispatcher に依頼
                UiDispatcher.Invoke(action);
            }
        }

        #region IDisposable

        private bool _disposed = false;

        /// <summary>
        /// リソースの開放を行います。
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// リソースの開放を行います。
        /// </summary>
        /// <param name="disposing"></param>
        protected virtual void Dispose(bool disposing)
        {
            if (_disposed) return;

            _disposed = true;

            if (disposing)
            {
                // マネージ リソースの解放処理
            }
            // アンマネージ リソースの解放処理
        }

        #endregion

        /// <summary>
        /// デストラクタ。
        /// </summary>
        ~ViewModelBase()
        {
            Dispose(false);
        }
    }
}
