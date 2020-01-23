using System.Collections.Generic;
using Xunit;
using MakCraft.ViewModels.Validations;

namespace MakViewModelBaseCoreTest
{
#if DEBUG
    public class ValidationDictionaryTests
    {
        [Fact]
        public void IsValidTrue()
        {
            // Arrange
            var dictionary = new ValidationDictionary();

            // Act
            var actual = dictionary.IsValid;

            // Assert
            Assert.True(actual);
        }

        [Fact]
        public void IsValidFalse()
        {
            // Arrange
            var inner = new Dictionary<string, List<string>>
            {
                { "Item", new List<string>{ "ErrorMessage." } }
            };
            var dictionary = new ValidationDictionary(inner);

            // Act
            var actual = dictionary.IsValid;

            // Assert
            Assert.False(actual);
        }

        [Fact]
        public void AddErrorOne()
        {
            // Arrange
            var inner = new Dictionary<string, List<string>>();
            var dictionary = new ValidationDictionary(inner);
            var propertyName = "Item";
            var errorMessage = "ErrorMessage.";

            // Act
            dictionary.AddError(propertyName, errorMessage);

            // Assert
            Assert.Equal(errorMessage, inner[propertyName][0]);
        }

        [Fact]
        public void AddErrorTwo()
        {
            // Arrange
            var inner = new Dictionary<string, List<string>>();
            var dictionary = new ValidationDictionary(inner);
            var propertyName1 = "Item1";
            var errorMessage1 = "ErrorMessage2.";
            var errorMessage2 = "ErrorMessage2.";

            // Act
            dictionary.AddError(propertyName1, errorMessage1);
            dictionary.AddError(propertyName1, errorMessage2);

            // Assert
            Assert.Equal(errorMessage2, inner[propertyName1][1]);
        }

        [Fact]
        public void RemoveErrorByKey1_0()
        {
            // Arrange
            var propertyName1 = "Item1";
            var errorMessage1 = "ErrorMessage2.";
            var errorMessage2 = "ErrorMessage2.";
            var inner = new Dictionary<string, List<string>>
            {
                { propertyName1, new List<string>{ errorMessage1, errorMessage2 } }
            };
            var dictionary = new ValidationDictionary(inner);

            // Act
            dictionary.RemoveErrorByKey(propertyName1);

            // Assert
            Assert.Empty(inner);
        }

        [Fact]
        public void RemoveErrorByKey2_1()
        {
            // Arrange
            var propertyName1 = "Item1";
            var errorMessage1_1 = "ErrorMessage1_1.";
            var errorMessage1_2 = "ErrorMessage1_2.";
            var propertyName2 = "Item2";
            var errorMessage2_1 = "ErrorMessage2_1.";
            var errorMessage2_2 = "ErrorMessage2_2.";
            var inner = new Dictionary<string, List<string>>
            {
                { propertyName1, new List<string>{ errorMessage1_1, errorMessage1_2 } },
                { propertyName2, new List<string>{ errorMessage2_1, errorMessage2_2 } },
            };
            var dictionary = new ValidationDictionary(inner);

            // Act
            dictionary.RemoveErrorByKey(propertyName1);

            // Assert
            Assert.False(inner.ContainsKey(propertyName1));
            Assert.True(inner.ContainsKey(propertyName2));
        }

        [Fact]
        public void GetValidationError_Item_1()
        {
            // Arrange
            var propertyName1 = "Item1";
            var errorMessage1 = "ErrorMessage2.";
            var errorMessage2 = "ErrorMessage2.";
            var inner = new Dictionary<string, List<string>>
            {
                { propertyName1, new List<string>{ errorMessage1, errorMessage2 } }
            };
            var dictionary = new ValidationDictionary(inner);

            // Act
            var actual = dictionary.GetValidationError(propertyName1);

            // Assert
            var expected = new List<string>
            {
                errorMessage1,
                errorMessage2
            };
            Assert.True(TestHelper.IsSameCollection(expected, actual));
        }

        [Fact]
        public void GetValidationError_Item_2()
        {
            // Arrange
            var propertyName1 = "Item1";
            var errorMessage1_1 = "ErrorMessage1_1.";
            var errorMessage1_2 = "ErrorMessage1_2.";
            var propertyName2 = "Item2";
            var errorMessage2_1 = "ErrorMessage2_1.";
            var errorMessage2_2 = "ErrorMessage2_2.";
            var inner = new Dictionary<string, List<string>>
            {
                { propertyName1, new List<string>{ errorMessage1_1, errorMessage1_2 } },
                { propertyName2, new List<string>{ errorMessage2_1, errorMessage2_2 } },
            };
            var dictionary = new ValidationDictionary(inner);

            // Act
            var actual = dictionary.GetValidationError(propertyName1);

            // Assert
            var expected = new List<string>
            {
                errorMessage1_1,
                errorMessage1_2
            };
            Assert.True(TestHelper.IsSameCollection(expected, actual));
        }

        [Fact]
        public void GetValidationError_All_Item_2()
        {
            // Arrange
            var propertyName1 = "Item1";
            var errorMessage1_1 = "ErrorMessage1_1.";
            var errorMessage1_2 = "ErrorMessage1_2.";
            var propertyName2 = "Item2";
            var errorMessage2_1 = "ErrorMessage2_1.";
            var errorMessage2_2 = "ErrorMessage2_2.";
            var inner = new Dictionary<string, List<string>>
            {
                { propertyName1, new List<string>{ errorMessage1_1, errorMessage1_2 } },
                { propertyName2, new List<string>{ errorMessage2_1, errorMessage2_2 } },
            };
            var dictionary = new ValidationDictionary(inner);

            // Act
            var actual = dictionary.GetValidationError(null);

            // Assert
            var expected = new List<string>
            {
                errorMessage1_1,
                errorMessage1_2,
                errorMessage2_1,
                errorMessage2_2,
            };
            Assert.True(TestHelper.IsSameCollection(expected, actual));
        }
    }
#endif
}
