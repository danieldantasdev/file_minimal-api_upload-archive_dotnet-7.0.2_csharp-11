var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();


app.MapPost("/uploadArquivo", async (IFormFile arquivo) =>
{
    string tempfile = CreateTempfilePath(arquivo);
    using var stream = File.OpenWrite(tempfile);
    await arquivo.CopyToAsync(stream);

    return Results.Ok("Arquivo enviado com sucesso");
});

app.MapPost("/uploadArquivos", async (IFormFileCollection arquivos) =>
{
    foreach (var file in arquivos)
    {
        string tempfile = CreateTempfilePath(file);
        using var stream = File.OpenWrite(tempfile);
        await file.CopyToAsync(stream);
    }
    return Results.Ok("Arquivos enviados com sucesso");
});

static string CreateTempfilePath(IFormFile file)
{
    var extension = file.FileName.Split(".")[1];
    var filename = $@"{DateTime.Now.Ticks}.{extension}";

    var directoryPath = Path.Combine("temp", "uploads");

    if (!Directory.Exists(directoryPath)) 
        Directory.CreateDirectory(directoryPath);

    return Path.Combine(directoryPath, filename);
}
app.Run();
