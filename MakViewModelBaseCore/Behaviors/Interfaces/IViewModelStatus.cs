namespace MakCraft.Behaviors.Interfaces
{
    /// <summary>
    /// 画面遷移を行うビューモデルの処理状況のプロパティのインターフェイス。
    /// </summary>
    public interface IViewModelStatus
    {
        /// <summary>
        /// 画面遷移を行うビューモデルの処理状況。
        /// </summary>
        ViewModelStatus CurrentStatus { get; set; }
    }

    /// <summary>
    /// 画面遷移を行うビューモデルの処理状況を表す列挙型です。
    /// </summary>
    public enum ViewModelStatus
    {
        /// <summary>
        /// 未完了。
        /// </summary>
        Halfway,
        /// <summary>
        /// 完了。
        /// </summary>
        Completed
    }

    /// <summary>
    /// 画面遷移を行うビューモデルへセットするウィンドウの状態を表す列挙型です。
    /// </summary>
    public enum WindowAction
    {
        /// <summary>
        /// 表示する。
        /// </summary>
        Show,
        /// <summary>
        /// 非表示にする。
        /// </summary>
        Hide,
        /// <summary>
        /// 閉じる。
        /// </summary>
        Close
    }

    /// <summary>
    /// ダイアログの表示種別。
    /// </summary>
    public enum DialogModes
    {
        /// <summary>
        /// モーダル ダイアログとして表示する。
        /// </summary>
        Modal,
        /// <summary>
        /// モードレス ダイアログとして表示する。
        /// </summary>
        Modeless
    }
}
