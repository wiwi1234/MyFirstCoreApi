using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyFirstCoreData.Models;
using MyFirstCoreService.Interface;

namespace MyFirstCoreApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentController : ControllerBase
    {
        private IStudentService _studentService;
        public StudentController(IStudentService stdservice)
        {
            _studentService = stdservice;
        }

        [HttpGet]
        [Authorize]
        [ApiExplorerSettings(GroupName = "v1")]
        [Route("{id}")]
        public ActionResult<Student> GetSingle(int id)
        {
            Student s = _studentService.GetSingleStudent(id);
            if(s == null)
            {
                return NotFound();
            }
            else
            {
                return Ok(s);
            }
        }
    }
}
