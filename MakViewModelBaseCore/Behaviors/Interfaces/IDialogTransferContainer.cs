namespace MakCraft.Behaviors.Interfaces
{
    /// <summary>
    /// 生成元ウィンドウからのデータの受取用プロパティのインターフェイス。
    /// </summary>
    public interface IDialogTransferContainer
    {
        /// <summary>
        /// 生成元ウィンドウからのデータの受取用プロパティ。
        /// </summary>
        object Container { get; set; }
    }
}
