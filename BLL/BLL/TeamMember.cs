using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    [Serializable]
    public class TeamMember : Person
    {
        public int ID { get; set; }

        public TeamMember(string name, string surname, int id) : base(name, surname)
        {
            ID = id;
        }

        // перевизначений метод для виводу рядка з інформацією про учасника
        public override void DisplayInfo()
        {
            Console.Write($"ID: {ID}, ");
            base.DisplayInfo();
        }
     }
}
