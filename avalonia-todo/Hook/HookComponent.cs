using System;
using System.Collections.Generic;
using Avalonia.Controls;

namespace avalonia_todo.Hook;

// 3. 基础 Hook 组件
public abstract class HookComponent : UserControl{
    protected readonly HookManager HookManager = new();
    private readonly List<object[]> _dependencyStates = new(); // 保存依赖状态
    private int _effectIndex = 0; // 跟踪 effect 索引

    // 创建状态 - 类似 useState
    protected State<T> UseState<T>(T initialValue){
        return new State<T>{ Value = initialValue };
    }

    // 副作用 - 类似 useEffect
    protected void UseEffect(Action effect, params object[] dependencies){
        var effectIndex = _effectIndex++;
        
        // 确保有足够的依赖状态存储空间
        while (_dependencyStates.Count <= effectIndex){
            _dependencyStates.Add(null);
        }

        HookManager.UseEffect(() => { effect(); }, () => {
            var lastDependencies = _dependencyStates[effectIndex];
            
            // 如果是第一次调用或者依赖发生变化
            if (lastDependencies == null || DependenciesChanged(lastDependencies, dependencies)){
                _dependencyStates[effectIndex] = (object[])dependencies.Clone();
                return true;
            }
            
            return false;
        });
    }

    private bool DependenciesChanged(object[] oldDeps, object[] newDeps){
        if (oldDeps.Length != newDeps.Length) return true;
        
        for (int i = 0; i < oldDeps.Length; i++){
            if (!Equals(oldDeps[i], newDeps[i])){
                return true;
            }
        }
        return false;
    }

    // 订阅状态变化
    protected void WatchState<T>(State<T> state, Action<T> callback){
        state.PropertyChanged += (s, e) => {
            if (e.PropertyName == nameof(State<T>.Value)){
                callback(state.Value);
            }
        };
    }

    // 重置 effect 索引，用于重新渲染时
    protected void ResetEffectIndex(){
        _effectIndex = 0;
    }
}