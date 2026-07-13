using bramvandenbussche.readwiser.web.Components;
using bramvandenbussche.readwiser.web.Infrastructure;
using bramvandenbussche.readwiser.domain.Interface.Business;
using System.Globalization;
using System.Text;
using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services
    .AddDependencyInjection(builder.Configuration)
    .AddBlazorBootstrap()
    .AddRazorComponents()
    .AddInteractiveServerComponents();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
}

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

// Downloadable JSON export of all highlights (flat array, camelCase).
app.MapGet("/export/download", async (IHighlightService service) =>
{
    var highlights = await service.GetAll(0);

    var export = highlights.Select(h => new
    {
        title = h.Title,
        author = h.Author,
        text = h.Text,
        chapter = h.Chapter,
        note = h.Note,
        tags = h.Tags,
        raisedTime = h.RaisedTime
    });

    var json = JsonSerializer.Serialize(export, new JsonSerializerOptions(JsonSerializerDefaults.Web));
    var bytes = Encoding.UTF8.GetBytes(json);

    return Results.File(bytes, "application/json", "readwiser-export.json");
});

app.Run();
