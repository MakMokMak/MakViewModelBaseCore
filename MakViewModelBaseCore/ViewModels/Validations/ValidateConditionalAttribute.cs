using System;

namespace MakCraft.ViewModels.Validations
{
    /// <summary>
    /// データ検証を行う条件を指定します。
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class ValidateConditionalAttribute : Attribute
    {
        /// <summary>
        /// データ検証を行う条件を指定します。
        /// </summary>
        /// <param name="comparedProperty">条件の比較対象となるプロパティ名</param>
        /// <param name="value">条件となる値</param>
        public ValidateConditionalAttribute(string comparedProperty, object value)
        {
            ComparedProperty = comparedProperty;
            Value = value;
        }

        /// <summary>
        /// 条件の比較対象となるプロパティ名を取得します。
        /// </summary>
        public string ComparedProperty { get; }

        /// <summary>
        /// 条件となる値を取得します。
        /// </summary>
        public object Value { get; }
    }
}
