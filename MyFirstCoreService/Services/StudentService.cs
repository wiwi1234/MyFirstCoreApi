using MyFirstCoreData.Models;
using MyFirstCoreData.Repository;
using MyFirstCoreService.Interface;

namespace MyFirstCoreService
{
    public class StudentService : IStudentService
    {
        private readonly Repository<Student> _studentRepo;
        public StudentService(
            Repository<Student> studentRepo
        )
        {
            this._studentRepo = studentRepo;
        }

        public Student GetSingleStudent(int id)
        {
            return _studentRepo.GetSingle(s => s.Id == id);
        }
    }
}
