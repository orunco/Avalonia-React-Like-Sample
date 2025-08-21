using System;
using System.Collections.Generic;
using System.Linq;
using Avalonia.Controls;
using Avalonia.Headless.NUnit;
using Avalonia.Input;
using Avalonia.Threading;
using NUnit.Framework;
using avalonia_todo.Components;
using avalonia_todo.Models;
using Avalonia.Interactivity;

namespace avalonia_todo_test.UT.Headless;

public class UT_Headless_Footer
{
    private Todos _todos = null!;
    private bool _allDoneCallbackInvoked;
    private bool _removeAllDoneCallbackInvoked;
    private Footer _footer = null!;

    [SetUp]
    public void Setup()
    {
        _todos = new Todos();
        _allDoneCallbackInvoked = false;
        _removeAllDoneCallbackInvoked = false;

        _footer = new Footer(
            _todos,
            flag =>
            {
                _allDoneCallbackInvoked = true;
            },
            () =>
            {
                _removeAllDoneCallbackInvoked = true;
            });
    }

    [AvaloniaTest]
    public void Footer_Controls_Should_Be_Initialized()
    {
        // Assert
        Assert.That(_footer.CheckBox, Is.Not.Null);
        Assert.That(_footer.CheckBoxLabel, Is.Not.Null);
        Assert.That(_footer.ClearButton, Is.Not.Null);
        Assert.That(_footer.StatusTextBlock, Is.Not.Null);
    }

    [AvaloniaTest]
    public void StatusTextBlock_Should_Update_When_Todos_Change()
    {
        // Arrange
        _todos.Add(new Todo(1, "Test 1", false));
        _todos.Add(new Todo(2, "Test 2", true));

        // Act
        Dispatcher.UIThread.RunJobs();

        // Assert
        Assert.That(_footer.StatusTextBlock.Text, Is.EqualTo("已完成 1 / 全部 2"));
    }

    [AvaloniaTest]
    public void CheckBox_Should_Be_Checked_When_All_Todos_Are_Done()
    {
        // Arrange
        _todos.Add(new Todo(1, "Test 1", true));
        _todos.Add(new Todo(2, "Test 2", true));

        // Act
        Dispatcher.UIThread.RunJobs();

        // Assert
        Assert.That(_footer.CheckBox.IsChecked, Is.True);
    }

    [AvaloniaTest]
    public void CheckBox_Should_Be_Unchecked_If_Not_All_Todos_Are_Done()
    {
        // Arrange
        _todos.Add(new Todo(1, "Test 1", true));
        _todos.Add(new Todo(2, "Test 2", false));

        // Act
        Dispatcher.UIThread.RunJobs();

        // Assert
        Assert.That(_footer.CheckBox.IsChecked, Is.False);
    }

    [AvaloniaTest]
    public void Clicking_CheckBox_Should_Invoke_AllDone_Callback()
    {
        // Arrange
        _todos.Add(new Todo(1, "Test 1", true));

        // Act
        _footer.CheckBox.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));

        // Assert
        Assert.That(_allDoneCallbackInvoked, Is.True);
    }

    [AvaloniaTest]
    public void Clicking_ClearButton_Should_Invoke_RemoveAllDone_Callback()
    {
        // Act
        _footer.ClearButton.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));

        // Assert
        Assert.That(_removeAllDoneCallbackInvoked, Is.True);
    }
}
