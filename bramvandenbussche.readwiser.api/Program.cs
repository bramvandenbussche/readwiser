using bramvandenbussche.readwiser.api.Infrastructure;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddDependencyInjection(configuration);
builder.Services.AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = ApiKeyAuthenticationOptions.DefaultScheme;
        options.DefaultChallengeScheme = ApiKeyAuthenticationOptions.DefaultScheme;
    })
    .AddApiKeySupport(options => { });

builder.Services.AddSwaggerGen(options =>
    {
        options.SwaggerDoc("v1", new OpenApiInfo
        {
            Version = "v1",
            Title = "Readwiser API",
            Description = "API to manage the readwiser application",
            //TermsOfService = new Uri("https://example.com/terms"),
            //Contact = new OpenApiContact
            //{
            //    Name = "Example Contact",
            //    Url = new Uri("https://example.com/contact")
            //},
            //License = new OpenApiLicense
            //{
            //    Name = "Example License",
            //    Url = new Uri("https://example.com/license")
            //}
        });
    });


var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
else
{
    // Configure the HTTP request pipeline.
    // Let's not use this because we are running in Docker after all. 
    // We can use SSL offloading through the reverse proxy
    //app.UseHttpsRedirection();
}

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
