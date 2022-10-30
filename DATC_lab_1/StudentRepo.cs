using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;

namespace DATC_lab_1;

public class StudentRepo: AzureTable
{
    public List<Student> StudentList = new List<Student>();
    
    public StudentRepo(string storageAccount, string storageKey, string tableName): 
        base(storageAccount, storageKey, tableName)
    {
        Console.WriteLine("A connection to the table named: " + tableName + " has been established\n");
    }

    public async Task<List<Student>> GetAllStudents()
    {
        CloudTable table = await GetTable();
        TableQuery<Student> query = new TableQuery<Student>();
        List<Student> studentList = new List<Student>();

        TableContinuationToken continuationToken = null;

        do{
            TableQuerySegment<Student> queryResults = 
                await table.ExecuteQuerySegmentedAsync(query, continuationToken);
            continuationToken = queryResults.ContinuationToken;
            studentList.AddRange(queryResults.Results);
        } while(continuationToken != null);

        return studentList;
    }

    public async Task InsertStudent(Student student)
    {
        CloudTable table = await GetTable();
        TableOperation insert_operation = TableOperation.Insert(student);
        await table.ExecuteAsync(insert_operation);
    }

}
