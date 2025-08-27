using System.Collections.Generic;
using Avalonia.Controls;
using Avalonia.Controls.Templates;

namespace avalonia_todo.Components;

public partial class Xbutton : UserControl{
    public Xbutton(){
        Button button = new Button();
        button.Content = "Hello";
        button.Classes.Add("accent");  
        button.Classes.Add("large");
        Content = button;

        // Button button = new Button
        // {
        //     Content = "Hello",
        //     //Theme = (ControlTheme)Application.Current.FindResource("RedBorderButtonTheme")
        // };
        // Content = button;
    }
}