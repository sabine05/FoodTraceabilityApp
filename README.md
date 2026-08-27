# 🥗 FoodTraceabilityApp

Application de traçabilité pour l'industrie agroalimentaire, développée en **C# / ASP.NET Core**. Elle permet de suivre le lien entre les matières premières reçues des fournisseurs et les produits finis fabriqués, afin de pouvoir retrouver rapidement l'origine d'un lot en cas de besoin (non conformité,rappel produit, contrôle qualité, audit).

Ce projet a été conçu comme projet personnel pour appliquer les compétences en développement back-end C#/.NET, en s'appuyant sur une expérience professionnelle en qualité et sécurité agroalimentaire.

## Fonctionnalités

- Gestion des **fournisseurs** (CRUD complet)
- Gestion des **matières premières** reçues, avec numéro de lot, dates de réception/péremption et statut (CRUD complet)
- Gestion des **produits finis**, avec numéro de lot interne et date de production (CRUD complet)
- Gestion des **liens de traçabilité** entre matières premières et produits finis :
  - Création d'un lien avec vérification que les deux entités existent
  - Recherche des matières premières utilisées pour un produit fini donné (traçabilité amont)
  - Recherche des produits finis ayant utilisé une matière première donnée (traçabilité aval)
- Une **interface web simple** (HTML/JS) pour visualiser et manipuler les données sans passer par Swagger.

## Stack technique

| Domaine | Technologie |
|---|---|
| Back-end | C# / ASP.NET Core Web API |
| Accès aux données | Entity Framework Core |
| Base de données | SQLite |
| Documentation API | Swagger / OpenAPI |
| Tests | xUnit + EF Core InMemory |
| Interface | HTML / CSS / JavaScript (vanilla) |

## Structure du projet

```
FoodTraceApp/
├── FoodTraceabilityApp/          # API back-end
│   ├── Controllers/               # FinishedProducts, RawMaterials, Suppliers, TraceabilityLinks
│   ├── Models/                    # Entités
│   └── Data/                      # AppDbContext
├── FoodTraceabilityApp.Tests/    # Tests unitaires (xUnit)
├── index.html                     # Interface web simple
└── README.md
```

## Lancer le projet en local

### 1. Prérequis
- [.NET 8 SDK](https://dotnet.microsoft.com/download)

### 2. Lancer l'API
```bash
cd FoodTraceabilityApp
dotnet run
```
L'API démarre (par défaut sur `http://localhost:5029`) et Swagger est accessible sur `http://localhost:5029/swagger`.

### 3. Lancer les tests
```bash
cd FoodTraceabilityApp.Tests
dotnet test
```

### 4. Utiliser l'interface web
Ouvre simplement le fichier `index.html` dans un navigateur (double-clic, ou clic droit → "Open with Live Server" dans VS Code).

Au premier lancement, vérifie que l'URL de l'API en haut de la page correspond à celle affichée dans ton terminal (`dotnet run`), puis clique sur "Enregistrer".

> ⚠️ **CORS** : par défaut, ASP.NET Core bloque les requêtes venant d'une page HTML ouverte en local. Si l'interface affiche des erreurs de chargement, ajoute ceci dans `Program.cs` de l'API :
> ```csharp
> builder.Services.AddCors(options =>
> {
>     options.AddPolicy("AllowAll", policy =>
>         policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
> });
> // ...
> app.UseCors("AllowAll");
> ```

## Endpoints principaux

| Méthode | Endpoint | Description |
|---|---|---|
| GET/POST | `/api/FinishedProducts` | Liste / création de produits finis |
| GET/PUT/DELETE | `/api/FinishedProducts/{id}` | Détail / modification / suppression |
| GET/POST | `/api/RawMaterials` | Liste / création de matières premières |
| GET/POST | `/api/Suppliers` | Liste / création de fournisseurs |
| POST | `/api/TraceabilityLinks` | Créer un lien de traçabilité |
| GET | `/api/TraceabilityLinks/product/{finishedProductId}` | Matières premières utilisées pour un produit fini |
| GET | `/api/TraceabilityLinks/rawmaterial/{rawMaterialId}` | Produits finis ayant utilisé une matière première |

## Tests

Le projet inclut des tests unitaires (xUnit + base de données EF Core InMemory) couvrant notamment :
- Le rejet d'une création de lien lorsque la matière première n'existe pas
- Le rejet d'une création de lien lorsque le produit fini n'existe pas
- La création réussie d'un lien lorsque les deux entités existent

## Pistes d'évolution

- Validation métier renforcée (quantités positives, unicité des liens)
- Gestion des erreurs de concurrence sur les mises à jour
- Authentification / autorisation
- Déploiement de l'API et de l'interface

## Auteure

**Sabine Ismail** 
[GitHub](https://github.com/sabine05) · [LinkedIn](https://linkedin.com/in/sabine-ismail34874)