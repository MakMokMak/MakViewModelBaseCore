using System.Collections.Generic;
using MakCraft.ViewModels;

namespace TransitionTestApp.ViewModels
{
    class ModalWindow2ViewModel : ModalViewModelBase
    {
        public ModalWindow2ViewModel()
        {
            Setup();
        }

        private List<Book> _books;
        public List<Book> Books
        {
            get { return _books; }
            set
            {
                // NotifyObject.SetProperty メソッドを利用してプロパティの変更・変更通知を行う例
                // SetProperty メソッドの呼び出しは、propertyName の指定を省略しています
                base.SetProperty(ref _books, value);
            }
        }

        private Book _selectedBook;
        public Book SelectedBook
        {
            get { return _selectedBook; }
            set
            {
                // プロパティの実装コードでプロパティの変更を行い、NotifyObject.RaisePropertyChanged メソッドを
                // 利用してプロパティ変更通知を行う例(プロパティ名の取得に nameof 演算子を利用)
                _selectedBook = value;
                base.RaisePropertyChanged(nameof(SelectedBook));
            }
        }

        private void Setup()
        {
            Books = new List<Book> {
                new Book { Title = "猫の本", Note = "著者: みけ\r\n出版社: 猫の出版社\r\n概要: 猫ねこネコ" },
                new Book { Title = "犬の本", Note = "著者: ぽち\r\n出版社: 犬の出版社\r\n概要: 犬いぬイヌ" },
                new Book { Title = "金魚の本", Note = "著者: きん\r\n出版社: 金魚の出版社\r\n概要: 金魚きんぎょキンギョ" },
                new Book { Title = "鳥の本", Note = "著者: ぴよ\r\n出版社: 鳥の出版社\r\n概要: 鳥とりトリ" },
            };
        }

        protected override bool OkCanExecute(object param)
        {
            return (SelectedBook != null);
        }

        protected override void Dispose(bool disposing)
        {
            System.Diagnostics.Debug.WriteLine("** ModalWindow2ViewModel disposed.");
            base.Dispose(disposing);
        }
    }
}
