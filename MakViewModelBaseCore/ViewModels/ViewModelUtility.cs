using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace MakCraft.ViewModels
{
    /// <summary>
    /// ViewModel 関連のユーティリティクラス。
    /// </summary>
    public static class ViewModelUtility
    {
        /// <summary>
        /// MainWindow となっている Window の ViewModel を返します。
        /// </summary>
        /// <returns></returns>
        public static ViewModelBase GetMainWindowViewModel()
        {
            var viewModel = Application.Current.MainWindow.DataContext;
            if (viewModel is ViewModelBase result)
            {
                return result;
            }
            throw new InvalidCastException(
              "MainWindow の ViewModel が ViewModelBase から派生していません。");
        }

        /// <summary>
        /// 指定されたビューモデルのインスタンスの数を返します。
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static int Count(Type type)
        {
            if (!type.IsSubclassOf(typeof(ViewModelBase))) throw new ArgumentException(
                string.Format("引数の型が ViewModelBase の派生クラスになっていません(引数の型:{0})。", type.Name));
            var result = 0;
            TargetViewModelDoAction(type, n => ++result);
            return result;
        }

        /// <summary>
        /// 指定されたビューモデルのインスタンスの一覧を返します。
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static IReadOnlyList<ViewModelBase> GetViewModels(Type type)
        {
            if (!type.IsSubclassOf(typeof(ViewModelBase))) throw new ArgumentException(
                string.Format("引数の型が ViewModelBase の派生クラスになっていません(引数の型:{0})。", type.Name));
            var result = new List<ViewModelBase>();
            TargetViewModelDoAction(type, n => result.Add(n));
            return result;
        }

        /// <summary>
        /// 指定されたビューモデルのインスタンスの IWindowCloseCommand インターフェイス の
        /// WindowClose メソッドを実行します。
        /// </summary>
        /// <returns></returns>
        public static void CloseViewModels(Type type)
        {
            if (!type.IsSubclassOf(typeof(ViewModelBase))) throw new ArgumentException(
                string.Format("引数の型が ViewModelBase の派生クラスになっていません(引数の型:{0})。", type.Name));
            if (Count(type) == 0) return;

            var list = GetViewModels(type);
            if (list.First() as IWindowCloseCommand == null)
            {
                throw new InvalidCastException(
                "オブジェクトは IWindowCloseCommand インターフェイスを実装していません。: " + type.ToString());
            }

            // ウィンドウが閉じることのできる状態か確認
            if (IsReadyCloseWindows(list))
            {
                // ウィンドウを閉じる
                foreach (var n in list)
                {
                    (n as IWindowCloseCommand).WindowClose();
                }
            }
            else
            {
                throw new WindowPendingProcessException(
                    string.Format("ViewModel:{0} が閉じることの出来る状態にありません。", type.Name));
            }
        }

        /// <summary>
        /// すべてのウィンドウが閉じることが可能か確認します。
        /// </summary>
        public static bool IsReadyCloseAllWindows
        {
            get
            {
                var list = new List<ViewModelBase>();
                TargetViewModelDoAction(typeof(ViewModelBase), n => list.Add(n), true);
                return IsReadyCloseWindows(list);
            }
        }

        // リストで渡されたウィンドウが閉じることが可能か確認します。
        private static bool IsReadyCloseWindows(IReadOnlyList<ViewModelBase> list)
        {
            var result = true;
            foreach (var n in list)
            {
                if (n is IWindowCloseCommand vm)
                {
                    if (!vm.CanCloseWindow)
                    {
                        result = false;
                        break;
                    }
                }
                else
                {
                    throw new InvalidCastException(
                      "ViewModel は IWindowCloseCommand インターフェイスを実装していません。: " + n.GetType().Name);
                }
            }
            return result;
        }

        // Application クラスで管理しているインスタンス化された Window のコレクションからビューモデルを取得して、action の処理を行います。
        private static void TargetViewModelDoAction(Type type, Action<ViewModelBase> action, bool isSubClass = false)
        {
            foreach (var n in Application.Current.Windows)
            {
                var window = n as Window;
                if (window.DataContext == null) continue;

                var vmType = window.DataContext.GetType();
                var cond = isSubClass ? vmType == type || vmType.IsSubclassOf(typeof(ViewModelBase)) :
                                        vmType == type;

                if (cond)
                {
                    if (window.DataContext is ViewModelBase viewModel)
                    {
                        action(viewModel);
                    }
                }
            }
        }
    }
}
