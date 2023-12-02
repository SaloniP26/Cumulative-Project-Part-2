using Microsoft.Ajax.Utilities;
using MySql.Data.MySqlClient;
using Cumulative_Project_Part_2.Models;
using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Linq.Expressions;
using System.Security.Claims;
using System.Xml.Linq;
using Microsoft.AspNetCore.Cors;

namespace Cumulative_Project_Part_2.Controllers
{
    public class TeacherDataController : ApiController
    {
        private SchoolDbContext School = new SchoolDbContext();

        // Lists teachers from database

        [HttpGet]
        [Route("api/TeacherData/ListTeachers/{SearchKey?}")]

        public IEnumerable<Teacher> ListTeachers(string SearchKey = null)
        {
            //Create a connection
            MySqlConnection Conn = School.AccessDatabase();

            //Open the connection
            Conn.Open();

            //Command for our database
            MySqlCommand cmd = Conn.CreateCommand();

            //SQL QUERY
            cmd.CommandText = "SELECT * FROM Teachers where lower(teacherfname) like lower(@key) or lower(teacherlname) like lower(@key) or lower(concat(teacherfname, ' ', teacherlname)) like lower(@key);";

            cmd.Parameters.AddWithValue("@key", "%" + SearchKey + "%");
            cmd.Prepare();

            MySqlDataReader ResultSet = cmd.ExecuteReader();

            List<Teacher> Teachers = new List<Teacher> {};

            while (ResultSet.Read())
            {
                {
                    int Id = Convert.ToInt32(ResultSet["teacherid"]);
                    string Name = ResultSet["teacherfname"].ToString();
                    string LastName = ResultSet["teacherlname"].ToString();
                    string EmpNum = ResultSet["employeenumber"].ToString();
                    DateTime HireDate = Convert.ToDateTime(ResultSet["hiredate"]);
                    Decimal Salary = Convert.ToDecimal(ResultSet["salary"]);

                    Teacher NewTeacher = new Teacher();
                    NewTeacher.Id = Id;
                    NewTeacher.Name = Name;
                    NewTeacher.LastName = LastName;
                    NewTeacher.EmpNum = EmpNum;
                    NewTeacher.HireDate = HireDate;
                    NewTeacher.Salary = Salary;

                    Teachers.Add(NewTeacher);
                }
            }

            Conn.Close();

            return Teachers;
        }


        // Find teacher from database

        [HttpGet]
        public Teacher Findteacher(int id)
        {
            Teacher NewTeacher = new Teacher();

            //Create a connection

            MySqlConnection Conn = School.AccessDatabase();

            //Open the connection

            Conn.Open();

            //Command for our database
            MySqlCommand cmd = Conn.CreateCommand();

            //SQL QUERY
            cmd.CommandText = "Select * from Teachers where teacherid = @id";
            cmd.Parameters.AddWithValue("@id", id);
            cmd.Prepare();

            MySqlDataReader ResultSet = cmd.ExecuteReader();

            while (ResultSet.Read())
            {
                int Id = Convert.ToInt32(ResultSet["teacherid"]);
                string Name = ResultSet["teacherfname"].ToString();
                string LastName = ResultSet["teacherlname"].ToString();
                string EmpNum = ResultSet["employeenumber"].ToString();
                DateTime HireDate = Convert.ToDateTime(ResultSet["hiredate"]);
                Decimal Salary = Convert.ToDecimal(ResultSet["salary"]);

                NewTeacher.Id = Id;
                NewTeacher.Name = Name;
                NewTeacher.LastName = LastName;
                NewTeacher.EmpNum = EmpNum;
                NewTeacher.HireDate = HireDate;
                NewTeacher.Salary = Salary;
            }
            Conn.Close();

            return NewTeacher;
        }


        // Deletes teacher from the Database
        
        [HttpPost]
        public void DeleteTeacher(int id)
        {
            //Create a connection
            MySqlConnection Conn = School.AccessDatabase();

            //Open the connection
            Conn.Open();

            //Command for our database
            MySqlCommand cmd = Conn.CreateCommand();

            //SQL QUERY
            cmd.CommandText = "Delete from Teachers where teacherid = @id";
            cmd.Parameters.AddWithValue("@id", id);
            cmd.Prepare();

            cmd.ExecuteNonQuery();

            Conn.Close();

        }

        
        
        // Adds teacher to Database.
        
        [HttpPost]
        public void AddTeacher([FromBody] Teacher NewTeacher)
        {
            //Create a connection
            MySqlConnection Conn = School.AccessDatabase();

            Debug.WriteLine(NewTeacher.Name);

            //Open the connection
            Conn.Open();

            //Command for our database
            MySqlCommand cmd = Conn.CreateCommand();

            //SQL QUERY
            cmd.CommandText = "insert into Teachers (teacherfname, teacherlname, employeenumber, hiredate, salary) values (@Name,@LastName,@EmpNum,@HireDate, @Salary)";
            cmd.Parameters.AddWithValue("@Name", NewTeacher.Name);
            cmd.Parameters.AddWithValue("@LastName", NewTeacher.LastName);
            cmd.Parameters.AddWithValue("@EmpNum", NewTeacher.EmpNum);
            cmd.Parameters.AddWithValue("@HireDate", NewTeacher.HireDate);
            cmd.Parameters.AddWithValue("@Salary", NewTeacher.Salary);
            cmd.Prepare();

            cmd.ExecuteNonQuery();

            Conn.Close();


        }
    }
}