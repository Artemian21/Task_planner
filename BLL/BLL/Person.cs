using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    // Абстрактний клас для просто особи
    public abstract class Person
    {
        public string Name { get; set; }
        public string Surname { get; set; }

        protected Person(string name, string surname)
        {
            Name = name;
            Surname = surname;
        }

        public virtual void DisplayInfo()
        {
            Console.WriteLine($"Ім'я: {Name}, Прізвище: {Surname}");
        }
    }
}
