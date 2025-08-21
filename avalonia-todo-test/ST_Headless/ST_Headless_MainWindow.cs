using System.Linq;
using Avalonia.Controls;
using Avalonia.Headless.NUnit;
using Avalonia.Input;
using Avalonia.Threading;
using avalonia_todo.Components;
using Avalonia.VisualTree;
using NUnit.Framework;

namespace avalonia_todo;

public class ST_Headless_MainWindow{
    [AvaloniaTest]
    public void Test_MainWindow_Loads_And_Contains_AppComponent(){
        // Arrange
        var window = new MainWindow();

        // Act
        window.Show();
        Dispatcher.UIThread.RunJobs();

        // Assert
        Assert.That(window.Title, Is.EqualTo("Avalonia Todo App"));
        Assert.That(window.Content, Is.TypeOf<MainLayout>());
    }

    [AvaloniaTest]
    public void Test_Add_New_Todo_Item(){
        // Arrange
        var window = new MainWindow();
        window.Show();
        Dispatcher.UIThread.RunJobs();

        var appComponent = (MainLayout)window.Content;

        // 通过反射获取 AppComponent 中的 _todos 字段来直接验证数据
        var todosField = typeof(MainLayout).GetField("_todos",
            System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        Assert.That(todosField, Is.Not.Null);

        var todos = todosField.GetValue(appComponent) as avalonia_todo.Models.Todos;
        Assert.That(todos, Is.Not.Null);

        int initialCount = todos.Count;

        var header = appComponent.GetVisualDescendants().OfType<Header>().FirstOrDefault();
        Assert.That(header, Is.Not.Null);

        var textBox = header.GetVisualDescendants().OfType<TextBox>().FirstOrDefault();
        Assert.That(textBox, Is.Not.Null);

        // Act
        textBox.Text = "测试任务";
        textBox.RaiseEvent(new KeyEventArgs{
            RoutedEvent = InputElement.KeyUpEvent,
            Key = Key.Enter
        });

        // 等待一段时间确保数据更新
        Dispatcher.UIThread.RunJobs();

        // Assert - 直接验证数据模型
        Assert.That(todos.Count, Is.EqualTo(initialCount + 1));
        Assert.That(todos[0].Name, Is.EqualTo("测试任务"));
        Assert.That(todos[0].Done, Is.False);
    }
}