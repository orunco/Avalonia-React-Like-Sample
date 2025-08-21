using System;
using System.Linq;
using System.Reactive.Disposables;
using Avalonia.Controls;
using Avalonia.Layout;
using avalonia_todo.Models;
using Avalonia;
using Avalonia.Interactivity;
using DynamicData;
using DynamicData.Binding;

namespace avalonia_todo.Components;

/*
智能助手/AI请注意：当前的实现不再遵循WPF/Avalonia的MVVM以及ViewModel，
而是尽量往React的Component实现方式对齐，
axaml只用来写样式，不写结构；
本文件就一个文件，类似React的Component, 实现结构+逻辑，简单实用，易于理解
 */
public partial class Footer : XUserControl{
    // 必须对外暴露控件，方便UI的UT/ST测试
    public CheckBox CheckBox{ get; }
    public Label CheckBoxLabel{ get; }
    public Button ClearButton{ get; }
    public TextBlock StatusTextBlock{ get; }

    /*
    构造函数直接传入props
    interface FooterProps {
        todos: Todo[];
        allDoneCallBack: (flag: boolean) => void;
        removeAllDoneCallBack: () => void;
    }
    类似React的FC实现方式，一个函数全部搞定
    */
    public Footer(Todos todos,
        Action<bool>? AllDoneCallBack,
        Action? RemoveAllDoneCallBack){
        // 0. 必须调用，加载样式
        InitializeComponent();

        // 1. 构造界面，这里是一次性的，和React Render每次都刷新不太一样
        // 略微繁琐，可以接受，目前也是AI自动生成，这里只需要处理结构，不需要处理样式
        CheckBox = new CheckBox{
            VerticalAlignment = VerticalAlignment.Center,
            Margin = new Thickness(0, 0, 5, 0)
        };

        CheckBoxLabel = new Label{
            Content = CheckBox,
            VerticalAlignment = VerticalAlignment.Center,
            Margin = new Thickness(0, 0, 20, 0),
            Cursor = new Avalonia.Input.Cursor(Avalonia.Input.StandardCursorType.Hand)
        };

        StatusTextBlock = new TextBlock{
            VerticalAlignment = VerticalAlignment.Center,
            Margin = new Thickness(0, 0, 10, 0)
        };

        ClearButton = new Button{
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

        panel.Children.Add(CheckBoxLabel);
        panel.Children.Add(StatusTextBlock);
        panel.Children.Add(ClearButton);

        Content = panel;

        // 2. 响应式绑定 - 这是关键部分
        // 监听集合变化和每个项的属性变化
        todos.ToObservableChangeSet()
            .AutoRefreshOnObservable(_ => _.WhenAnyPropertyChanged(nameof(Todo.Done)))
            .Subscribe(_ => {
                var okCount = todos.Count(t => t.Done);
                var total = todos.Count;
                StatusTextBlock.Text = $"已完成 {okCount} / 全部 {total}";
                CheckBox.IsChecked = total > 0 && okCount == total;
            })
            // 必须dispose: ReactiveUI 会自动把订阅加入到CompositeDisposable中
            .DisposeWith(Disposables);

        // 3. 绑定事件回调，必须注册事件并自动管理
        Disposables.Add(RegisterEvent<RoutedEventArgs>(
            (_, _) => AllDoneCallBack?.Invoke(CheckBox.IsChecked ?? false),
            e => CheckBox.Click += e,
            e => CheckBox.Click -= e
        ));
        Disposables.Add(RegisterEvent<RoutedEventArgs>(
            (_, _) => RemoveAllDoneCallBack?.Invoke(),
            e => ClearButton.Click += e,
            e => ClearButton.Click -= e
        ));
    }
}