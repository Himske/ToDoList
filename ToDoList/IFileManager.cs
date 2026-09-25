using System.Collections.Generic;

namespace ToDoList {
    internal interface IFileManager {
        List<ToDo> LoadTodos();
        void SaveTodos(List<ToDo> todos);
    }
}
