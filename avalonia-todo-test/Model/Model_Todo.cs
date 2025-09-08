using avalonia_todo.Models;
using NUnit.Framework;

namespace avalonia_todo_test.Model;

[TestFixture]
public class Model_Todo{
    [Test]
    public void Constructor_InitializesPropertiesCorrectly(){
        // Arrange
        var id = 1;
        var name = "Test Task";
        var done = false;

        // Act
        var todo = new Todo(id, name, done);

        // Assert
        Assert.That(todo.Id, Is.EqualTo(id));
        Assert.That(todo.Name, Is.EqualTo(name));
        Assert.That(todo.Done, Is.EqualTo(done));
        Assert.That(todo.Temperature, Is.EqualTo(20)); // Default value
    }

    [Test]
    public void Constructor_SetsDefaultDoneToFalse(){
        // Arrange
        var id = 2;
        var name = "Another Task";

        // Act
        var todo = new Todo(id, name); // done 参数未提供，应默认为 false

        // Assert
        Assert.That(todo.Done, Is.False);
    }

    [Test]
    public void Forecast_ReturnsCorrectValueBasedOnTemperature(){
        // Arrange
        var todo = new Todo(1, "Test");

        // Act & Assert
        todo.Temperature = 20;
        Assert.That(todo.Forecast, Is.EqualTo("凉爽"));

        todo.Temperature = 25;
        Assert.That(todo.Forecast, Is.EqualTo("凉爽"));

        todo.Temperature = 26;
        Assert.That(todo.Forecast, Is.EqualTo("炎热"));
    }

    [Test]
    public void Forecast_UpdatesWhenTemperatureChanges(){
        // Arrange
        var todo = new Todo(1, "Test");

        // Act
        todo.Temperature = 30;

        // Assert
        Assert.That(todo.Forecast, Is.EqualTo("炎热"));

        // Act
        todo.Temperature = 15;

        // Assert
        Assert.That(todo.Forecast, Is.EqualTo("凉爽"));
    }

    [Test]
    public void Properties_AreReactiveAndNotifyChanges(){
        // Arrange
        var todo = new Todo(1, "Original Name");
        var idChanged = false;
        var nameChanged = false;
        var doneChanged = false;
        var temperatureChanged = false;

        todo.PropertyChanged += (sender, e) => {
            switch (e.PropertyName){
                case nameof(Todo.Id):
                    idChanged = true;
                    break;
                case nameof(Todo.Name):
                    nameChanged = true;
                    break;
                case nameof(Todo.Done):
                    doneChanged = true;
                    break;
                case nameof(Todo.Temperature):
                    temperatureChanged = true;
                    break;
            }
        };

        // Act
        todo.Id = 2;
        todo.Name = "Updated Name";
        todo.Done = true;
        todo.Temperature = 25;

        // Assert
        Assert.That(idChanged, Is.True);
        Assert.That(nameChanged, Is.True);
        Assert.That(doneChanged, Is.True);
        Assert.That(temperatureChanged, Is.True);
    }
}