using System.Collections.Generic;
using Avalonia.Controls;
using Avalonia.Controls.Templates;

namespace avalonia_todo.Components;

public partial class XListBox : UserControl{
    public XListBox(){
        var listBox = new ListBox{
            ItemsSource = new List<string>{
                "第一个元素",
                "第二个元素"
            },
            ItemTemplate = new FuncDataTemplate<string>((item, _) => new TextBlock{
                Text = $"• {item}"
            })
        };

        Content = listBox;
    }
}