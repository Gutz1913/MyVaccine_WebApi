using FluentValidation;
using MyVaccine.WebApi.Configurations;

var builder = WebApplication.CreateBuilder(args);



builder.Services.AddControllers();
builder.Services.AddValidatorsFromAssemblyContaining<Program>();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.SetDatabaseConfiguration();
builder.Services.SetMyVaccineAuthConfiguration();
builder.Services.SetDependencyInjection();
builder.Services.AddAutoMapper(cfg => { }, typeof(Program));

var app = builder.Build();

// ── Middleware Pipeline ──
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapGet("/", () => Results.Redirect("/swagger"));

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();
