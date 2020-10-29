using MyFirstCoreData.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyFirstCoreService.Interface
{
    public interface IStudentService
    {
        Student GetSingleStudent(int id);
    }
}
