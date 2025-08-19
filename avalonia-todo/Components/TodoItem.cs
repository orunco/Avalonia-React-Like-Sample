using System;
using System.Diagnostics;
using Avalonia.Controls;
using avalonia_todo.Models;
using Avalonia;
using Avalonia.Controls.Primitives;
using Avalonia.Data;
using Avalonia.Interactivity;
using Avalonia.Layout;
using MsBox.Avalonia;
using MsBox.Avalonia.Enums;

// ReSharper disable AsyncVoidMethod

namespace avalonia_todo.Components;

/*
React ts代码原型：
import React from "react";
import styled from 'styled-components';

interface TodoItemProps {
    id: number;
    name: string;
    done: boolean;
    changeDoneFlagCallBack: (id: number, done: boolean) => void;
    handleDeleteCallBack: (id: number) => void;
}

const TodoItem: React.FC<TodoItemProps> = ({id, name, done, changeDoneFlagCallBack, handleDeleteCallBack}) => {
    const handleCheck = (e: React.ChangeEvent<HTMLInputElement>) => {
        changeDoneFlagCallBack(id, e.target.checked);
    };

    const handleDelete = () => {
        if (confirm("确定删除吗？")) {
            handleDeleteCallBack(id);
        }
    };

    return (
        <TodoItemWrapper>
            <label>
                <input
                    type="checkbox"
                    id={id.toString()}
                    checked={done}
                    onChange={handleCheck}
                />
                <span>{name}</span>
            </label>
            <DeleteButton onClick={handleDelete}>
                删除
            </DeleteButton>
        </TodoItemWrapper>
    );
};

export const TodoItemWrapper = styled.li`
    list-style: none;
    height: 36px;
    ......
`;

export const DeleteButton = styled.button`
    color: #fff;
    background-color: #dc3545;
    ......
`;

export default TodoItem;
 */

public class TodoItem : UserControl{
    // 采用类似react props的数据传递和事件回调方式
    private readonly Todo _todo;
    public event EventHandler<(int id, bool done)>? ChangeDoneFlagCallBack;
    public event EventHandler<int>? HandleDeleteCallBack;

    public TodoItem(Todo todo){
        _todo = todo;
        InitializeComponent();
        DataContext = _todo;
    }

    // 类似react的render()，但是只会运行一次
    // 禁止用axaml写界面，太多bug
    private void InitializeComponent(){
        Console.WriteLine($"InitializeComponent called for: {_todo.Name}");

        var panel = new StackPanel{
            Orientation = Orientation.Horizontal,
            Margin = new Thickness(5),
            VerticalAlignment = VerticalAlignment.Center
        };

        var _checkBox = new CheckBox{
            VerticalAlignment = VerticalAlignment.Center
        };
        _checkBox.Bind(
            ToggleButton.IsCheckedProperty,
            new Binding(nameof(Todo.Done))); // 绑定 Done 属性
        _checkBox.Click += HandleCheck;

        var _textBlock = new TextBlock{
            Margin = new Thickness(5, 0),
            VerticalAlignment = VerticalAlignment.Center
        };
        _textBlock.Bind(
            TextBlock.TextProperty,
            new Binding(nameof(Todo.Name))); // 绑定 Name 属性

        var deleteButton = new Button{
            Content = "删除",
            Margin = new Thickness(10, 0, 0, 0)
        };
        deleteButton.Click += HandleDelete;

        panel.Children.Add(_checkBox);
        panel.Children.Add(_textBlock);
        panel.Children.Add(deleteButton);

        Content = panel;
    }

    // 事件处理
    private void HandleCheck(object? sender, RoutedEventArgs e){
        if (sender is CheckBox checkBox){
            _todo.Done = checkBox.IsChecked ?? false;
            ChangeDoneFlagCallBack?.Invoke(this, (_todo.Id, _todo.Done));
        }
    }

    private async void HandleDelete(object? sender, RoutedEventArgs e){
        var box = MessageBoxManager.GetMessageBoxStandard(
            "Caption",
            "确定删除吗？",
            ButtonEnum.YesNo);
        var result = await box.ShowAsync();
        if (result == ButtonResult.Yes){
            HandleDeleteCallBack?.Invoke(this, _todo.Id);
        }
    }
}