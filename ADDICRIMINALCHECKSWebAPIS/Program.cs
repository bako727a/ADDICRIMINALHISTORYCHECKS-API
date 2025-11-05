using CCInterfaces.Contracts;
using CCInterfaces.Services;
using CCRepo.IRepos;
using CCRepo.Repos;
using Microsoft.AspNetCore.Authentication.Negotiate;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using System.Data;

var builder = WebApplication.CreateBuilder(args);

// ==================== CONNECTION ====================
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddTransient<IDbConnection>(sp => new SqlConnection(connectionString));

// ==================== SERVICES ====================
builder.Services.AddHttpContextAccessor();

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

// ==================== WINDOWS AUTH ====================
builder.Services.AddAuthentication(NegotiateDefaults.AuthenticationScheme)
    .AddNegotiate();

builder.Services.AddAuthorization();

// ==================== CORS ====================
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowReactApp", policy =>
    {
        policy.WithOrigins(
                "http://localhost:3000",
                "http://10.131.82.12:3000",
                "http://dhr99aswtdwb01v:8090"
            )
            .SetIsOriginAllowed(_ => true)
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials(); // Required for Windows Auth cookies
    });
});

// ==================== CONTROLLERS ====================
builder.Services.AddControllers(options =>
{
    // Optional: use [Authorize] globally if desired
    // options.Filters.Add(new AuthorizeFilter());
});

// ==================== SWAGGER ====================
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.AddSecurityDefinition("WindowsAuth", new OpenApiSecurityScheme
    {
        Type = SecuritySchemeType.Http,
        Scheme = "negotiate",
        Description = "Windows Authentication"
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "WindowsAuth"
                }
            },
            new string[] {}
        }
    });
});

// ==================== BUILD APP ====================
var app = builder.Build();

app.UseHttpsRedirection();

// CORS must be before Auth
app.UseCors("AllowReactApp");

// Swagger should come after Auth if you want it secured
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "ADDICCWebAPIS v1");
    c.RoutePrefix = "swagger";
});

app.Use(async (context, next) =>
{
    if (context.Request.Method == HttpMethods.Options)
    {
        // ✅ Skip auth challenge for CORS preflight
        context.Response.StatusCode = 200;
        context.Response.Headers.Append("Access-Control-Allow-Origin", context.Request.Headers["Origin"]);
        context.Response.Headers.Append("Access-Control-Allow-Methods", "GET, POST, PUT, DELETE, OPTIONS");
        context.Response.Headers.Append("Access-Control-Allow-Headers", "Content-Type, Authorization");
        context.Response.Headers.Append("Access-Control-Allow-Credentials", "true");
        await context.Response.CompleteAsync();
    }
    else
    {
        await next();
    }
});



// Authentication & Authorization
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
