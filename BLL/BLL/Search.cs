using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace BLL
{
    public class Search
    {
        private TeamManagement teamManagement;
        private TaskManagement taskManagement;
        private AssignmentManagement assignmentManagement;

        public Search(TeamManagement teamMgmt, TaskManagement taskMgmt, AssignmentManagement assignmentMgmt)
        {
            this.teamManagement = teamMgmt;
            this.taskManagement = taskMgmt;
            this.assignmentManagement = assignmentMgmt;
        }

        // Пошук виконавця за ID та виведення його завдань
        public void SearchMemberAndTasksById(int memberId)
        {
            try
            {
                var member = teamManagement.GetTeamMemberById(memberId);
                if (member != null)
                {
                    Console.WriteLine($"\nІм'я: {member.Name}, Прізвище: {member.Surname}, ID: {member.ID}");

                    var tasks = assignmentManagement.GetTasksByMemberId(memberId);
                    if (tasks.Count > 0)
                    {
                        Console.WriteLine("\nЗакріплені завдання:");
                        foreach (var task in tasks)
                        {
                            Console.WriteLine(task.ToString());
                        }
                    }
                    else
                    {
                        Console.WriteLine("\nУ цього члена команди немає закріплених завдань.");
                    }
                }
                else
                {
                    Console.WriteLine("\nЧлена команди з таким ID не знайдено.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Помилка під час пошуку члена команди: {ex.Message}");
            }
        }

        // Пошук завдань закріплених за виконавцем
        public void SearchTaskExecutorsById(int taskId)
        {
            try
            {
                var task = taskManagement.GetTaskById(taskId);
                if (task != null)
                {
                    Console.WriteLine($"\nЗавдання: " + task.ToString());
                    var members = assignmentManagement.GetMemberByTaskId(taskId);
                    if (members.Count > 0)
                    {
                        Console.WriteLine($"\nВиконавці завдання:\n");
                        foreach (var member in members)
                        {
                            Console.WriteLine($"Ім'я: {member.Name}, Прізвище: {member.Surname}, ID: {member.ID}");
                        }
                    }
                    else
                    {
                        Console.WriteLine("\nНемає виконавців, закріплених за цим завданням.");
                    }
                }
                else
                {
                    Console.WriteLine("\nЗавдання з таким ID не знайдено.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Помилка під час пошуку виконавців завдання: {ex.Message}");
            }
        }

        // Пошук виконаних або невиконаних завдань
        public void SearchTasksByStatus(bool isCompleted)
        {
            try
            {
                var tasks = taskManagement.GetAllTasks()
                    .Where(t => t.IsCompleted == isCompleted)
                    .ToList();

                string status = isCompleted ? "виконані" : "невиконані";
                if (tasks.Count > 0)
                {
                    Console.WriteLine($"\nСписок завдань, що {status}:");
                    foreach (var task in tasks)
                    {
                        Console.WriteLine(task.ToString());
                    }
                }
                else
                {
                    Console.WriteLine($"\nНемає завдань, що {status}.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Помилка під час пошуку завдань за статусом: {ex.Message}");
            }
        }
    }
}
