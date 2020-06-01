using System;
using System.ComponentModel;
using MakCraft.ViewModels;

namespace AppSettingsSample.ViewModels
{
    internal class Modeless01WindowViewModel : TransitionViewModelBase
    {
        private readonly IOptionSources _optionSource;

        public Modeless01WindowViewModel() : this(OptionSources.Instance) { }
        public Modeless01WindowViewModel(IOptionSources optionSource)
        {
            System.Diagnostics.Debug.WriteLine($"[{DateTime.Now}] Create Modeless01WindowViewModel");
            _optionSource = optionSource;
            PropertyChangedEventManager.AddListener(_optionSource, this, nameof(IOptionSources.FontSize));
            PropertyChangedEventManager.AddListener(_optionSource, this, nameof(IOptionSources.FontSizeMagnification));
            FontSize = _optionSource.FontSize;
        }

        /// <summary>
        /// PropertyChangedEvent を受信したときに実行する仮想メソッドです。
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
            }
        }

        private double _titleFontSize;
        public double TitleFontSize
        {
            get => _titleFontSize;
            set => SetProperty(ref _titleFontSize, value);
        }

        ~Modeless01WindowViewModel()
        {
            System.Diagnostics.Debug.WriteLine($"[{DateTime.Now}] Destruct Modeless01WindowViewModel");
        }
    }
}
