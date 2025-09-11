using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;
using TANFInterfaces.Contracts;
using TANFInterfaces.Services;
using TANFRepo.IRepos;
using TANFRepo.Repos;


var builder = WebApplication.CreateBuilder(args);

// ==================== CONNECTION ====================
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddTransient<IDbConnection>(sp => new SqlConnection(connectionString));

// ==================== DEPENDENCY INJECTION ====================
builder.Services.AddScoped<ILookupService, LookupService>();
builder.Services.AddScoped<ILookupRepo, LookupRepo>();
builder.Services.AddScoped<IAdminService, AdminService>();
builder.Services.AddScoped<AdminRepo>();
builder.Services.AddScoped<IAdvanceSearchService, AdvancedSearchService>();
builder.Services.AddScoped<AdvanceSearchRepo>();
builder.Services.AddScoped<ISecurityService, SecurityService>();
builder.Services.AddScoped<SecurityRepo>();
builder.Services.AddScoped<IAuditService, AuditService>();
builder.Services.AddScoped<AuditRepo>();
builder.Services.AddScoped<ICaseAuditService, CaseAuditService>();
builder.Services.AddScoped<CaseAuditRepo>();
builder.Services.AddScoped<ICaseDocumentService, CaseDocumentService>();
builder.Services.AddScoped<CaseDocumentRepo>();
builder.Services.AddScoped<ICaseManagementService, CaseManagementService>();
builder.Services.AddScoped<CaseManagementRepo>();
builder.Services.AddScoped<IConfidentialService, ConfidentialService>();
builder.Services.AddScoped<ConfidentialRepo>();
builder.Services.AddScoped<IDashboardService, DashboardService>();
builder.Services.AddScoped<DashboardRepo>();
builder.Services.AddScoped<IImportService, ImportService>();
builder.Services.AddScoped<ImportRepo>();

builder.Services.AddAutoMapper(cfg => cfg.AddMaps(typeof(Program).Assembly));

// ==================== LOGGING ====================
builder.Logging.AddConsole();

// ==================== CORS ====================
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowReactApp", policy =>
    {
        policy.WithOrigins("http://localhost:3000", "http://10.131.82.12:3000")
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials();
    });
});

// ==================== CONTROLLERS & AUTH ====================
builder.Services.AddControllers(options =>
{
    options.Filters.Add(new AllowAnonymousFilter()); // optional
});



builder.Services.AddAuthentication(Microsoft.AspNetCore.Server.IISIntegration.IISDefaults.AuthenticationScheme);
builder.Services.AddAuthorization();

// ==================== SWAGGER ====================
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// ==================== MIDDLEWARE ====================
app.UseCors("AllowReactApp"); // must be before Auth

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "ADDITANFWebAPIS v1");
    c.RoutePrefix = "swagger"; // navigate to https://localhost:44321/swagger
});

app.UseHttpsRedirection();

app.Use(async (context, next) =>
{
    if (context.Request.Method == "OPTIONS")
    {
        context.Response.StatusCode = 200;
        await context.Response.CompleteAsync();
    }
    else
    {
        await next();
    }
});

// 5. HTTPS & Auth middleware
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

// 6. Map controllers
app.MapControllers();

app.Run();
