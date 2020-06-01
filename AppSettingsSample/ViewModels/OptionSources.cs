using System;
using MakCraft.ViewModels;

namespace AppSettingsSample.ViewModels
{
    sealed class OptionSources : NotifyObject, IOptionSources
    {
        static OptionSources() { } // コンパイラによる beforefieldinit フラグの付加を抑止する

        private OptionSources() { } // 外部からのインスタンス生成を抑止

        private object _lockObj = new object();

        private static Lazy<OptionSources> _instance = new Lazy<OptionSources>(() =>
        {
            var instance = new OptionSources();
            return instance;
        });

        public static OptionSources Instance => _instance.Value;

        private double _fontSize = 16.0;
        public double FontSize
        {
            get => _fontSize;
            set
            {
                if (_fontSize != value)
                {
                    SetProperty(ref _fontSize, value);
                }
            }
        }

        private double _fontSizeMagnification = 1.2;
        public double FontSizeMagnification
        {
            get => _fontSizeMagnification;
            set
            {
                if (_fontSizeMagnification != value)
                {
                    SetProperty(ref _fontSizeMagnification, value);
                }
            }
        }
    }
}
