# GroupeF

## QUIZ APP 

QuizApp2 est une application web simple développée en .NET Core permettant de répondre à un quiz. Les questions peuvent être basées sur du texte ou inclure des images.

## Fonctionnalités principales
- Affiche des questions avec des options de réponse.
- Les questions peuvent inclure une image ou être uniquement textuelles.
- Un score est calculé en fonction des réponses correctes.
- Les questions sont chargées dynamiquement à partir d'un fichier JSON.

## Prérequis
- SDK .NET 6.0 ou supérieur.

## Installation
1. Téléchargez ou clonez le projet.


## Lancer l'application
1. Depuis le dossier "QuizApp2", exécutez :
   ```bash
   dotnet watch run
   ```

    (il ouvrira la page web de maniere auto sinon ⬇️⬇️)

   
2. Ouvrez l'URL affichée dans la console, comme `https://localhost:7041`.

## Structure du projet
- **Controllers** : Contient la logique métier (e.g., `QuizController.cs`).
- **Models** : Définit les modèles de données (e.g., `Question.cs`).
- **Views** : Contient les fichiers d'interface utilisateur (`Start.cshtml`, `Question.cshtml`, `Result.cshtml`).
- **wwwroot** : Contient les fichiers statiques, comme `Data/question.json`.

## Vous souhaitez  Modifier les questions🤔 ?
1. Ouvrez `wwwroot/Data/question.json`.
2. Ajoutez ou modifiez les questions. Exemple :
   ```json
   {
       "Text": "Quel fruit est représenté sur cette image ?",
       "ImageUrl": "https://via.placeholder.com/400x300?text=Pomme",
       "Options": ["Banane", "Pomme", "Poire", "Orange"],
       "CorrectAnswerIndex": 1
   }
   ```
3. Sauvegardez le fichier.

Amusez-vous bien avec la Quiz App du 😁GROUPE FFFFFFFFFF!


## TIC TAC TOE
