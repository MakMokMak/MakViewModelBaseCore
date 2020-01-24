using System;

namespace MakCraft.ViewModels
{
    /// <summary>
    /// 画面遷移の際のデータコンテナの基底クラス。
    /// </summary>
    public class TransitionContainerBase : ITransContainer
    {
        /// <summary>
        /// 画面遷移のキー及び遷移開始元ビューモデルを設定して画面遷移の際のデータコンテナを作成します。
        /// </summary>
        /// <param name="key"></param>
        /// <param name="viewModel"></param>
        public TransitionContainerBase(string key, TransitionViewModelBase viewModel)
        {
            _key = key;
            _transStartViewmodel = viewModel ?? throw new ArgumentException(
                    "viewmodel に null は指定できません。");
            PreviousViewModel = viewModel;
        }

        #region ITransContainer
        private readonly string _key;
        /// <summary>
        /// 遷移を区別するためのキーを取得します。
        /// 一つのビューモデルで複数の画面遷移を持つ場合の処理の分岐用。
        /// </summary>
        public string Key
        {
            get { return _key; }
        }

        private readonly TransitionViewModelBase _transStartViewmodel;
        /// <summary>
        /// 遷移動作の開始元ビューモデルを取得します。
        /// </summary>
        public TransitionViewModelBase TransStartViewModel
        {
            get { return _transStartViewmodel; }
        }

        /// <summary>
        /// 前画面のビューモデルを取得・設定します。
        /// </summary>
        public TransitionViewModelBase PreviousViewModel { get; set; }
        #endregion
    }
}
