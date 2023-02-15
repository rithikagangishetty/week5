using week5.Models;
using week5.Services;
using week5;


var builder = WebApplication.CreateBuilder(args);
builder.Services.Configure<EmployeeDetailsSettings>(
builder.Configuration.GetSection("EmployeeDatabase"));
builder.Services.AddSingleton<EmployeeServices>();
builder.Services.AddControllers().AddNewtonsoftJson();
builder.Services.AddControllers(options =>
{
    options.InputFormatters.Insert(0, MyJPIF.GetJsonPatchInputFormatter());
});
builder.Services.AddControllers()
.AddJsonOptions(
        options => options.JsonSerializerOptions.PropertyNamingPolicy = null);

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

app.UseAuthorization();

app.MapControllers();

app.Run();
