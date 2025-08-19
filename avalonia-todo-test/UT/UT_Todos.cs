using System.Collections.ObjectModel;
using System.Linq;
using NUnit.Framework;
using avalonia_todo.Models;

namespace avalonia_todo_test;

[TestFixture]
public class UT_Todos{
    [Test]
    public void Todos_InheritsFromObservableCollection(){
        // Arrange
        var todos = new Todos();

        // Act & Assert
        Assert.That(todos, Is.InstanceOf<ObservableCollection<Todo>>());
    }

    [Test]
    public void Todos_Can_Add_Todo(){
        // Arrange
        var todos = new Todos();
        var todo = new Todo(1, "Test Task");

        // Act
        todos.Add(todo);

        // Assert
        Assert.That(todos.Count, Is.EqualTo(1));
        Assert.That(todos[0], Is.SameAs(todo));
    }

    [Test]
    public void Todos_Can_Remove_Todo(){
        // Arrange
        var todos = new Todos();
        var todo = new Todo(1, "Test Task");
        todos.Add(todo);

        // Act
        todos.Remove(todo);

        // Assert
        Assert.That(todos.Count, Is.EqualTo(0));
    }

    [Test]
    public void Todos_Can_Remove_By_Index(){
        // Arrange
        var todos = new Todos();
        todos.Add(new Todo(1, "Task 1"));
        todos.Add(new Todo(2, "Task 2"));

        // Act
        todos.RemoveAt(0);

        // Assert
        Assert.That(todos.Count, Is.EqualTo(1));
        Assert.That(todos[0].Name, Is.EqualTo("Task 2"));
    }

    [Test]
    public void Todos_Can_Insert_Todo_At_Index(){
        // Arrange
        var todos = new Todos();
        todos.Add(new Todo(1, "Task 1"));
        var newTodo = new Todo(2, "Inserted Task");

        // Act
        todos.Insert(0, newTodo);

        // Assert
        Assert.That(todos.Count, Is.EqualTo(2));
        Assert.That(todos[0].Name, Is.EqualTo("Inserted Task"));
    }

    [Test]
    public void Todos_CollectionChanged_Event_Triggered_On_Add(){
        // Arrange
        var todos = new Todos();
        var wasCalled = false;
        todos.CollectionChanged += (sender, e) => wasCalled = true;

        // Act
        todos.Add(new Todo(1, "New Task"));

        // Assert
        Assert.That(wasCalled, Is.True);
    }

    [Test]
    public void Todos_CollectionChanged_Event_Triggered_On_Remove(){
        // Arrange
        var todos = new Todos();
        todos.Add(new Todo(1, "Task To Remove"));
        var wasCalled = false;
        todos.CollectionChanged += (sender, e) => wasCalled = true;

        // Act
        todos.Remove(todos.First());

        // Assert
        Assert.That(wasCalled, Is.True);
    }
}