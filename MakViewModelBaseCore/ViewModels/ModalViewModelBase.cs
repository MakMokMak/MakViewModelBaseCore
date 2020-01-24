using System.Windows.Input;

namespace MakCraft.ViewModels
{
    /// <summary>
    /// データ検証とモーダルダイアログ表示機能を持つビューモデルの基底クラスです。
    /// </summary>
    public abstract class ModalViewModelBase : DialogViewModelBase
    {
        /// <summary>
        /// コンストラクタ。
        /// </summary>
        public ModalViewModelBase() { }

        private bool? _result;
        /// <summary>
        /// View 側の <see cref="System.Windows.Window.DialogResult"/> セットのトリガーとなるプロパティです。
        /// View 側で <see cref="Microsoft.Xaml.Behaviors.Core.PropertyChangedTrigger"/> の Binding と
        /// <see cref="Microsoft.Xaml.Behaviors.Core.ChangePropertyAction"/> の Value にバインドしてください。
        /// </summary>
        public bool? Result
        {
            get { return _result; }
            set
            {
                base.SetProperty(ref _result, value);
            }
        }

        /// <summary>
        /// モーダルダイアログの OK ボタンクリック時の処理。
        /// 仮想メソッドは Window の DialogResult プロパティに true をセットする動作のみです。制御が必要な場合はオーバーライドしてください。
        /// </summary>
        protected virtual void OkExecute()
        {
            // Result プロパティに true をセットすることで Window のトリガが起動し、Window の DialogResult プロパティに値をセットする
            Result = true;
        }
        /// <summary>
        /// OkCommand の 有効/無効 を返します。
        /// データ検証エラーの有無を返します。データ検証エラーを用いないで判断したい場合はオーバーライドしてください。
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        protected virtual bool OkCanExecute(object param)
        {
            return base.IsValid;
        }
        private ICommand _okCommand;
        /// <summary>
        /// OK ボタン用のコマンドです。
        /// ボタンの有効・無効は <see cref="OkCanExecute(object)"/>、コマンド処理の実体は <see cref="OkExecute"/> を設定します。
        /// </summary>
        public ICommand OkCommand
        {
            get
            {
                if (_okCommand == null)
                    _okCommand = new RelayCommand(OkExecute, OkCanExecute);
                return _okCommand;
            }
        }
    }
}
