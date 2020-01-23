using System.Collections.Generic;
using ValidationTestApp.Models;
using MakCraft.ViewModels.Validations;

namespace ValidationTestApp.Services
{
    public interface IMemoService
    {
        IReadOnlyList<Memo> GetMemos();
        Memo GetMemo(int id);
        Memo AddMemo(Memo memo, IValidationDictionary validationDic);
        void UpdateMemo(Memo memo, IValidationDictionary validationDic);
        void DeleteMemo(int id);

        void CheckTitleNote(string propatyName, string note, string title, IValidationDictionary validationDic);
    }
}
