using OfficeOpenXml;

var builder = WebApplication.CreateBuilder(args);

// EPPlus license
ExcelPackage.License.SetNonCommercialPersonal("Shailesh");

// Add services
builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseAuthorization();

app.MapControllers();

app.Run();