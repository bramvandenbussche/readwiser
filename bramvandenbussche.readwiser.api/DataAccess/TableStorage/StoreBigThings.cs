using System.Text;

namespace bramvandenbussche.readwiser.api.DataAccess.TableStorage;

public class StoreBigThings: IStoreBigThings
{
    private readonly IStorageClientFactory _clientFactory;

    public StoreBigThings(IStorageClientFactory clientFactory)
    {
        _clientFactory = clientFactory;
    }

    public async Task<string> StoreBigString(string eventName, Guid eventId, string bigString)
    {
        eventName = eventName.ToLower();
        var client = await _clientFactory.GetBlobClient(eventName, $"{eventId}.json");
        await client.UploadAsync(new MemoryStream(Encoding.UTF8.GetBytes(bigString)));
        return $"{eventName}#-#{eventId}";
    }

    public async Task<string> GetBigString(string reference)
    {
        var explosion = reference.Split("#-#");
        var eventName = explosion[0];
        var eventId = explosion[1];

        var client = await _clientFactory.GetBlobClient(eventName, $"{eventId}.json");
        var stream = (await client.DownloadAsync()).Value.Content;
        using var streamReader = new StreamReader(stream, Encoding.UTF8);
        
        return await streamReader.ReadToEndAsync();
    }
}