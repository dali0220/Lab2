using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Lab2.Data;

var builder = WebApplication.CreateBuilder(args);

var lab2ConnectionString = builder.Configuration.GetConnectionString("Lab2Context")
    ?? throw new InvalidOperationException("Connection string 'Lab2Context' not found.");

builder.Services.AddDbContext<Lab2Context>(options =>
    options.UseSqlServer(lab2ConnectionString));

var identityConnectionString = builder.Configuration.GetConnectionString("LibraryIdentityContextConnection")
    ?? throw new InvalidOperationException("Connection string 'LibraryIdentityContextConnection' not found.");

builder.Services.AddDbContext<LibraryIdentityContext>(options =>
    options.UseSqlServer(identityConnectionString));

builder.Services
    .AddDefaultIdentity<IdentityUser>(options =>
    {
        options.SignIn.RequireConfirmedAccount = false;
        options.Password.RequireDigit = false;
        options.Password.RequireLowercase = false;
        options.Password.RequireUppercase = false;
        options.Password.RequireNonAlphanumeric = false;
        options.Password.RequiredLength = 3;
    })
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<LibraryIdentityContext>();
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminPolicy", policy =>
        policy.RequireRole("Admin"));
});




builder.Services.AddRazorPages(options=>
{
    options.Conventions.AuthorizeFolder("/Books");
    options.Conventions.AllowAnonymousToPage("/Books/Index");
    options.Conventions.AllowAnonymousToPage("/Books/Details");
    options.Conventions.AuthorizeFolder("/Members", "AdminPolicy");
    options.Conventions.AuthorizeFolder("/Publishers", "AdminPolicy");
    options.Conventions.AuthorizeFolder("/Categories", "AdminPolicy");
}
);

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.MapRazorPages();
app.Run();
