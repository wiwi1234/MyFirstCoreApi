using System;
using System.Collections.Generic;

namespace MyFirstCoreData.Models
{
    public partial class StudentLessonMapping
    {
        public int Id { get; set; }
        public int StudentId { get; set; }
        public int LessonId { get; set; }
        public int Period { get; set; }
    }
}
