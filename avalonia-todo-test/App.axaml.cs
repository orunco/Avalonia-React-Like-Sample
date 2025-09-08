using System;
using System.Threading.Tasks;
using AlexandreHtrb.AvaloniaUITest;
using avalonia_todo_test.Visual;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using avalonia_todo;
using Avalonia.Controls;
using Avalonia.Threading;

namespace avalonia_todo_test;

public partial class App : Application{
    public override void Initialize(){
        AvaloniaXamlLoader.Load(this);
    }

    public override void OnFrameworkInitializationCompleted(){
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop){
            
            // 创建被测试工程的主窗口
            var mainWindow = new MainWindow();
            desktop.MainWindow = mainWindow;

            // 订阅主窗口的 Opened 事件，在窗口打开后再运行测试
            if (mainWindow is Window window){
                window.Opened += (sender, args) => {
                    // 同步事件，可以确保控件完成加载
                    Dispatcher.UIThread.RunJobs();
                    RunUITests(mainWindow);
                };
            }

            // 显示主窗口
            mainWindow.Show();
        }

        base.OnFrameworkInitializationCompleted();
    }

    private void RunUITests(MainWindow mainWindow){
        UITestsPrepareWindowViewModel vm = new(
            defaultActionWaitingTimeInMs: 2000,
            uiTests:[
                new Visual_MainWindow()
                // 添加更多测试类
            ],
            uiTestsFinishedCallback: Console.WriteLine);

        UITestsPrepareWindow uiTestsPrepareWindow = new(vm);
        uiTestsPrepareWindow.Show(mainWindow);
    }
}