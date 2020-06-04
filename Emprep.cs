using Localdatabase.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Collections;


namespace Localdatabase.Repository
{
    public class Emprep

    {

        private SqlConnection con;
        //To Handle connection related activities    
        private void connection()
        {
            string constr = "Server = DESKTOP-KIMOTC0; Database = localdb; Integrated Security = True";
            con = new SqlConnection(constr);

        }
        //To Add Employee details    
        public bool AddEmployee(Empmodel obj)
        {


            connection();
            SqlCommand com = new SqlCommand("insert into Employeeform values(@Empid,@Name,@Branch,@Mobilenumber,@Zip)", con);

            com.Parameters.AddWithValue("@Empid", obj.Empid);
            com.Parameters.AddWithValue("@Name", obj.Name);
            com.Parameters.AddWithValue("@Branch", obj.Branch);
            com.Parameters.AddWithValue("@Mobilenumber", obj.Mobilenumber);
            com.Parameters.AddWithValue("@Zip", obj.Zip);

            con.Open();
            int i = com.ExecuteNonQuery();
            con.Close();
            if (i >= 1)
            {

                return true;

            }
            else
            {

                return false;
            }


        }
        //To view employee details with generic list     
        public List<Empmodel> Getallempdetails()
        {
            connection();
            List<Empmodel> EmpList = new List<Empmodel>();


            SqlCommand com = new SqlCommand("select * from Employeeform", con);

            SqlDataAdapter da = new SqlDataAdapter(com);
            DataTable dt = new DataTable();

            con.Open();
            da.Fill(dt);
            con.Close();
            //Bind EmpModel generic list using dataRow     
            foreach (DataRow dr in dt.Rows)
            {

                EmpList.Add(

                    new Empmodel
                    {

                        Empid = Convert.ToInt32(dr["Employee id"]),
                        Name = Convert.ToString(dr["Employee Name"]),
                        Branch = Convert.ToString(dr["Branch"]),
                        Mobilenumber = Convert.ToString(dr["Mobilenumber"]),
                        Zip = Convert.ToString(dr["Zip"])

                    }
                    );


            }

            return EmpList;


        }

        //To search employee details  
        public List<Empmodel> Searching(string Name)
        {
            Name = Name ?? "";
            connection();
            List<Empmodel> EmpList = new List<Empmodel>();


            SqlCommand com = new SqlCommand(" select [Employee Name] from Employeeform  where [Employee Name] like '%@Name%';", con);
            com.Parameters.AddWithValue("@Name", Name);

            SqlDataAdapter da = new SqlDataAdapter(com);
            DataTable dt = new DataTable();
            

            con.Open();
            da.Fill(dt);
            con.Close();
            //Bind EmpModel generic list using dataRow     
            foreach (DataRow dr in dt.Rows)
            {

                EmpList.Add(

                    new Empmodel
                    {

                        Empid = Convert.ToInt32(dr["Employee id"]),
                        Name = Convert.ToString(dr["Employee Name"]),
                        Branch = Convert.ToString(dr["Branch"]),
                        Mobilenumber = Convert.ToString(dr["Mobilenumber"]),
                        Zip = Convert.ToString(dr["Zip"])

                    }
                    );


            }

            return EmpList;


        }
        // To view details of first five employees
        public List<Empmodel> Getfiveemp()
        {
            connection();
            List<Empmodel> EmpList = new List<Empmodel>();


            SqlCommand com = new SqlCommand("select top 5 * from Employeeform", con);

            SqlDataAdapter da = new SqlDataAdapter(com);
            DataTable dt = new DataTable();

            con.Open();
            da.Fill(dt);
            con.Close();
            //Bind EmpModel generic list using dataRow     
            foreach (DataRow dr in dt.Rows)
            {

                EmpList.Add(

                    new Empmodel
                    {

                        Empid = Convert.ToInt32(dr["Employee id"]),
                        Name = Convert.ToString(dr["Employee Name"]),
                        Branch = Convert.ToString(dr["Branch"]),
                        Mobilenumber = Convert.ToString(dr["Mobilenumber"]),
                        Zip = Convert.ToString(dr["Zip"])

                    }
                    );


            }

            return EmpList;
        }
        // To view details of second five employees
        public List<Empmodel> Getsecondfiveemp()
        {
            connection();
            List<Empmodel> EmpList = new List<Empmodel>();


            SqlCommand com = new SqlCommand("select * from Employeeform where [Employee id]>122 and [Employee id]<129;", con);

            SqlDataAdapter da = new SqlDataAdapter(com);
            DataTable dt = new DataTable();

            con.Open();
            da.Fill(dt);
            con.Close();
            //Bind EmpModel generic list using dataRow     
            foreach (DataRow dr in dt.Rows)
            {

                EmpList.Add(

                    new Empmodel
                    {

                        Empid = Convert.ToInt32(dr["Employee id"]),
                        Name = Convert.ToString(dr["Employee Name"]),
                        Branch = Convert.ToString(dr["Branch"]),
                        Mobilenumber = Convert.ToString(dr["Mobilenumber"]),
                        Zip = Convert.ToString(dr["Zip"])

                    }
                    );


            }

            return EmpList;
        }
        // To view details of last five employees
        public List<Empmodel> Getlastfiveemp()
        {
            connection();
            List<Empmodel> EmpList = new List<Empmodel>();


            SqlCommand com = new SqlCommand("select * from Employeeform where [Employee id]>128 and [Employee id]<=133;", con);

            SqlDataAdapter da = new SqlDataAdapter(com);
            DataTable dt = new DataTable();

            con.Open();
            da.Fill(dt);
            con.Close();
            //Bind EmpModel generic list using dataRow     
            foreach (DataRow dr in dt.Rows)
            {

                EmpList.Add(

                    new Empmodel
                    {

                        Empid = Convert.ToInt32(dr["Employee id"]),
                        Name = Convert.ToString(dr["Employee Name"]),
                        Branch = Convert.ToString(dr["Branch"]),
                        Mobilenumber = Convert.ToString(dr["Mobilenumber"]),
                        Zip = Convert.ToString(dr["Zip"])

                    }
                    );


            }

            return EmpList;
        }

        //To view all Employees in Ascending order   
        public List<Empmodel> Ascending()
        {
            connection();
            List<Empmodel> EmpList = new List<Empmodel>();


            SqlCommand com = new SqlCommand("select * from Employeeform order by [Employee Name]", con);

            SqlDataAdapter da = new SqlDataAdapter(com);
            DataTable dt = new DataTable();

            con.Open();
            da.Fill(dt);
            con.Close();
            //Bind EmpModel generic list using dataRow    
            foreach (DataRow dr in dt.Rows)
            {

                EmpList.Add(

                    new Empmodel
                    {

                        Empid = Convert.ToInt32(dr["Employee id"]),
                        Name = Convert.ToString(dr["Employee Name"]),
                        Branch = Convert.ToString(dr["Branch"]),
                        Mobilenumber = Convert.ToString(dr["Mobilenumber"]),
                        Zip = Convert.ToString(dr["Zip"])

                    }
                    );


            }

            return EmpList;


        }
        //To view all Employees in Descending order   
        public List<Empmodel> Descending()
        {
            connection();
            List<Empmodel> EmpList = new List<Empmodel>();


            SqlCommand com = new SqlCommand("select * from Employeeform order by [Employee Name]desc", con);

            SqlDataAdapter da = new SqlDataAdapter(com);
            DataTable dt = new DataTable();

            con.Open();
            da.Fill(dt);
            con.Close();
            //Bind EmpModel generic list using dataRow     
            foreach (DataRow dr in dt.Rows)
            {

                EmpList.Add(

                    new Empmodel
                    {

                        Empid = Convert.ToInt32(dr["Employee id"]),
                        Name = Convert.ToString(dr["Employee Name"]),
                        Branch = Convert.ToString(dr["Branch"]),
                        Mobilenumber = Convert.ToString(dr["Mobilenumber"]),
                        Zip = Convert.ToString(dr["Zip"])

                    }
                    );


            }

            return EmpList;


        }


        //To Update Employee details    
        public bool UpdateEmployee(Empmodel obj)
        {

            connection();
            SqlCommand com = new SqlCommand("Update Employeeform set Name=@Name,Branch=@Branch,Mobilenumber=@Mobilenumber,Zip=@Zip where Employee id=@EmpId", con);

            com.CommandType = CommandType.Text;
            com.Parameters.AddWithValue("@EmpId", obj.Empid);
            com.Parameters.AddWithValue("@Name", obj.Name);
            com.Parameters.AddWithValue("@Branch", obj.Branch);
            com.Parameters.AddWithValue("@Mobilenumber", obj.Mobilenumber);
            com.Parameters.AddWithValue("@Zip", obj.Zip);
            con.Open();
            int i = com.ExecuteNonQuery();
            con.Close();
            if (i >= 1)
            {

                return true;

            }
            else
            {

                return false;
            }


        }
        //To delete Employee details    
        public bool DeleteEmployee(int Id)
        {

            connection();
            SqlCommand com = new SqlCommand("Delete from Employeeform where [Employee Id]=@EmpId", con);


            com.Parameters.AddWithValue("@EmpId", Id);

            con.Open();
            int i = com.ExecuteNonQuery();
            con.Close();
            if (i >= 1)
            {

                return true;

            }
            else
            {

                return false;
            }


        }
    }
}
