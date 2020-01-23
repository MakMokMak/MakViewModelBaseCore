using System.ComponentModel;
using Xunit;
using MakCraft.ViewModels;

namespace MakViewModelBaseCoreTest
{
    public class NotifyObjectTests
    {
        public int DoCount { get; set; }
        public string FiredPropertyName { get; set; }

        [Fact]
        public void HaveAnINotifyPropertyChanged()
        {
            // Arrange
            var notifyObject = new TestableNotifyObject();

            // Act
            var interfaceObject = notifyObject as INotifyPropertyChanged;

            // Assert
            Assert.NotNull(interfaceObject);
        }

        [Fact]
        public void SetProperty_ChangeValue()
        {
            // Arrange
            var notifyObject = new TestableNotifyObject();

            // Act
            notifyObject.PropertyA = 1;

            // Assert
            Assert.Equal(1, notifyObject.PropertyA);
        }

        [Fact]
        public void SetProperty_ChangeValue_Fire_PropertyChanged()
        {
            // Arrange
            var notifyObject = new TestableNotifyObject();
            notifyObject.PropertyChanged += HandleEvent;

            // Act
            notifyObject.PropertyA = 1;

            // Assert
            Assert.Equal(1, DoCount);

            notifyObject.PropertyChanged -= HandleEvent;
        }

        [Fact]
        public void SetProperty_ChangeValue_Fire_PropertyChanged_Property_Name()
        {
            // Arrange
            var notifyObject = new TestableNotifyObject();
            notifyObject.PropertyChanged += HandleEvent;

            // Act
            notifyObject.PropertyA = 1;

            // Assert
            Assert.Equal("PropertyA", FiredPropertyName);

            notifyObject.PropertyChanged -= HandleEvent;
        }

        [Fact]
        public void SetProperty_SetSameValue_DoNotIgnite_PropertyChanged()
        {
            // Arrange
            var notifyObject = new TestableNotifyObject();
            notifyObject.PropertyChanged += HandleEvent;

            // Act
            notifyObject.PropertyA = 0;

            // Assert
            Assert.Equal(0, DoCount);

            notifyObject.PropertyChanged -= HandleEvent;
        }

        [Fact]
        public void RaisePropertyChanged_with_property_name_null()
        {
            // Arrange
            var notifyObject = new TestableNotifyObject();
            notifyObject.PropertyChanged += HandleEvent;

            // Act
            notifyObject.PropertyB1 = 1;

            // Assert
            Assert.Equal("PropertyB1", FiredPropertyName);

            notifyObject.PropertyChanged -= HandleEvent;
        }

        [Fact]
        public void RaisePropertyChanged_with_property_name()
        {
            // Arrange
            var notifyObject = new TestableNotifyObject();
            notifyObject.PropertyChanged += HandleEvent;

            // Act
            notifyObject.PropertyB2 = 1;

            // Assert
            Assert.Equal("PropertyB2", FiredPropertyName);

            notifyObject.PropertyChanged -= HandleEvent;
        }

        private void HandleEvent(object sender, PropertyChangedEventArgs e)
        {
            ++DoCount;
            FiredPropertyName = e.PropertyName;
        }

        private class TestableNotifyObject : NotifyObject
        {
            private int _propertyA;
            public int PropertyA
            {
                get { return _propertyA; }
                set
                {
                    base.SetProperty(ref _propertyA, value);
                }
            }

            private int _propertyB1;
            public int PropertyB1
            {
                get { return _propertyB1; }
                set
                {
                    _propertyB1 = value;
                    base.RaisePropertyChanged();
                }
            }

            private int _propertyB2;
            public int PropertyB2
            {
                get { return _propertyB2; }
                set
                {
                    _propertyB2 = value;
                    base.RaisePropertyChanged(nameof(PropertyB2));
                }
            }
        }
    }
}
