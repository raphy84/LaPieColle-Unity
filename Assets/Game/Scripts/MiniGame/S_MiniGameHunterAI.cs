using UnityEngine;
using UnityEngine.AI;
using System.Collections;

[RequireComponent(typeof(NavMeshAgent))]
public class S_MiniGameHunterAI : MonoBehaviour
{
    private enum HunterState
    {
        Walk,
        Inspect,
        DashToStoredPos,
        Pursuit
    }

    [SerializeField] private HunterState currentState = HunterState.Walk;
    [SerializeField] private float walkSpeed = 2f;
    [SerializeField] private float runSpeed = 6.5f;
    [SerializeField] private float patrolDistance = 8f;
    [SerializeField] private float inspectDuration = 2f;
    [SerializeField] private float dashMaxTime = 4f; 
    
    [Header("Vision Cone & Obstacles")]
    [SerializeField] private float viewRadius = 8f;
    [SerializeField] private float viewAngle = 60f;
    [SerializeField] private float lightIntensity = 2f;
    [SerializeField] private LayerMask obstacleMask; // Configurer ceci pour inclure le sol/murs
    
    [SerializeField] private Transform playerTransform;
    [SerializeField] private S_MiniGameManager miniGameManager;
    private Animator animator;
    private Light spotLight;
    private NavMeshAgent agent;

    private Vector3 startPosition;
    private Vector3 patrolTarget;
    private Vector3 storedDashPosition;
    
    private int patrolDirection = 1;
    private float stateTimer = 0f;

    void Start()
    {
        animator = GetComponentInChildren<Animator>();
        agent = GetComponent<NavMeshAgent>();
        
        spotLight = GetComponentInChildren<Light>();
        if (spotLight == null || spotLight.type != LightType.Spot)
        {
            GameObject lightGO = new GameObject("VisionConeLight");
            lightGO.transform.SetParent(transform);
            lightGO.transform.localPosition = new Vector3(0, 1.5f, 0);
            lightGO.transform.localRotation = Quaternion.identity;
            spotLight = lightGO.AddComponent<Light>();
            spotLight.type = LightType.Spot;
        }

        spotLight.spotAngle = viewAngle;
        spotLight.range = viewRadius;
        spotLight.color = Color.yellow;
        spotLight.intensity = lightIntensity;

        startPosition = transform.position;
        patrolTarget = startPosition + transform.right * patrolDistance * patrolDirection;
        
        SetState(HunterState.Walk);
    }

    void Update()
    {
        CheckVision();

        switch (currentState)
        {
            case HunterState.Walk:
                UpdateWalkState();
                break;
            case HunterState.Inspect:
                UpdateInspectState();
                break;
            case HunterState.DashToStoredPos:
                UpdateDashToStoredPosState();
                break;
            case HunterState.Pursuit:
                UpdatePursuitState();
                break;
        }
    }

    private void CheckVision()
    {
        if (playerTransform == null) return;

        Vector3 dirToPlayer = (playerTransform.position - transform.position);
        float distance = dirToPlayer.magnitude;

        // Skcurit anti-moteur physique : si le Hunter est littralement sur le joueur, Game Over !
        if (distance < 1.5f)
        {
            if (miniGameManager != null)
            {
                miniGameManager.LoseGame();
                return;
            }
        }

        if (currentState == HunterState.Pursuit) return;

        if (distance <= viewRadius)
        {
            float angle = Vector3.Angle(transform.forward, dirToPlayer.normalized);
            if (angle <= viewAngle / 2f)
            {
                // Un check de Raycast pour tre sr qu'aucun mur n'est entre les deux
                // Origin un peu surleve pour ne pas taper le sol
                Vector3 origin = transform.position + Vector3.up * 1f; 
                Vector3 playerTarget = playerTransform.position + Vector3.up * 1f;
                Vector3 rayDir = (playerTarget - origin).normalized;

                if (!Physics.Raycast(origin, rayDir, distance, obstacleMask))
                {
                    // Aucun obstacle trouv, cne rouge
                    spotLight.color = Color.red;
                    SetState(HunterState.Pursuit);
                }
            }
        }
    }

    private void SetState(HunterState newState)
    {
        currentState = newState;
        stateTimer = 0f;

        switch (newState)
        {
            case HunterState.Walk:
                spotLight.color = Color.yellow;
                agent.speed = walkSpeed;
                agent.isStopped = false;
                if (animator != null) {
                    animator.SetFloat("Speed", 1f);
                }
                break;
            case HunterState.Inspect:
                agent.isStopped = true; // S'arrte de bouger
                if (animator != null) {
                    animator.SetFloat("Speed", 0f);
                }
                break;
            case HunterState.DashToStoredPos:
                spotLight.color = Color.red;
                agent.speed = runSpeed;
                agent.isStopped = false;
                if (animator != null) animator.SetFloat("Speed", 1f);
                break;
            case HunterState.Pursuit:
                spotLight.color = Color.red;
                agent.speed = runSpeed;
                agent.isStopped = false;
                if (animator != null) animator.SetFloat("Speed", 1f);
                break;
        }
    }

    private void UpdateWalkState()
    {
        stateTimer += Time.deltaTime;
        agent.SetDestination(patrolTarget);

        if (agent.remainingDistance < 0.2f && !agent.pathPending)
        {
            patrolDirection *= -1;
            patrolTarget = startPosition + transform.right * patrolDistance * patrolDirection;
        }

        if (stateTimer > Random.Range(3f, 6f))
        {
            SetState(HunterState.Inspect);
        }
    }

    private void UpdateInspectState()
    {
        stateTimer += Time.deltaTime;

        if (playerTransform != null)
        {
            Vector3 dirToPlayer = (playerTransform.position - transform.position).normalized;
            dirToPlayer.y = 0;
            if (dirToPlayer != Vector3.zero)
                transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(dirToPlayer), 300f * Time.deltaTime);
        }

        if (stateTimer >= inspectDuration)
        {
            if (playerTransform != null)
                storedDashPosition = playerTransform.position;
            SetState(HunterState.DashToStoredPos);
        }
    }

    private void UpdateDashToStoredPosState()
    {
        stateTimer += Time.deltaTime;

        agent.SetDestination(storedDashPosition);

        if ((agent.remainingDistance < 0.5f && !agent.pathPending) || stateTimer >= dashMaxTime)
        {
            SetState(HunterState.Walk);
        }
    }

    private void UpdatePursuitState()
    {
        if (playerTransform == null) return;
        agent.SetDestination(playerTransform.position);
    }

    private void TriggerLose(GameObject otherGO)
    {
        if (otherGO.CompareTag("Player") || (playerTransform != null && otherGO.transform == playerTransform))
        {
            if (miniGameManager != null)
            {
                miniGameManager.LoseGame();
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        TriggerLose(other.gameObject);
    }

    private void OnCollisionEnter(Collision collision)
    {
        TriggerLose(collision.gameObject);
    }
}
