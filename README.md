# CampusTask

Application mobile de gestion des tâches universitaires pour étudiants.

**Version 1.0 – Fonctionnement 100 % local (SQLite)**

## Fonctionnalités

- Inscription et connexion locales (mots de passe hashés avec BCrypt)
- Tableau de bord personnel
- Création, modification, suppression de tâches
- Marquer comme terminée / réouvrir
- Recherche et filtres (À faire, Terminées, En retard)
- Statistiques calculées en temps réel
- Profil et déconnexion
- Persistance des données après redémarrage

## Technologies

- .NET 8 MAUI
- C# / XAML
- SQLite (sqlite-net-pcl)
- CommunityToolkit.Mvvm
- BCrypt.Net-Next
- Architecture MVVM + Shell

## Prérequis

- Visual Studio 2022 (17.8+) ou VS Code + .NET 8 SDK
- Charge de travail **.NET Multi-platform App UI development**
- SDK Android 24+ (API level 24 minimum)

## Ouverture du projet

1. Ouvrir `CampusTask.sln` dans Visual Studio
2. Restaurer les packages NuGet (clic droit → Restore)
3. Sélectionner un émulateur Android ou un appareil physique
4. Appuyer sur F5 (Debug)

## Commandes utiles

```bash
# Restaurer les packages
dotnet restore

# Compiler pour Android
dotnet build -f net8.0-android

# Lancer sur un appareil/émulateur connecté
dotnet build -t:Run -f net8.0-android
```

## Structure

```
src/CampusTask/
├── Models/          → User, AcademicTask, enums
├── Data/            → DatabaseService (SQLite)
├── Repositories/    → Accès aux données
├── Services/        → Auth, Task, Statistics, Session
├── ViewModels/      → Logique de présentation
├── Views/           → Pages XAML
└── Resources/       → Styles, couleurs
```

## Notes importantes

- Les données sont stockées localement dans le dossier privé de l’application.
- La désinstallation de l’application efface la base SQLite.
- Aucune connexion Internet n’est requise pour l’utilisation normale.
- Les tâches en retard sont calculées dynamiquement (date limite dépassée + statut ≠ Terminée).

## Documentation complète

Voir le dossier `docs/` (à compléter avec les guides INSTALLATION, GUIDE_UTILISATEUR, etc.).

---

**CampusTask** – Organisez vos études sereinement.
