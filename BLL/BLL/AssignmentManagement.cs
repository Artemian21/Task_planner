using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.IO;

namespace BLL
{
    public class AssignmentManagement
    {
        private List<Assignment> assignments;
        private TeamManagement teamManagement;
        private TaskManagement taskManagement;
        private const string filePath = "assignments.json";
        private List<Task> tasks;

        public AssignmentManagement(TeamManagement teamManagement, TaskManagement taskManagement)
        {
            this.teamManagement = teamManagement;
            this.taskManagement = taskManagement;

            try
            {
                assignments = Load() ?? new List<Assignment>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Помилка при завантаженні призначень: {ex.Message}");
                assignments = new List<Assignment>();
            }

            tasks = new List<Task>();

            try
            {
                taskManagement.Load();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Помилка при завантаженні завдань: {ex.Message}");
            }
        }

        // Розподіл завдань між виконавцями
        public void AssignTaskToMember(int taskId, int memberId)
        {
            try
            {
                Task task = taskManagement.GetTaskById(taskId);
                TeamMember member = teamManagement.GetTeamMemberById(memberId);

                if (task != null && member != null)
                {
                    assignments.Add(new Assignment(task, member));
                    Console.WriteLine($"Завдання '{task.Name}' було призначено {member.Name}.");
                    Save();
                    Console.WriteLine("Завдання успішно призначене.");
                    Console.ReadLine();
                }
                else
                {
                    Console.WriteLine("Невірний ID завдання або виконавця.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Помилка при призначенні завдання: {ex.Message}");
            }
        }

        // Оновлення статусу виконання завдання
        public void UpdateTaskStatus(int taskId, bool isCompleted)
        {
            try
            {
                var assignment = assignments.FirstOrDefault(a => a.Task.Id == taskId);
                if (assignment != null)
                {
                    assignment.Task.IsCompleted = isCompleted;
                    taskManagement.Save();
                    Console.WriteLine($"Статус завдання '{assignment.Task.Name}' оновлено на '{(isCompleted ? "Виконано" : "Не виконано")}'.");
                }
                else
                {
                    Console.WriteLine("Завдання не знайдено.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Помилка при оновленні статусу завдання: {ex.Message}");
            }
        }

        // Перевірка терміну виконання
        public void CheckTaskDeadline(int taskId)
        {
            try
            {
                var assignment = assignments.FirstOrDefault(a => a.Task.Id == taskId);
                if (assignment != null)
                {
                    var now = DateTime.Now;
                    if (now <= assignment.Task.Deadline)
                    {
                        Console.WriteLine($"Завдання '{assignment.Task.Name}' триває. Дедлайн: {assignment.Task.Deadline}");
                    }
                    else
                    {
                        Console.WriteLine($"Завдання '{assignment.Task.Name}' пропущено. Дедлайн був: {assignment.Task.Deadline}");
                    }
                }
                else
                {
                    Console.WriteLine("Завдання не знайдено.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Помилка при перевірці дедлайну завдання: {ex.Message}");
            }
        }

        // Перевірка завантаженості виконавців
        public void CheckTeamLoad(int memberId)
        {
            try
            {
                var member = teamManagement.GetTeamMemberById(memberId);
                if (member != null)
                {
                    var assignedTasks = assignments.Where(a => a.AssignedMember.ID == memberId).ToList();
                    Console.WriteLine($"Виконавець {member.Name} має {assignedTasks.Count} завдання(нь).");

                    foreach (var assignment in assignedTasks)
                    {
                        Console.WriteLine($"- Завдання: {assignment.Task.Name}, Статус: {(assignment.Task.IsCompleted ? "Виконано" : "Не виконано")}, Дедлайн: {assignment.Task.Deadline}");
                    }
                }
                else
                {
                    Console.WriteLine("Виконавець не знайдений.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Помилка при перевірці завантаженості виконавця: {ex.Message}");
            }
        }

        // Отримання стану виконання проекту
        public void GetProjectStatus()
        {
            try
            {
                int totalTasks = assignments.Count;
                int completedTasks = assignments.Count(a => a.Task.IsCompleted);
                int ongoingTasks = assignments.Count(a => !a.Task.IsCompleted && DateTime.Now <= a.Task.Deadline);
                int overdueTasks = assignments.Count(a => !a.Task.IsCompleted && DateTime.Now > a.Task.Deadline);

                Console.WriteLine($"Загальна кількість завдань, що розподілені: {totalTasks}");

                Console.WriteLine($"\nВиконані завдання: {completedTasks}");
                foreach (var assignment in assignments.Where(a => a.Task.IsCompleted))
                {
                    Console.WriteLine($"- Завдання: {assignment.Task.Name}, Дедлайн: {assignment.Task.Deadline}");
                }

                Console.WriteLine($"\nЗавдання в процесі: {ongoingTasks}");
                foreach (var assignment in assignments.Where(a => !a.Task.IsCompleted && DateTime.Now <= a.Task.Deadline))
                {
                    Console.WriteLine($"- Завдання: {assignment.Task.Name}, Дедлайн: {assignment.Task.Deadline}");
                }

                Console.WriteLine($"\nПрострочені завдання: {overdueTasks}");
                foreach (var assignment in assignments.Where(a => !a.Task.IsCompleted && DateTime.Now > a.Task.Deadline))
                {
                    Console.WriteLine($"- Завдання: {assignment.Task.Name}, Дедлайн: {assignment.Task.Deadline}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Помилка при отриманні стану виконання проекту: {ex.Message}");
            }
        }

        // Метод для отримання виконаних та невиконаних 
        public void ViewCompletedTasks(bool isCompleted)
        {
            var filteredAssignments = assignments.Where(a => a.Task.IsCompleted == isCompleted).ToList();

            if (filteredAssignments.Count == 0)
            {
                Console.WriteLine(isCompleted ? "Немає виконаних завдань." : "Немає невиконаних завдань.");
            }
            else
            {
                Console.WriteLine(isCompleted ? $"Виконані завдання: {filteredAssignments.Count}" : $"Невиконані завдання: {filteredAssignments.Count}");

                foreach (var assignment in filteredAssignments)
                {
                    Console.WriteLine($"- ID: {assignment.Task.Id} Завдання: {assignment.Task.Name}, Дедлайн: {assignment.Task.Deadline}");
                }
            }
        }

        // Метод для збереження завдань у файл 
        public void Save()
        {
            try
            {
                var options = new JsonSerializerOptions
                {
                    WriteIndented = true,
                    Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping
                };

                var json = JsonSerializer.Serialize(assignments, options);
                File.WriteAllText(filePath, json, Encoding.UTF8);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Помилка при збереженні завдань: {ex.Message}");
            }
        }

        // Метод для завантаження завдань з файлу
        public List<Assignment> Load()
        {
            if (!File.Exists(filePath))
            {
                return new List<Assignment>();
            }

            try
            {
                var json = File.ReadAllText(filePath, Encoding.UTF8);
                return JsonSerializer.Deserialize<List<Assignment>>(json) ?? new List<Assignment>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Помилка при завантаженні завдань: {ex.Message}");
                return new List<Assignment>();
            }
        }

        public List<Assignment> GetAssignments()
        {
            return assignments;
        }

        // Метод для пошуку завдань за ID учасника
        public List<Task> GetTasksByMemberId(int memberId)
        {
            try
            {
                var member = teamManagement.GetTeamMemberById(memberId);
                if (member != null)
                {
                    return assignments.Where(a => a.AssignedMember.ID == memberId).Select(a => a.Task).ToList();
                }
                return new List<Task>(); 
            }
            catch (Exception ex)
            {
                return new List<Task>();
            }
        }

        // Метод для пошуку виконавців за ID завдання
        public List<TeamMember> GetMemberByTaskId(int taskId)
        {
            try
            {
                var task = taskManagement.GetTaskById(taskId);
                if (task != null)
                {
                    return assignments.Where(a => a.Task.Id == taskId).Select(a => a.AssignedMember).ToList();
                }
                return new List<TeamMember>();
            }
            catch (Exception ex)
            {
                return new List<TeamMember>();
            }
        }
    }
}
