using Azure.Storage.Blobs;

// docker run -p 10000:10000 -p 10001:10001 -p 10002:10002 mcr.microsoft.com/azure-storage/azurite

const string ConnectionString = "UseDevelopmentStorage=True";
const string ContainerName = "test-container";
const string BlobName = "test-blob";

var container = new BlobContainerClient(ConnectionString, ContainerName);

container.CreateIfNotExists();

var blob = container.GetBlobClient(BlobName);

using (var stream = blob.OpenWrite(true))
{
    using var writer = new StreamWriter(stream);
    writer.Write("In the end it doesn't even matter");
}

using (var stream = blob.OpenRead()) {
    using var reader = new StreamReader(stream);
    Console.WriteLine(reader.ReadLine());
}
