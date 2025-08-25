using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;
using TANFInterfaces.Contracts;
using TANFInterfaces.Services;
using TANFRepo.IRepos;
using TANFRepo.Repos;


var builder = WebApplication.CreateBuilder(args);

// Connection string from appsettings.json
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

// Inject a new SqlConnection per request
builder.Services.AddTransient<IDbConnection>(sp =>
    new SqlConnection(connectionString));

// Register repo & services
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

builder.Logging.AddConsole();
//Angular app declaration

// 1. Add CORS services
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowReactApp",
        policy =>
        {
            policy.WithOrigins(
                "http://localhost:3000",
                "http://10.131.82.12:3000"
            )// React app URL 
                  .AllowAnyHeader()
                  .AllowAnyMethod()
                  .AllowCredentials();
        });
});

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


var app = builder.Build();

//cors
app.UseCors("AllowReactApp");
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