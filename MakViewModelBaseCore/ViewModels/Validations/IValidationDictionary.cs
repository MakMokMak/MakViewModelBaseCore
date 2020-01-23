using System.Collections.Generic;

namespace MakCraft.ViewModels.Validations
{
    /// <summary>
    /// サービス層とビューモデル層のデータ検証との間のインターフェイス。
    /// </summary>
    public interface IValidationDictionary
    {
        /// <summary>
        /// データ検証エラーの発生の有無を取得する。
        /// </summary>
        bool IsValid { get; }

        /// <summary>
        /// データ検証エラーメッセージを追加する。
        /// </summary>
        /// <param name="key">プロパティ名</param>
        /// <param name="errorMessage">エラーメッセージ</param>
        void AddError(string key, string errorMessage);

        /// <summary>
        /// propertyName に設定されているエラーメッセージを削除します。
        /// </summary>
        /// <param name="propertyName"></param>
        void RemoveErrorByKey(string propertyName);

        /// <summary>
        /// 指定されたプロパティまたはエンティティ全体の検証エラーを取得します。
        /// </summary>
        /// <param name="propertyName">検証エラーを取得するプロパティの名前。または、エンティティ レベルのエラーを取得する場合は null または <see cref="System.String.Empty"/></param>
        /// <returns>プロパティまたはエンティティの検証エラー</returns>
        IList<string> GetValidationError(string propertyName);
    }
}
