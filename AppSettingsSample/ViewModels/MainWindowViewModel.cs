using System;
using System.ComponentModel;
using System.Windows.Input;
using MakCraft.ViewModels;

namespace AppSettingsSample.ViewModels
{
    internal class MainWindowViewModel : DialogViewModelBase
    {
        private readonly IOptionSources _optionSource;

        public MainWindowViewModel() : this(OptionSources.Instance) { }
        public MainWindowViewModel(IOptionSources optionSource)
        {
            System.Diagnostics.Debug.WriteLine($"[{DateTime.Now}] Create MainWindowViewModel");
            _optionSource = optionSource;
            PropertyChangedEventManager.AddListener(_optionSource, this, nameof(IOptionSources.FontSize));
            PropertyChangedEventManager.AddListener(_optionSource, this, nameof(IOptionSources.FontSizeMagnification));
            FontSize = _optionSource.FontSize;
        }

        /// <summary>
        /// PropertyChangedEvent を受信したときに実行する仮想メソッドをオーバーライドします。
        /// </summary>
        /// <param name="managerType"></param>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected override void OnReceivedPropertyChangeNotification(Type managerType, object sender, EventArgs e)
        {
            if (managerType != typeof(PropertyChangedEventManager))
            {
                return;
            }

            if (!(sender is IOptionSources optionSources))
            {
                return;
            }
            if (!(e is PropertyChangedEventArgs eventArgs))
            {
                return;
            }
            double newValue;
            switch (eventArgs.PropertyName)
            {
                case nameof(IOptionSources.FontSize):
                    newValue = optionSources.FontSize;
                    if (_fontSize != newValue)
                    {
                        FontSize = newValue;
                    }
                    break;
                case nameof(IOptionSources.FontSizeMagnification):
                    newValue = optionSources.FontSizeMagnification;
                    if (_titleFontSize != newValue)
                    {
                        TitleFontSize = _fontSize * newValue;
                    }
                    break;
                default:
                    throw new NotImplementedException(eventArgs.PropertyName);
            }
        }

        private double _fontSize;
        public double FontSize
        {
            get => _fontSize;
            set
            {
                SetProperty(ref _fontSize, value);
                TitleFontSize = _fontSize * _optionSource.FontSizeMagnification;
                MenuFontSize = _fontSize * 0.8;
            }
        }

        private double _menuFontSize;
        public double MenuFontSize
        {
            get => _menuFontSize;
            set => SetProperty(ref _menuFontSize, value);
        }

        private double _titleFontSize;
        public double TitleFontSize
        {
            get => _titleFontSize;
            set => SetProperty(ref _titleFontSize, value);
        }

        private object _optionWindowKick;
        /// <summary>
        /// モーダルダイアログの表示のキック用
        /// </summary>
        public object OptionWindowKick
        {
            get => _optionWindowKick;
            set => base.SetProperty(ref _optionWindowKick, value);
        }

        private object _modelessKick;
        /// <summary>
        /// モードレスダイアログの表示のキック用
        /// </summary>
        public object ModelessKick
        {
            get { return _modelessKick; }
            set
            {
                base.SetProperty(ref _modelessKick, value);
            }
        }

        private void OptionCommandExecute()
        {
            base.DialogType = typeof(OptionWindow);
            base.CommunicationDialog = null;
            base.DialogActionCallback = null;
            OptionWindowKick = new object();
        }
        private ICommand _optionComamnd;
        public ICommand OptionCommand
        {
            get
            {
                if (_optionComamnd == null)
                {
                    _optionComamnd = new RelayCommand(OptionCommandExecute);
                }
                return _optionComamnd;
            }
        }

        private void OpenModelessCommandExecute()
        {
            base.DialogType = typeof(Modeless01Window);
            base.CommunicationDialog = null;
            ModelessKick = new object();
        }
        private ICommand _openModelessCommand;
        public ICommand OpenModelessCommand
        {
            get
            {
                if (_openModelessCommand == null)
                {
                    _openModelessCommand = new RelayCommand(OpenModelessCommandExecute);
                }
                return _openModelessCommand;
            }
        }

        private void GcExecute()
        {
            System.GC.Collect();
            System.GC.WaitForPendingFinalizers();
            System.GC.Collect();
        }
        private ICommand _gcCommand;
        public ICommand GcCommand
        {
            get
            {
                if (_gcCommand == null)
                {
                    _gcCommand = new RelayCommand(GcExecute);
                }
                return _gcCommand;
            }
        }

        ~MainWindowViewModel()
        {
            System.Diagnostics.Debug.WriteLine($"[{DateTime.Now}] Destruct MainWindowViewModel");
        }
    }
}
