using Microsoft.VisualBasic;
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
                    Console.Clear();
                    ShowAppName();
                    EditTask();
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
            else if (task.DueDate < DateTime.Now && task.Status != Status.Done) {
                Console.ForegroundColor = ConsoleColor.Red;
            }
            else if (task.Status == Status.Canceled) {
                Console.ForegroundColor = ConsoleColor.DarkGray;
            }
            else if (task.Status == Status.Blocked || task.Status == Status.On_Hold) {
                Console.ForegroundColor = ConsoleColor.Yellow;
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
            Console.WriteLine("3. Edit Task (update/remove)");
            Console.WriteLine("4. Save and Quit");
        }

        public static void ShowDisplayMenu() {
            Console.WriteLine("1. Sort by Due Date");
            Console.WriteLine("2. Sort by Project");
        }

        public static void ShowEditMenu() {
            Console.WriteLine("1. Update Task (Title, Due Date or Project)");
            Console.WriteLine("2. Change Task Status");
            Console.WriteLine("3. Remove Task");
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

        public static string GetInput(string prompt) {
            Console.Write(prompt);
            string input = Console.ReadLine() ?? string.Empty;
            return input.Trim();
        }

        public static string GetStringInput(string prompt, bool editing = false) {
            string input = GetInput($"{prompt}: ");
            if (!editing && input.Equals(string.Empty)) {
                throw new NoNullAllowedException($"{prompt} can't be empty!");
            }
            return input;
        }

        public static int GetIntInput(string prompt) {
            string input = GetInput($"{prompt}: ");
            if (input.Equals(string.Empty)) {
                throw new NoNullAllowedException($"{prompt} can't be empty!");
            }
            if (!int.TryParse(input, out int result)) {
                throw new ArgumentException($"{input} is not valid number!");
            }
            return result;
        }

        public static DateTime GetDueDate(string input) {
            if (input.Equals(string.Empty)) {
                throw new ArgumentException("Due Date can't be empty!");
            }
            if (!DateTime.TryParse(input, out  DateTime dueDate)) {
                throw new ArgumentException($"{input} is not a valid date!");
            }
            return dueDate;
        }

        public static void AddTask() {
            try {
                string title = GetStringInput("Title");
                string dueDateStr = GetStringInput("Due Date(YYYY-MM-DD)");
                DateTime dueDate = GetDueDate(dueDateStr);
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

        public static void EditTask() {
            ShowEditMenu();
            Console.WriteLine();
            string option = GetInput("Select Option: ");
            Console.WriteLine();
            switch (option) {
                case "1":
                    UpdateTask();
                    Console.WriteLine();
                    Pause();
                    break;
                case "2":
                    ChangeTaskStatus();
                    Console.WriteLine();
                    Pause();
                    break;
                case "3":
                    RemoveTask();
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

        private static void UpdateTask() {
            try {
                int id = GetIntInput("Id");
                ToDo task = ToDoListManager.GetTask(id);
                Console.WriteLine();
                ShowListHeadings();
                ShowListRow(task);
                Console.WriteLine();
                string input = GetStringInput("Is this the task you want to update?", true);
                Console.WriteLine();
                if (input.ToUpper().Equals("Y")) {
                    string newTitle = GetStringInput("Title", true);
                    if (newTitle.Equals(string.Empty)) {
                        newTitle = task.Title;
                    }
                    DateTime newDueDate;
                    string newDueDateStr = GetStringInput("Due Date(YYYY-MM-DD)", true);
                    if (newDueDateStr.Equals(string.Empty)) {
                        newDueDate = task.DueDate;
                    }
                    else {
                        newDueDate = GetDueDate(newDueDateStr);
                    }
                    string newProject = GetStringInput("Project", true);
                    if (newProject.Equals(string.Empty)) {
                        newProject = task.Project;
                    }
                    if (newTitle != task.Title || newDueDate != task.DueDate || newProject != task.Project) {
                        ToDoListManager.UpdateToDo(task.Id, newTitle, newDueDate, newProject);
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine();
                        Console.WriteLine("Task updated successfully.");
                    }
                    else {
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.WriteLine();
                        Console.WriteLine("No changes were made.");
                    }
                }
                else {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine("No changes were made.");
                }
            }
            catch (Exception ex) {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine();
                Console.WriteLine(ex.Message);
            }
            finally {
                Console.ResetColor();
            }
        }

        private static void ChangeTaskStatus() {
            try {
                int id = GetIntInput("Id");
                ToDo task = ToDoListManager.GetTask(id);
                Console.WriteLine();
                ShowListHeadings();
                ShowListRow(task);
                Console.WriteLine();
                string input = GetStringInput("Is this the task you want to update?", true);
                Console.WriteLine();
                if (input.ToUpper().Equals("Y")) {
                    List<string> statuses = [.. Enum.GetNames<Status>()];
                    for (int i=0; i < Enum.GetNames<Status>().Length; i++) {
                        Console.WriteLine($"{i}. {statuses[i]}");
                    }
                    Console.WriteLine();
                    Char newStatus = Char.Parse(GetStringInput("New status"));
                    if (newStatus < '0' || newStatus > '6') {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine();
                        Console.WriteLine("Invalid status");
                    }
                    else if(Enum.TryParse<Status>(newStatus.ToString(), out var status)) {
                        ToDoListManager.UpdateStatus(id, status);
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine();
                        Console.WriteLine("Status updated successfully.");
                    }
                }
                else {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine("Status wasn't changed.");
                }
            }
            catch (Exception ex) {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine();
                Console.WriteLine(ex.Message);
            }
            finally {
                Console.ResetColor();
            }
        }

        private static void RemoveTask() {
            try {
                int id = GetIntInput("Id");
                ToDo task = ToDoListManager.GetTask(id);
                Console.WriteLine();
                ShowListHeadings();
                ShowListRow(task);
                Console.WriteLine();
                string input = GetStringInput("Is this the task you want to remove?", true);
                Console.WriteLine();
                if (input.ToUpper().Equals("Y") && ToDoListManager.RemoveToDo(task)) {
                    Console.ForegroundColor= ConsoleColor.Green;
                    Console.WriteLine($"Task: \"{task.Title}\" removed successfully.");
                }
                else {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine("Task wasn't removed.");
                }
            }
            catch (Exception ex) {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine();
                Console.WriteLine(ex.Message);
            }
            finally {
                Console.ResetColor();
            }
        }
    }
}
