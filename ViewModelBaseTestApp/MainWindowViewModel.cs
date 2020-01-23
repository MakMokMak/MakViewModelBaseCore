using System.Threading;
using System.Windows.Input;
using MakCraft.ViewModels;

namespace ViewModelBaseTestApp
{
    public enum CommandParam
    {
        ParamA,
        ParamB,
        ParamC
    }

    class MainWindowViewModel : ViewModelBase
    {
        private const int DEFAULT_TIME = 30;
        private const int INTERVAL_TIME = 1000;
        private int _countdownSecInt;
        private readonly Timer _timer;
        private string _itemX;
        private string _itemY;
        private string _output;

        public MainWindowViewModel()
        {
            CountdownSec = DEFAULT_TIME.ToString();
            _countdownSecInt = DEFAULT_TIME;
            IsEditTime = true;
            _timer = new Timer(NoticeTime, null, Timeout.Infinite, 0);
        }

        public string ItemX
        {
            get { return _itemX; }
            set
            {
                // RaisePropertyChanged の引数を文字列で指定する例
                _itemX = value;
                base.RaisePropertyChanged(nameof(ItemX));
            }
        }

        public string ItemY
        {
            get { return _itemY; }
            set
            {
                // RaisePropertyChanged の引数を省略する例
                _itemY = value;
                base.RaisePropertyChanged();
            }
        }

        public string Output
        {
            get { return _output; }
            private set
            {
                // SetProperty メソッドを使用する例
                base.SetProperty(ref _output, value);
            }
        }

        private string _countdownSec;
        public string CountdownSec
        {
            get { return _countdownSec; }
            set
            {
                base.SetProperty(ref _countdownSec, value);
                if (int.TryParse(value, out _countdownSecInt))
                {
                    DisplaySec = _countdownSecInt;
                }
            }
        }

        private bool _isEditTime;
        public bool IsEditTime
        {
            get { return _isEditTime; }
            set
            {
                base.SetProperty(ref _isEditTime, value);
                if (_isEditTime)
                {
                    StartButtonText = "Start";
                }
                else
                {
                    StartButtonText = "Stop";
                }
            }
        }

        private string _startButtonText;
        public string StartButtonText
        {
            get { return _startButtonText; }
            set
            {
                base.SetProperty(ref _startButtonText, value);
            }
        }

        private int _displaySec;
        public int DisplaySec
        {
            get { return _displaySec; }
            set
            {
                // SetProperty メソッドを利用する(PropertyName は省略)例
                base.SetProperty(ref _displaySec, value);

                if (_displaySec <= 0)
                {
                    StopTimer();
                    base.InvalidateRequerySuggested();
                }
            }
        }

        private void OutputCommandExecute()
        {
            Output = string.Format("\'{0}\' was \'{1}\'.", ItemX, ItemY);

        }
        private bool OutputCommandCanExecute
        {
            get { return (!string.IsNullOrEmpty(ItemX) && !string.IsNullOrEmpty(ItemY)); }
        }
        private ICommand _outputCommand;
        public ICommand OutputCommand
        {
            get
            {
                if (_outputCommand == null)
                    _outputCommand = new RelayCommand(
                        OutputCommandExecute,
                        param => OutputCommandCanExecute
                        );
                return _outputCommand;
            }
        }

        private void OutputWithParamExecute(CommandParam param)
        {
            Output = string.Format("\'{0}\' was \'{1}\'. parameter: {2}", ItemX, ItemY, param);
        }
        private bool OutputWithParamCanExecute(object param)
        {
            return (!string.IsNullOrEmpty(ItemX) && !string.IsNullOrEmpty(ItemY));
        }
        private RelayCommand<CommandParam> _outputWithParamCommand;
        public ICommand OutputWithParamCommand
        {
            get
            {
                if (_outputWithParamCommand == null)
                    _outputWithParamCommand = new RelayCommand<CommandParam>(
                        OutputWithParamExecute, OutputWithParamCanExecute);
                return _outputWithParamCommand;
            }
        }

        private void CountdownExecute()
        {
            if (IsEditTime)
            {
                IsEditTime = false;
                _timer.Change(INTERVAL_TIME, INTERVAL_TIME);
            }
            else
            {
                StopTimer();
            }
        }
        private bool CountdownCanExecute(object param)
        {
            return DisplaySec != 0;
        }
        private ICommand _countdownCommand;
        public ICommand CountdownCommand
        {
            get
            {
                if (_countdownCommand == null)
                {
                    _countdownCommand = new RelayCommand(CountdownExecute, CountdownCanExecute);
                }
                return _countdownCommand;
            }
        }

        private void ResetExecute()
        {
            DisplaySec = _countdownSecInt;
        }
        private ICommand _resetCommand;
        public ICommand ResetCommand
        {
            get
            {
                if (_resetCommand == null)
                {
                    _resetCommand = new RelayCommand(ResetExecute);
                }
                return _resetCommand;
            }
        }

        private void NoticeTime(object state)
        {
            --DisplaySec;
        }

        private void StopTimer()
        {
            _timer.Change(Timeout.Infinite, 0);
            IsEditTime = true;
        }

        protected override void Dispose(bool disposing)
        {
            _timer.Dispose();

            base.Dispose(disposing);
        }
    }
}
