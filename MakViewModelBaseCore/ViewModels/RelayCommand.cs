using System;
using System.Windows.Input;

namespace MakCraft.ViewModels
{
    /// <summary>
    /// デリゲートを呼び出すことによって、コマンドを他のオブジェクトに中継する。CanExecute メソッドの既定値は 'true'。
    /// </summary>
    public class RelayCommand : ICommand
    {
        #region fields

        private readonly Action _execute;
        private readonly Predicate<object> _canExecute;

        #endregion // Fields

        #region Constructor

        /// <summary>
        /// 実行可否判定のないコマンドを作成。
        /// </summary>
        /// <param name="execute"></param>
        public RelayCommand(Action execute) : this(execute, null) { }

        /// <summary>
        /// コマンドを作成。
        /// </summary>
        /// <param name="execute"></param>
        /// <param name="canExecute"></param>
        public RelayCommand(Action execute, Predicate<object> canExecute)
        {
            _execute = execute ?? throw new ArgumentNullException(nameof(execute));
            _canExecute = canExecute;
        }

        #endregion // Constructor

        #region ICommand Members

        /// <summary>
        /// 現在の状態でこのコマンドを実行できるかどうかを判断します。
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        public bool CanExecute(object parameter)
        {
            return _canExecute == null ? true : _canExecute(parameter);
        }

        /// <summary>
        /// コマンドを実行するかどうかに影響するような変更があった場合に発生するイベントです。
        /// </summary>
        public event EventHandler CanExecuteChanged
        {
            add
            {
                if (_canExecute != null)
                    CommandManager.RequerySuggested += value;
            }
            remove
            {
                if (_canExecute != null)
                    CommandManager.RequerySuggested -= value;
            }
        }

        /// <summary>
        /// コマンドの起動時に呼び出されるメソッドです。
        /// </summary>
        /// <param name="parameter"></param>
        public void Execute(object parameter)
        {
            _execute();
        }

        #endregion // ICommand Members
    }

    /// <summary>
    /// デリゲートを呼び出すことによって、コマンドを他のオブジェクトに中継する。CanExecute メソッドの既定値は 'true'。
    /// </summary>
    public class RelayCommand<T> : ICommand
    {
        #region fields

        private readonly Action<T> _execute;
        private readonly Predicate<object> _canExecute;

        #endregion // Fields

        #region Constructor

        /// <summary>
        /// 実行可否判定のないコマンドを作成。
        /// </summary>
        /// <param name="execute"></param>
        public RelayCommand(Action<T> execute)
            : this(execute, null)
        {
        }

        /// <summary>
        /// コマンドを作成。
        /// </summary>
        /// <param name="execute"></param>
        /// <param name="canExecute"></param>
        public RelayCommand(Action<T> execute, Predicate<object> canExecute)
        {
            _execute = execute ?? throw new ArgumentNullException(nameof(execute));
            _canExecute = canExecute;
        }

        #endregion // Constructor

        #region ICommand Members

        /// <summary>
        /// 現在の状態でこのコマンドを実行できるかどうかを判断します。
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        public bool CanExecute(object parameter)
        {
            return _canExecute == null ? true : _canExecute(parameter);
        }

        /// <summary>
        /// コマンドを実行するかどうかに影響するような変更があった場合に発生するイベントです。
        /// </summary>
        public event EventHandler CanExecuteChanged
        {
            add
            {
                if (_canExecute != null)
                    CommandManager.RequerySuggested += value;
            }
            remove
            {
                if (_canExecute != null)
                    CommandManager.RequerySuggested -= value;
            }
        }

        /// <summary>
        /// コマンドの起動時に呼び出されるメソッドです。
        /// </summary>
        /// <param name="parameter"></param>
        public void Execute(object parameter)
        {
            _execute((T)parameter);
        }

        #endregion // ICommand Members
    }
}
