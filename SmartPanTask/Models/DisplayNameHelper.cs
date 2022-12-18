using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SmartPanTask.Models
{
    [MetadataType(typeof(DepartmentMetaData))]
    public partial class Department
    {
    }
    public class DepartmentMetaData
    {
        [DisplayName("Department Name")]
        [Required(ErrorMessage = "Please enter the Department name")]
        public string DepartmentName { get; set; }
    }


    [MetadataType(typeof(EmployeeTaskMetaData))]
    public partial class EmployeeTask
    {

    }
    public class EmployeeTaskMetaData
    {
        [DisplayName("Task Title")]
        [Required(ErrorMessage = "Please enter the Task Title")]
        public string TaskTitle { get; set; }
        [DisplayName("Task Description")]
        public string TaskDescription { get; set; }
        [DisplayName("Employee Name")]
        public Nullable<int> EmployeeID { get; set; }
        [DisplayName("Date Assigned")]
        public Nullable<System.DateTime> DateAssigned { get; set; }
        [DisplayName("Task Status")]
        public string TaskStatus { get; set; }
        [DisplayName("Date Updated")]
        public Nullable<System.DateTime> DateUpdated { get; set; }
        [DisplayName("Date Started")]
        public Nullable<System.DateTime> DateStarted { get; set; }
    }



    [MetadataType(typeof(EmployeeMetaData))]
    public partial class Employee
    {
    }
     public class EmployeeMetaData
    {
        [DisplayName( "First Name")]
        [Required(ErrorMessage = "Please enter your first name")]
        public string FirstName { get; set; }
        [DisplayName("Last Name")]
        [Required(ErrorMessage = "Please enter your Last name")]
        public string LastName { get; set; }
        [Required(ErrorMessage = "Please enter your salary")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:c}")]
        public Nullable<decimal> Salary { get; set; }
        [DisplayName("Profile Photo")]
        [Required(ErrorMessage = "Please Load an image")]
        public string Image { get; set; }
        [DisplayName("Manager Name")]
        public Nullable<int> ManagerID { get; set; }
    }

}