using StackExchange.Profiling;
using StackExchange.Profiling.Storage;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.EntityFrameworkCore;
using DatabaseAPI;
using BusinessAPI;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;



var builder = WebApplication.CreateBuilder(args);


builder.Services.AddMemoryCache();

builder.Services.AddMiniProfiler(options =>
{
    options.RouteBasePath = "/profiler";
    options.Storage = new MemoryCacheStorage(
        builder.Services.BuildServiceProvider().GetRequiredService<IMemoryCache>(),
        TimeSpan.FromMinutes(60)
    );
    options.SqlFormatter = new StackExchange.Profiling.SqlFormatters.InlineFormatter();
    options.TrackConnectionOpenClose = true;
    options.ResultsAuthorize = request => true; 
    options.ResultsListAuthorize = request => true; 
}).AddEntityFramework();

builder.Services.AddControllers();

// Veritabanı bağlantısı
builder.Services.AddDbContext<LogoApiDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
);


builder.Services.AddScoped<ProductService>();


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();


if (app.Environment.IsDevelopment())
{
    app.UseMiniProfiler();
    app.UseDeveloperExceptionPage(); 
    app.UseSwagger();
    app.UseSwaggerUI();
    
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
