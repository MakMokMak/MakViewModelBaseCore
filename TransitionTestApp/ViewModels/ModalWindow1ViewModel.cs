using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Windows;
using System.Windows.Input;
using MakCraft.Behaviors;
using MakCraft.ViewModels;

namespace TransitionTestApp.ViewModels
{
    class ModalWindow1ViewModel : ModalViewModelBase
    {
        public ModalWindow1ViewModel()
        {
            Setup();
            base.Validate();
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
        [Required(ErrorMessage = "選択してください。")]
        public Book SelectedBook
        {
            get { return _selectedBook; }
            set
            {
                // ValidationViewModelsBase.SetPropertyWithRemoveItemValidationError メソッドを利用して
                // プロパティの変更・変更通知を行う例
                // SetProperty メソッドの呼び出しは、propertyName の指定を省略しています
                base.SetProperty(ref _selectedBook, value);
            }
        }

        private string _messageText;
        public string MessageText
        {
            get { return _messageText; }
            set
            {
                // プロパティの実装コードでプロパティの変更を行い、NotifyObject.RaisePropertyChanged メソッドを
                // 利用してプロパティ変更通知を行う例(プロパティ名の取得に nameof 演算子を利用)
                _messageText = value;
                base.RaisePropertyChanged(nameof(MessageText));
            }
        }

        private void MessageExecute()
        {
            MessageText = string.Empty;
            // コールバックの処理を設定
            base.MessageDialogActionCallback = result =>
            {
                switch (result)
                {
                    case MessageBoxResult.Yes:
                        MessageText = "Yes が選択されました。";
                        break;
                    case MessageBoxResult.No:
                        MessageText = "No が選択されました。";
                        break;
                }
            };
            // MessageDialogAction ビヘイビアを使用してメッセージボックスを出す
            base.MessageDialogActionParam = new MessageDialogActionParameter("クリックされました。",
                                        "メッセージ", MessageBoxButton.YesNo, true);
        }
        private ICommand _messageCommand;
        public ICommand MessageCommand
        {
            get
            {
                if (_messageCommand == null)
                    _messageCommand = new RelayCommand(MessageExecute);
                return _messageCommand;
            }
        }

        private void Message2Execute()
        {
            MessageText = string.Empty;
            // コールバックの処理を設定
            base.MessageDialogActionCallback = result =>
            {
                switch (result)
                {
                    case MessageBoxResult.OK:
                        MessageText = "OK が選択されました。";
                        break;
                    case MessageBoxResult.Cancel:
                        MessageText = "Cancel が選択されました。";
                        break;
                }
            };
            // MessageDialogAction ビヘイビアを使用してメッセージボックスを出す
            base.MessageDialogActionParam = new MessageDialogActionParameter("クリックされました。",
                                        "メッセージ", MessageBoxButton.OKCancel, true);
        }
        private ICommand _message2Command;
        public ICommand Message2Command
        {
            get
            {
                if (_message2Command == null)
                    _message2Command = new RelayCommand(Message2Execute);
                return _message2Command;
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

        protected override void Dispose(bool disposing)
        {
            System.Diagnostics.Debug.WriteLine("** ModalWindow1ViewModel disposed.");
            base.Dispose(disposing);
        }
    }

    public class Book
    {
        public string Title { get; set; }
        public string Note { get; set; }
    }
}
