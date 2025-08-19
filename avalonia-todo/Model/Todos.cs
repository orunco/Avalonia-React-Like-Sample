using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace avalonia_todo.Models;

public class Todos : ObservableCollection<Todo>{
    public Todos() : base(){
    }

    public Todos(IEnumerable<Todo> collection) : base(collection){
    }
}