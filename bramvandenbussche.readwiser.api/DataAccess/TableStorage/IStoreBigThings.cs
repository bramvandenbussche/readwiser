namespace bramvandenbussche.readwiser.api.DataAccess.TableStorage;

public interface IStoreBigThings
{
    /// <summary>
    /// This will store big things in a big thing storage tank
    /// </summary>
    /// <param name="eventId">The id of the event (will be the blob name)</param>
    /// <param name="bigString">The big string</param>
    /// <param name="eventName">The name of the event (will be the container name)</param>
    /// <returns>The reference to retrieve the big string</returns>
    Task<string> StoreBigString(string eventName, Guid eventId, string bigString);

    /// <summary>
    /// This will get big things from the big thing storage tank
    /// </summary>
    /// <param name="reference"></param>
    /// <returns></returns>
    Task<string> GetBigString(string reference);
}