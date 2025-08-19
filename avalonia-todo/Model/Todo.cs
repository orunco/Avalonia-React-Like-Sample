using ReactiveUI;

namespace avalonia_todo.Models;

/*
React ts代码：
interface Todo {
    id: number;
    name: string;
    done: boolean;
}
参考React的实现，数据模型单独文件定义。
 */
public class Todo : ReactiveObject{
    private int _id;
    private string _name;
    private bool _done;
    
    // 添加构造函数，保证属性赋值触发通知
    public Todo(int id, string name, bool done = false)
    {
        Id = id;
        Name = name;
        Done = done;
    }
    
    public int Id{
        get => _id;
        set => this.RaiseAndSetIfChanged(ref _id, value);
    }

    public string Name{
        get => _name;
        set => this.RaiseAndSetIfChanged(ref _name, value);
    }

    public bool Done{
        get => _done;
        set => this.RaiseAndSetIfChanged(ref _done, value);
    }
}