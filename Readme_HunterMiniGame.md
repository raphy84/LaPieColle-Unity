# Mini-Jeu Hunter (Cache-Cache) - La Pie Colle

## Description du jeu
Il s'agit d'un mini-jeu additionnel créé pour s'intégrer à la boucle de gameplay (Loop Hero like).
Lorsque le joueur marche sur une cellule piégée (S_TrapCell), au lieu de subir des dégâts instantanément, il est transporté dans une zone de "Cache-Cache" avec le Hunter.

### Conditions de Victoire et Défaite
- **Victoire** : Le joueur (contrôlé avec ZQSD / WASD / Flèches) doit se rendre du point de spawn jusqu'à la zone d'arrivée (Zone verte ou Trigger `Finish`) sans se faire toucher. S'il y parvient, le piège est annulé dans le jeu de plateau principal. Le joueur gagne et conserve sa santé !
- **Défaite** : Si le Hunter parvient à toucher le joueur (Trigger), le mini-jeu se termine sur une défaite. De retour sur le plateau principal, le joueur subit son point de dégât initial (comme prévu classiquement par la cellule).

## Intelligence Artificielle du Hunter
L'IA du Hunter repose sur une machine à trois états (States) simples pour créer un jeu de rythme "1, 2, 3 Soleil / Cache-Cache" agressif :
1. **Walk (Patrouille)** : Le Hunter patrouille de gauche à droite sur son axe en jouant l'animation `Crouch Walk Forward`.
2. **Inspect (Inspection)** : Régulièrement, le Hunter s'arrête, se tourne vers le joueur et joue l'animation `Crouching`. À cet instant, il enregistre mentalement la dernière position visible du joueur.
3. **Dash (Attaque)** : Dès que l'animation est finie, le Hunter se jette violemment sur la position enregistrée en jouant l'animation `Pontera`. Le joueur doit utiliser ce timing pour feinter le Hunter et l'esquiver avant qu'il ne reprenne sa ronde.

## Intégration Dynamique et Bonus
Le changement de scène s'opère de manière transparente via l'API de scène (`LoadSceneMode.Additive`). La caméra principale est éteinte pendant que le joueur affronte l'IA dans l'arène dédiée.
Le système garantit que les données (ex: `SO_PlayerDatas_wonMiniGame`) sont transmises pour modifier dynamiquement les conséquences du plateau (bonus de parade aux dégâts).

## Configuration Unity Manquante (Rig Humanoide)
Pour palier à l'erreur d'import de base sur l'Avatar (`mixamorig:LeftEye` en conflit), un outil Custom a été développé dans l'éditeur. Pour le lancer, naviguez dans la barre Unity : 
- Allez dans `Tools > Fix Hunter Rig (MiniGame)`
L'outil s'assurera automatiquement que le système de rig est fonctionnel pour les 3 animations spécifiquement.
