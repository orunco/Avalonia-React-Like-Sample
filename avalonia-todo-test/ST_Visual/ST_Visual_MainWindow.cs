using System;
using System.Threading.Tasks;
using AlexandreHtrb.AvaloniaUITest;
using avalonia_todo_test.Extensions;
using avalonia_todo;
using avalonia_todo.VisualTests;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Controls.ApplicationLifetimes;

namespace MyApp.UITesting.Tests;

public sealed class ST_Visual_MainWindow : UITest{
    private MainLayoutRobot mainLayoutRobot{ get; set; }

    private MainWindow GetMainWindow(){
        if (Application.Current?.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop){
            return desktop.MainWindow as MainWindow;
        }

        return null;
    }

    public override async Task RunAsync(){
        var mainWindow = GetMainWindow();
        if (mainWindow == null){
            throw new InvalidOperationException("无法获取主窗口实例");
        }

        // 每次运行测试时都重新创建
        mainLayoutRobot = new MainLayoutRobot(mainWindow);

        AppendToLog("开始测试主窗口 UI");

        // 验证控件存在且可见
        AppendToLog("验证控件是否存在");
        mainLayoutRobot.InputBox.AssertIsVisible();
        mainLayoutRobot.TodoItemsList.AssertIsVisible();
        mainLayoutRobot.StatusTextBlock.AssertIsVisible();
        mainLayoutRobot.AllDoneCheckBox.AssertIsVisible();
        mainLayoutRobot.ClearCompletedButton.AssertIsVisible();

        // 验证初始状态
        AppendToLog("验证初始状态");
        mainLayoutRobot.StatusTextBlock.AssertHasText("已完成 3 / 全部 4");
        // Robot.AllDoneCheckBox.AssertIsChecked(); // 注意：根据你的逻辑，应该是全部完成才 checked

        // 模拟输入新任务
        AppendToLog("输入新任务：买菜");
        await mainLayoutRobot.InputBox.TypeText("买菜");
        await mainLayoutRobot.InputBox.PressKey(Key.Enter);

        // 验证新增成功
        AppendToLog("验证新增任务后状态");
        mainLayoutRobot.StatusTextBlock.AssertHasText("已完成 3 / 全部 5");

        // 点击"清除已完成"
        AppendToLog("点击清除已完成按钮");
        //await mainLayoutRobot.ClearCompletedButton.ClickOn();
        //上面这个失效是因为测试框架不成熟，只支持command
        await mainLayoutRobot.ClearCompletedButton.ClickOnReliably();
        // 验证只剩未完成的任务
        AppendToLog("验证清除后状态");
        mainLayoutRobot.StatusTextBlock.AssertHasText("已完成 0 / 全部 2");

        AppendToLog("主窗口 UI 测试完成");
    }
}