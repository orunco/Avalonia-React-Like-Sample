using System;
using System.Linq;
using Avalonia.Controls;
using Avalonia.Controls.Samples;
using Avalonia.Headless;
using Avalonia.Headless.NUnit;
using Avalonia.Input;
using Avalonia.Threading;
using Avalonia.VisualTree;
using NUnit.Framework;

namespace Avalonia.Controls.Samples;

public class UT_Headless_Demo{
    
    [SetUp]
    public void Setup(){
        // 每个测试前的设置
    }

    [AvaloniaTest]
    public void Test_Demo(){
        // var window = new Window{ Width = 800, Height = 600 };
        // window.Content = page;
        // window.Show();

        // Act - 等待UI更新，同步操作，等待UI完成状态
        // Dispatcher.UIThread.RunJobs();

        // Assert
        // 注意是NUnit 4 API，类似Assert.That(true, Is.Equal())等等
        // Assert.That(); 

        // window.Close();
    }
 
}