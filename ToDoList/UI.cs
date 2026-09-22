using Microsoft.VisualBasic.FileIO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace ToDoList {
    internal static class UI {
        public static bool ShowMain() {
            ShowHeader();
            ShowMainMenu();
            Console.WriteLine();
            string option = GetInput("Select Option: ");
            switch (option) {
                case "1":
                    Console.Clear();
                    ShowAppName();
                    ShowTaskList();
                    break;
                case "2":
                    Console.Clear();
                    ShowAppName();
                    AddTask();
                    break;
                case "3":
                    break;
                case "4":
                    SaveToDoList();
                    return false;
                default:
                    Console.WriteLine();
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine($"{option} is not a valid option!");
                    Console.ResetColor();
                    Console.WriteLine();
                    Pause();
                    break;
            }
            Console.Clear();
            return true;
        }

        public static void LoadToDoList() {
            try {
                string message = ToDoListManager.Load();
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine(message);
                Thread.Sleep(1000);
            }
            catch (FileNotFoundException ex) {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine(ex.Message);
                Thread.Sleep(1000);
            }
            catch (Exception ex) {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(ex.Message);
                Console.WriteLine();
                Console.ResetColor();
                Pause();
            }
            finally {
                Console.ResetColor();
                Console.Clear();
            }
        }

        public static void SaveToDoList() {
            try {
                string message = ToDoListManager.Save();
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine();
                Console.WriteLine(message);
                Thread.Sleep(1000);
            }
            catch (Exception ex) {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine();
                Console.WriteLine(ex.Message);
                Console.WriteLine();
                Console.ResetColor();
                Pause();
            }
            finally {
                Console.ResetColor();
            }
        }

        public static string GetInput(string prompt) {
            Console.Write(prompt);
            string input = Console.ReadLine() ?? string.Empty;
            return input.Trim();
        }

        public static void ShowListHeadings() {
            Console.ForegroundColor= ConsoleColor.White;
            Console.WriteLine("Id".PadRight(5) + "Title".PadRight(30) + "Due Date".PadRight(11) + "Status".PadRight(15) + "Project");
            Console.ResetColor();
        }

        public static void ShowListRow(ToDo task) {
            Console.WriteLine($"{task.Id,-5}{task.Title,-30}{task.DueDate:yyyy-MM-dd} {task.Status.ToString().Replace("_", " "),-15}{task.Project}");
        }

        public static void ShowListRowHighlighted(ToDo task) {
            if (task.DueDate > DateTime.Now && task.Status == Status.Done) {
                Console.ForegroundColor = ConsoleColor.Green;
            }
            else if (task.DueDate < DateTime.Now || task.Status == Status.Done) {
                Console.ForegroundColor = ConsoleColor.DarkGray;
            }
            else if (task.Status == Status.Canceled || task.Status == Status.Blocked) {
                Console.ForegroundColor = ConsoleColor.Red;
            }
            ShowListRow(task);
            Console.ResetColor();
        }

        public static void ShowToDosByDate() {
            ShowListHeadings();
            foreach (ToDo task in ToDoListManager.ToDoList.OrderBy(t => t.DueDate)) {
                ShowListRowHighlighted(task);
            }
        }

        public static void ShowToDosByProject() {
            ShowListHeadings();
            foreach (ToDo task in ToDoListManager.ToDoList.OrderBy(t => t.Project)) {
                ShowListRowHighlighted(task);
            }
        }

        public static void ShowAppName() {
            Console.WriteLine("Welcome to ToDoLy");
            Console.WriteLine();
        }

        public static void ShowHeader() {
            ShowAppName();
            Console.WriteLine($"You have {ToDoListManager.ToDoList.Count} tasks todo and {ToDoListManager.ToDoList.Count(t => t.Status == Status.Done)} tasks are done!");
            Console.WriteLine();
        }

        public static void ShowMainMenu() {
            Console.WriteLine("1. Show Task List (by date or project)");
            Console.WriteLine("2. Add New Task");
            Console.WriteLine("3. Edit Task (update, mark as done, remove");
            Console.WriteLine("4. Save and Quit");
        }

        public static void ShowDisplayMenu() {
            Console.WriteLine("1. Sort by Due Date");
            Console.WriteLine("2. Sort by Project");
        }

        public static void ShowEditMenu() {
            Console.WriteLine("1. Update Task");
            Console.WriteLine("2. Mark as Done");
            Console.WriteLine("3. Remove");
        }

        public static void Pause() {
            Console.Write("Press any key to continue.");
            Console.ReadKey();
        }

        public static void ShowTaskList() {
            ShowDisplayMenu();
            Console.WriteLine();
            string option = GetInput("Select Option: ");
            Console.WriteLine();
            switch (option) {
                case "1":
                    ShowToDosByDate();
                    Console.WriteLine();
                    Pause();
                    break;
                case "2":
                    ShowToDosByProject();
                    Console.WriteLine();
                    Pause();
                    break;
                default:
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine($"{option} is not a valid option!");
                    Console.ResetColor();
                    Console.WriteLine();
                    Pause();
                    break;
            }

        }

        public static string GetStringInput(string prompt) {
            string title = GetInput($"{prompt}: ");
            if (title.Equals(string.Empty)) {
                throw new NoNullAllowedException($"{prompt} can't be empty!");
            }
            return title;
        }

        public static DateTime GetDueDate() {
            string dueDateStr = GetInput("Due Date (YYYY-MM-DD): ");
            if (dueDateStr.Equals(string.Empty)) {
                throw new ArgumentException("Due Date can't be empty!");
            }
            if (!DateTime.TryParse(dueDateStr, out  DateTime dueDate)) {
                throw new ArgumentException($"{dueDateStr} is not a valid date!");
            }
            return dueDate;
        }

        public static void AddTask() {
            try {
                string title = GetStringInput("Title");
                DateTime dueDate = GetDueDate();
                string project = GetStringInput("Project");
                ToDoListManager.AddToDo(title, dueDate, Status.Not_Started, project);
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine();
                Console.WriteLine("Task added successfully.");
            }
            catch (Exception ex) {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine();
                Console.WriteLine(ex.Message);
            }
            Console.ResetColor();
            Console.WriteLine();
            Pause();
        }
    }
}
