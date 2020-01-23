using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Xunit;
using MakCraft.ViewModels.Validations;

namespace MakViewModelBaseCoreTest
{
    public class ValidationUtilityTests
    {
        [Fact]
        public void GetPropatyNamesWithAttribute()
        {
            // Arrange
            var target = new TargetClass();

            // Act
            var actual = ValidationUtility.GetPropatyNamesWithAttribute(typeof(TargetClass));

            // Assert
            var expected = new List<string> { "Id", "Message" };
            Assert.True(TestHelper.IsSameCollection(expected, (List<string>)actual));
        }

        private class TargetClass
        {
            [Required]
            public int Id { get; set; }

            [StringLength(20, MinimumLength = 1)]
            public string Message { get; set; }

            public string Remark { get; set; }
        }
    }
}
