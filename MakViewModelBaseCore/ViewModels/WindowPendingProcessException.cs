using System;
using System.Runtime.Serialization;

namespace MakCraft.ViewModels
{
    /// <summary>
    /// ウィンドウを閉じようとした際にビューモデルが処理途中等で閉じることができない場合にスローされる例外。
    /// </summary>
    [Serializable]
    public class WindowPendingProcessException : Exception
    {
        /// <summary>
        /// コンストラクタ。
        /// </summary>
        public WindowPendingProcessException() : base() { }
        /// <summary>
        /// コンストラクタ。
        /// </summary>
        /// <param name="message"></param>
        public WindowPendingProcessException(string message) : base(message) { }
        /// <summary>
        /// コンストラクタ。
        /// </summary>
        /// <param name="message"></param>
        /// <param name="inner"></param>
        public WindowPendingProcessException(string message, Exception inner) : base(message, inner) { }
        /// <summary>
        /// コンストラクタ。
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        public WindowPendingProcessException(SerializationInfo info, StreamingContext context) { }
    }
}
