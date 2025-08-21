using System;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using avalonia_todo.Models;
using Avalonia;
using Avalonia.Automation;
using Avalonia.Controls.Templates;

namespace avalonia_todo.Components;

/*
import React from "react";
import styled from 'styled-components';
import TodoItem from "./TodoItem.tsx";

interface Todo {
    id: number;
    name: string;
    done: boolean;
}

interface TodoListProps {
    todos: Todo[];
    changeDoneFlagCallBack: (id: number, done: boolean) => void;
    handleDeleteCallBack: (id: number) => void;
}

const TodoList: React.FC<TodoListProps> = ({
    todos,
    changeDoneFlagCallBack,
    handleDeleteCallBack}) => {
    return (
        <TodoListContainer>
            {todos.map((todo) => (
                <TodoItem
                    key={todo.id}
                    {...todo}
                    changeDoneFlagCallBack={changeDoneFlagCallBack}
                    handleDeleteCallBack={handleDeleteCallBack}
                />
            ))}
        </TodoListContainer>
    );
};

export const TodoListContainer = styled.div`
    width: 600px;
    margin: 0 auto;
`;

export default TodoList;

 */

public class TodoList : UserControl{
    
    // 采用类似react props的数据传递和事件回调方式
    private Todos _todos{ get; }
    public Action<int, bool>? ChangeDoneFlagCallBack{ get; set; }

    public Action<int>? HandleDeleteCallBack{ get; set; }

    // 构造函数 - 必须传入 Todos
    public TodoList(Todos todos){
        _todos = todos;
        InitializeComponent();
        DataContext = _todos;
    }

    // 类似react的render()，但是只会运行一次
    // 禁止用axaml写界面，太多bug
    private void InitializeComponent(){
        ItemsControl _itemsControl = new ItemsControl{
            ItemsSource = _todos,
            ItemTemplate = new FuncDataTemplate<Todo>((todo, _) => {
                var item = new TodoItem(todo);

                // 转发事件回调
                item.ChangeDoneFlagCallBack += (s, e) => ChangeDoneFlagCallBack?.Invoke(e.Item1, e.Item2);
                item.HandleDeleteCallBack += (s, e) => HandleDeleteCallBack?.Invoke(e);

                return item;
            })
        };
		
        Content = _itemsControl;
    }
}