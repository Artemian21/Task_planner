using System.Globalization;
using BLL;

namespace Cursova
{
    public class ConsoleMenu
    {
        private TeamManagement teamManagement;
        private TaskManagement taskManagement;
        private AssignmentManagement assignmentManagement;
        private Search search;

        //Ініціалізація об'єктів
        public ConsoleMenu()
        {
            teamManagement = new TeamManagement(); 
            taskManagement = new TaskManagement();
            assignmentManagement = new AssignmentManagement(teamManagement, taskManagement);
            search = new Search(teamManagement, taskManagement, assignmentManagement);   
        }

        //Головне меню програми 
        public void ShowMainMenu()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("Головне меню");
                Console.WriteLine("1. Управління членами команди");
                Console.WriteLine("2. Управління завданнями");
                Console.WriteLine("3. Управління розподілом та виконанням завдань");
                Console.WriteLine("4. Пошук");
                Console.WriteLine("0. Вийти");

                string choice = Console.ReadLine();


                switch (choice)
                {
                    case "1":
                        ShowTeamManagementMenu();
                        break;
                    case "2":
                        ShowTaskManagementMenu();
                        break;
                    case "3":
                        ShowAssignmentManagementMenu();
                        break;
                    case "4":
                        ShowSearchMenu();
                        break;
                    case "0":
                        return;
                    default:
                        Console.WriteLine("Невірний вибір, спробуйте знову.");
                        Console.ReadLine();
                        break;
                }
            }
        }

        // Підменю для членів команди
        private void ShowTeamManagementMenu()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("Управління членами команди");
                Console.WriteLine("1. Додати виконавця");
                Console.WriteLine("2. Видалити виконавця");
                Console.WriteLine("3. Змінити дані про виконавця");
                Console.WriteLine("4. Переглянути список всіх членів команди");
                Console.WriteLine("0. Повернутись до головного меню");

                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        AddTeamMember();
                        break;
                    case "2":
                        RemoveTeamMember();
                        break;
                    case "3":
                        UpdateTeamMember();
                        break;
                    case "4":
                        ViewTeamMembers();
                        break;
                    case "0":
                        return;
                    default:
                        Console.WriteLine("Невірний вибір, спробуйте знову.");
                        Console.ReadKey();
                        break;
                }
            }
        }

        // Підменю для завдань
        private void ShowTaskManagementMenu()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("Управління завданнями");
                Console.WriteLine("1. Додати завдання");
                Console.WriteLine("2. Видалити завдання");
                Console.WriteLine("3. Змінити дані завдання");
                Console.WriteLine("4. Переглянути список всіх завдань");
                Console.WriteLine("5. Переглянути виконані завдання");
                Console.WriteLine("6. Переглянути невиконані завдання");
                Console.WriteLine("0. Повернутись до головного меню");

                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        AddTask();
                        break;
                    case "2":
                        RemoveTask();
                        break;
                    case "3":
                        UpdateTask();
                        break;
                    case "4":
                        ViewTasks();
                        break;
                    case "5":
                        ViewCompletedTasks(true);
                        break;
                    case "6":
                        ViewCompletedTasks(false);
                        break;
                    case "0":
                        return;
                    default:
                        Console.WriteLine("Невірний вибір, спробуйте знову.");
                        Console.ReadKey();
                        break;
                }
            }
        }

        // Підменю для розподілу
        private void ShowAssignmentManagementMenu()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("Управління розподілом та виконанням завдань");
                Console.WriteLine("1. Розподілити завдання між виконавцями");
                Console.WriteLine("2. Вказати статус виконання");
                Console.WriteLine("3. Перевірити термін виконання");
                Console.WriteLine("4. Перевірити завантаженість виконавців");
                Console.WriteLine("5. Отримати стан виконання проекту");
                Console.WriteLine("0. Повернутись до головного меню");

                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        AssignTaskToMember();
                        break;
                    case "2":
                        UpdateTaskStatus();
                        break;
                    case "3":
                        CheckTaskDeadline();
                        break;
                    case "4":
                        CheckTeamLoad();
                        break;
                    case "5":
                        GetProjectStatus();
                        break;
                    case "0":
                        return;
                    default:
                        Console.WriteLine("Невірний вибір, спробуйте знову.");
                        Console.ReadKey();
                        break;
                }
            }
        }

        // Підменю для пошуку
        private void ShowSearchMenu()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("Пошук");
                Console.WriteLine("1. Пошук виконавця та його завдань");
                Console.WriteLine("2. Пошук виконавців певного завдання");
                Console.WriteLine("3. Пошук виконаних/невиконаних завдань");
                Console.WriteLine("0. Повернутись до головного меню");

                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        SearchMemberAndTasks();
                        break;
                    case "2":
                        SearchTaskExecutors();
                        break;
                    case "3":
                        SearchTaskByStatus();
                        break;
                    case "0":
                        return;
                    default:
                        Console.WriteLine("Невірний вибір, спробуйте знову.");
                        Console.ReadKey();
                        break;
                }
            }
        }

        // Додавання учасників команди
        private void AddTeamMember()
        {
            try
            {
                Console.Write("Введіть ім'я: ");
                string name = Console.ReadLine();

                if (string.IsNullOrWhiteSpace(name))
                {
                    throw new InvalidInputException("Ім'я не може бути порожнім.");
                }

                Console.Write("Введіть прізвище: ");
                string surname = Console.ReadLine();

                if (string.IsNullOrWhiteSpace(surname))
                    throw new InvalidInputException("Прізвище не може бути порожнім.");

                int id = GetValidatedId("Введіть ID (4 цифри): ", 4);

                if (teamManagement.IsIdExist(id))
                    throw new InvalidInputException("ID вже використовується.");

                teamManagement.AddTeamMember(name, surname, id);
                Console.WriteLine("Учасника успішно додано.");
            }
            catch (InvalidInputException ex)
            {
                Console.WriteLine($"Помилка введення: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Невідома помилка: {ex.Message}");
            }
        }

        // Видалення учасників
        private void RemoveTeamMember()
        {
            try
            {
                teamManagement.DisplayList();
                int id = GetValidatedId("Введіть ID члена команди, якого потрібно видалити (4 цифри): ", 4);
                TeamMember member = teamManagement.GetTeamMemberById(id);

                if (member == null)
                {
                    Console.WriteLine($"Виконавця для видалення з таким ID - {id}, не знайдено.");
                    Console.ReadKey();
                    return;
                }
                else
                {
                    teamManagement.RemoveTeamMember(id);
                    Console.WriteLine("Учасника успішно видалено.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Помилка: {ex.Message}");
            }
        }

        // Оновлення інформації про учасників
        private void UpdateTeamMember()
        {
            try
            {
                teamManagement.DisplayList();
                int id = GetValidatedId("Введіть ID члена команди для оновлення (4 цифри): ", 4);
                TeamMember member = teamManagement.GetTeamMemberById(id);

                if (member == null)
                {
                    Console.WriteLine($"Виконавця для оновлення з таким ID - {id}, не знайдено.");
                    Console.ReadKey();
                    return;
                }
                else
                {
                    Console.Write("Введіть нове ім'я: ");
                    string newName = Console.ReadLine();

                    if (string.IsNullOrWhiteSpace(newName))
                    {
                        throw new InvalidInputException("Ім'я не може бути порожнім.");
                    }

                    Console.Write("Введіть нове прізвище: ");
                    string newSurname = Console.ReadLine();

                    if (string.IsNullOrWhiteSpace(newSurname))
                    {
                        throw new InvalidInputException("Прізвище не може бути порожнім.");
                    }

                    teamManagement.UpdateTeamMember(id, newName, newSurname);
                    Console.WriteLine("Дані учасника успішно оновлено.");
                }
            }
            catch (InvalidInputException ex)
            {
                Console.WriteLine($"Помилка введення: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Невідома помилка: {ex.Message}");
            }
        }

        // Виведення всіх учасників
        private void ViewTeamMembers()
        {
            teamManagement.DisplayList();
            Console.ReadKey();
        }

        // Додавання завдання
        private void AddTask()
        {
            try
            {
                int id = GetValidatedId("Введіть ID завдання (3 цифри): ", 3);

                if (taskManagement.IsIdExist(id))
                {
                    throw new InvalidInputException("ID завдання вже використовується.");
                }

                Console.Write("Введіть назву завдання: ");
                string name = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(name))
                { 
                    throw new InvalidInputException("Назва завдання не може бути порожньою."); 
                }

                Console.Write("Введіть опис завдання: ");
                string description = Console.ReadLine();

                DateTime deadline;
                while (true)
                {
                    Console.Write("Введіть дедлайн завдання (у форматі рік-місяць-день): ");
                    string dateInput = Console.ReadLine();
                    if (DateTime.TryParseExact(dateInput, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out deadline))
                    {
                        break;
                    }
                    Console.WriteLine("Неправильний формат дати. Спробуйте ще раз.");
                }

                taskManagement.AddTask(id, name, description, deadline);
            }
            catch (InvalidInputException ex)
            {
                Console.WriteLine($"Помилка введення: {ex.Message}");
            }
            catch (FormatException)
            {
                Console.WriteLine("Неправильний формат дати. Спробуйте ще раз.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Невідома помилка: {ex.Message}");
            }
        }

        // Видалення завдання
        private void RemoveTask()
        {
            try
            {
                taskManagement.DisplayList();
                int id = GetValidatedId("Введіть ID завдання, яке потрібно видалити: ", 3);
                BLL.Task task = taskManagement.GetTaskById(id);

                if (task == null)
                {
                    Console.WriteLine($"Завдання для видалення з таким ID - {id}, не знайдено.");
                    Console.ReadKey();
                    return;
                }
                else { taskManagement.RemoveTask(id); }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Помилка: {ex.Message}");
            }
        }

        // Оновлення завдання
        private void UpdateTask()
        {
            try
            {
                taskManagement.DisplayList();
                int id = GetValidatedId("Введіть ID завдання для оновлення (3 цифри): ", 3);

                BLL.Task task = taskManagement.GetTaskById(id);

                if (task == null)
                {
                    Console.WriteLine($"Завдання для видалення з таким ID - {id}, не знайдено.");
                    Console.ReadKey();
                    return;
                }
                else
                {
                    Console.Write("Введіть нову назву завдання: ");
                    string newName = Console.ReadLine();
                    if (string.IsNullOrWhiteSpace(newName))
                    {
                        throw new InvalidInputException("Назва завдання не може бути порожньою.");
                    }

                    Console.Write("Введіть новий опис завдання: ");
                    string newDescription = Console.ReadLine();

                    DateTime newDeadline;
                    while (true)
                    {
                        Console.Write("Введіть новий дедлайн завдання (у форматі рік-місяць-день): ");
                        string dateInput = Console.ReadLine();
                        if (DateTime.TryParseExact(dateInput, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out newDeadline))
                        {
                            break;
                        }
                        Console.WriteLine("Неправильний формат дати. Спробуйте ще раз.");
                    }

                    taskManagement.UpdateTask(id, newName, newDescription, newDeadline);
                }
            }
            catch (InvalidInputException ex)
            {
                Console.WriteLine($"Помилка введення: {ex.Message}");
            }
            catch (FormatException)
            {
                Console.WriteLine("Неправильний формат дати. Спробуйте ще раз.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Невідома помилка: {ex.Message}");
            }
        }

        // Виведення списку завдань
        private void ViewTasks()
        {
            taskManagement.DisplayList();
            Console.ReadKey();
        }

        // Виведення списку виконаних або невиконаних завдань
        private void ViewCompletedTasks(bool isCompleted)
        {
            assignmentManagement.ViewCompletedTasks(isCompleted);
            Console.WriteLine("Натисніть будь-яку клавішу, щоб продовжити...");
            Console.ReadKey();
        }

        // Розподіл завдань учасникам
        private void AssignTaskToMember()
        {
            try
            {
                taskManagement.DisplayList();
                int taskId;
                BLL.Task task;
                do
                {
                    int id = GetValidatedId("Введіть ID завдання, яке потрібно присвоїти: ", 3);
                    taskId = id;
                    task = taskManagement.GetTaskById(taskId);

                    if (task == null)
                    {
                        throw new NotFoundException($"Завдання з таким ID - {taskId}, не знайдено.");
                    }
                } while (task == null);

                int memberId;
                TeamMember member;
                do
                {
                    teamManagement.DisplayList();
                    int id = GetValidatedId("Введіть ID виконавця, яке потрібно присвоїти: ", 4);
                    memberId = id;
                    member = teamManagement.GetTeamMemberById(memberId);

                    if (member == null)
                    {
                        throw new NotFoundException($"Виконавця з таким ID - {memberId}, не знайдено.");
                    }
                } while (member == null);

                assignmentManagement.AssignTaskToMember(taskId, memberId);
            }
            catch (NotFoundException ex)
            {
                Console.WriteLine($"Помилка пошуку: {ex.Message}");
            }
            catch (TaskAssignmentException ex)
            {
                Console.WriteLine($"Помилка призначення: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Невідома помилка: {ex.Message}");
            }
        }

        // Оновлення статусу виконання завдань
        private void UpdateTaskStatus()
        {
            try
            {
                taskManagement.DisplayList();
                int id = GetValidatedId("Введіть ID завдання: (3 цифри): ", 3);

                BLL.Task task = taskManagement.GetTaskById(id);

                if (task == null)
                {
                    Console.WriteLine($"Завдання для оновлення статусу з таким ID - {id}, не знайдено.");
                    Console.ReadKey();
                    return;
                }
                else
                {
                    Console.WriteLine("Чи виконане завдання? (введіть '1' для так або '0' для ні):");
                    string isCompleted = Console.ReadLine();
                    if (isCompleted == "1")
                        assignmentManagement.UpdateTaskStatus(id, true); 

                    else if (isCompleted == "0")
                        assignmentManagement.UpdateTaskStatus(id, false);

                    else
                        throw new InvalidInputException("Введено може бути '1' або '0'.");
                    Console.ReadKey();
                }
            }
            catch (InvalidInputException ex)
            {
                Console.WriteLine($"Помилка введення: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Помилка: {ex.Message}");
            }
        }

        // Перевірка строку завдання
        private void CheckTaskDeadline()
        {
            try
            {
                taskManagement.DisplayList();
                int id = GetValidatedId("Введіть ID завдання: (3 цифри): ", 3);

                BLL.Task task = taskManagement.GetTaskById(id);

                if (task == null)
                {
                    Console.WriteLine($"Завдання для перевірки з таким ID - {id}, не знайдено.");
                    Console.ReadKey();
                    return;
                }
                else
                {
                    assignmentManagement.CheckTaskDeadline(id);
                    Console.ReadKey();
                }
            }
            catch (InvalidInputException ex)
            {
                Console.WriteLine($"Помилка введення: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Помилка: {ex.Message}");
            }
        }

        // Перевірка завантаженості виконавця
        private void CheckTeamLoad()
        {
            try
            {
                teamManagement.DisplayList();
                int id = GetValidatedId("Введіть ID виконавця: (4 цифри): ", 4);
                TeamMember member = teamManagement.GetTeamMemberById(id);

                if (member == null)
                {
                    Console.WriteLine($"Виконавця з таким ID - {id}, не знайдено.");
                    Console.ReadKey();
                    return;
                }
                else
                {
                    assignmentManagement.CheckTeamLoad(id);
                    Console.ReadKey();
                }
            }
            catch (InvalidInputException ex)
            {
                Console.WriteLine($"Помилка введення: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Помилка: {ex.Message}");
            }
        }

        // Отримання стану виконання проекту
        private void GetProjectStatus()
        {
            try
            {
                assignmentManagement.GetProjectStatus();
                Console.ReadKey();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Помилка: {ex.Message}");
            }
        }

        // Пошук завдань прикріплених учаснику
        private void SearchMemberAndTasks()
        {
            try
            {
                teamManagement.DisplayList();
                int id = GetValidatedId("Введіть ID члена команди: (4 цифри): ", 4);

                TeamMember member = teamManagement.GetTeamMemberById(id);

                if (member == null)
                {
                    throw new NotFoundException($"Виконавця з таким ID - {id}, не знайдено.");
                }

                search.SearchMemberAndTasksById(id);
                Console.Read();
            }
            catch (NotFoundException ex)
            {
                Console.WriteLine($"Помилка: {ex.Message}");
            }
            catch (InvalidInputException ex)
            {
                Console.WriteLine($"Помилка введення: {ex.Message}");
            }
            catch (OperationFailedException ex)
            {
                Console.WriteLine($"Помилка операції: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Непередбачена помилка: {ex.Message}");
            }
        }

        // Пошук учасників прикріплених до завдання
        private void SearchTaskExecutors()
        {
            try
            {
                taskManagement.DisplayList();
                int id = GetValidatedId("Введіть ID завдання: (3 цифри): ", 3);

                BLL.Task task = taskManagement.GetTaskById(id);

                if (task == null)
                {
                    throw new NotFoundException($"Завдання для пошуку з таким ID - {id}, не знайдено.");
                }

                search.SearchTaskExecutorsById(id);
                Console.Read();
            }
            catch (NotFoundException ex)
            {
                Console.WriteLine($"Помилка: {ex.Message}");
            }
            catch (InvalidInputException ex)
            {
                Console.WriteLine($"Помилка введення: {ex.Message}");
            }
            catch (OperationFailedException ex)
            {
                Console.WriteLine($"Помилка операції: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Непередбачена помилка: {ex.Message}");
            }
        }
        
        // Пошук всіх виконаних або невиконаних завдань
        private void SearchTaskByStatus()
        {
            try
            {
                Console.Write("Введіть статус (виконано/невиконано): ");
                string statusInput = Console.ReadLine().ToLower();
                bool isCompleted;

                if (statusInput == "виконано")
                {
                    isCompleted = true;
                }
                else if (statusInput == "невиконано")
                {
                    isCompleted = false;
                }
                else
                {
                    throw new InvalidInputException("Неправильний ввід. Введіть 'виконано' або 'невиконано'.");
                }

                search.SearchTasksByStatus(isCompleted);
                Console.Read();
            }
            catch (InvalidInputException ex)
            {
                Console.WriteLine($"Помилка введення: {ex.Message}");
            }
            catch (OperationFailedException ex)
            {
                Console.WriteLine($"Помилка операції: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Непередбачена помилка: {ex.Message}");
            }
        }

        // Перевірка правильності ID
        private int GetValidatedId(string prompt, int length)
        {
            while (true)
            {
                try
                {
                    Console.Write(prompt);
                    string input = Console.ReadLine();

                    if (!int.TryParse(input, out int id) || input.Length != length)
                    {
                        throw new InvalidInputException($"ID повинен містити тільки цифри і бути довжиною {length}");
                    }

                    return id;
                }
                catch (InvalidInputException ex)
                {
                    Console.WriteLine($"Помилка введення: {ex.Message}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Непередбачена помилка: {ex.Message}");
                }
            }
        }
    }
}
