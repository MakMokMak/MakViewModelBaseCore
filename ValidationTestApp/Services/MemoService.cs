using System;
using System.Collections.Generic;
using MakCraft.ViewModels.Validations;
using ValidationTestApp.DAL;
using ValidationTestApp.Models;

namespace ValidationTestApp.Services
{
    public class MemoService : IMemoService
    {
        private readonly IMemosRepository _repository;

        public MemoService() : this(new MemosRepository()) { }
        public MemoService(IMemosRepository repository)
        {
            _repository = repository;
        }

        #region IMemoService
        public IReadOnlyList<Memo> GetMemos()
        {
            return _repository.Find();
        }

        public Memo GetMemo(int id)
        {
            return _repository.GetMemo(id);
        }

        public Memo AddMemo(Memo memo, IValidationDictionary validationDic)
        {
            if (validationDic == null)
            {
                throw new ArgumentNullException(nameof(validationDic));
            }
            if (validationDic.IsValid)
            {
                return _repository.Add(memo);
            }

            return memo;
        }

        public void UpdateMemo(Memo memo, IValidationDictionary validationDic)
        {
            throw new NotImplementedException();
        }

        public void DeleteMemo(int id)
        {
            throw new NotImplementedException();
        }

        public void CheckTitleNote(string propatyName, string note, string title, IValidationDictionary validationDic)
        {
            if (validationDic == null)
            {
                throw new ArgumentNullException(nameof(validationDic));
            }

            if (!string.IsNullOrEmpty(note) && !string.IsNullOrEmpty(title) && note.IndexOf(title, StringComparison.Ordinal) < 0)
            {
                validationDic.AddError(propatyName, "本文中にタイトルが記述されていません。");
                validationDic.AddError(propatyName, "もひとつエラー。");
            }
        }
        #endregion
    }
}
