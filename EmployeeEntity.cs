using System;
using System.Collections.Generic;
using System.Text;

namespace EmployeeFunction
{
    public class EmployeeEntity
    {
        public string EmployeeId
        {
            get; set;
        } = Guid.NewGuid().ToString("n");
        public string EmployeeName
        {
            get; set;
        }
        public double EmployeeSalary
        {
            get; set;
        }
    }
}
