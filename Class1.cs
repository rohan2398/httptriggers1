using System;
using System.Collections.Generic;
using System.Text;

namespace EmployeeFunction
{
    public class EmployeeEntity
    {
        public string employeeId
        {
            get; set;
        } = Guid.NewGuid().ToString("n");
        public string employeeName
        {
            get; set;
        }
        public string employeeSalary
        {
            get; set;
        }

        public string location
        {
            get; set;
        }
        public string address
        {
            get; set;
        }

        public string pincode
        {
            get; set;
        }

        public DateTime date
        {
            get; set;
        }
    }

    public class EmployeesEntity
    {
        public string id
        {
            get; set;
        } = Guid.NewGuid().ToString("n");
        public string employeeName
        {
            get; set;
        }
        public string employeeSalary
        {
            get; set;
        }
        public string location
        {
            get; set;
        }
        public string address
        {
            get; set;
        }

        public string pincode
        {
            get; set;
        }

        public DateTime date
        {
            get; set;
        }
    }
}
