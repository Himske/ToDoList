using System;
using System.Collections.Generic;
using System.Linq;

namespace ToDoList {
    internal class ToDoListManager {
        public static List<ToDo> ToDoList { get; set; } = [];

        private static IFileManager s_fileManager = new FileManager();

        public static void SetFileManager(IFileManager fileManager) => s_fileManager = fileManager;

        public static ToDo GetTask(int id) {
            ToDo task = ToDoList.Find(t => t.Id == id) ?? throw new ArgumentException($"There is no task with Id: {id}");
            return task;
        }

        private static int GetNextAvailableId() {
            var ids = ToDoList.OrderBy(t => t.Id).Select(t => t.Id).ToList();

            // Find first gap in sequence
            for (int i = 0; i < ids.Count; i++) {
                if (ids[i] != i + 1)
                    return i + 1;
            }

            // If no gaps, next ID is last + 1
            return ids.Last() + 1;
        }

        public static void AddToDo(string title, DateTime dueDate, string project) {
            int newId = GetNextAvailableId();
            ToDoList.Add(new ToDo(newId, title, dueDate, Status.Not_Started, project));
        }

        public static void UpdateToDo(ToDo task, string title, DateTime dueDate, string project) {
            task.Title = title;
            task.DueDate = dueDate;
            task.Project = project;
            task.UpdateDate = DateTime.Now;
        }

        public static void UpdateStatus(ToDo task, Status status) {
            task.Status = status;
            task.UpdateDate = DateTime.Now;
        }

        public static bool RemoveToDo(ToDo task) {
            return ToDoList.Remove(task);
        }

        public static string Load() {
            ToDoList = s_fileManager.LoadTodos();
            return "ToDo List Loaded Successfully.";
        }

        public static string Save() {
            s_fileManager.SaveTodos(ToDoList);
            return "ToDo List Saved Successfully!";
        }
    }
}
