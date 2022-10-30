namespace DATC_lab_1;

public class StudentRepo
{
    public List<Student> StudentList = new List<Student>();
    
    public StudentRepo()
    {
        this.StudentList.Add(new Student("Popescu", 4, "UPT AC IS", 99));
        this.StudentList.Add(new Student("Ionescu", 3, "UPT AC CTI", 82));
    }

}
