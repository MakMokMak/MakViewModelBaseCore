using System;
using System.Collections.Generic;
using System.Linq;
using ValidationTestApp.Models;

namespace ValidationTestApp.DAL
{
    public class MemosRepository : IMemosRepository
    {
        private readonly List<Memo> _db;
        private int _nextId;

        public MemosRepository()
        {
            _db = new List<Memo>();
            _nextId = 0;
        }

        public IReadOnlyList<Memo> Find()
        {
            return _db.ToList();
        }

        public Memo GetMemo(int id)
        {
            return _db.Find(w => w.Id == id);
        }

        public Memo Add(Memo memo)
        {
            if (memo == null)
            {
                throw new ArgumentNullException(nameof(memo));
            }
            memo.Id = _nextId++;
            _db.Add(memo);
            return memo;
        }

        public void Update(Memo memo)
        {
            if (memo == null)
            {
                throw new ArgumentNullException(nameof(memo));
            }
            var dbMemo = GetMemo(memo.Id);
            dbMemo.Title = memo.Title;
            dbMemo.Note = memo.Note;
            dbMemo.Age = memo.Age;
            dbMemo.Remark = memo.Remark;
            dbMemo.Remark2 = memo.Remark2;
        }

        public void Delete(int id)
        {
            var dbMemo = GetMemo(id);
            _db.Remove(dbMemo);
        }
    }
}
