using ShortherUrlCore.Storage.InMemory;
using ShortherUrlCore.Storage;
using ShortherUrlCore.Business;
using ShortherUrlCore.Storage.SqlServer;
using ShortherUrlCore.Storage.AzureTableStorage;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

// Choose which Data Provider to use. Choose 1
//builder.Services.AddSingleton<IStorageManager, InMemoryStorageManager>();
builder.Services.AddSingleton<IStorageManager, AzureTableStorageManager>();
//builder.Services.AddSingleton<IStorageManager, SqlServerStorageManager>();

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
