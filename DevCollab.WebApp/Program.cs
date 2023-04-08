using DevCollab.Domain.Interfaces;
using DevCollab.Domain.Services;
using DevCollab.Infra.Context;
using DevCollab.Infra.Repositories;
using DevCollab.WebApp.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
	options.UseSqlServer(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
	.AddEntityFrameworkStores<ApplicationDbContext>();
builder.Services.AddControllersWithViews();

builder.Services.AddScoped<DevCollabDbContext, DevCollabDbContext>();
builder.Services.AddScoped<UsuarioService, UsuarioService>();
builder.Services.AddScoped<IUsuarioRepository, UsuarioRepository>();

builder.Services.AddScoped<SeguidorSeguidoService, SeguidorSeguidoService>();
builder.Services.AddScoped<ISeguidorSeguidoRepository, SeguidorSeguidoRepository>();

builder.Services.AddScoped<PublicacaoService, PublicacaoService>();
builder.Services.AddScoped<IPublicacaoRepository, PublicacaoRepository>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.UseMigrationsEndPoint();
}
else
{
	app.UseExceptionHandler("/Home/Error");
	// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
	app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
	name: "default",
	pattern: "{controller=Usuarios}/{action=Index}/{id?}");

app.MapControllerRoute(
	name: "relacionamento",
	pattern: "{controller=SeguidorSeguido}/{action=Index}/{id?}");

app.MapControllerRoute(
	name: "publicacao",
	pattern: "{controller=Publicacao}/{action=Index}/{id?}");

app.MapRazorPages();

app.Run();
