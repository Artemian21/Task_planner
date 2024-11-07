using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public class Assignment
    {
        public Task Task { get; set; }
        public TeamMember AssignedMember { get; set; }

        public Assignment() { }

        public Assignment(Task task, TeamMember member)
        {
            Task = task;
            AssignedMember = member;
            
        }
    }

}
