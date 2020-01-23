using System;
using Xunit;
using MakCraft.ViewModels;

namespace MakViewModelBaseCoreTest
{
    public class ViewModelBaseTests
    {
        [Fact]
        public void HaveAnIDisposable()
        {
            // Arrange
            var notifyObject = new TestableViewModelBase();

            // Act
            var interfaceObject = notifyObject as IDisposable;

            // Assert
            Assert.NotNull(interfaceObject);
        }

        [Fact]
        public void InheritingNotifyObject()
        {
            // Arrange
            var notifyObject = new TestableViewModelBase();

            // Act
            var inheritingObject = notifyObject as NotifyObject;

            // Assert
            Assert.NotNull(inheritingObject);
        }

        [Fact]
        public void SameThreadCall()
        {
            // Arrange
            var notifyObject = new TestableViewModelBase();

            // Act
            bool isUiThread = notifyObject.IsTestIsUiThread;

            // Assert
            Assert.True(isUiThread);
        }

        [Fact]
        public void AnotherThreadCall()
        {
            // Arrange
            var notifyObject = new TestableViewModelBase();

            // Act
            bool isUiThread = true;
            System.Threading.Tasks.Task[] tasks = new System.Threading.Tasks.Task[1];
            tasks[0] = System.Threading.Tasks.Task.Factory.StartNew(
                    () => isUiThread = notifyObject.IsTestIsUiThread);
            System.Threading.Tasks.Task.Factory.ContinueWhenAll(tasks, completedTasks => {
                // Assert
                Assert.False(isUiThread);
            });
        }

        private class TestableViewModelBase : ViewModelBase
        {
            public bool IsTestIsUiThread
            {
                get { return IsUiThread(); }
            }
        }
    }
}
