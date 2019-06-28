// ************************************************************************
// *****      COPYRIGHT 2014 - 2018 HONEYWELL INTERNATIONAL SARL      *****
// ************************************************************************

namespace Offering.Pipeline.TodoService.Tests
{
    using FakeItEasy;
    using FluentAssertions;
    using Honeywell.IotStreamApp.IotStreamService;
    using Honeywell.IotStreamApp.IotStreamService.DependentInterfaces;
    using Honeywell.IotStreamApp.IotStreamService.Impl;
    using System;
    using System.Diagnostics;
    using System.Linq;
    using Xunit;
    using Xunit.Abstractions;

    public class TodoServiceTests : IDisposable
    {
        private readonly ITodoRepository _repository;

        private readonly TodoItem _todoItem = new TodoItem
        {
            Id = Guid.NewGuid(),
            CreatedTimeStamp = DateTime.UtcNow,
            Description = "item description",
            IsCompleted = true,
            Title = "item title"
        };

        private readonly TodoService _todoService;

        private readonly TraceListener _traceListener;

        public TodoServiceTests(ITestOutputHelper output)
        {
            _traceListener = new XUnitTraceListener(output);
            Trace.Listeners.Add(_traceListener);

            _repository = A.Fake<ITodoRepository>();
            _todoService = new TodoService(_repository);
        }

        public void Dispose()
        {
            Trace.Listeners.Remove(_traceListener);
        }

        [Fact]
        public void Add_NewItem_ReturnsResultObject()
        {
            // Arrange
            A.CallTo(() => _repository.AddItem(A<TodoItem>._)).Returns(true);

            // Act
            var result = _todoService.Add(_todoItem.Title, _todoItem.Description);

            // Assert
            result.IsSuccess.Should().BeTrue();
            result.NewItem.Should().NotBeNull();
            result.NewItem.Title.Should().Be(_todoItem.Title);
            result.NewItem.Description.Should().Be(_todoItem.Description);
            result.NewItem.CreatedTimeStamp.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromMilliseconds(100));
            result.NewItem.Id.Should().NotBeEmpty();
            result.NewItem.IsCompleted.Should().BeFalse();
        }

        [Fact]
        public void Add_DuplicateItem_ReturnsFalse()
        {
            // Arrange
            A.CallTo(() => _repository.AddItem(A<TodoItem>._)).Returns(false);

            // Act
            var result = _todoService.Add(_todoItem.Title, _todoItem.Description);

            // Assert
            result.IsSuccess.Should().BeFalse();
            result.NewItem.Should().BeNull();
        }

        [Fact]
        public void Add_WhenRepositoryThrows_ReturnsFalse()
        {
            // Arrange
            A.CallTo(() => _repository.AddItem(A<TodoItem>._)).Throws<NullReferenceException>();

            // Act
            var result = _todoService.Add(_todoItem.Title, _todoItem.Description);

            // Assert
            result.IsSuccess.Should().BeFalse();
            result.NewItem.Should().BeNull();
        }

        [Fact]
        public void Delete_IdExists_ReturnsTrue()
        {
            // Arrange
            TodoItem outResult;
            A.CallTo(() => _repository.RemoveItem(A<Guid>._, out outResult)).Returns(true);

            // Act
            var result = _todoService.Delete(_todoItem.Id);

            // Assert
            result.IsSuccess.Should().BeTrue();
            result.IsNotFoundError.Should().BeFalse();
        }

        [Fact]
        public void Delete_IdDoesNotExist_ReturnsFalseAndIsNotFound()
        {
            // Arrange
            TodoItem outResult;
            A.CallTo(() => _repository.RemoveItem(A<Guid>._, out outResult)).Returns(false);

            // Act
            var result = _todoService.Delete(_todoItem.Id);

            // Assert
            result.IsSuccess.Should().BeFalse();
            result.IsNotFoundError.Should().BeTrue();
        }

        [Fact]
        public void Delete_EmptyId_ReturnsFalse()
        {
            // Act
            var result = _todoService.Delete(Guid.Empty);

            // Assert
            result.IsSuccess.Should().BeFalse();
            result.IsNotFoundError.Should().BeFalse();
        }

        [Fact]
        public void Delete_WhenRepositoryThrows_ReturnsFalse()
        {
            // Arrange
            TodoItem outResult;
            A.CallTo(() => _repository.RemoveItem(A<Guid>._, out outResult)).Throws<NullReferenceException>();

            // Act
            var result = _todoService.Delete(_todoItem.Id);

            // Assert
            result.IsSuccess.Should().BeFalse();
            result.IsNotFoundError.Should().BeFalse();
        }

        [Fact]
        public void Update_IdExists_ReturnsTrue()
        {
            // Arrange
            var existingItem = GetExistingItemForUpdateTests();

            TodoItem removedItem;
            A.CallTo(() => _repository.RemoveItem(A<Guid>._, out removedItem)).Returns(true).AssignsOutAndRefParameters(existingItem);
            A.CallTo(() => _repository.AddItem(existingItem)).Returns(true);
            // Act

            var result = _todoService.Update(existingItem.Id, null, null, null);

            // Assert
            result.IsSuccess.Should().BeTrue();
            result.UpdatedItem.Should().BeEquivalentTo(existingItem);
        }

        private TodoItem GetExistingItemForUpdateTests()
        {
            var existingItem = new TodoItem
            {
                Id = Guid.NewGuid(),
                CreatedTimeStamp = _todoItem.CreatedTimeStamp,
                Description = "description",
                IsCompleted = false,
                Title = "title"
            };
            return existingItem;
        }

        [Fact]
        public void Update_OnlyCompletedStatus_ResultObjectHasCorrectCompletedStatus()
        {
            // Arrange
            var existingItem = GetExistingItemForUpdateTests();

            TodoItem removedItem;
            A.CallTo(() => _repository.RemoveItem(A<Guid>._, out removedItem)).Returns(true).AssignsOutAndRefParameters(existingItem);
            A.CallTo(() => _repository.AddItem(existingItem)).Returns(true);
            // Act

            var result = _todoService.Update(existingItem.Id, null, null, true);

            // Assert
            result.IsSuccess.Should().BeTrue();
            result.IsNotFoundError.Should().BeFalse();
            result.UpdatedItem.IsCompleted.Should().Be(true);
        }

        [Fact]
        public void Update_OnlyDescription_ResultObjectHasCorrectDescription()
        {
            // Arrange
            var existingItem = GetExistingItemForUpdateTests();

            TodoItem removedItem;
            A.CallTo(() => _repository.RemoveItem(A<Guid>._, out removedItem)).Returns(true).AssignsOutAndRefParameters(existingItem);
            A.CallTo(() => _repository.AddItem(existingItem)).Returns(true);
            // Act

            var result = _todoService.Update(existingItem.Id, null, "newDescription", null);

            // Assert
            result.IsSuccess.Should().BeTrue();
            result.IsNotFoundError.Should().BeFalse();
            result.UpdatedItem.Description.Should().Be("newDescription");
        }

        [Fact]
        public void Update_OnlyTitle_ResultObjectHasCorrectTitle()
        {
            // Arrange
            var existingItem = GetExistingItemForUpdateTests();

            TodoItem removedItem;
            A.CallTo(() => _repository.RemoveItem(A<Guid>._, out removedItem)).Returns(true).AssignsOutAndRefParameters(existingItem);
            A.CallTo(() => _repository.AddItem(existingItem)).Returns(true);
            // Act

            var result = _todoService.Update(existingItem.Id, "newTitle", null, null);

            // Assert
            result.IsSuccess.Should().BeTrue();
            result.IsNotFoundError.Should().BeFalse();
            result.UpdatedItem.Title.Should().Be("newTitle");
        }

        [Fact]
        public void Update_EmptyId_ReturnsIsNotFound()
        {
            // Act
            var result = _todoService.Update(Guid.Empty, _todoItem.Title, _todoItem.Description, _todoItem.IsCompleted);

            // Assert
            result.IsSuccess.Should().BeFalse();
            result.IsNotFoundError.Should().BeTrue();
            result.UpdatedItem.Should().BeNull();
        }

        [Fact]
        public void Update_IdUnknown_ReturnsIsNotFound()
        {
            // Arrange
            TodoItem outRemoved;
            A.CallTo(() => _repository.RemoveItem(A<Guid>._, out outRemoved)).Returns(false);

            // Act
            var result = _todoService.Update(Guid.NewGuid(), _todoItem.Title, _todoItem.Description, _todoItem.IsCompleted);

            // Assert
            result.IsSuccess.Should().BeFalse();
            result.IsNotFoundError.Should().BeTrue();
            result.UpdatedItem.Should().BeNull();
        }

        [Fact]
        public void Update_RepositoryThrows_ReturnsNotSuccessful()
        {
            // Arrange
            TodoItem outResult;
            A.CallTo(() => _repository.RemoveItem(A<Guid>._, out outResult)).Throws<NullReferenceException>();

            // Act
            var result = _todoService.Update(_todoItem.Id, _todoItem.Title, _todoItem.Description, _todoItem.IsCompleted);

            // Assert
            result.IsSuccess.Should().BeFalse();
            result.IsNotFoundError.Should().BeFalse();
            result.UpdatedItem.Should().BeNull();
        }

        [Fact]
        public void GetById_IdFound_ReturnsItem()
        {
            // Arrange
            A.CallTo(() => _repository.GetItem(A<Guid>._)).Returns(_todoItem);

            // Act
            var result = _todoService.GetById(_todoItem.Id);

            // Assert
            result.IsSuccess.Should().BeTrue();
            result.Item.Should().BeEquivalentTo(_todoItem);
        }

        [Fact]
        public void GetById_IdUnknown_ReturnsNotSuccessful()
        {
            // Arrange
            A.CallTo(() => _repository.GetItem(A<Guid>._)).Returns(null);

            // Act
            var result = _todoService.GetById(_todoItem.Id);

            // Assert
            result.IsSuccess.Should().BeFalse();
            result.Item.Should().BeNull();
        }

        [Fact]
        public void GetById_EmptyId_ReturnsNotSuccessful()
        {
            // Arrange
            A.CallTo(() => _repository.GetItem(A<Guid>._)).Returns(null);

            // Act
            var result = _todoService.GetById(Guid.Empty);

            // Assert
            result.IsSuccess.Should().BeFalse();
            result.Item.Should().BeNull();
        }

        [Fact]
        public void GetById_WhenRepositoryThrows_ReturnsNotSuccessful()
        {
            // Arrange
            A.CallTo(() => _repository.GetItem(A<Guid>._)).Throws<NullReferenceException>();

            // Act
            var result = _todoService.GetById(_todoItem.Id);

            // Assert
            result.IsSuccess.Should().BeFalse();
            result.Item.Should().BeNull();
        }

        [Fact]
        public void GetAll_ItemsExist_ReturnsItems()
        {
            // Arrange
            A.CallTo(() => _repository.GetAllItems()).Returns(new[] {_todoItem});

            // Act
            var result = _todoService.GetAll();

            // Assert
            result.IsSuccess.Should().BeTrue();
            result.Items.Single().Should().BeEquivalentTo(_todoItem);
        }

        [Fact]
        public void GetAll_WhenRepositoryReturnsNull_ReturnsNotSuccessful()
        {
            // Arrange
            A.CallTo(() => _repository.GetAllItems()).Returns(null);

            // Act
            var result = _todoService.GetAll();

            // Assert
            result.IsSuccess.Should().BeFalse();
            result.Items.Should().BeEmpty();
        }

        [Fact]
        public void GetAll_WhenRepositoryThrows_ReturnsNotSuccessful()
        {
            // Arrange
            A.CallTo(() => _repository.GetAllItems()).Throws<NullReferenceException>();

            // Act
            var result = _todoService.GetAll();

            // Assert
            result.IsSuccess.Should().BeFalse();
            result.Items.Should().BeEmpty();
        }
    }
}