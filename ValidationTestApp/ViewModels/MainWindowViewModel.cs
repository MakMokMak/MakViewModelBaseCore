using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Windows.Input;
using MakCraft.ViewModels;
using MakCraft.ViewModels.Validations;
using ValidationTestApp.Models;
using ValidationTestApp.Services;

namespace ValidationTestApp.ViewModels
{
    class MainWindowViewModel : ValidationViewModelBase
    {
        private readonly IMemoService _service;

        public MainWindowViewModel()
        {
            _service = new MemoService();

            // 初期化時に、すべての属性付きプロパティのデータ検証を行う
            Validate();
        }

        public IReadOnlyList<Memo> Memos => _service.GetMemos();

        private string _title;
        [Required(ErrorMessage = "この項目は必須項目です。")]
        [StringLength(20, MinimumLength = 1, ErrorMessage = "1文字以上20文字以下のテキストを入力してください。")]
        public string Title
        {
            get => _title;
            set
            {
                SetProperty(ref _title, value);
                Validate(nameof(Note)); // サービス層の 関連チェックを行う前に、関連チェック対象となる Note プロパティの検証エラーを属性検証のものだけにする
                _service.CheckTitleNote(nameof(Title), Note, Title, base.ViewModelState);
            }
        }

        private string _note;
        [Required(ErrorMessage = "この項目は必須項目です。")]
        [StringLength(100, MinimumLength = 1, ErrorMessage = "1文字以上100文字以下のテキストを入力してください。")]
        public string Note
        {
            get => _note;
            set
            {
                SetProperty(ref _note, value);
                Validate(nameof(Title)); // サービス層の 関連チェックを行う前に、関連チェック対象となる Title プロパティの検証エラーを属性検証のものだけにする
                _service.CheckTitleNote(nameof(Note), Note, Title, base.ViewModelState);
            }
        }

        private string _strAge;
        [Required(ErrorMessage = "この項目は必須項目です。")]
        [Range(1, 130, ErrorMessage = "年齢は1から130までの数字を入力してください。")]
        public string StrAge
        {
            get => _strAge;
            set
            {
                SetProperty(ref _strAge, value);
            }
        }

        private bool _option;
        public bool Option
        {
            get { return _option; }
            set
            {
                SetProperty(ref _option, value);
                // 検証条件が変わるため、対象プロパティのデータ検証を行う
                Validate(nameof(Remark));
            }
        }

        private string _remark;
        // データ検証の条件を指定
        [ValidateConditional("Option", true)]
        [Required(ErrorMessage = "この項目は必須項目です。")]
        [MaxLength(20, ErrorMessage = "備考の長さは20文字までにしてください。")]
        public string Remark
        {
            get { return _remark; }
            set
            {
                SetProperty(ref _remark, value);
            }
        }

        private bool _option2;
        public bool Option2
        {
            get { return _option2; }
            set
            {
                SetProperty(ref _option2, value);
                // 検証条件が変わるため、対象プロパティのデータ検証を行う
                Validate(nameof(Remark2));
            }
        }

        private string _remark2;
        [ValidateConditional("Option2", true)]
        [Required(ErrorMessage = "この項目は必須項目です。")]
        [MaxLength(5, ErrorMessage = "備考2の長さは5文字までにしてください。")]
        public string Remark2
        {
            get { return _remark2; }
            set
            {
                SetProperty(ref _remark2, value);
            }
        }

        private void AddMemoComamndExecute()
        {
            var culture = CultureInfo.CurrentCulture;
            var memo = new Memo
            {
                Title = Title,
                Note = Note,
                Age = int.Parse(StrAge, culture.NumberFormat)
            };
            if (Option)
            {
                memo.Remark = Remark;
            }
            if (Option2)
            {
                memo.Remark2 = Remark2;
            }
            _service.AddMemo(memo, base.ViewModelState);
            base.RaisePropertyChanged(nameof(Memos));
            Title = string.Empty;
            Note = string.Empty;
            StrAge = string.Empty;
            Option = false;
            Remark = string.Empty;
            Option2 = false;
            Remark2 = string.Empty;
        }
        private bool AddMemoComamndCanExecute(object param)
        {
            return base.IsValid;
        }
        private ICommand _addMemoComamnd;
        public ICommand AddMemoComamnd
        {
            get
            {
                if (_addMemoComamnd == null)
                    _addMemoComamnd = new RelayCommand(AddMemoComamndExecute, AddMemoComamndCanExecute);
                return _addMemoComamnd;
            }
        }
    }
}
