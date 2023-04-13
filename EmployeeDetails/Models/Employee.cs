using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeDetails.Models
{
    public class Employee
    {
        public static IEnumerable<Employee> ItemsSource { get; internal set; }
        public int id { get; set; }

        public string name { get; set; }

        public string status { get; set; }

        public string gender { get; set; }

        public string email { get; set; }
    }
}
