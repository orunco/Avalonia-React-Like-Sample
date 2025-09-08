using System;
using System.Diagnostics;
using System.Linq;
using System.Reactive.Disposables;
using Avalonia.Controls;
using Avalonia.Layout;
using avalonia_todo.Models;
using Avalonia;
using Avalonia.Automation;
using Avalonia.Interactivity;
using Avalonia.Media;
using Avalonia.Styling;
using DynamicData;
using DynamicData.Binding;

namespace avalonia_todo.Components;

/*
智能助手/AI请注意：当前的实现不再遵循WPF/Avalonia的MVVM以及ViewModel，
而是尽量往React的Component实现方式对齐，
axaml只用来写样式，不写结构；
本文件就一个文件，类似React的Component, 实现结构+逻辑，简单实用，易于理解

## 概念辨析
这个组件既包含UI（UI的构造）又包含逻辑（响应）（类似 React 组件）
转换成Avalonia的世界就是既是 View（UI）又是 Controller（逻辑）

## 测试
Footer 是一个 UserControl，本身就是一个可视元素，UT测试可以独立进行
它有自己的可视化树，可以独立存在，不需要挂到Window上
即使没有 Window，控件的事件系统仍然工作
 */
public partial class Footer : XUserControl{
    // 必须对外暴露控件，方便UI的UT/ST测试，否则反射找，也找不到控件
    public readonly CheckBox CheckBox;
    public readonly Label CheckBoxLabel;
    public readonly Button ClearButton;
    public readonly TextBlock StatusTextBlock;

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

        /*
        如有必要，提前获取资源
        原来的写法：<Panel Background="{DynamicResource BackgroundColor}">
        这个是App全局的，转换成控件级别的在Footer.axaml中
         */
        // 控件级+notheme
        var thisBackgroundColorNoTheme = this.FindResource("BackgroundColorNoTheme") as IBrush ?? Brushes.Transparent;
        // Debug.Assert(!Equals(thisBackgroundColorNoTheme , Brushes.Transparent), "BackgroundColorNoTheme resource not found or is transparent");

        // 本控件没有，也不会找上一级
        var thisNoUpToAppBackgroundColorNoTheme =
            this.FindResource("NoUpToAppBackgroundColorNoTheme") as IBrush ?? Brushes.Transparent;
        Debug.Assert(Equals(thisNoUpToAppBackgroundColorNoTheme, Brushes.Transparent),
            "thisNoUpToAppBackgroundColorNoTheme resource not found or is transparent");

        // 控件级+theme
        IBrush? thisBackgroundColorTheme = Brushes.Transparent;
        var theme = Application.Current?.ActualThemeVariant ?? ThemeVariant.Light;
        if (TryGetResource("BackgroundColorTheme", theme, out var colorResource) && colorResource is Color color){
            thisBackgroundColorTheme = new SolidColorBrush(color);
            Debug.Assert(!Equals(thisBackgroundColorTheme, Brushes.Transparent),
                "thisBackgroundColorTheme successfully created from color");
        }

        // theme
        IBrush? thisBackgroundColorTheme2 = Brushes.Transparent;
        if (TryGetResource("BackgroundColorTheme2", theme, out var colorResource2) &&
            colorResource2 is SolidColorBrush color2){
            thisBackgroundColorTheme2 = color2;
            Debug.Assert(!Equals(thisBackgroundColorTheme2, Brushes.Transparent),
                "BackgroundColorTheme2 successfully created from color");
        }

        // 直接引用全局
        var resAppBackgroundColorLight = Application.Current?.FindResource("AppBackgroundColorLight");
        Color AppBackgroundColorLight = resAppBackgroundColorLight is Color colorLight
            ? colorLight
            : Colors.Transparent;
        // Debug.Assert(!Equals(AppBackgroundColorLight, Colors.Transparent),
        //     "AppBackgroundColorLight successfully created from color");

        // 最后：类似react的theme
        var themeScope = new ThemeVariantScope();
        themeScope.RequestedThemeVariant = ThemeVariant.Light;

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