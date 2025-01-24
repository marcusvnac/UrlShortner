using ShortherUrlCore.Storage.InMemory;
using ShortherUrlCore.Storage;
using ShortherUrlCore.Business;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

// Choose which Data Provider to use
builder.Services.AddSingleton<IStorageManager, InMemoryStorageManager>();
//services.AddSingleton<IStorageManager, AzureTableStorageManager>();
//services.AddSingleton<IStorageManager, SqlServerStorageManager>();

builder.Services.AddScoped<IShortnerUrlBS, ShortnerUrlBS>();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseDeveloperExceptionPage();
}

app.UseRouting();

app.UseAuthorization();

app.MapControllers();

app.Run();
