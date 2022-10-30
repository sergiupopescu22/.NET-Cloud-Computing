using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;

namespace DATC_lab_1;

public class Student: TableEntity
{
    public string Name 
    { get; set; }

    public int Year 
    { get; set; }

    public string Faculty
    { get; set; }

    public int Id 
    { get; set; }

    public Student()
    {
        this.PartitionKey = "-";
        this.RowKey = "-";
        this.Name = "-";
        this.Id = 0;
        this.Faculty = "-";
        this.Year = 0;
    }

    public Student(string partitionKey, string rowKey, string name, int year, string faculty, int id)
    {
        this.PartitionKey = partitionKey;
        this.RowKey = rowKey;
        this.Name = name;
        this.Year = year;
        this.Faculty = faculty;
        this.Id = id;
    }

}


