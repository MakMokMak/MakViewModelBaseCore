using System;
using System.Windows;
using System.Windows.Input;
using Microsoft.Xaml.Behaviors;
using MakCraft.Utils;

namespace MakCraft.Behaviors
{
    /// <summary>
    /// <see cref="MutiMonitorUtil"/> の利用を支援するビヘイビア
    /// SourceInitialized イベント発生時に <see cref="IMutiMonitorUtilProperty"/> 依存関係プロパティに MultiMonitorUtil クラスのインスタンスを設定するとともに <see cref="OnSourceInitializedCommandProperty"/> に設定されたコマンドを実行する。
    /// DisplaySettingsChanged イベント発生時に <see cref="OnDisplaySettingsChangedCommandProperty"/> に設定されたコマンドを実行する。
    /// </summary>
    public class MultiMonitorUtilBehavior : Behavior<Window>
    {
        /// <summary>
        /// 依存関係プロパティ IMutiMonitorUtilProperty
        /// </summary>
        public static readonly DependencyProperty IMutiMonitorUtilProperty = DependencyProperty.Register(
            "IMutiMonitorUtil", typeof(IMutiMonitorUtil), typeof(MultiMonitorUtilBehavior), new UIPropertyMetadata
            {
                DefaultValue = null
            });
        /// <summary>
        /// MultiMonitorUtil クラスのインスタンスを取得または設定します。
        /// </summary>
        public IMutiMonitorUtil IMutiMonitorUtil
        {
            get { return (IMutiMonitorUtil)GetValue(IMutiMonitorUtilProperty); }
            set { SetValue(IMutiMonitorUtilProperty, value); }
        }

        /// <summary>
        /// 依存関係プロパティ OnSourceInitializedCommandProperty
        /// </summary>
        public static readonly DependencyProperty OnSourceInitializedCommandProperty = DependencyProperty.Register(
            "OnSourceInitializedCommand", typeof(ICommand), typeof(MultiMonitorUtilBehavior), new UIPropertyMetadata());

        /// <summary>
        /// OnSourceInitializedCommand プロパティ
        /// </summary>
        public ICommand OnSourceInitializedCommand
        {
            get { return (ICommand)GetValue(OnSourceInitializedCommandProperty); }
            set { SetValue(OnSourceInitializedCommandProperty, value); }
        }

        /// <summary>
        /// 依存関係プロパティ OnDisplaySettingsChangedCommandProperty
        /// </summary>
        public static readonly DependencyProperty OnDisplaySettingsChangedCommandProperty = DependencyProperty.Register(
            "OnDisplaySettingsChangedCommand", typeof(ICommand), typeof(MultiMonitorUtilBehavior), new UIPropertyMetadata());

        /// <summary>
        /// OnDisplaySettingsChangedCommand プロパティ
        /// </summary>
        public ICommand OnDisplaySettingsChangedCommand
        {
            get { return (ICommand)GetValue(OnDisplaySettingsChangedCommandProperty); }
            set { SetValue(OnDisplaySettingsChangedCommandProperty, value); }
        }

        /// <summary>
        /// ビヘイビアーが AssociatedObject にアタッチされた後で呼び出されます。 
        /// </summary>
        protected override void OnAttached()
        {
            base.OnAttached();

            this.AssociatedObject.SourceInitialized += new EventHandler(OnSourceInitialized);
            this.AssociatedObject.Closed += new EventHandler(OnClosed); // OnDetaching が呼ばれない場合に備えて Window.Closed イベントでも後処理を行う
            Microsoft.Win32.SystemEvents.DisplaySettingsChanged +=
                new EventHandler(OnDisplaySettingsChanged);
        }

        /// <summary>
        /// ビヘイビアーが AssociatedObject からデタッチされるとき、その前に呼び出されます。
        /// </summary>
        protected override void OnDetaching()
        {
            base.OnDetaching();

            this.AssociatedObject.SourceInitialized -= new EventHandler(OnSourceInitialized);
            this.AssociatedObject.Closed -= new EventHandler(OnClosed);
            Microsoft.Win32.SystemEvents.DisplaySettingsChanged -=
                new EventHandler(OnDisplaySettingsChanged);
        }

        private void OnSourceInitialized(object sender, EventArgs e)
        {
            var multiMonitorUtil = Utils.MutiMonitorUtil.Instance;
            multiMonitorUtil.Refresh();
            IMutiMonitorUtil = multiMonitorUtil;

            if (OnSourceInitializedCommand == null) return;
            OnSourceInitializedCommand.Execute(e);
        }

        private void OnDisplaySettingsChanged(object sender, EventArgs e)
        {
            var multiMonitorUtil = Utils.MutiMonitorUtil.Instance;
            multiMonitorUtil.Refresh();

            if (OnDisplaySettingsChangedCommand == null) return;
            OnDisplaySettingsChangedCommand.Execute(e);
        }

        private void OnClosed(object sender, EventArgs e)
        {
            this.AssociatedObject.SourceInitialized -= new EventHandler(OnSourceInitialized);
            this.AssociatedObject.Closed -= new EventHandler(OnClosed);
            Microsoft.Win32.SystemEvents.DisplaySettingsChanged -=
                new EventHandler(OnDisplaySettingsChanged);
        }
    }
}
