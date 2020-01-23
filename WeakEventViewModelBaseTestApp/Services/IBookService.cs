using System.Collections.Generic;
using System.ComponentModel;
using WeakEventViewModelBaseTestApp.Models;

namespace WeakEventViewModelBaseTestApp.Services
{
    interface IBookService : INotifyPropertyChanged
    {
        int Count { get; }

        List<Book> Books { get; }

        Book GetBook(int id);

        void AddBook(Book book);
        void UpdateBook(Book book);
        void DeleteBook(int id);
    }
}
