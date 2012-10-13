using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics;

namespace ChinookDatabase.DataModel
{
    [DebuggerDisplay("{FirstName} {LastName} (EmployeeId = {EmployeeId})")]
    public class Employee
    {
        [Key]
        public int EmployeeId { get; set; }

        [Required, MaxLength(20)]
        public string LastName { get; set; }

        [Required, MaxLength(20)]
        public string FirstName { get; set; }

        [MaxLength(30)]
        public string Title { get; set; }

        public int ReportsTo { get; set; }

        public DateTime BirthDate { get; set; }

        public DateTime HireDate { get; set; }

        [MaxLength(70)]
        public string Address { get; set; }

        [MaxLength(40)]
        public string City { get; set; }

        [MaxLength(40)]
        public string State { get; set; }

        [MaxLength(40)]
        public string Country { get; set; }

        [MaxLength(10)]
        public string PostalCode { get; set; }

        [MaxLength(24)]
        public string Phone { get; set; }

        [MaxLength(24)]
        public string Fax { get; set; }

        [MaxLength(60)]
        public string Email { get; set; }

        [ForeignKey("ReportsTo")]
        public Employee ReportsToManager { get; set; }
    }
}
