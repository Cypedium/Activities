using API.Extensions;
using API.Middleware;
using API.SingnalR;
using Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using Persistence;

var builder = WebApplication.CreateBuilder(args);  //runs a castol server

// Add services to the container for new logic

builder.Services.AddControllers(option =>
{
    var policy = new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build();
    option.Filters.Add(new AuthorizeFilter(policy));
});
builder.Services.AddApplicationServices(builder.Configuration);
builder.Services.AddIdentityServices(builder.Configuration);

var app = builder.Build();

// Configure the HTTP request pipeline. (Middleware)
app.UseMiddleware<ExceptionMiddleware>();

// app.UseXContentTypeOptions();
// app.UseReferrerPolicy(opt => opt.NoReferrer());
// app.UseXXssProtection(opt => opt.EnabledWithBlockMode());
// app.UseXfo(opt => opt.Deny());
// app.UseCsp(opt => opt
//     .BlockAllMixedContent()
//     .StyleSources(s => s.Self().CustomSources("https://fonts.googleapis.com"))
//     .FontSources(s => s.Self().CustomSources("https://fonts.gstatic.com", "data:"))
//     .FormActions(s => s.Self())
//     .FrameAncestors(s => s.Self())
//     .ImageSources(s => s.Self().CustomSources("blob:", "https://res.cloudinary.com", "https://platform-lookaside.fbsbx.com"))
//     .ScriptSources(s => s.Self())
// );

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("CorsPolicy");

app.UseAuthentication();
app.UseAuthorization();

app.UseDefaultFiles();
app.UseStaticFiles();


app.MapControllers();
app.MapHub<ChatHub>("/chat");
app.MapFallbackToController("Index", "Fallback");

using var scope = app.Services.CreateScope(); //This will be removed when we not are running with "using"
var services = scope.ServiceProvider;

try
{
    var context = services.GetRequiredService<DataContext>();
    var userManager = services.GetRequiredService<UserManager<AppUser>>();
    await context.Database.MigrateAsync();
    await Seed.SeedData(context, userManager);
}
catch (Exception ex)
{
    var logger = services.GetRequiredService<ILogger<Program>>();
    logger.LogError(ex, "An error occured during migration");
}

app.Run();