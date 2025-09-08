using System.Linq;
using avalonia_todo.Models;
using NUnit.Framework;

namespace avalonia_todo_test.Model;

[TestFixture]
public class Model_Todos{
    [Test]
    public void Constructor_CreatesEmptyCollection(){
        // Act
        var todos = new Todos();

        // Assert
        Assert.That(todos, Is.Not.Null);
        Assert.That(todos.Count, Is.EqualTo(0));
    }

    [Test]
    public void Add_AddsTodoToCollection(){
        // Arrange
        var todos = new Todos();
        var todo = new Todo(1, "Test Task");

        // Act
        todos.Add(todo);

        // Assert
        Assert.That(todos.Count, Is.EqualTo(1));
        Assert.That(todos.First(), Is.EqualTo(todo));
    }

    [Test]
    public void Remove_RemovesTodoFromCollection(){
        // Arrange
        var todos = new Todos();
        var todo = new Todo(1, "Test Task");
        todos.Add(todo);

        // Act
        var result = todos.Remove(todo);

        // Assert
        Assert.That(result, Is.True);
        Assert.That(todos.Count, Is.EqualTo(0));
    }

    [Test]
    public void Clear_RemovesAllTodos(){
        // Arrange
        var todos = new Todos();
        todos.Add(new Todo(1, "Task 1"));
        todos.Add(new Todo(2, "Task 2"));
        todos.Add(new Todo(3, "Task 3"));

        // Act
        todos.Clear();

        // Assert
        Assert.That(todos.Count, Is.EqualTo(0));
    }

    [Test]
    public void Count_ReturnsCorrectNumber(){
        // Arrange
        var todos = new Todos();
        Assert.That(todos.Count, Is.EqualTo(0));

        // Act
        todos.Add(new Todo(1, "Task 1"));
        todos.Add(new Todo(2, "Task 2"));

        // Assert
        Assert.That(todos.Count, Is.EqualTo(2));

        // Act
        todos.RemoveAt(0);

        // Assert
        Assert.That(todos.Count, Is.EqualTo(1));
    }

    [Test]
    public void Indexer_ReturnsCorrectTodo(){
        // Arrange
        var todos = new Todos();
        var todo1 = new Todo(1, "Task 1");
        var todo2 = new Todo(2, "Task 2");
        todos.Add(todo1);
        todos.Add(todo2);

        // Act & Assert
        Assert.That(todos[0], Is.EqualTo(todo1));
        Assert.That(todos[1], Is.EqualTo(todo2));
    }

    [Test]
    public void Insert_InsertsTodoAtCorrectPosition(){
        // Arrange
        var todos = new Todos();
        var todo1 = new Todo(1, "Task 1");
        var todo2 = new Todo(2, "Task 2");
        var todo3 = new Todo(3, "Task 3");
        todos.Add(todo1);
        todos.Add(todo2);

        // Act
        todos.Insert(1, todo3);

        // Assert
        Assert.That(todos.Count, Is.EqualTo(3));
        Assert.That(todos[0], Is.EqualTo(todo1));
        Assert.That(todos[1], Is.EqualTo(todo3));
        Assert.That(todos[2], Is.EqualTo(todo2));
    }

    [Test]
    public void RemoveAt_RemovesTodoAtCorrectIndex(){
        // Arrange
        var todos = new Todos();
        var todo1 = new Todo(1, "Task 1");
        var todo2 = new Todo(2, "Task 2");
        var todo3 = new Todo(3, "Task 3");
        todos.Add(todo1);
        todos.Add(todo2);
        todos.Add(todo3);

        // Act
        todos.RemoveAt(1);

        // Assert
        Assert.That(todos.Count, Is.EqualTo(2));
        Assert.That(todos[0], Is.EqualTo(todo1));
        Assert.That(todos[1], Is.EqualTo(todo3));
    }
}