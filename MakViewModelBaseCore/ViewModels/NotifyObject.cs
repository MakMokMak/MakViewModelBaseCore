using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace MakCraft.ViewModels
{
    /// <summary>
    /// プロパティ変更通知を実装した基底クラス。
    /// </summary>
    public abstract class NotifyObject : INotifyPropertyChanged
    {
        /// <summary>
        /// コンストラクタ。
        /// </summary>
        public NotifyObject() { }

        /// <summary>
        /// PropertyChanged イベント処理用のデリゲート。
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// プロパティ名 property を value の値で書き換え、PropertyChanged イベントを発火します。
        /// propertyName パラメータが省略された場合、呼び出し元のプロパティの名前を用います。
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="property"></param>
        /// <param name="value"></param>
        /// <param name="propertyName"></param>
        protected virtual void SetProperty<T>(ref T property, T value, [CallerMemberName] string propertyName = null)
        {
            if (!Equals(property, value))
            {
                property = value;
                RaisePropertyChanged(propertyName);
            }
        }

        /// <summary>
        /// PropertyChanged イベントを発火します。
        /// propertyName が省略された場合、呼び出し元のメソッドまたはプロパティの名前を用います。
        /// </summary>
        /// <param name="propertyName"></param>
        protected virtual void RaisePropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
