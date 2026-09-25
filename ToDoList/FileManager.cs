using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace ToDoList {
    internal class FileManager : IFileManager {
        private static readonly string s_fileName = "todos.json";
        private static readonly JsonSerializerOptions s_options = new() { WriteIndented = true };

        private static string GetDataDirectory() => Path.Combine(Directory.GetCurrentDirectory(), "Data");
        private static string GetFilePath() => Path.Combine(GetDataDirectory(), s_fileName);

        public List<ToDo> LoadTodos() {
            string filePath = GetFilePath();
            if (!File.Exists(filePath))
                throw new FileNotFoundException($"{s_fileName} not found.");

            string jsonString = File.ReadAllText(filePath);
            if (string.IsNullOrWhiteSpace(jsonString))
                return new List<ToDo>();

            try {
                return JsonSerializer.Deserialize<List<ToDo>>(jsonString, s_options) ?? new List<ToDo>();
            }
            catch (JsonException ex) {
                throw new Exception($"Error parsing JSON: {ex.Message}");
            }
        }

        public void SaveTodos(List<ToDo> todos) {
            try {
                string dir = GetDataDirectory();
                if (!Directory.Exists(dir))
                    Directory.CreateDirectory(dir);

                string jsonString = JsonSerializer.Serialize(todos, s_options);
                File.WriteAllText(GetFilePath(), jsonString);
            }
            catch (Exception ex) {
                throw new Exception("Failed To Save ToDo List", ex);
            }
        }
    }
}
