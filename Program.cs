using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SharedHosting.Data;
using SharedHosting.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddSingleton<WeatherForecastService>();

// Add Identity
builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
{
    options.Password.RequireDigit = true;
    options.Password.RequiredLength = 6;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
    options.Password.RequireLowercase = false;
})
.AddEntityFrameworkStores<ApplicationDbContext>()
.AddDefaultTokenProviders();

// Add DbContext
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Add services
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IWorkspaceService, WorkspaceService>();
builder.Services.AddScoped<IFolderService, FolderService>();
builder.Services.AddScoped<IFileService, FileService>();
builder.Services.AddScoped<IGalleryService, GalleryService>();
builder.Services.AddScoped<ICurrentUserService, CurrentUserService>();
builder.Services.AddScoped<IConfigurationService, ConfigurationService>();
builder.Services.AddHttpContextAccessor();

// Configure storage service
builder.Services.AddAWSService<IAmazonS3>();
var storageType = builder.Configuration["Storage:Type"];
switch (storageType)
{
    case "Local":
        var localPath = builder.Configuration["Storage:Local:BasePath"] ?? "wwwroot/uploads";
        builder.Services.AddSingleton<IStorageService>(new LocalStorageService(localPath));
        break;
    case "Ftp":
        var ftpHost = builder.Configuration["Storage:Ftp:Host"];
        var ftpUser = builder.Configuration["Storage:Ftp:Username"];
        var ftpPass = builder.Configuration["Storage:Ftp:Password"];
        builder.Services.AddSingleton<IStorageService>(new FtpStorageService(ftpHost, ftpUser, ftpPass));
        break;
    case "S3":
        var bucketName = builder.Configuration["Storage:S3:BucketName"];
        builder.Services.AddSingleton<IStorageService>(sp =>
        {
            var s3Client = sp.GetRequiredService<IAmazonS3>();
            return new S3StorageService(s3Client, bucketName);
        });
        break;
    default:
        throw new InvalidOperationException($"Unsupported storage type: {storageType}");
}

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();
