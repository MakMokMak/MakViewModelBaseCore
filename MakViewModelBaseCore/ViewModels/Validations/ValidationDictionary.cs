using System;
using System.Collections.Generic;
using System.Diagnostics;

#if DEBUG
[assembly: System.Runtime.CompilerServices.InternalsVisibleTo("MakViewModelBaseCoreTest")]
#endif

namespace MakCraft.ViewModels.Validations
{
    /// <summary>
    /// データ検証に用いるディクショナリ。
    /// </summary>
    [DebuggerDisplay("DicItem Count = {_innerDic.Count}, IsValid = {IsValid}")]
    class ValidationDictionary : IValidationDictionary
    {
        private readonly Dictionary<string, List<string>> _innerDic;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public ValidationDictionary() : this(new Dictionary<string, List<string>>(StringComparer.OrdinalIgnoreCase)) { }
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="dictionary"></param>
        public ValidationDictionary(Dictionary<string, List<string>> dictionary)
        {
            _innerDic = dictionary;
        }

        private List<string> GetErrorsForKey(string key)
        {
            if (key == null)
            {
                throw new ArgumentException(key);
            }

            if (!TryGetValue(key, out List<string> errors))
            {
                errors = new List<string>();
                this[key] = errors;
            }

            return errors;
        }

        #region IvalidationDictionary Members

        /// <summary>
        /// データ検証エラーの発生の有無を取得する。
        /// </summary>
        public bool IsValid
        {
            get
            {
                static bool hasError(ICollection<List<string>> values)
                {
                    foreach (var n in values)
                    {
                        if (n.Count != 0) return false;
                    }
                    return true;
                }
                return hasError(Values);
            }
        }

        /// <summary>
        /// データ検証エラーメッセージを追加する。
        /// </summary>
        /// <param name="key"></param>
        /// <param name="errorMessage"></param>
        public void AddError(string key, string errorMessage)
        {
            GetErrorsForKey(key).Add(errorMessage);
        }

        /// <summary>
        /// propertyName に設定されているエラーメッセージを削除します。
        /// </summary>
        /// <param name="propertyName"></param>
        public void RemoveErrorByKey(string propertyName)
        {
            if (_innerDic.ContainsKey(propertyName))
            {
                var errorCollection = this[propertyName];
                errorCollection.Clear();
                Remove(propertyName);
            }
        }

        /// <summary>
        /// 指定されたプロパティまたはエンティティ全体の検証エラーを取得します。
        /// </summary>
        /// <param name="propertyName">検証エラーを取得するプロパティの名前。または、エンティティ レベルのエラーを取得する場合は null または <see cref="System.String.Empty"/></param>
        /// <returns>プロパティまたはエンティティの検証エラー</returns>
        public IList<string> GetValidationError(string propertyName)
        {
            if (!string.IsNullOrEmpty(propertyName))
            {
                return GetErrorsForKey(propertyName);
            }
            else
            {
                var errors = new List<string>();
                foreach (var n in _innerDic.Values)
                {
                    if (n.Count != 0)
                    {
                        errors.AddRange(n);
                    }
                }
                return errors;
            }
        }

        #endregion

        private bool Remove(string key)
        {
            return _innerDic.Remove(key);
        }

        private bool TryGetValue(string key, out List<string> value)
        {
            return _innerDic.TryGetValue(key, out value);
        }

        private ICollection<List<string>> Values
        {
            get { return _innerDic.Values; }
        }

        private List<string> this[string key]
        {
            get
            {
                _innerDic.TryGetValue(key, out List<string> value);
                return value;
            }
            set
            {
                _innerDic[key] = value;
            }
        }
    }
}
