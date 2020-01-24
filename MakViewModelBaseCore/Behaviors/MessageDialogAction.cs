using System;
using System.Windows;
using Microsoft.Xaml.Behaviors;
using MakCraft.Behaviors.Interfaces;

namespace MakCraft.Behaviors
{
    /// <summary>
    /// MessageBox を表示するアクション。
    /// </summary>
    public class MessageDialogAction : TriggerAction<FrameworkElement>
    {
        /// <summary>
        /// メッセージボックスやダイアログを出すために必要となる情報を受け取る。
        /// </summary>
        public static readonly DependencyProperty ParameterProperty = DependencyProperty.Register(
            "Parameter", typeof(IMessageDialogActionParameter), typeof(MessageDialogAction),
            new UIPropertyMetadata()
            );
        /// <summary>
        /// メッセージボックスやダイアログを出すために必要となる情報を受け取る。
        /// </summary>
        public IMessageDialogActionParameter Parameter
        {
            get { return (IMessageDialogActionParameter)GetValue(ParameterProperty); }
            set { SetValue(ParameterProperty, value); }
        }

        /// <summary>
        /// ダイアログでの選択結果をViewModelに通知するコールバックメソッド。
        /// </summary>
        public static readonly DependencyProperty ActionCallBackProperty = DependencyProperty.Register(
            "ActionCallBack", typeof(Action<MessageBoxResult>), typeof(MessageDialogAction),
            new UIPropertyMetadata(null)
            );
        /// <summary>
        /// ダイアログでの選択結果をViewModelに通知するコールバックメソッド。
        /// </summary>
        public Action<MessageBoxResult> ActionCallBack
        {
            get { return (Action<MessageBoxResult>)GetValue(ActionCallBackProperty); }
            set { SetValue(ActionCallBackProperty, value); }
        }

        /// <summary>
        /// Invokes the action. 
        /// </summary>
        /// <param name="obj"></param>
        protected override void Invoke(object obj)
        {
            if (Parameter.IsDialog)
            {
                ActionCallBack(MessageBox.Show(Parameter.Message, Parameter.Caption, Parameter.Button));
            }
            else
            {
                MessageBox.Show(Parameter.Message, Parameter.Caption);
            }
        }
    }
}
