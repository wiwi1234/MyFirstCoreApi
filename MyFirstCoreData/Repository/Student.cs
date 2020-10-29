using System;
using System.Collections.Generic;

namespace MyFirstCoreData.Models
{
    public partial class Student
    {
        public int Id { get; set; }
        public int StudentNo { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string City { get; set; }
    }
}
