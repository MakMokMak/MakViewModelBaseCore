using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Input;
using MakCraft.ViewModels;
using WeakEventViewModelBaseTestApp.Models;
using WeakEventViewModelBaseTestApp.Services;

namespace WeakEventViewModelBaseTestApp.ViewModels
{
    class MainWindowViewModel : WeakEventViewModelBase
    {
        private readonly IBookService _service;
        private readonly IPropertyChangedWeakEventListener _listener;

        public MainWindowViewModel() : this(new BookService(), new PropertyChangedWeakEventListener()) { }
        public MainWindowViewModel(IBookService service, IPropertyChangedWeakEventListener listener)
        {
            _service = service;
            _listener = listener;
            _listener.WeakPropertyChanged += OnWeakListenerPropertyChanged;
            base.AddListener(_service, _listener);
        }

        public int BooksCount
        {
            get { return _service.Count; }
        }

        public List<Book> Books
        {
            get { return _service.Books; }
        }

        private void AddBookCommandExecute()
        {
            var book = new Book
            {
                Id = 0,
                Title = "くまの本",
                Author = "森のくまさん",
                Price = 1600,
                Feature = "森のくまさんの生活を描いた自信作"
            };
            _service.AddBook(book);
        }
        private ICommand _addBookCommand;
        public ICommand AddBookCommand
        {
            get
            {
                if (_addBookCommand == null)
                    _addBookCommand = new RelayCommand(AddBookCommandExecute);
                return _addBookCommand;
            }
        }

        private void OnWeakListenerPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.RaisePropertyChanged(nameof(BooksCount));
            base.RaisePropertyChanged(nameof(Books));
        }
    }
}
