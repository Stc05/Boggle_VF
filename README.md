# Boogle

Boogle est une adaptation numérique du jeu de lettres Boggle, entièrement développée en C#. Le projet met l'accent sur la programmation orientée objet et l'utilisation d'algorithmes récursifs pour la recherche de mots.

## Fonctionnalités principales

- **Programmation orientée objet** :  
  Le projet est structuré autour de plusieurs classes principales (`Joueur`, `Dé`, `Plateau`, `Dictionnaire`, `Jeu`, `NuageDeMot`), chacune responsable d'un aspect du jeu, pour assurer une architecture claire.
- **Validation et recherche de mots** :  
  Des algorithmes récursifs vérifient la validité des mots saisis par le joueur en explorant le plateau de lettres. Plusieurs méthodes de recherche ont été étudiées et comparées pour optimiser l'efficacité.
- **Nuage de mots personnalisé** :  
  À la fin de chaque partie, la classe `NuageDeMot` génère automatiquement un nuage à partir des mots trouvés par le joueur, offrant ainsi une visualisation originale de sa performance.

## Structure du projet

- `Joueur.cs` : Gère les informations et le score de chaque joueur.
- `Dé.cs` : Modélise les dés du plateau avec génération aléatoire des lettres.
- `Plateau.cs` : Génère et gère l'agencement du plateau de jeu.
- `Dictionnaire.cs` : Contient la liste des mots valides et les méthodes de recherche associées.
- `Jeu.cs` : Coordonne le déroulement de la partie et l'interaction entre les différentes classes.
- `NuageDeMot.cs` : Crée un nuage de mots à partir des mots trouvés par le joueur à la fin de la partie.

## Algorithmes

- **Recherche récursive** : Pour explorer toutes les combinaisons possibles de lettres adjacentes et vérifier si un mot proposé peut être formé sur le plateau.
- **Optimisation de la recherche** : Étude et implémentation de différentes stratégies pour accélérer la vérification des mots dans le dictionnaire, comme l'utilisation d'un trie fusion.
- **Génération du nuage de mots** : Création automatique d'une représentation graphique des mots trouvés après chaque partie.



