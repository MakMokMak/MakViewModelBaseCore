using System.Collections.Generic;
using ValidationTestApp.Models;

namespace ValidationTestApp.DAL
{
    public interface IMemosRepository
    {
        IReadOnlyList<Memo> Find();
        Memo GetMemo(int id);
        Memo Add(Memo memo);
        void Update(Memo memo);
        void Delete(int id);
    }
}
