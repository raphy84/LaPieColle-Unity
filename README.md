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
-Lance aléatoirement (1 chance sur 2) l'un des deux mini-jeux : "Hunter" ou "Tree Climber".
-En cas de victoire, le joueur évite les dégâts et la case est supprimée.
-En cas de défaite, inflige 1 point de dégât au joueur.


Mini-Jeu Hunter (Cache-Cache)
Le joueur doit rejoindre la zone d'arrivée sans se faire attraper par Serpant (le Hunter).

Comportements du Hunter
-Déplacement pathfinding pour contourner le décor.
-Cône de vision (Spotlight) :
	-Lumière Jaune : Mode recherche.
	-Lumière Rouge : Joueur repéré, déclenche la poursuite.
-Si le joueur est repéré par le hunter il est poursuivie jusqu'a le toucher.
-Au milieu de c'est patrouilles il fait des poses pour sentire la trace du joueur, a la fin de l'annimation il se rend a la position du joueur, une foit arriver il reprend sa routine.
-Si le Hunter touche le joueur, le mini-jeu est perdu.


Mini-Jeu Tree Climber (Grimpette FPS)
Le joueur doit monter tout en haut d'un arbre en sautant de branche en branche sans être touché par les balles tirées par le chasseur.

Comportements et Objectifs
-Vue à la première personne (FPS).
-Déplacement avec ZQSD et Saut avec Espace.
-Des balles apparaissent autour du joueur et se dirigent vers lui.
-Si le joueur est touché par une balle ou s'il tombe dans le vide, le mini-jeu est perdu.
-Si le joueur atteint le sommet de l'arbre, le mini-jeu est gagné.


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