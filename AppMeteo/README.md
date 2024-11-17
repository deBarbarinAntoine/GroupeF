# Application Météo

Une application Windows Forms simple en C# qui permet d'afficher les conditions météorologiques actuelles et les prévisions pour une ville donnée en utilisant l'[API OpenWeatherMap](https://openweathermap.org/api).

# Fonctionnalités
- Recherche par ville : Permet d'obtenir la météo actuelle d'une ville spécifiée.
- Affichage de la météo actuelle : Température, description, et icône représentant le temps.
- Prévisions météorologiques : Affiche les prévisions météo de la ville
- Mise à jour : Rafraîchissement des données météo en temps réel.

# Technologies Utilisées
- C# et .NET Framework pour l'interface utilisateur et la logique de l'application.
- Windows Forms pour créer l'interface graphique.
- [API OpenWeatherMap](https://openweathermap.org/api) pour récupérer les données météorologiques en temps réel.

# Configuration

Requirements

- .NET SDK v8.0.110 ou version ultérieure
- Windows Forms Framework (nécessite .NET Core ou .NET 5.0+)
- [API OpenWeatherMap](https://openweathermap.org/api) pour les données météorologiques (avec clé API gratuite)

Dependencies

- Newtonsoft.Json v13.0.3 ou version ultérieure (pour le traitement des données JSON)
- System.Net.Http (inclus dans .NET Framework par défaut)

Installation

- Clonez ou téléchargez ce dépôt

- Ajoutez votre clé API OpenWeatherMap :

- Allez dans le fichier Form1.cs et remplacez la valeur de la variable apiKey par votre clé API personnelle obtenue sur [OpenWeatherMap](https://openweathermap.org/api).

`private string apiKey = "VOTRE_CLÉ_API";`

- Installez les dépendances

Ouvrez le Package Manager Console et entrez la commande suivante pour installer Newtonsoft.Json :

`Install-Package Newtonsoft.Json -Version 13.0.3`

(Remarque : System.Net.Http est inclus par défaut avec .NET, donc aucune installation supplémentaire n'est nécessaire pour cette dépendance.)

# Exécutez l'application :

En utilisant la ligne de commande, à partir du répertoire racine de votre projet :

`dotnet run`