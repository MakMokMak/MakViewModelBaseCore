using System;
using System.Collections.Generic;

namespace MakCraft.ViewModels.Validations
{
    /// <summary>
    /// データ検証を補助するツール類です。
    /// </summary>
    public static class ValidationUtility
    {
        /// <summary>
        /// 型情報からカスタム属性を持つプロパティ名の列挙子を取得します。
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
        public static IEnumerable<string> GetPropatyNamesWithAttribute(Type target)
        {
            if (target == null)
            {
                throw new ArgumentNullException($"引数名 'target'");
            }

            var result = new List<string>();

            foreach (var propertyInfo in target.GetProperties())
            {
                var attrs = Attribute.GetCustomAttributes(propertyInfo);
                if (attrs.Length != 0)
                {
                    result.Add(propertyInfo.Name);
                }
            }

            return result;
        }
    }
}
