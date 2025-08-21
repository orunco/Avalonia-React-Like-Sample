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
 */
// ReactiveUI+ReactiveUI.Fody提供元素支持
public class Todo(
    int id,
    string name,
    bool done = false)
    : ReactiveObject{
    [Reactive] public int Id{ get; set; } = id;
    [Reactive] public string Name{ get; set; } = name;
    [Reactive] public bool Done{ get; set; } = done;
}