using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
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
        [ApiExplorerSettings(GroupName = "v1")]  //Version
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
