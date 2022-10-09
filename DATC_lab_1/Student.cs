namespace DATC_lab_1;

public class Student
{
    public string name 
    { get; set; }

    public int year 
    { get; set; }

    public string faculty
    { get; set; }

    public int id 
    { get; set; }

    public Student(string name, int year, string faculty, int id)
    {
        this.name = name;
        this.year = year;
        this.faculty = faculty;
        this.id = id;
    }

}


