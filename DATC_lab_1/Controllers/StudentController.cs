using Microsoft.AspNetCore.Mvc;

namespace DATC_lab_1.Controllers;

[ApiController]
[Route("student")]
public class StudentController : ControllerBase
{
    StudentRepo DataBase = new StudentRepo(
        "datcstudents",
        "rq4rGIjkZx4URNsS1RxwDsRFArB8GYbfpnGEqee9C6add+5sBxQMgbfy/SsKucYGRmOHSw2N2y8u+AStPBIOCA==",
        "laborator3"
    );

    [HttpGet()]
    public IActionResult Get()
    {
        List<Student> studentList = new List<Student>();
        Task.Run(async() => {studentList = await DataBase.GetAllStudents();}).GetAwaiter().GetResult();
        return Ok(studentList);
    }

    [HttpGet("{id}")]
    public IActionResult Get(int id)
    {
        List<Student> studentList = new List<Student>();
        Task.Run(async() => {studentList = await DataBase.GetAllStudents();}).GetAwaiter().GetResult();

        foreach (Student x in studentList)
        {
            if (x.Id == id)
                return Ok(x);
            else
                return Ok("The student with the given id cannot be found");
        }

        return Ok("List Empty");
    }

    [HttpPost("PostStudent")]
    public async Task<string> Post(Student student)
    {
        await DataBase.InsertStudent(student);
        return "Post Succesful";
    }
}
