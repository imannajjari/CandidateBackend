using System.Text;
using AutoMapper;
using Candidate.Api.IoC;
using Candidate.Core.Mapping;
using Candidate.Data.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<DatabaseContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("CandidateDBConnectionString"));
});

builder.Services.AddAntiforgery(o => o.SuppressXFrameOptionsHeader = true);

builder.Services.AddAutoMapper(typeof(Program).Assembly);

var mappingConfig = new MapperConfiguration(mc =>
{
    mc.AddProfile(new MyMapper());
});

IMapper mapper = mappingConfig.CreateMapper();
builder.Services.AddSingleton(mapper);

builder.Services.AddSwaggerGen();
builder.Services.AddControllersWithViews().AddRazorRuntimeCompilation();


// Add services to the container.
builder.Services.AddRazorPages();

RegisterServices(builder.Services);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}


app.Use(async (context, next) =>
{
    context.Response.Headers.Add("X-Xss-Protection", "1");
    context.Response.Headers.Add("X-Frame-Options", "SAMEORIGIN");
    context.Response.Headers.Remove("Server");
    context.Request.Headers.Remove("Server");
    await next();
});

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "My Test1 Api v1");
});

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

app.MapRazorPages();

app.Run();

void RegisterServices(IServiceCollection services)
{
    Injector.RegisterServices(services);
}