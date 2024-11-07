using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public class Task
    {
        // Властивості завдання
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsCompleted { get; set; }
        public DateTime Deadline { get; set; } 

        public Task(int id, string name, string description, DateTime deadline)
        {
            Id = id;
            Name = name;
            Description = description;
            IsCompleted = false; 
            Deadline = deadline;
        }

        public void UpdateTask(string newName, string newDescription, DateTime newDeadline)
        {
            Name = newName;
            Description = newDescription;
            Deadline = newDeadline;
        }

        // Перевизначення для інформації про завдання
        public override string ToString()
        {
            return $"ID: {Id}, Назва: {Name}, Опис: {Description}";
        }
    }
}
