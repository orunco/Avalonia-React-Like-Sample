using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using avalonia_todo;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Headless.NUnit;
using Avalonia.Threading;
using NUnit.Framework;
using VerifyAvaAction;

namespace avalonia_todo_test.UT.Headless;

[TestFixture]
// key1
[Apartment(ApartmentState.STA)]
public class Headless_Demo{
    // key2
    [SetUp]
    public void SetUp(){
        Trace.Listeners.Clear();
        Trace.Listeners.Add(new ConsoleTraceListener());

        // 确保Avalonia应用程序已初始化
        if (Application.Current == null){
            AppBuilder.Configure<Application>()
                .UsePlatformDetect()
                .SetupWithoutStarting()
                .LogToTrace();
            TestContext.WriteLine($"Setup: app is {Application.Current?.GetHashCode()}");
        }
    }

    // key3
    public static void RunLoop(int maxWaitMs = 3000){
        // 测试环境中，后台线程启动和Dispatcher任务执行需要时间，
        // 而测试方法很快就结束了，没有给这些异步操作执行的机会。
        // 实际测试，如此设置可确保队列强制完成任务
        var startTime = Environment.TickCount; // 记录开始时间
        while (Environment.TickCount - startTime < maxWaitMs){
            Thread.Sleep(50);
            // 强制同步处理所有队列任务
            Dispatcher.UIThread.RunJobs();

            // 优化：如果没有任务了，提前退出
            if (Dispatcher.UIThread
                .HasJobsWithPriority(DispatcherPriority.SystemIdle)){
                TestContext.WriteLine("No job exist.");
                break;
            }
        }
        // 最多maxWaitMs后自动退出
    }

    [AvaloniaTest]
    public void Test1(){
        // key4：每一个用例都是独立的app，互不干扰
        var app = new App();
        app.Initialize();

        TestContext.WriteLine($"==Enter Test1: app is {Application.Current?.GetHashCode()}");

        LoaderControl.SetSource(new Image(), "test1");
        RunLoop();

        TestContext.WriteLine("==Level Test1");
    }

    [AvaloniaTest]
    public void Test2(){
        // 创建新的应用程序实例
        var app = new App();
        app.Initialize();

        TestContext.WriteLine($"==Enter Test2: app is {Application.Current?.GetHashCode()}");

        LoaderControl.SetSource(new Image(), "test2");
        RunLoop();

        TestContext.WriteLine("==Level Test2");
    }

    [Test]
    public void TestAsyncDispatcher(){
        var executed = false;
        var app = new App();
        app.Initialize();

        Dispatcher.UIThread.Post(async () => {
            Console.WriteLine("Before await");
            await Task.Delay(1000); // 只会打印 "Before await"
            Console.WriteLine("After await - this is Hello2 equivalent");
            executed = true;
        });

        Console.WriteLine("Before RunJobs");
        Dispatcher.UIThread.RunJobs(); // 只会打印 "Before await"
        Console.WriteLine($"After RunJobs, executed={executed}"); // executed = false

        Thread.Sleep(1500); // 等待 Task.Delay 完成
        Dispatcher.UIThread.RunJobs(); // 现在会打印 "After await"
        Console.WriteLine($"Final, executed={executed}"); // executed = true
    }
}