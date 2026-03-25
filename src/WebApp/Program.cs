using FFBC.Models;
using FFBC.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();

var seedEvents = new List<Event>
{
    new() { Date = new DateTime(2026, 4, 5), Title = "Spring Enduro Opener", Notes = "Meet at the trailhead at 8am" },
    new() { Date = new DateTime(2026, 4, 19), Title = "Sunday Shred – Blue Ribbons Trail", Notes = null },
    new() { Date = new DateTime(2026, 5, 3), Title = "Club Race Day", Notes = "Register by April 28" },
    new() { Date = new DateTime(2026, 5, 17), Title = "Skills Clinic – Drops & Jumps", Notes = "Beginner-friendly" },
};
builder.Services.AddSingleton<IEventStore>(new InMemoryEventStore(seedEvents));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();
app.MapRazorPages()
   .WithStaticAssets();

app.Run();
