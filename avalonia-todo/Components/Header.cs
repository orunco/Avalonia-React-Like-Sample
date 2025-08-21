using System;
using Avalonia;
using Avalonia.Automation;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Layout;

namespace avalonia_todo.Components;

public class Header : UserControl{
    
    // 必须对外暴露控件，方便UI的UT/ST测试，否则反射找，也找不到控件
    public readonly TextBox InputBox;
    
    // 类似 React 的 props 回调
    public Action<string>? OnEnter{ get; set; }

    public Header(){
          InputBox = new TextBox{
            Width = 560,
            Height = 28,
            FontSize = 14,
            Padding = new Thickness(4, 7),
            Watermark = "请输入你的任务名称，按回车键确认"
        };
		
        InputBox.KeyUp += OnInputKeyUp;

        var container = new StackPanel{
            Orientation = Orientation.Horizontal,
            HorizontalAlignment = HorizontalAlignment.Center,
            Children ={ InputBox }
        };

        Content = container;
    }

    private void OnInputKeyUp(object? sender, KeyEventArgs e){
        if (e.Key == Key.Enter && sender is TextBox textBox){
            var value = textBox.Text?.Trim();
            if (!string.IsNullOrEmpty(value)){
                OnEnter?.Invoke(value);
            }

            textBox.Text = string.Empty;
        }
    }
}