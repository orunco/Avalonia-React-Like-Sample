using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace avalonia_todo.Hook;

// 1. 状态管理类 - 类似 useState
public class State<T> : INotifyPropertyChanged{
    private T _value;

    public T Value{
        get => _value;
        set{
            if (!EqualityComparer<T>.Default.Equals(_value, value)){
                _value = value;
                OnPropertyChanged();
            }
        }
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    protected virtual void OnPropertyChanged(
        [CallerMemberName] string? propertyName = null){
        PropertyChanged?.Invoke(this, 
            new PropertyChangedEventArgs(propertyName));
    }

    public static implicit operator T(State<T> state) => state.Value;
}