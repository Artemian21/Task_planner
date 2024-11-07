using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.IO;
using System.Threading.Tasks;

namespace BLL
{
    public class TaskManagement : IDisplay, ISerialization
    {
        private List<Task> tasks;
        private const string filePath = "tasks.json"; // Шлях до файлу для збереження завдань

        public TaskManagement()
        {
            tasks = new List<Task>();
            Load(); // Завантаження завдань з файлу при створенні об'єкта
        }

        // Додати завдання
        public void AddTask(int id, string name, string description, DateTime deadline)
        {
            try
            {
                if (IsIdExist(id))
                {
                    Console.WriteLine("Завдання з таким ID вже існує.");
                    return;
                }
                Task newTask = new Task(id, name, description, deadline);
                tasks.Add(newTask);
                Save(); // Збереження завдань після додавання
                Console.WriteLine("Завдання додано успішно.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Помилка при додаванні завдання: {ex.Message}");
            }
            Console.ReadKey();
        }

        // Видалити завдання
        public void RemoveTask(int id)
        {
            try
            {
                Task taskToRemove = tasks.FirstOrDefault(t => t.Id == id);
                if (taskToRemove != null)
                {
                    tasks.Remove(taskToRemove);
                    Save(); // Збереження після видалення
                    Console.WriteLine("Завдання видалено.");
                }
                else
                {
                    Console.WriteLine("Завдання з таким ID не знайдено.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Помилка при видаленні завдання: {ex.Message}");
            }
            Console.ReadKey();
        }

        // Оновити завдання
        public void UpdateTask(int id, string newName, string newDescription, DateTime newDeadline)
        {
            try
            {
                Task taskToUpdate = tasks.FirstOrDefault(t => t.Id == id);
                if (taskToUpdate != null)
                {
                    taskToUpdate.UpdateTask(newName, newDescription, newDeadline);
                    Save(); // Збереження після оновлення
                    Console.WriteLine("Завдання оновлено.");
                }
                else
                {
                    Console.WriteLine("Завдання з таким ID не знайдено.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Помилка при оновленні завдання: {ex.Message}");
            }
            Console.ReadKey();
        }

        // Зберегти завдання у файл
        public void Save()
        {
            try
            {
                var options = new JsonSerializerOptions
                {
                    WriteIndented = true, // Форматований JSON для кращої читабельності
                    Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping // Збереження кирилиці
                };

                string jsonData = JsonSerializer.Serialize(tasks, options);
                File.WriteAllText(filePath, jsonData, Encoding.UTF8);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Помилка при збереженні завдань: {ex.Message}");
            }
        }

        // Завантажити завдання з файлу
        public void Load()
        {
            if (!File.Exists(filePath))
            {
                // Якщо файл не існує, створюємо порожній файл
                File.WriteAllText(filePath, "[]", Encoding.UTF8); // Записуємо порожній JSON масив
                tasks = new List<Task>(); // Ініціалізуємо порожній список завдань
                Console.WriteLine($"Файл '{filePath}' не знайдено. Створено новий файл.");
            }
            else
            {
                try
                {
                    string jsonData = File.ReadAllText(filePath, Encoding.UTF8);
                    tasks = JsonSerializer.Deserialize<List<Task>>(jsonData) ?? new List<Task>();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Помилка при завантаженні завдань: {ex.Message}");
                }
            }
        }

        // Отримання завдання за ID
        public Task GetTaskById(int taskId)
        {
            try
            {
                var task = tasks.FirstOrDefault(t => t.Id == taskId);
                if (task == null)
                {
                    Console.WriteLine("Завдання з таким ID не знайдено.");
                }
                return task;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Помилка при пошуку завдання: {ex.Message}");
                return null;
            }
        }
        
        // Отримання списку
        public List<Task> GetAllTasks()
        {
            return tasks;
        }

        // Виведення списку
        public void DisplayList()
        {
            try
            {
                if (tasks == null || tasks.Count == 0)
                {
                    Console.WriteLine("Список порожній.");
                    return;
                }
                Console.WriteLine("_______________________________________________________________________________");
                Console.WriteLine("Список задач: \n");
                foreach (var task in tasks)
                {
                    Console.WriteLine(task.ToString());
                }
                Console.WriteLine("_______________________________________________________________________________");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Помилка при виведенні списку завдань: {ex.Message}");
            }
        }

        // Перевірка оригінальності ID
        public bool IsIdExist(int id)
        {
            return tasks.Any(t => t.Id == id);
        }
    }
}
