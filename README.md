# LaPieColle-Unity

Concept de Jeu

Le joueur incarne un gecko.
L’objectif est d’éliminer Serpant le chasseur à l’aide de déplacements sur un plateau et de l’aide des singes.


Objectifs

Objectif Principal
-Réduire les points de vie de Serpant à 0.

Conditions de Défaite
-Le joueur tombe à 0 points de vie.
-En cas de défaite, la partie recommence.


Boucle de Gameplay

Le joueur lance un dé.
Le joueur avance du nombre de cases indiqué (1 à 6).
Serpant se déplace librement entre les cases.
Serpant place un piège sur la case où il s’arrête.
Les effets de la case du joueur sont appliqués.
Le tour se termine.
Tous les 5 tours, de nouvelles cases sont générées aléatoirement.


Types de Cases

Case Normale
Aucun effet.

Case Piège
-Placée uniquement par Serpant.
-Inflige 1 point de dégât au joueur lorsqu’il atterrit dessus.

Case Nourriture
-Rend 1 point de vie au joueur lorsqu’il marche dessus.

Case Singe
-Permet au joueur de parler au roi des singes.
-Le joueur peut choisir une action :
	-Retirer 5 pièges du plateau.
	-Attaquer Serpant.

Après 5 attaques de singes, Serpant meurt.


Génération de Cases
Toutes les 5 manches, de nouvelles cases sont générées aléatoirement.

Les types de cases générées sont :
-Case Singe
-Case Nourriture

Les cases existantes peuvent être remplacées.