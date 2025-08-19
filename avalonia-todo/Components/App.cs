using System;
using System.Linq;
using Avalonia.Controls;
using avalonia_todo.Models;
using Avalonia;
using MsBox.Avalonia;
using MsBox.Avalonia.Enums;

namespace avalonia_todo.Components;

public class AppComponent : UserControl{
    // ==================== Props ====================
    private readonly Todos _todos = new();
    private readonly Header _header;
    private readonly TodoList _todoList;
    private readonly Footer _footer;

    // ==================== 初始化界面 ====================
    public AppComponent(){
        // 初始化 Todos 数据
        _todos.Add(new Todo(1, "吃饭", true));
        _todos.Add(new Todo(2, "睡觉", true));
        _todos.Add(new Todo(3, "写代码", false));
        _todos.Add(new Todo(4, "逛街", true));

        // 初始化组件
        _header = new Header();
        _todoList = new TodoList(_todos);
        _footer = new Footer(_todos,AllDoneCallBack,RemoveAllDoneCallBack);
        
        // 主容器
        var mainPanel = new StackPanel{
            Margin = new Thickness(10)
        };

        mainPanel.Children.Add(_header);
        mainPanel.Children.Add(_todoList);
        mainPanel.Children.Add(_footer);

        Content = mainPanel;

        // ==================== 事件处理 ====================
        SetupEventHandlers();
    }

    private void SetupEventHandlers(){
        _header.OnEnter = HandleAddNewTodo;
        _todoList.ChangeDoneFlagCallBack = ChangeDoneFlagCallBack;
        _todoList.HandleDeleteCallBack = HandleDeleteCallBack; 
    }

    // ==================== 事件处理方法 ====================
    private async void HandleAddNewTodo(string newName){
        var isNameUnique = !_todos.Any(todo => todo.Name.Equals(newName, StringComparison.OrdinalIgnoreCase));

        if (isNameUnique){
            var newId = _todos.Count > 0 ? _todos.Max(t => t.Id) + 1 : 1;
            _todos.Insert(0, new Todo(newId, newName, false));
        }
        else{
            var box = MessageBoxManager.GetMessageBoxStandard(
                "提示",
                $"任务名称 \"{newName}\" 已存在",
                ButtonEnum.Ok);
            await box.ShowAsync();
        }
    }

    private void ChangeDoneFlagCallBack(int id, bool done){
        var todo = _todos.FirstOrDefault(t => t.Id == id);
        if (todo != null){
            todo.Done = done;
        }
        else{
            Console.WriteLine($"警告: 未找到 ID 为 {id} 的任务");
        }
    }

    private void HandleDeleteCallBack(int id){
        var todo = _todos.FirstOrDefault(t => t.Id == id);
        if (todo != null){
            _todos.Remove(todo);
        }
        else{
            Console.WriteLine($"警告: 无法删除，未找到 ID 为 {id} 的任务");
        }
    }

    private void AllDoneCallBack(bool flag){
        foreach (var todo in _todos){
            todo.Done = flag;
        }
    }

    private void RemoveAllDoneCallBack(){
        // 从后往前删，避免索引问题
        for (int i = _todos.Count - 1; i >= 0; i--){
            if (_todos[i].Done){
                _todos.RemoveAt(i);
            }
        }
    }
}