using System;
using System.Collections.Generic;
using System.Linq;
using WeakEventViewModelBaseTestApp.Models;

namespace WeakEventViewModelBaseTestApp.DAL
{
    class BooksRepository : IBooksRepository
    {
        public BooksRepository()
        {
            booksInit();
        }

        private List<Book> _books;

        public int Count
        {
            get { return _books.Count; }
        }

        public IQueryable<Book> FindBooks()
        {
            return _books.AsQueryable();
        }

        public Book GetBook(int id)
        {
            return _books.Find(w => w.Id == id);
        }

        public Book Add(Book book)
        {
            var newId = _books.Max(w => w.Id);
            if (newId == int.MaxValue)
            {
                throw new OverflowException("Book の ID が 許容される最大値に達しているため追加できませんでした");
            }
            book.Id = ++newId;
            _books.Add(book);

            return book;
        }

        public void Update(Book book)
        {
            var index = _books.FindIndex(w => w.Id == book.Id);
            if (index < 0)
            {
                throw new ArgumentException("更新対象のキーを持つデータは存在しません。");
            }
            _books[index] = book;
        }

        public void Delete(int id)
        {
            var target = _books.Find(w => w.Id == id);
            _books.Remove(target);
        }

        private void booksInit()
        {
            _books = new List<Book> {
                new Book {
                    Id = 0,
                    Title = "猫の本",
                    Author = "三毛猫",
                    Price = 1200,
                    Feature = "三毛猫の日常を描いた問題作"
                },
                new Book {
                    Id = 1,
                    Title = "犬の本",
                    Author = "スピッツ",
                    Price = 1400,
                    Feature = "スピッツの日常を描いた話題作"
                },
            };
        }
    }
}
