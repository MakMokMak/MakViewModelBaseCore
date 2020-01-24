namespace MakCraft.ViewModels
{
    /// <summary>
    /// 画面遷移完了時の操作に用いるインターフェイスです。
    /// </summary>
    interface IReceiveFinished
    {
        /// <summary>
        /// 画面遷移操作完了時に実行されるメソッド。
        /// </summary>
        void OnFinished(ITransContainer container);
    }
}
