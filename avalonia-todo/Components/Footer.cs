using System;
using System.ComponentModel;
using System.Linq;
using Avalonia.Controls;
using Avalonia.Layout;
using avalonia_todo.Models;
using Avalonia;

namespace avalonia_todo.Components;

// 实现React的FC实现方式
public class Footer : UserControl{
    /*
    构造函数直接传入props
    interface FooterProps {
        todos: Todo[];
        allDoneCallBack: (flag: boolean) => void;
        removeAllDoneCallBack: () => void;
    }
     */
    public Footer(Todos todos,
        Action<bool>? AllDoneCallBack,
        Action? RemoveAllDoneCallBack){
        Todos _todos = todos;

        // 1. 构造界面+样式，这里是一次性的，和render每次都刷新不太一样
        // 略微繁琐，可以接受
        var checkBox = new CheckBox{
            VerticalAlignment = VerticalAlignment.Center,
            Margin = new Thickness(0, 0, 5, 0)
        };

        var checkBoxLabel = new Label{
            Content = checkBox,
            VerticalAlignment = VerticalAlignment.Center,
            Margin = new Thickness(0, 0, 20, 0),
            Cursor = new Avalonia.Input.Cursor(Avalonia.Input.StandardCursorType.Hand)
        };

        var statusTextBlock = new TextBlock{
            VerticalAlignment = VerticalAlignment.Center,
            Margin = new Thickness(0, 0, 10, 0)
        };

        var clearButton = new Button{
            Content = "清除已完成任务",
            VerticalAlignment = VerticalAlignment.Center,
            HorizontalAlignment = HorizontalAlignment.Right,
            Margin = new Thickness(10, 5, 0, 0)
        };

        var panel = new StackPanel{
            Orientation = Orientation.Horizontal,
            Height = 40,
            Margin = new Thickness(6, 5, 6, 0)
        };

        panel.Children.Add(checkBoxLabel);
        panel.Children.Add(statusTextBlock);
        panel.Children.Add(clearButton);

        Content = panel;

        // 2. 数据绑定和事件处理
        void UpdateUI(){
            var okCount = _todos.Count(t => t.Done);
            var total = _todos.Count;
            statusTextBlock.Text = $"已完成 {okCount} / 全部 {total}";
            checkBox.IsChecked = total > 0 && okCount == total;
        }

        // 初始更新
        UpdateUI();

        // 监听集合变化（增删项）
        _todos.CollectionChanged += (s, e) => {
            // 为新增的项订阅属性变化事件
            if (e.NewItems != null){
                foreach (Todo todo in e.NewItems){
                    todo.PropertyChanged += OnTodoPropertyChanged;
                }
            }

            // 为被移除的项取消订阅属性变化事件
            if (e.OldItems != null){
                foreach (Todo todo in e.OldItems){
                    todo.PropertyChanged -= OnTodoPropertyChanged;
                }
            }

            UpdateUI();
        };

        // 为已存在的项订阅属性变化事件
        foreach (var todo in _todos){
            todo.PropertyChanged += OnTodoPropertyChanged;
        }

        // 当某个Todo的属性发生变化时的处理方法
        void OnTodoPropertyChanged(object? sender, PropertyChangedEventArgs e){
            // 只关心Done属性的变化
            if (e.PropertyName == nameof(Todo.Done)){
                UpdateUI();
            }
        }

        // 3. 事件绑定
        checkBox.Click += (s, e) =>
            AllDoneCallBack?.Invoke(checkBox.IsChecked ?? false);
        clearButton.Click += (s, e) =>
            RemoveAllDoneCallBack?.Invoke();
    }
}