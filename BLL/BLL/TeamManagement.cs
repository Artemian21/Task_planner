using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace BLL
{
    [Serializable]
    public class TeamManagement : IDisplay, ISerialization
    {
        private List<TeamMember> teamMembers = new List<TeamMember>();
        private const string filePath = "teamMembers.json"; // Назва файлу Json для членів команди

        public TeamManagement()
        {
            try
            {
                Load();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Помилка завантаження даних: {ex.Message}");
            }
        }

        // Додавання члена команди
        public void AddTeamMember(string name, string surname, int id)
        {
            try
            {
                var member = new TeamMember(name, surname, id);
                teamMembers.Add(member);
                Save();
                Console.WriteLine("Член команди успішно доданий.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Помилка додавання члена команди: {ex.Message}");
            }
            finally
            {
                Console.ReadLine();
            }
        }

        // Видалення учасника
        public void RemoveTeamMember(int id)
        {
            try
            {
                var member = teamMembers.Find(m => m.ID == id);
                if (member != null)
                {
                    teamMembers.Remove(member);
                    Save();
                    Console.WriteLine("Член команди успішно видалений.");
                }
                else
                {
                    Console.WriteLine("Члена команди з вказаним ID не знайдено.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Помилка видалення члена команди: {ex.Message}");
            }
            finally
            {
                Console.ReadLine();
            }
        }

        // Оновлення інформації про учасника
        public void UpdateTeamMember(int id, string newName, string newSurname)
        {
            try
            {
                var member = teamMembers.Find(m => m.ID == id);
                if (member != null)
                {
                    member.Name = newName;
                    member.Surname = newSurname;
                    Save();
                    Console.WriteLine("Дані про члена команди успішно оновлені.");
                }
                else
                {
                    Console.WriteLine("Члена команди з вказаним ID не знайдено.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Помилка оновлення даних члена команди: {ex.Message}");
            }
            finally
            {
                Console.ReadLine();
            }
        }

        // Метод для збереження інформації
        public void Save()
        {
            try
            {
                var options = new JsonSerializerOptions
                {
                    Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping, WriteIndented = true
                };

                string json = JsonSerializer.Serialize(teamMembers, options);
                File.WriteAllText(filePath, json, Encoding.UTF8);
            }
            catch (UnauthorizedAccessException ex)
            {
                Console.WriteLine($"Помилка доступу до файлу: {ex.Message}");
            }
            catch (IOException ex)
            {
                Console.WriteLine($"Помилка запису у файл: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Непередбачена помилка при збереженні: {ex.Message}");
            }
        }

        // Метод для завантаження інформації
        public void Load()
        {
            try
            {
                if (!File.Exists(filePath))
                {
                    File.Create(filePath).Dispose();
                    return;
                }

                string json = File.ReadAllText(filePath, Encoding.UTF8);
                teamMembers = JsonSerializer.Deserialize<List<TeamMember>>(json) ?? new List<TeamMember>();
            }
            catch (FileNotFoundException ex)
            {
                Console.WriteLine($"Файл не знайдено: {ex.Message}");
            }
            catch (JsonException ex)
            {
                Console.WriteLine($"Помилка обробки JSON: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Непередбачена помилка при завантаженні: {ex.Message}");
            }
        }

        // Метод для пошуку члена команди за ID
        public TeamMember GetTeamMemberById(int memberId)
        {
            try
            {
                return teamMembers.FirstOrDefault(m => m.ID == memberId);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Помилка при пошуку члена команди: {ex.Message}");
                return null;
            }
        }

        // Метод для виводу списка
        public void DisplayList()
        {
            try
            {
                if (teamMembers == null || teamMembers.Count == 0)
                {
                    Console.WriteLine("Список порожній.");
                    return;
                }
                Console.WriteLine("_______________________________________________________________________________");
                Console.WriteLine("Список виконавців: \n");
                foreach (var teamMember in teamMembers)
                {
                    teamMember.DisplayInfo();
                }
                Console.WriteLine("_______________________________________________________________________________");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Помилка відображення списку: {ex.Message}");
            }
        }

        // Перевірка на оригінальність ID
        public bool IsIdExist(int id)
        {
            try
            {
                return teamMembers.Any(member => member.ID == id);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Помилка перевірки існування ID: {ex.Message}");
                return false;
            }
        }
    }
}
