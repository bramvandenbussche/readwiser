using bramvandenbussche.readwiser.api.Dto;

namespace bramvandenbussche.readwiser.api.Domain;

public interface IHighlightService
{
    Task<IEnumerable<Highlight>> GetAll(int timestamp);
    Task<IEnumerable<Highlight>> GetForBook(string title, string author);
    Task Add(List<CreateHighlightRequestDto.HighlightDto> notes);
}