using System.Reflection;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using MudBlazor.Services;
using YottySuba.Components;
using SixLabors.ImageSharp.Web.DependencyInjection;
using SixLabors.ImageSharp.Web.Providers;
using YottySuba;
using YottySuba.Components.Services;
using YottySuba.Database;
using YottySuba.Database.Services;

var codeBase = Assembly.GetExecutingAssembly().Location;
var uri = new UriBuilder(codeBase);
var path = Uri.UnescapeDataString(uri.Path);
var rootDirectory = Path.GetDirectoryName(path)!;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder
    .Services
    .AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddMudServices();
builder.Services.AddDbContext<YottysubaContext>(opt =>
    opt.UseNpgsql(builder.Configuration.GetConnectionString("Main")), ServiceLifetime.Transient);

builder.Services.Configure<StaticFileOptions>(options =>
{
    var contentTypeProvider = new FileExtensionContentTypeProvider();
    options.ContentTypeProvider = contentTypeProvider;
    options.FileProvider = new CompositeFileProvider(builder.Environment.WebRootFileProvider, new PhysicalFileProvider(rootDirectory));
});

builder.Services.AddImageSharp()
    .AddProvider<PhysicalFileSystemProvider>()
    .Configure<PhysicalFileSystemProviderOptions>(options =>
    {
        options.ProcessingBehavior = ProcessingBehavior.All;
        options.ProviderRootPath = rootDirectory;
    });

builder.Services.AddScoped<AttachmentUploadService>(provider => new AttachmentUploadService(rootDirectory, provider.GetRequiredService<YottysubaContext>()));
builder.Services.AddTransient<ImageProcessingService>(_ => new ImageProcessingService(rootDirectory));
builder.Services.AddTransient<VideoProcessingService>(_ => new VideoProcessingService(rootDirectory));
builder.Services.AddScoped<BoardService>();
builder.Services.AddScoped<NavBarService>();
builder.Services.AddScoped<ThreadService>();
builder.Services.AddScoped<PostFilterService>();

builder.Services
    .AddAuthentication()
    .AddJwtBearerConfiguration(builder.Configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseImageSharp();
app.UseStaticFiles();
app.UseAntiforgery();


app
    .MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();