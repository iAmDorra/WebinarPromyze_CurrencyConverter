Article associé :
[Différents tests pour un developpeur](https://www.arolla.fr/blog/2020/01/differents_tests_pour_developpeur/)

Ce projet est utilisé pour experimenter les differents tests qu'un développeur puisse réaliser.
* Unit tests
* Integrated tests (narrow & broad)
* End to end tests
* property-based-testing
* Health check tests

Si les tests end-to-end sont rouges vérifiez l'url et port dans le test. Il faudra peut être les modifier.

Suivre ces étapes si vous voulez créer une base de données :
https://docs.microsoft.com/fr-fr/ef/core/get-started/netcore/new-db-sqlite

**Au cas où le lien n'est pas accéssible, voici les informations à retenir :**

Installez Microsoft.EntityFrameworkCore.Sqlite et Microsoft.EntityFrameworkCore.Design :

```dotnet add package Microsoft.EntityFrameworkCore.Sqlite``` 
et 
```dotnet add package Microsoft.EntityFrameworkCore.Design```

Créez le modèle : Le DbContext et les entités

Une fois que vous avez un modèle, vous utilisez des migrations pour créer une base de données.
Exécutez 
```
dotnet ef migrations add InitialCreate 
```
pour générer automatiquement un modèle de migration et créer l’ensemble initial de tables du modèle.
Exécutez 
```
dotnet ef database update 
```
pour appliquer la nouvelle migration à la base de données. Cette commande crée la base de données avant d’appliquer des migrations.

Pour plus d'info sur les migrations :
https://docs.microsoft.com/fr-fr/ef/core/managing-schemas/migrations/index

Modification du modèle :
Si vous apportez des modifications au modèle, vous pouvez utiliser la commande 
```
dotnet ef migrations add 
```
pour générer automatiquement une nouvelle migration. 
Après avoir vérifié le code de modèle généré automatiquement (et effectué toutes les modifications nécessaires), vous pouvez utiliser la commande 
```
dotnet ef database update 
```
pour appliquer les modifications de schéma à la base de données.

EF Core utilise une table ```__EFMigrationsHistory``` dans la base de données pour effectuer le suivi des migrations qui ont déjà été appliquées à la base de données.
Le moteur de base de données SQLite ne prend pas en charge certaines modifications de schéma qui sont prises en charge par la plupart des autres bases de données relationnelles. 
Par exemple, l’opération ```DropColumn``` n’est pas prise en charge. 

Les migrations EF Core génèrent du code pour ces opérations, mais si vous tentez de les appliquer à une base de données ou de générer un script, EF Core lève des exceptions. 
Consultez Limitations de SQLite. 
https://docs.microsoft.com/fr-fr/ef/core/providers/sqlite/limitations

Pour tout nouveau développement, il est préférable de supprimer la base de données et d’en créer une nouvelle plutôt que d’utiliser des migrations quand le modèle change.
