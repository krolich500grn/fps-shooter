using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class WayPointZombieAI : MonoBehaviour
{
  public NavMeshAgent navAgent;
  public enum ZombieState {Walk, Chase, Attack, Dead};
  public ZombieState currentState = ZombieState.Walk;
  public Animator animator;
  public Transform player;
  public float zombieSphereRadius = 10f;
  public float chaseDistance = 10f;
  public float attackDistance = 2f;
  public float attackCooldown = 2f;
  public float attackDelay = 2f;
  public int damage = 10;
  public int health = 100;
  private CapsuleCollider capsuleCollider;
  private bool isAttacking;
  private bool isMoving = false;
  private float lastAttackTime;
  public GameObject bloodScreenEffect;
  private GameObject instantiatedObject;

  void Start()
  {
    GameObject playerObject = GameObject.FindWithTag("Player");
    if (playerObject != null) 
    {
      player = playerObject.transform;
    } else 
    {
      Debug.Log("Not found Player after spawn");
    }

    capsuleCollider = GetComponent<CapsuleCollider>();
    navAgent = GetComponent<NavMeshAgent>();
    lastAttackTime = -attackCooldown;
    animator = GetComponent<Animator>();
  }

  void Update() 
  {
    switch (currentState)
    {
      case ZombieState.Walk:
        if (!isMoving || navAgent.remainingDistance < 0.1f)
        { Walk(); }
        if (IsPlayerInRange(chaseDistance))
        {
          currentState = ZombieState.Chase;
        }
        break;
      case ZombieState.Chase:
        ChasePlayer();
        if (IsPlayerInRange(attackDistance))
        {
          currentState = ZombieState.Attack;
        }
        break;
      case ZombieState.Attack:
        AttackPlayer();
        if (!IsPlayerInRange(attackDistance))
        {
          currentState = ZombieState.Chase;
        }
        break;
      case ZombieState.Dead:
        animator.SetBool("IsChasing", false);
        animator.SetBool("IsAttacking", false);
        animator.SetBool("IsDead", true);
        navAgent.enabled = false;
        capsuleCollider.enabled = false;
        enabled = false;
        GameManager.instance.currentScore += 1;
        Debug.Log("dead");
        break;
    }
  }

  private bool IsPlayerInRange(float range)
  {
    return Vector3.Distance(transform.position, player.position) <= range;
  }

  private void Walk() 
  {
    navAgent.speed = 0.3f;
    Vector3 randomPosition = RandomNavMeshPosition();
    navAgent.SetDestination(randomPosition);
    isMoving = true;
  }

  private Vector3 RandomNavMeshPosition()
  {
    Vector3 randomDirection = Random.insideUnitSphere * zombieSphereRadius;
    randomDirection += transform.position;
    NavMeshHit hit;
    NavMesh.SamplePosition(randomDirection, out hit, zombieSphereRadius, NavMesh.AllAreas);

    return hit.position;
  }

  private void ChasePlayer()
  {
    navAgent.speed = 3f;
    animator.SetBool("IsChasing", true);
    animator.SetBool("IsAttacking", false);
    navAgent.SetDestination(player.position);
  }

  public void AttackPlayer()
  {
    animator.SetBool("IsChasing", false);
    animator.SetBool("IsAttacking", true);
    navAgent.SetDestination(transform.position);
    if (!isAttacking && Time.time - lastAttackTime >= attackCooldown) 
    {
      StartCoroutine(AttackWithDelay());
      StartCoroutine(ActivateBloodScreenEffect());
      lastAttackTime = Time.time;

      PlayerMovement playerMovement = player.GetComponent<PlayerMovement>();
      if (playerMovement != null)
      {
        playerMovement.TakeDamage(damage);
      }
    }
  }

  private IEnumerator AttackWithDelay()
  {
    isAttacking = true;
    yield return new WaitForSeconds(attackDelay);
    isAttacking = false;
  
  }

  private IEnumerator ActivateBloodScreenEffect()
  {
    InstantiatedObject();
    yield return new WaitForSeconds(attackDelay);
    DeleteObject();
  }

  public void TakeDamage(int damageAmount)
  {
    if (currentState == ZombieState.Dead)
    { return; }

    health -= damageAmount;

    if (health <= 0)
    { 
      health = 0;
      Die();
    }
  }

  private void Die()
  {
    currentState = ZombieState.Dead;
    //death scree
    Debug.Log("zombie died");
  }

   void InstantiatedObject()
   {
    instantiatedObject = Instantiate(bloodScreenEffect);
   }

   void DeleteObject()
   {
    if (instantiatedObject != null) 
    {
      Destroy(instantiatedObject);
      instantiatedObject = null; 
    }
   }

  
}
