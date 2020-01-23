using System.Linq;
using WeakEventViewModelBaseTestApp.Models;

namespace WeakEventViewModelBaseTestApp.DAL
{
    interface IBooksRepository
    {
        int Count { get; }

        IQueryable<Book> FindBooks();
        Book GetBook(int id);

        Book Add(Book book);
        void Update(Book book);
        void Delete(int id);
    }
}
