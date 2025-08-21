using Avalonia.Controls;
using avalonia_todo.Components;

namespace avalonia_todo;

// 等价于React的index.html+main.tsx
public class MainWindow : Window{
    
    public MainWindow(){
        Title = "Avalonia Todo App";
        Width = 600;
        Height = 500;
 
        Content = new MainLayout();
    }
}