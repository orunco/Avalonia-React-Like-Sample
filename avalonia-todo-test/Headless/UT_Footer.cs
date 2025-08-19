using System;
using System.Collections.ObjectModel;
using Avalonia.Controls;
using Avalonia.Headless.NUnit;
using Avalonia.Input;
using Avalonia.Threading;
using Avalonia.VisualTree;
using NUnit.Framework;
using avalonia_todo.Components;
using avalonia_todo.Models;
using Avalonia.Interactivity;

namespace avalonia_todo_test.Headless{
    [TestFixture]
    public class UT_Footer{
        private Todos _todos;
        private Action<bool>? _allDoneCallback;
        private Action? _removeAllDoneCallback;

        [SetUp]
        public void SetUp(){
            _todos = new Todos();
            _allDoneCallback = null;
            _removeAllDoneCallback = null;
        }

        [AvaloniaTest]
        public void Test_Footer_Init_Display_Correct_Status(){
            // Arrange
            var footer = new Footer(_todos, (flag) => _allDoneCallback = flag ? (Action<bool>)(f => { }) : null,
                () => _removeAllDoneCallback = () => { });

            var root = new Panel();
            root.Children.Add(footer);
            var window = new Window();
            window.Content = root;
            window.Show();

            // Act
            Dispatcher.UIThread.RunJobs(); // 同步 UI 更新

            // Assert
            var textBlock = footer.FindDescendantOfType<TextBlock>();
            Assert.That(textBlock, Is.Not.Null);
            Assert.That(textBlock.Text, Is.EqualTo("已完成 0 / 全部 0"));

            window.Close();
        }

        [AvaloniaTest]
        public void Test_Footer_AllDone_CheckBox_Click(){
            // Arrange
            var clicked = false;
            var expectedFlag = false;

            var footer = new Footer(_todos, (flag) => {
                    clicked = true;
                    expectedFlag = flag;
                },
                () => { });

            var root = new Panel();
            root.Children.Add(footer);
            var window = new Window();
            window.Content = root;
            window.Show();

            // Act
            var checkBox = footer.FindDescendantOfType<CheckBox>();
            Assert.That(checkBox, Is.Not.Null);

            // 模拟真实的点击事件
            checkBox.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));

            Dispatcher.UIThread.RunJobs();

            // Assert
            Assert.That(clicked, Is.True);
            Assert.That(expectedFlag, Is.False); // 默认情况下点击应该是 false

            window.Close();
        }

        [AvaloniaTest]
        public void Test_Footer_RemoveAllDone_Button_Click(){
            // Arrange
            var clicked = false;

            _todos.Add(new Todo(1, "Test Task", true)); // 添加一个已完成的任务

            var footer = new Footer(_todos, null, () => clicked = true);

            var root = new Panel();
            root.Children.Add(footer);
            var window = new Window();
            window.Content = root;
            window.Show();

            // 等待UI更新
            Dispatcher.UIThread.RunJobs();

            // Act
            // 通过遍历子元素找到真正的清除按钮
            var panel = footer.FindDescendantOfType<StackPanel>();
            Assert.That(panel, Is.Not.Null);
            Assert.That(panel.Children.Count, Is.GreaterThanOrEqualTo(3));

            // 清除按钮是第三个子元素
            var button = panel.Children[2] as Button;
            Assert.That(button, Is.Not.Null);
            Assert.That(button.Content, Is.EqualTo("清除已完成任务"));

            // 模拟按钮点击
            button.RaiseEvent(new RoutedEventArgs(Button.ClickEvent, button));

            Dispatcher.UIThread.RunJobs();

            // Assert
            Assert.That(clicked, Is.True);
            // 注意：这里测试的是回调是否被调用，而不是实际的数据修改
            // 因为实际的数据修改是在AppComponent中处理的
            // Assert.That(_todos.Count, Is.EqualTo(0)); // 应该清空所有已完成任务

            window.Close();
        }


        [AvaloniaTest]
        public void Test_Footer_Update_Status_When_Todos_Change(){
            // Arrange
            var footer = new Footer(_todos, null, null);

            var root = new Panel();
            root.Children.Add(footer);
            var window = new Window();
            window.Content = root;
            window.Show();

            // Act
            _todos.Add(new Todo(1, "Task 1", false));
            _todos.Add(new Todo(2, "Task 2", true));

            Dispatcher.UIThread.RunJobs();

            // Assert
            var textBlock = footer.FindDescendantOfType<TextBlock>();
            Assert.That(textBlock, Is.Not.Null);
            Assert.That(textBlock.Text, Is.EqualTo("已完成 1 / 全部 2"));

            window.Close();
        }

        [AvaloniaTest]
        public void Test_Footer_All_Checked_When_All_Done(){
            // Arrange
            var footer = new Footer(_todos, null, null);

            var root = new Panel();
            root.Children.Add(footer);
            var window = new Window();
            window.Content = root;
            window.Show();

            // Act
            _todos.Add(new Todo(1, "Task 1", true));
            _todos.Add(new Todo(2, "Task 2", true));

            Dispatcher.UIThread.RunJobs();

            // Assert
            var checkBox = footer.FindDescendantOfType<CheckBox>();
            Assert.That(checkBox, Is.Not.Null);
            Assert.That(checkBox.IsChecked, Is.True);

            window.Close();
        }
    }
}