var builder = WebApplication.CreateBuilder(args);

// Ajouter les services au conteneur
builder.Services.AddControllersWithViews();
builder.Services.AddSession(); // Ajouter le service de session

var app = builder.Build();

// Configurer le pipeline de requête HTTP
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseSession(); // Activer les sessions dans le pipeline

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Quiz}/{action=Start}/{id?}");

// Ajouter un message personnalisé pour l'URL de démarrage
Console.WriteLine("=======================================");
Console.WriteLine("Application démarrée !");
Console.WriteLine("Veuillez utiliser ce lien pour accéder à l'application :");
Console.WriteLine("https://localhost:7041/Quiz/Start");
Console.WriteLine("=======================================");

app.Run();
