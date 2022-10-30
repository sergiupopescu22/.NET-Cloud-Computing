using Microsoft.AspNetCore.Mvc;

namespace DATC_lab_1.Controllers;

[ApiController]
[Route("student")]
public class StudentController : ControllerBase
{
     private StudentRepo DataBase;

    public StudentController()
    {
        DataBase = new StudentRepo();
    }

    [HttpGet()]
    public IActionResult Get()
    {
        return Ok(DataBase.StudentList);
    }

    [HttpGet("{id}")]
    public IActionResult Get(int id)
    {
        foreach (Student x in DataBase.StudentList)
        {
            if (x.id == id)
                return Ok(x);
            else
                return Ok("The student with the given id cannot be found");
        }

        return Ok("List Empty");
    }
}
