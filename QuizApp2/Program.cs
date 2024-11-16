var builder = WebApplication.CreateBuilder(args);

// Ajouter les services nécessaires pour les contrôleurs
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Ajouter le gestionnaire d'exceptions et d'autres middlewares
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

// Configurer le routage pour les contrôleurs
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Quiz}/{action=Start}/{id?}");

app.Run();
