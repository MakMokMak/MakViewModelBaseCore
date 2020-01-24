namespace MakCraft.ViewModels
{
    /// <summary>
    /// ウィンドウを閉じるためのビューモデルのインターフェイスです。
    /// </summary>
    public interface IWindowCloseCommand
    {
        /// <summary>
        /// ウィンドウがクローズできる状態かを返します。
        /// </summary>
        bool CanCloseWindow { get; }

        /// <summary>
        /// ビューモデルからウィンドウへ Close を通知するメソッドです。
        /// </summary>
        void WindowClose();
    }
}
