using System;
using System.Collections.Generic;

namespace avalonia_todo.Hook;

// 2. Hook 管理器 - 类似 useEffect
public class HookManager{
    private readonly List<Action> _effects = new();

    private readonly List<Action> _cleanupActions = new();

    // 注册副作用
    public void UseEffect(Action effect, Func<bool> dependencyChecker){
        _effects.Add(() => {
            if (dependencyChecker()){
                effect();
            }
        });
    }

    // 执行所有副作用
    public void RunEffects(){
        foreach (var effect in _effects){
            effect();
        }
    }
}