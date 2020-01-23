using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using MakCraft.ViewModels.Validations;

namespace MakCraft.ViewModels
{
    /// <summary>
    /// データ検証を実装したビューモデルの基底クラス。
    /// </summary>
    public abstract class ValidationViewModelBase : WeakEventViewModelBase, INotifyDataErrorInfo
    {
        private readonly IValidationDictionary _dictionary;
        // 式木のキャッシュ
        private static readonly Dictionary<string, Func<object, object>> _cacheExpTree = new Dictionary<string, Func<object, object>>();

        /// <summary>
        /// コンストラクタ。
        /// </summary>
        public ValidationViewModelBase() : this(new ValidationDictionary()) { }
        /// <summary>
        /// コンストラクタ。
        /// </summary>
        /// <param name="dictionary">データ検証に用いるディクショナリ</param>
        public ValidationViewModelBase(IValidationDictionary dictionary)
        {
            _dictionary = dictionary;
        }

        /// <summary>
        /// データ検証エラーの発生の有無を取得します。
        /// </summary>
        public bool IsValid
        {
            get { return _dictionary.IsValid; }
        }

        /// <summary>
        /// 指定されたプロパティの System.ComponentModel.DataAnnotations のデータ検証アトリビュート検査の結果を確認します。
        /// propertyName が省略された場合、呼び出し元のメソッドまたはプロパティの名前を用います。
        /// </summary>
        /// <param name="propertyName"></param>
        /// <returns>検証エラーが発生していれば true</returns>
        public bool IsPropertyAnnotationError([CallerMemberName] string propertyName = null)
        {
            return (this[propertyName].Length != 0);
        }

        /// <summary>
        /// 指定した名前のプロパティに関するエラー メッセージの配列を取得します。
        /// </summary>
        /// <param name="columnName"></param>
        /// <returns></returns>
        public string[] this[string columnName]
        {
            get
            {
                return ((List<string>)_dictionary.GetValidationError(columnName)).ToArray();
            }
        }

        /// <summary>
        /// ビューモデルの状態及びバインディングの検証の状態を格納するビューモデル状態ディクショナリ オブジェクトを取得します。
        /// </summary>
        public IValidationDictionary ViewModelState
        {
            get { return _dictionary; }
        }

        /// <summary>
        /// すべてのデータ検証対象プロパティのデータ検証を行います。
        /// </summary>
        /// <returns></returns>
        protected bool Validate()
        {
            var result = true;
            var properties = ValidationUtility.GetPropatyNamesWithAttribute(GetType());
            foreach (var property in properties)
            {
                _dictionary.RemoveErrorByKey(property);
                if (!Validate(property))
                {
                    result = false;
                }
            }

            return result;
        }

        /// <summary>
        /// プロパティのデータ検証を行います。
        /// </summary>
        /// <param name="propertyName">プロパティ名</param>
        /// <returns></returns>
        protected bool Validate(string propertyName)
        {
            _dictionary.RemoveErrorByKey(propertyName);
            var results = new List<ValidationResult>();
            var result = Validator.TryValidateProperty(GetType().GetProperty(propertyName).GetValue(this, null), new ValidationContext(this, null, null) { MemberName = propertyName }, results);
            if (!result)
            {
                results.ForEach(n => _dictionary.AddError(propertyName, n.ErrorMessage));
            }
            ConditionalValidate(propertyName); // データ検証を行う条件を確認して条件が成立しなかったら検証エラーを削除する
            RaiseErrorsChanged(propertyName);

            return result;
        }

        // データ検証を行う条件を確認して条件が成立しなかったら検証エラーを削除する(ValidateConditionalAttribute が設定されている場合)
        private void ConditionalValidate(string propertyName)
        {
            var attrib = Attribute.GetCustomAttribute(GetType().GetProperty(propertyName),
                typeof(ValidateConditionalAttribute));
            if (attrib == null) return; // ValidateConditionalAttribute が設定されていない

            var conditional = attrib as ValidateConditionalAttribute;
            var targetAttrib = GetType().GetProperty(conditional.ComparedProperty);
            if (targetAttrib == null)
            {
                var message = $"データ検証を行う比較対象として指定されたプロパティが見つかりませんでした(検証プロパティ名: {propertyName}, 比較対象プロパティ名: {conditional.ComparedProperty})。";
                throw new MissingMemberException(message);
            }
            // 式木を使って columnName の条件の比較対象となるプロパティ値を取得(作成した式木はキャッシュしておく)
            var t = GetType();
            var key = $"{t.FullName}.{targetAttrib.Name}";
            if (!_cacheExpTree.TryGetValue(key, out Func<object, object> f))
            {
                f = CreateMethod(t, targetAttrib.Name);
                _cacheExpTree[key] = f;
            }
            var target = f(this);

            bool condition = false;
            if (target == null)
            {
                // 比較対象プロパティが null なので、条件値が null であるか比較する
                if (conditional.Value == null)
                {
                    condition = true;
                }
            }
            else
            {
                // 比較対象プロパティの動的な型が持つ Equals メソッドを呼び出して条件値と比較する
                // (動的型付け変数を利用することで毎回リフレクションを呼び出すコストを回避)
                dynamic equalsObj = target;
                condition = (bool)equalsObj.Equals(conditional.Value);
            }
            if (!condition)
            {
                // 条件が成立しないので検証エラーを削除
                _dictionary.RemoveErrorByKey(propertyName);
            }
        }

        // columnName のプロパティ値を取得する式木を生成する
        private static Func<object, object> CreateMethod(Type t, string propertyName)
        {
            var x = Expression.Parameter(typeof(object));
            var p = Expression.Parameter(t);

            var exp = Expression.Lambda(
                Expression.Block(
                    new[] { p },
                    Expression.Assign(p, Expression.Convert(x, t)),
                    Expression.Convert(Expression.Property(p, propertyName), typeof(object))),
                new ParameterExpression[] { x }
                );
            var d = (Func<object, object>)exp.Compile();
            return d;
        }

        /// <summary>
        /// メンバ変数 <paramref name="variable"/> を <paramref name="value"/> の値で書き換え、<paramref name="propertyName"/> を用いてPropertyChanged イベントを発火し、データ検証属性を用いたデータの検証を行います。
        /// <paramref name="propertyName"/> が省略された場合、呼び出し元のプロパティの名前を用います。
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="variable"></param>
        /// <param name="value"></param>
        /// <param name="propertyName"></param>
        protected override void SetProperty<T>(ref T variable, T value, [CallerMemberName] string propertyName = null)
        {
            if (!Equals(variable, value))
            {
                variable = value;
                RaisePropertyChanged(propertyName);
                Validate(propertyName);
            }
        }

        /// <summary>
        /// ErrorsChanged イベントを発火します。
        /// </summary>
        /// <param name="propertyName">エラー状態が変化したプロパティの名前</param>
        protected virtual void RaiseErrorsChanged(string propertyName)
        {
            ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(propertyName));
        }
        #region INotifyDataErrorInfo
        /// <summary>
        /// エンティティに検証エラーがあるかどうかを示す値を取得します。
        /// </summary>
        public bool HasErrors => !_dictionary.IsValid;

        /// <summary>
        /// プロパティまたはエンティティ全体の検証エラーが変更されたときに発生します。
        /// </summary>
        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;

        /// <summary>
        /// 指定されたプロパティまたはエンティティ全体の検証エラーを取得します。
        /// </summary>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        public IEnumerable GetErrors(string propertyName)
        {
            return _dictionary.GetValidationError(propertyName);
        }
        #endregion
    }
}
