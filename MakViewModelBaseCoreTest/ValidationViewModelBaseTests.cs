using Moq;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using Xunit;
using MakCraft.ViewModels;
using MakCraft.ViewModels.Validations;

namespace MakViewModelBaseCoreTest
{
    public class ValidationViewModelBaseTests
    {
        [Fact]
        public void IsValidWithError()
        {
            // Arrange
            var mock = new Mock<IValidationDictionary>();
            mock.Setup(x => x.IsValid).Returns(false);
            var viewModel = new TestableValidationViewModelBase(mock.Object);
            //viewModel.AddErrorMessage(nameof(viewModel.TestNum), "Error");

            // Act
            var actual = viewModel.IsValid;

            // Assert
            Assert.False(actual);
        }

        [Fact]
        public void IsValidWithNonError()
        {
            // Arrange
            var mock = new Mock<IValidationDictionary>();
            mock.Setup(x => x.IsValid).Returns(true);
            var viewModel = new TestableValidationViewModelBase(mock.Object);

            // Act
            var actual = viewModel.IsValid;

            // Assert
            Assert.True(actual);
        }

        [Fact]
        public void IsPropertyAnnotationErrorWithError()
        {
            // Arrange
            var propatyName = "TestProperty";
            var mock = new Mock<IValidationDictionary>();
            mock.Setup(x => x.GetValidationError(propatyName)).Returns(new List<string> { "Error message." });
            var viewModel = new TestableValidationViewModelBase(mock.Object);

            // Act
            var actual = viewModel.IsPropertyAnnotationError(propatyName);

            // Assert
            Assert.True(actual);
        }

        [Fact]
        public void IsPropertyAnnotationErrorWithNonError()
        {
            // Arrange
            var propatyName = "TestProperty";
            var mock = new Mock<IValidationDictionary>();
            mock.Setup(x => x.GetValidationError(propatyName)).Returns(new List<string>());
            var viewModel = new TestableValidationViewModelBase(mock.Object);

            // Act
            var actual = viewModel.IsPropertyAnnotationError(propatyName);

            // Assert
            Assert.False(actual);
        }

        [Fact]
        public void IndexAccessWithError()
        {
            // Arrange
            var propatyName = "TestProperty";
            var mes1 = "Error message1.";
            var mes2 = "Error message2.";
            var mock = new Mock<IValidationDictionary>();
            mock.Setup(x => x.GetValidationError(propatyName)).Returns(new List<string> { mes1, mes2 });
            var viewModel = new TestableValidationViewModelBase(mock.Object);

            // Act
            var actual = viewModel[propatyName];

            // Assert
            var expected = new string[] { mes1, mes2 };
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void IndexAccessWithNonError()
        {
            // Arrange
            var propatyName = "TestProperty";
            var mock = new Mock<IValidationDictionary>();
            mock.Setup(x => x.GetValidationError(propatyName)).Returns(new List<string>());
            var viewModel = new TestableValidationViewModelBase(mock.Object);

            // Act
            var actual = viewModel[propatyName];

            // Assert
            var expected = new string[] { };
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void ViewModelState()
        {
            // Arrange
            var mock = new Mock<IValidationDictionary>();
            var viewModel = new TestableValidationViewModelBase(mock.Object);

            // Act
            var actual = viewModel.ViewModelState;

            // Assert
            Assert.Same(mock.Object, actual);
        }

        [Fact]
        public void ValidateOk()
        {
            // Arrange
            var mock = new Mock<IValidationDictionary>();
            var viewModel = new TestableValidationViewModelBase(mock.Object);
            var propatyName = nameof(viewModel.TestNum);
            mock.Setup(x => x.RemoveErrorByKey(propatyName));

            // Act
            viewModel.TestNum = 10;

            // Assert
            mock.VerifyAll();
        }

        [Fact]
        public void ValidateOk_GetMessage()
        {
            // Arrange
            var mock = new Mock<IValidationDictionary>();
            var viewModel = new TestableValidationViewModelBase(mock.Object);
            var propatyName = nameof(viewModel.TestNum);
            mock.Setup(x => x.RemoveErrorByKey(propatyName));
            mock.Setup(x => x.GetValidationError(propatyName)).Returns(new List<string>());

            // Act
            viewModel.TestNum = 10;
            var actual = viewModel[propatyName];

            // Assert
            var expected = new string[] { };
            Assert.Equal(expected, actual);
            mock.VerifyAll();
        }

        [Fact]
        public void ValidateNg()
        {
            // Arrange
            var mock = new Mock<IValidationDictionary>();
            var viewModel = new TestableValidationViewModelBase(mock.Object);
            var propatyName = nameof(viewModel.TestNum);
            mock.Setup(x => x.RemoveErrorByKey(propatyName));
            mock.Setup(x => x.AddError(propatyName, "Range error!"));

            // Act
            viewModel.TestNum = 101;

            // Assert
            mock.VerifyAll();
        }

        [Fact]
        public void ValidateNg_GetMessageOne()
        {
            // Arrange
            var mock = new Mock<IValidationDictionary>();
            var viewModel = new TestableValidationViewModelBase(mock.Object);
            var propatyName = nameof(viewModel.TestNum);
            var message = "Range error!";
            mock.Setup(x => x.RemoveErrorByKey(propatyName));
            mock.Setup(x => x.AddError(propatyName, message));
            mock.Setup(x => x.GetValidationError(propatyName)).Returns(new List<string> { message });

            // Act
            viewModel.TestNum = 101;
            var actual = viewModel[propatyName];

            // Assert
            var expected = new string[] { message };
            Assert.Equal(expected, actual);
            mock.VerifyAll();
        }

        [Fact]
        public void ValidateNg_GetMessageTwo()
        {
            // Arrange
            var mock = new Mock<IValidationDictionary>();
            var viewModel = new TestableValidationViewModelBase(mock.Object);
            var propatyName = nameof(viewModel.TestNum);
            var message1 = "Range error!";
            var message2 = "one more error.";
            mock.Setup(x => x.RemoveErrorByKey(propatyName));
            mock.Setup(x => x.AddError(propatyName, message1));
            mock.Setup(x => x.GetValidationError(propatyName)).Returns(new List<string> { message1, message2 });

            // Act
            viewModel.TestNum = 101;
            var actual = viewModel[propatyName];

            // Assert
            var expected = new string[] { message1, message2 };
            Assert.Equal(expected, actual);
            mock.VerifyAll();
        }

        [Fact]
        public void ValidateOptionNotApplicable_CountRemove()
        {
            // Arrange
            int countCallRemove = 0;
            var mock = new Mock<IValidationDictionary>();
            var viewModel = new TestableValidationViewModelBase(mock.Object);
            var propertyName = nameof(viewModel.Remark);
            mock.Setup(x => x.RemoveErrorByKey(propertyName)).Callback(() => ++countCallRemove);
            mock.Setup(x => x.AddError(propertyName, "この項目は必須項目です。"));

            // Act
            viewModel.Option = false;

            // Assert
            var expected = 2;
            Assert.Equal(expected, countCallRemove);
            mock.VerifyAll();
        }

        [Fact]
        public void ValidateOptionApplicable_CountRemove()
        {
            // Arrange
            int countCallRemove = 0;
            var mock = new Mock<IValidationDictionary>();
            var viewModel = new TestableValidationViewModelBase(mock.Object);
            var propertyName = nameof(viewModel.Remark);
            mock.Setup(x => x.RemoveErrorByKey(propertyName)).Callback(() => ++countCallRemove);
            mock.Setup(x => x.AddError(propertyName, "この項目は必須項目です。"));

            // Act
            viewModel.Option = true;

            // Assert
            var expected = 1;
            Assert.Equal(expected, countCallRemove);
            mock.VerifyAll();
        }

        private class TestableValidationViewModelBase : ValidationViewModelBase
        {
            public TestableValidationViewModelBase(IValidationDictionary dictionary) : base(dictionary) { }

            private int _testNum;
            [Range(1, 100, ErrorMessage = "Range error!")]
            public int TestNum
            {
                get => _testNum;
                set
                {
                    SetProperty(ref _testNum, value);
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
            public string Remark
            {
                get { return _remark; }
                set
                {
                    SetProperty(ref _remark, value);
                }
            }
        }
    }
}
