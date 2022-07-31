using System.Text;

namespace bramvandenbussche.readwiser.api.DataAccess.TableStorage;

public class StoreBigThings: IStoreBigThings
{
    private readonly IStorageClientFactory _clientFactory;

    public StoreBigThings(IStorageClientFactory clientFactory)
    {
        _clientFactory = clientFactory;
    }

    public async Task<string> StoreBigString(string dataRecordName, Guid dataRecordId, string bigString)
    {
        dataRecordName = dataRecordName.ToLower();
        var client = await _clientFactory.GetBlobClient(dataRecordName, $"{dataRecordId}.json");
        await client.UploadAsync(new MemoryStream(Encoding.UTF8.GetBytes(bigString)));
        return $"{dataRecordName}#-#{dataRecordId}";
    }

    public async Task<string> GetBigString(string reference)
    {
        var explosion = reference.Split("#-#");
        var dataRecordName = explosion[0];
        var dataRecordId = explosion[1];

        var client = await _clientFactory.GetBlobClient(dataRecordName, $"{dataRecordId}.json");
        var stream = (await client.DownloadAsync()).Value.Content;
        using var streamReader = new StreamReader(stream, Encoding.UTF8);
        
        return await streamReader.ReadToEndAsync();
    }
}