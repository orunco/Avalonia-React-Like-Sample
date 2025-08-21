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
        // 1. 创建控件（不需要 Window）
        // var footer = new Footer(todos, callback, callback);
    
        // 2. 控件初始化完成，绑定生效
        // Dispatcher.UIThread.RunJobs();  // 刷新绑定
    
        // 3. 模拟用户交互
        // footer.CheckBox.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
    
        // 4. 验证结果
        // 注意是NUnit 4 API，类似Assert.That(true, Is.Equal())等等
        // Assert.That(callbackWasCalled, Is.True);
    }
 
}