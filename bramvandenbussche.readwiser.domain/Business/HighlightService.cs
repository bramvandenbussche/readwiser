﻿using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using bramvandenbussche.readwiser.domain.Interface.Business;
using bramvandenbussche.readwiser.domain.Interface.DataAccess;
using bramvandenbussche.readwiser.domain.Model;
using Microsoft.Extensions.Logging;

namespace bramvandenbussche.readwiser.domain.Business;

public class HighlightService : IHighlightService
{
    private readonly INoteRepository _repository;
    private readonly ILogger<HighlightService> _logger;

    public HighlightService(INoteRepository repository, ILogger<HighlightService> logger)
    {
        _repository = repository;
        _logger = logger;
    }

    public async Task<IEnumerable<Highlight>> GetAll(int timestamp)
        => await _repository.GetAll(timestamp);

    public async Task<IEnumerable<Highlight>> GetForBook(string title, string author) 
        => await _repository.GetForBook(title, author);

    public async Task Add(IEnumerable<Highlight> notes)
    {
        // The cache is used to store the data already in storage for the books mentioned in the input
        var cache = new Dictionary<string, List<Highlight>>();
        var makeKey = (string author, string title) => $"{author}-{title}";

        foreach (var note in notes)
        {
            // Get all existing notes for book
            var key = makeKey(note.Author, note.Title);
            if (!cache.ContainsKey(key))
            {
                var data = await _repository.GetForBook(note.Title, note.Author);
                cache.Add(key, data.ToList());
            }

            // Prevent duplicates
            if (!cache[key].Any(x => x.Chapter == note.Chapter && x.Text == note.Text))
            {
                cache[key].Add(note);
                await _repository.Save(note);
            }
        }
    }

    public async Task<IEnumerable<Book>> GetBooksForAuthor(string author)
    {
        var notes = await _repository.GetNotesForAuthor(author);

        var books = notes
            .GroupBy(x => x.Title)
            .Select(x => new Book()
            {
                Author = x.First().Author, // or author?
                Title = x.Key,
                Highlights = x.Select(note => new Highlight()
                {
                    NoteId = note.NoteId,
                    Text = note.Text,
                    Chapter = note.Chapter,
                    Note = note.Note,
                    RaisedTime = note.RaisedTime,
                    SortOrder = note.Chapter.FindChapterNumber()

                }).OrderBy(x => x.SortOrder)
                    .ThenBy(x => x.RaisedTime)
                    .ToList()
            });

        return books;
    }

    public async Task<IEnumerable<string>> GetAllAuthors()
        => await _repository.GetAllAuthors();

    public async Task<IEnumerable<Book>> GetAllBooks()
    {
        var notes = await _repository.GetAll(0);

        var books = notes
            .GroupBy(x => x.Title)
            .Select(x => new Book()
            {
                Author = x.First().Author, // or author?
                Title = x.Key,
                Highlights = x.Select(note => new Highlight()
                {
                    Text = note.Text,
                    Chapter = note.Chapter,
                    Note = note.Note,
                    RaisedTime = note.RaisedTime,
                    SortOrder = note.Chapter.FindChapterNumber()

                }).OrderBy(x => x.SortOrder)
                    .ThenBy(x => x.RaisedTime)
                    .ToList()
            });

        return books;
    }

    public Task<IEnumerable<Highlight>> GetRecentHighlights(int amount)
        => _repository.GetRecent(amount);

    public async Task UpdateHighlight(Highlight highlight)
        => await _repository.UpdateHighlight(highlight);

    public async Task DeleteHighlight(string noteId)
        => await _repository.DeleteHighlight(noteId);

    public async Task<List<string>> GetAllTags()
    {
        var data = await _repository.GetAllTags();

        if (data == null) return new List<string>();

        return data.Where(tag => !string.IsNullOrEmpty(tag)).ToList();
    }

    public async Task AddTag(string noteId, string tag)
     => await _repository.AddTag(noteId, tag);

    public async Task RemoveTag(string noteId, string tag)
        => await _repository.RemoveTag(noteId, tag);
}