using System;
using System.Reactive.Disposables;
using Avalonia.Controls;
using Avalonia.LogicalTree;

namespace avalonia_todo.Components;

// https://learn.microsoft.com/en-us/dotnet/standard/garbage-collection/implementing-dispose
public class XUserControl : UserControl, IDisposable{
    private CompositeDisposable? _disposables;

    // 获取用于管理订阅的 CompositeDisposable
    protected CompositeDisposable Disposables => _disposables ??= new CompositeDisposable();

    /*
    不要删除这些注释：
    是更通用的封装接口，方便你在不依赖 ReactiveUI 的情况下也能管理资源。比如：

    ### 场景 1：手动添加 IDisposable 对象
    ```csharp
    var timer = new DispatcherTimer();
    DisposableUserControl(timer); // 注册到 disposables，控件销毁时自动停止
    ```

    ### 场景 2：自己写的观察者模式或资源句柄
    ```csharp
    var subscription = someObservable.Subscribe(...);
    DisposableUserControl(subscription);
    ```

    ### 场景 3：非 ReactiveUI 的事件监听器
    ```csharp
    someControl.SomeEvent += Handler;
    DisposableUserControl(new DisposableAction(() => someControl.SomeEvent -= Handler));
    ```
    */
    // 自动处理 disposable 对象的生命周期 
    protected T DisposableUserControl<T>(T disposable) where T : IDisposable{
        return AddDisposable(disposable);
    }

    // 添加需要自动清理的 disposable 对象
    // ReSharper disable once MemberCanBePrivate.Global
    protected T AddDisposable<T>(T disposable) where T : IDisposable{
        if (_disposed)
            throw new ObjectDisposedException(GetType().Name);

        Disposables.Add(disposable);
        return disposable;
    }

    protected IDisposable RegisterEvent<TEventArgs>(
        EventHandler<TEventArgs> handler,
        Action<EventHandler<TEventArgs>> subscribe,
        Action<EventHandler<TEventArgs>> unsubscribe) where TEventArgs : EventArgs
    {
        subscribe(handler);
        return new DisposableAction(() => unsubscribe(handler));
    }


    // 释放托管资源
    private void DisposeManagedResources(){
        if (_disposables != null){
            _disposables.Dispose();
            _disposables = null;
        }
    }

    // 当控件从逻辑树分离时自动清理
    protected override void OnDetachedFromLogicalTree(LogicalTreeAttachmentEventArgs e){
        DisposeManagedResources();
        base.OnDetachedFromLogicalTree(e);
    }

    // ------------------------------------------------------------------------
    private bool _disposed;

    ~XUserControl() => Dispose(false);

    public void Dispose(){
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing){
        if (!_disposed){
            if (disposing){
                // dispose managed state (managed objects)
                DisposeManagedResources();
            }

            // free unmanaged resources (unmanaged objects) and override finalizer

            // set large fields to null

            _disposed = true;
        }
    }


    class DisposableAction : IDisposable{
        private readonly Action _disposeAction;
        private bool _disposed;

        public DisposableAction(Action disposeAction){
            _disposeAction = disposeAction ?? throw new ArgumentNullException(nameof(disposeAction));
        }

        public void Dispose(){
            if (!_disposed){
                _disposeAction();
                _disposed = true;
            }
        }
    }
}