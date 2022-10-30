using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;

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