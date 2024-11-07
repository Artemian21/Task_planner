using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    // Ексепшн для специфічних помилок програми
    public class CustomException : Exception
    {
        public CustomException(string message) : base(message) { }
    }

    // Ексепшн про неправильний ввід
    public class InvalidInputException : CustomException
    {
        public InvalidInputException(string message) : base(message)
        {
            Console.WriteLine(message);
            Console.ReadKey();
        }
    }

    // Ексепшн про незнайденість
    public class NotFoundException : CustomException
    {
        public NotFoundException(string message) : base(message)
        {
            Console.WriteLine(message);
            Console.ReadKey();
        }
    }

    // Ексепшн для помилки призначення 
    public class TaskAssignmentException : CustomException
    {
        public TaskAssignmentException(string message) : base(message)
        {
            Console.WriteLine("Помилка при призначенні завдання: " + message);
            Console.ReadKey();
        }
    }

    // Ексепшн про виконання операції
    public class OperationFailedException : CustomException
    {
        public OperationFailedException(string message) : base(message)
        {
            Console.WriteLine(message);
            Console.ReadKey();
        }
    }
}
