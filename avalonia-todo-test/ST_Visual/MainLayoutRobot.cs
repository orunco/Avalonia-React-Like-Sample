using System;
using AlexandreHtrb.AvaloniaUITest;
using avalonia_todo.Components;
using Avalonia.Controls;

namespace avalonia_todo.VisualTests;

public sealed class MainLayoutRobot(Control rootView) : BaseRobot(rootView)
{
    // 通过 rootView 获取 MainLayout 实例
    private MainLayout? GetMainLayout()
    {
        if (rootView is MainLayout mainLayout)
            return mainLayout;
        
        // 如果 rootView 是 Window，尝试从 Content 获取
        if (rootView is Window window && window.Content is MainLayout layout)
            return layout;
            
        return null;
    }

    // Header 输入框
    internal TextBox InputBox => GetMainLayout()?.Header.InputBox
                                 ?? throw new InvalidOperationException("无法获取 InputBox");

    // TodoList 列表容器
    internal ItemsControl TodoItemsList => GetMainLayout()?.TodoList.Content as ItemsControl
                                           ?? throw new InvalidOperationException("无法获取 TodoItemsList");

    // Footer 状态文本
    internal TextBlock StatusTextBlock => GetMainLayout()?.Footer.StatusTextBlock
                                          ?? throw new InvalidOperationException("无法获取 StatusTextBlock");

    // Footer 全选按钮
    internal CheckBox AllDoneCheckBox => GetMainLayout()?.Footer.CheckBox
                                         ?? throw new InvalidOperationException("无法获取 AllDoneCheckBox");

    // Footer 清除已完成按钮
    internal Button ClearCompletedButton => GetMainLayout()?.Footer.ClearButton
                                            ?? throw new InvalidOperationException("无法获取 ClearCompletedButton");
}