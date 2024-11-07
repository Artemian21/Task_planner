using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    // Інтерфейс для сереалізації
    internal interface ISerialization
    {
        void Save();
        void Load();
    }
}
