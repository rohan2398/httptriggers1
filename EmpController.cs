using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Localdatabase.Models;
using System.Data;
using System.Data.SqlClient;
using System.ComponentModel.DataAnnotations;
using Localdatabase.Repository;
using PagedList.Mvc;
using PagedList;
using System.Configuration;
using System.Collections;



namespace Localdatabase.Controllers
{   
    public class EmpController : Controller
    {
        private SqlConnection con;
        //To Handle connection related activities    
        private void connection()
        {
            string constr = "Server = DESKTOP-KIMOTC0; Database = localdb; Integrated Security = True";
            con = new SqlConnection(constr);

        }

        // GET: Employee/GetAllEmpDetails        
        public ActionResult Getallempdetails()
        {

            Emprep EmpRepo = new Emprep();
            ModelState.Clear();
            return View(EmpRepo.Getallempdetails());
        }
        // GET: Employee/first 5 employees
        public ActionResult Getfiveemp()
        {
            Emprep EmpRepo = new Emprep();
            ModelState.Clear();
            return View(EmpRepo.Getfiveemp());
        }
        // GET: Employee/ second 5 employees
        public ActionResult Getsecondfiveemp()
        {
            Emprep EmpRepo = new Emprep();
            ModelState.Clear();
            return View(EmpRepo.Getsecondfiveemp());
        }
        // GET: Employee/last 5 employees
        public ActionResult Getlastfiveemp()
        {
            Emprep EmpRepo = new Emprep();
            ModelState.Clear();
            return View(EmpRepo.Getlastfiveemp());
        }
        // GET: Employees/Ascending
        public ActionResult Ascending()
        {
            Emprep EmpRepo = new Emprep();
            ModelState.Clear();
            return View(EmpRepo.Ascending());
        }
        // GET: Employees/Descending
        public ActionResult Descending()
        {
            Emprep EmpRepo = new Emprep();
            ModelState.Clear();
            return View(EmpRepo.Descending());
        }

        // GET: Employee/AddEmployee    
        public ActionResult AddEmployee()
        {
            return View();
        }
        // Search box
        public ActionResult Searching(string Name)
        {
            Emprep EmpRepo = new Emprep();
            ModelState.Clear();
            return View(EmpRepo.Searching(Name));
              
        }

       

        // POST: Employee/AddEmployee    
        [HttpPost]
        public ActionResult AddEmployee(Empmodel Emp)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    Emprep EmpRepo = new Emprep();

                    if (EmpRepo.AddEmployee(Emp))
                    {
                        ViewBag.Message = "Employee details added successfully";
                    }
                }

                return View();
            }
            catch (Exception ex)
            {
                return View();
            }
        }

        // GET: Employee/EditEmpDetails/5    
        public ActionResult EditEmpDetails(int id)
        {
            Emprep EmpRepo = new Emprep();



            return View(EmpRepo.Getallempdetails().Find(Emp => Emp.Empid == id));

        }

        // POST: Employee/UpdateEmpDetails/5    
        [HttpPut]

        public ActionResult UpdateEmpDetails(int id, Empmodel obj)
        {
            try
            {   
                Emprep EmpRepo = new Emprep();

                EmpRepo.UpdateEmployee(obj);




                return RedirectToAction("Getallempdetails");
            }
            catch(Exception ex)
            {
                
                return View();
            }
        }

        // GET: Employee/DeleteEmp/5    
        public ActionResult DeleteEmp(int id)
        {
            try
            {
                Emprep EmpRepo = new Emprep();
                if (EmpRepo.DeleteEmployee(id))
                {   
                    ViewBag.AlertMsg = "Employee details deleted successfully";

                }
                return RedirectToAction("Getallempdetails");

            }
            catch(Exception ex)
            {
                return View();
            }
        }
       




    }
}
    
