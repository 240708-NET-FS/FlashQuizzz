using System.Text;
using Azure.Monitor.OpenTelemetry.AspNetCore;
using FlashQuizzz.API.DAO;
using FlashQuizzz.API.DAO.Interfaces;
using FlashQuizzz.API.Models;
using FlashQuizzz.API.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models; 
using Swashbuckle.AspNetCore.Filters;

var builder = WebApplication.CreateBuilder(args); 

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options => {
    options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey
    });

    options.OperationFilter<SecurityRequirementsOperationFilter>();
});

//Here we will register our dependencies (Services and DbContext, etc) so that we can satisfy our constructors
//and inject dependecies where needed
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IFlashCardService, FlashCardService>();
builder.Services.AddScoped<IFlashCardCategoryService, FlashCardCategoryService>();
builder.Services.AddScoped<IFlashCardAnswerService, FlashCardAnswerService>();
//
builder.Services.AddScoped<IUserRepo, UserRepo>();
builder.Services.AddScoped<IFlashCardRepo, FlashCardRepo>();
builder.Services.AddScoped<IFlashCardCategoryRepo, FlashCardCategoryRepo>();
builder.Services.AddScoped<IFlashCardAnswerRepo, FlashCardAnswerRepo>();

// Add OpenTelemetry and configure it to use Azure Monitor.
// builder.Services.AddOpenTelemetry().UseAzureMonitor(options => {
//     options.ConnectionString = builder.Configuration.GetConnectionString("DefaultConnection");
// });

builder.Services.AddControllers().AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
    });

// Load configuration from appsettings.json and environment-specific files
// builder.Configuration
//     .SetBasePath(Directory.GetCurrentDirectory())
//     .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
//     .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true)
//     .AddEnvironmentVariables();

builder.Services.AddDbContext<AppDbContext>(options => 
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Logging.AddConsole();

builder.Services.AddIdentityApiEndpoints<User>()
        .AddEntityFrameworkStores<AppDbContext>();

//CORS CORS CORS CORS CORS CORS CORS CORS CORS CORS CORS CORS CORS CORS CORS
/*
builder.Services.AddCors(co => {
    co.AddPolicy("CORS" , pb =>{
        pb.WithOrigins("http://localhost:5500"); //<- your localhost port here!!!
    });
});
*/

builder.Services.AddCors(options =>
{
    options.AddPolicy("CORS", policy =>
    {
        policy.AllowAnyOrigin()  // Allow all origins (for development, specify exact origins for production)
              .AllowAnyMethod()  // Allow any HTTP method
              .AllowAnyHeader(); // Allow any header
    });
});

//CORS CORS CORS CORS CORS CORS CORS CORS CORS CORS CORS CORS CORS CORS CORS  
     

builder.Services.AddAuthorization();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//CORS CORS CORS CORS CORS CORS CORS CORS CORS CORS CORS CORS CORS CORS CORS
app.UseCors("CORS"); //<-USE CORS with your policy name
//CORS CORS CORS CORS CORS CORS CORS CORS CORS CORS CORS CORS CORS CORS CORS 

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.MapIdentityApi<User>();

app.Run();
