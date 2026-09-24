using System;
using System.Collections.Generic;
using System.Text;

namespace ToDoList {
    internal class ToDo {
        public ToDo(int id, string title, DateTime dueDate, Status status, string project) {
            Id = id;
            Title = title;
            DueDate = dueDate;
            Status = status;
            Project = project;
        }

        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public DateTime DueDate { get; set; }
        public Status Status { get; set; }
        public string Project { get; set; } = string.Empty;
        public DateTime UpdateDate { get; set; } = DateTime.Now;
    }
}
