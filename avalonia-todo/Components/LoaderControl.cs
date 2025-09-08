using System;
using System.Threading;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Logging;
using Avalonia.Threading;

namespace VerifyAvaAction;

/*
LoaderControl 是一个 AttachedProperty 的容器类，
它并不需要被添加到可视化树中，也不需要呈现 UI，它只是一个逻辑组件，用于定义和处理附加属性。
 */
public class LoaderControl{
    // Attached property for the image source URL.
    public static readonly AttachedProperty<string?> SourceProperty =
        AvaloniaProperty.RegisterAttached<LoaderControl, Image, string?>("Source");

    private static readonly ParametrizedLogger? vLogger =
        Logger.TryGet(LogEventLevel.Information, nameof(LoaderControl));

    static LoaderControl(){
        SourceProperty.Changed.AddClassHandler<Image>(HandleSourceChanged);
    }

    // Sets the source URL for the image.
    public static void SetSource(Image obj, string? value){
        obj.SetValue(SourceProperty, value);
    }

    // Gets the source URL for the image.
    public static string? GetSource(Image obj){
        return obj.GetValue(SourceProperty);
    }

    public static void log(string msg){
        Console.WriteLine($"{Application.Current?.GetHashCode()}: {msg}");
        // Logger.TryGet(LogEventLevel.Information, nameof(LoaderControl))
    }

    private static void HandleSourceChanged(
        Image sender,
        AvaloniaPropertyChangedEventArgs args){
        var newValue = args.NewValue as string;
        if (newValue == null) return;

        log($"DesignMode={Design.IsDesignMode}");
        
        Task.Run(() => {
                log($"+++Enter task1");
                // 这里实际业务就必须是同步的，正常app也是OK的，不要试图改为异步
                Dispatcher.UIThread.Invoke(() => Hello1(newValue));
                log($"+++Leave task1");
            })
            .ContinueWith(t => {
                log($"+++Enter task2");
                // 这里实际业务就必须是异步的，正常app也是OK的，不要试图改为同步
                Dispatcher.UIThread.Post(async () => {
                    await Task.Delay(2000); // 模拟真实业务
                    Hello2(newValue);
                });
                log($"+++Leave task2");
            });
    }

    public static void Hello1(string value){
        Thread.Sleep(500); // 模拟真实业务
        log($"----hello1: {value}");
    }

    public static void Hello2(string value){
        log($"----hello2: {value}");
    }
}