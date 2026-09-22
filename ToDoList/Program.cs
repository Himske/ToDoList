using ToDoList;

UI.LoadToDoList();

bool running = true;

while (running) {
    running = UI.ShowMain();
}

Environment.Exit(0);
