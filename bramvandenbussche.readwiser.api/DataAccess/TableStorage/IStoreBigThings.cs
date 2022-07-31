namespace bramvandenbussche.readwiser.api.DataAccess.TableStorage;

public interface IStoreBigThings
{
    /// <summary>
    /// This will store big things in a big thing storage tank
    /// </summary>
    /// <param name="dataRecordId">The id of the data record (will be the blob name)</param>
    /// <param name="bigString">The big string</param>
    /// <param name="dataRecordName">The name of the data record (will be the container name)</param>
    /// <returns>The reference to retrieve the big string</returns>
    Task<string> StoreBigString(string dataRecordName, Guid dataRecordId, string bigString);

    /// <summary>
    /// This will get big things from the big thing storage tank
    /// </summary>
    /// <param name="reference"></param>
    /// <returns></returns>
    Task<string> GetBigString(string reference);
}