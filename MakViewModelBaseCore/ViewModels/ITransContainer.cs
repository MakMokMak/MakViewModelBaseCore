namespace MakCraft.ViewModels
{
    /// <summary>
    /// 画面遷移の際のデータコンテナのインターフェイス。
    /// </summary>
    public interface ITransContainer
    {
        /// <summary>
        /// 遷移を区別するためのキーを取得します。
        /// 一つのビューモデルで複数の画面遷移を持つ場合の処理の分岐用。
        /// </summary>
        string Key { get; }

        /// <summary>
        /// 遷移動作の開始元ビューモデルを取得します。
        /// </summary>
        TransitionViewModelBase TransStartViewModel { get; }

        /// <summary>
        /// 前画面のビューモデルを取得・設定します。
        /// </summary>
        TransitionViewModelBase PreviousViewModel { get; set; }
    }
}
