using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Text.Json;

namespace ToDoList {
    internal class ToDoListManager {
        public static List<ToDo> ToDoList { get; set; } = [];
        private static readonly string s_fileName = "todos.json";
        private static readonly JsonSerializerOptions s_options = new() {
            WriteIndented = true
        };

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
        
        public static void AddToDo(string title, DateTime dueDate, Status status, string project) {
            // Maybe status should always be "Not Started" when adding a todo
            int newId = GetNextAvailableId();
            ToDoList.Add(new ToDo(newId, title, dueDate, status, project));
        }

        public static ToDo GetTask(int id) {
            ToDo task = ToDoList.Find(t => t.Id == id) ?? throw new ArgumentException($"There is no task with Id: {id}");
            return task;
        }

        public static void UpdateToDo(int id, string title, DateTime dueDate, string project) {
            ToDo task = ToDoList.Find(t => t.Id == id) ?? throw new ArgumentException($"There is no task with Id: {id}");
            task.Title = title;
            task.DueDate = dueDate;
            task.Project = project;
            task.UpdateDate = DateTime.Now;
        }

        public static bool RemoveToDo(ToDo task) {
            return ToDoList.Remove(task);
        }

        public static void UpdateStatus(int id, Status status) {
            ToDo task = ToDoList.Find(t => t.Id == id) ?? throw new ArgumentException($"There is no task with Id: {id}");
            task.Status = status;
            task.UpdateDate = DateTime.Now;
        }

        public static string Save() {
            try {
                string jsonString = JsonSerializer.Serialize(ToDoList, s_options);
                string filePath = Path.Combine(Directory.GetCurrentDirectory(), "Data", s_fileName);
                File.WriteAllText(filePath, jsonString);
                return "ToDo List Saved Successfully!";
            }
            catch {
                throw new Exception("Failed To Save ToDo List");
            }
        }

        public static string Load() {
            string filePath = Path.Combine(Directory.GetCurrentDirectory(), "Data", s_fileName);
            if (!File.Exists(filePath)) {
                throw new FileNotFoundException($"{s_fileName} not found.");
            }
            else {
                string jsonString = File.ReadAllText(filePath);
                if (string.IsNullOrWhiteSpace(jsonString)) {
                    return $"{s_fileName} is empty.";
                }
                else {
                    try {
                        ToDoList = JsonSerializer.Deserialize<List<ToDo>>(jsonString) ?? [];
                        return "ToDo List Loaded Successfully.";

                    }
                    catch (JsonException ex) {
                        throw new Exception($"Error parsing JSON: {ex.Message}");
                    }
                }
            }
        }
    }
}
