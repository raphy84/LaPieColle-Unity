using UnityEngine;
using System.Collections;

public class S_TreeClimberManager : S_MiniGameManager
{
    [Header("Projectile Settings")]
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private float spawnInterval = 2f;
    [SerializeField] private float projectileSpeed = 10f;
    [SerializeField] private float spawnRadius = 15f;

    [Header("References")]
    [SerializeField] private Transform playerTransform;
    [SerializeField] private float winYPosition = 20f;
    [SerializeField] private float fallThreshold = -10f;

    private bool gameFinished = false;

    void Start()
    {
        if (playerTransform == null)
            playerTransform = GameObject.FindGameObjectWithTag("Player")?.transform;

        StartCoroutine(SpawnRoutine());
    }

    void Update()
    {
        if (gameFinished) return;

        CheckConditions();
    }

    IEnumerator SpawnRoutine()
    {
        while (!gameFinished)
        {
            yield return new WaitForSeconds(spawnInterval);
            SpawnProjectile();
        }
    }

    void SpawnProjectile()
    {
        if (projectilePrefab == null || playerTransform == null) return;

        // On spawn la balle à une certaine distance autour du joueur
        Vector3 randomDir = Random.onUnitSphere;
        Vector3 spawnPos = playerTransform.position + randomDir * spawnRadius;
        
        // On s'assure qu'elle ne spawn pas trop bas
        if (spawnPos.y < playerTransform.position.y - 2f) 
            spawnPos.y = playerTransform.position.y + 5f;

        GameObject projectileGO = Instantiate(projectilePrefab, spawnPos, Quaternion.identity);
        S_TreeClimberProjectile projectile = projectileGO.GetComponent<S_TreeClimberProjectile>();
        
        if (projectile != null)
        {
            // La balle prend la direction du joueur AU MOMENT du tir
            Vector3 targetDir = (playerTransform.position - spawnPos).normalized;
            projectile.Initialize(targetDir, projectileSpeed, this);
        }
    }

    void CheckConditions()
    {
        if (playerTransform == null) return;

        // Condition de Victoire : Atteindre le haut de l'arbre
        if (playerTransform.position.y >= winYPosition)
        {
            Win();
        }

        // Condition de Défaite : Tomber dans le vide
        if (playerTransform.position.y <= fallThreshold)
        {
            Lose();
        }
    }

    public void Win()
    {
        if (gameFinished) return;
        gameFinished = true;
        WinGame();
    }

    public void Lose()
    {
        if (gameFinished) return;
        gameFinished = true;
        LoseGame();
    }
}
