using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using Microsoft.WindowsAzure.Storage.Auth;

namespace laborator4
{

    class StudentBluePrint: TableEntity
    {
        public string Name {get; set;}
        public int Age {get; set;}
        public string Faculty {get; set;}
        public int Year {get; set;}

        public StudentBluePrint()
        {
            this.PartitionKey = "-";
            this.RowKey = "-";
            this.Name = "-";
            this.Age = 0;
            this.Faculty = "-";
            this.Year = 0;
        }
        public StudentBluePrint(string partitionKey, string rowKey, string Name , int Age , string Faculty , int Year )
        {
            this.PartitionKey = partitionKey;
            this.RowKey = rowKey;
            this.Name = Name;
            this.Age = Age;
            this.Faculty = Faculty;
            this.Year = Year;
        }
    }
    class StudentMetricsBluePrint: TableEntity
    {
        public int NumberOfStudents {get; set;}

        public StudentMetricsBluePrint()
        {
            this.PartitionKey = "";
            this.RowKey = "";
            this.NumberOfStudents = 0;
        }
        public StudentMetricsBluePrint(string partitionKey, int numberOfStudents)
        {
            this.PartitionKey = partitionKey;
            this.RowKey = "";
            this.NumberOfStudents = numberOfStudents;
        }
    }
    public class AzureTable
    {
        public string StorageAccount { get; }
        public string StorageKey { get; }
        public string TableName { get; }
        public AzureTable(string storageAccount, string storageKey, string tableName)
        {
            if (string.IsNullOrEmpty(storageAccount))
                throw new ArgumentNullException("StorageAccount");

            if (string.IsNullOrEmpty(storageKey))
                throw new ArgumentNullException("StorageKey");

            if (string.IsNullOrEmpty(tableName))
                throw new ArgumentNullException("TableName");

            this.StorageAccount = storageAccount;
            this.StorageKey = storageKey;
            this.TableName = tableName;
        }

        protected async Task<CloudTable> GetTable()
        {
            //Account
            CloudStorageAccount storageAccount = new CloudStorageAccount(
                new Microsoft.WindowsAzure.Storage.Auth.StorageCredentials(this.StorageAccount, this.StorageKey), false);

            //Client
            CloudTableClient tableClient = storageAccount.CreateCloudTableClient();

            //Table
            CloudTable table = tableClient.GetTableReference(this.TableName);
            await table.CreateIfNotExistsAsync();

            return table;
        }
    }
    class StudentTable: AzureTable
    {
        public StudentTable(string storageAccount, string storageKey, string tableName): 
            base(storageAccount, storageKey, tableName)
        {
            Console.WriteLine("The table named: " + tableName + " is ready for use!\n");
        }

        public async Task<List<StudentBluePrint>> GetAllStudents()
        {
            CloudTable table = await GetTable();
            TableQuery<StudentBluePrint> query = new TableQuery<StudentBluePrint>();
            List<StudentBluePrint> studentList = new List<StudentBluePrint>();

            TableContinuationToken continuationToken = null;

            do{
                TableQuerySegment<StudentBluePrint> queryResults = 
                    await table.ExecuteQuerySegmentedAsync(query, continuationToken);
                continuationToken = queryResults.ContinuationToken;
                studentList.AddRange(queryResults.Results);
            } while(continuationToken != null);

            return studentList;
        }
    }
    class StudentsMetricsTable: AzureTable
    {
        public StudentsMetricsTable(string storageAccount, string storageKey, string tableName): 
            base(storageAccount, storageKey, tableName)
        {
            Console.WriteLine("The table named: " + tableName + " is ready for use!\n");
        }

        public async Task<List<StudentMetricsBluePrint>> GetAllStudentsMetrics()
        {
            CloudTable table = await GetTable();
            TableQuery<StudentMetricsBluePrint> query = new TableQuery<StudentMetricsBluePrint>();
            List<StudentMetricsBluePrint> studentMetricsList = new List<StudentMetricsBluePrint>();

            TableContinuationToken continuationToken = null;

            do{
                TableQuerySegment<StudentMetricsBluePrint> queryResults = 
                    await table.ExecuteQuerySegmentedAsync(query, continuationToken);
                continuationToken = queryResults.ContinuationToken;
                studentMetricsList.AddRange(queryResults.Results);
            } while(continuationToken != null);

            return studentMetricsList;
        }

        public async Task InsertStudentMetric(StudentMetricsBluePrint metric)
        {
            CloudTable table = await GetTable();
            TableOperation insert_operation = TableOperation.Insert(metric);
            await table.ExecuteAsync(insert_operation);
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Starting the program\n\n");

            StudentTable studentTable = new StudentTable(
                "datcstudents",
                "rq4rGIjkZx4URNsS1RxwDsRFArB8GYbfpnGEqee9C6add+5sBxQMgbfy/SsKucYGRmOHSw2N2y8u+AStPBIOCA==",
                "laborator4"
            );

            StudentsMetricsTable studentMetricsTable = new StudentsMetricsTable(
                "datcstudents",
                "rq4rGIjkZx4URNsS1RxwDsRFArB8GYbfpnGEqee9C6add+5sBxQMgbfy/SsKucYGRmOHSw2N2y8u+AStPBIOCA==",
                "StudentsMetrics"
            );

            List<StudentBluePrint> studentList = new List<StudentBluePrint>();
            Task.Run(async() => {studentList = await studentTable.GetAllStudents();}).GetAwaiter().GetResult();

            List<StudentMetricsBluePrint> studentMetricsList = new List<StudentMetricsBluePrint>();
            Task.Run(async() => {studentMetricsList = await studentMetricsTable.GetAllStudentsMetrics();}).GetAwaiter().GetResult();

            if(studentList.Count() != 0)
            {
                foreach(StudentBluePrint student_i in studentList)
                {
                    Console.WriteLine(student_i.PartitionKey + " " + 
                                        student_i.RowKey + " " + 
                                        student_i.Name + " " + 
                                        student_i.Age + " " + 
                                        student_i.Faculty + " " + 
                                        student_i.Year);
                }
            }


            if(studentList.Count() != 0)
            {
                foreach(StudentBluePrint student_i in studentList)
                {
                    bool facultyExists = false;
                    foreach(StudentMetricsBluePrint metric_i in studentMetricsList)
                    {
                        if(student_i.Faculty == metric_i.PartitionKey)
                        {
                            metric_i.NumberOfStudents++;
                            facultyExists = true;
                            break;
                        }
                    }

                    if(!facultyExists)
                    {
                        studentMetricsList.Add(new StudentMetricsBluePrint(student_i.Faculty, 1));
                    }
                }
            }

            foreach(StudentMetricsBluePrint metric_i in studentMetricsList)
            {
                Task.Run(async() => {await studentMetricsTable.InsertStudentMetric(metric_i);}).GetAwaiter().GetResult();
            }
            
        }
    }
}
