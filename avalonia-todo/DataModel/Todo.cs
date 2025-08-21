using System.Reactive.Linq;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace avalonia_todo.Models;

/*
React ts代码：
interface Todo {
    id: number;
    name: string;
    done: boolean;
}
参考React的实现，数据模型单独文件定义

严格而言，Avalonia世界的一个ViewModel是既包含纯粹的数据模型，也包含了UI模型的响应逻辑。而React的世界里面，数据模型就是纯粹的数据模型，UI响应部分是写在FC里面的。
这里文件夹改成DataModel，是为了特意标注：Todo和Todos不是ViewModel。
 */
// ReactiveObject 是 ReactiveUI 的基础类，它提供了属性变更通知的功能
public class Todo
    : ReactiveObject{
    // ReactiveUI.Fody提供代码简化功能
    [Reactive] public int Id{ get; set; }
    [Reactive] public string Name{ get; set; }
    [Reactive] public bool Done{ get; set; }

    // 添加 Temperature 属性用于演示
    [Reactive] public int Temperature{ get; set; } = 20;
    
    // 演示只读计算属性
    public string Forecast => _forecast.Value;
    private readonly ObservableAsPropertyHelper<string> _forecast;
    
    public Todo(int id, string name, bool done = false) {
        Id=id;
        Name=name;
        Done=done;
        
        // 初始化 ObservableAsPropertyHelper
        _forecast = this.WhenAnyValue(x => x.Temperature)
            .Select(temp => temp > 25 ? "炎热" : "凉爽")
            .ToProperty(this, x => x.Forecast);
    }
 
}