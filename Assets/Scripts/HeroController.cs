using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityStandardAssets.Characters.ThirdPerson;

public class HeroController : MonoBehaviour
{
    ThirdPersonCharacter character;
    public Hero MainHero;
    public int TargetHero;
    public int ChildNo;
    public Player Owner;
    public Player Enemy;
    public List<Transform> Targets;
    public NavMeshAgent Agent;
    float closestTargetDistance;
    public float NormalAttackCooldown;
    bool isAttacking;
    public float radius = 25.0f; //This is sight Radius
    [Range(0, 360)]
    public float angle = 180f; //This is view Angle
    public GameObject playerRef;
    public LayerMask targetMask;
    public LayerMask obstructionMask;
    public bool canSeePlayer;
    List<Transform> inRange = new List<Transform>();
    public Animator animator;

    void Start()
    {
        character = gameObject.GetComponent<ThirdPersonCharacter>();
        animator = gameObject.GetComponent<Animator>();
        obstructionMask = LayerMask.GetMask("Obstacle");
        if (Owner.Name == "King1")
        {
            gameObject.layer = 7;
            targetMask = LayerMask.GetMask("King2");
        }
        else
        {
            gameObject.layer = 8;
            targetMask = LayerMask.GetMask("King1");
        }
        Agent = gameObject.GetComponent<NavMeshAgent>();
        Agent.updateRotation = false;
        MainHero = Owner.Team[ChildNo];
        closestTargetDistance = float.MaxValue;
        NormalAttackCooldown = 5 - (MainHero.Dexterity * 0.5f);
        if(NormalAttackCooldown < 0.7f)
        {
            NormalAttackCooldown = 0.7f;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (MainHero.Health <= 0)
        {
            Owner.Team.Remove(MainHero);
            animator.SetBool("DeathTrigger" , true);
            character.Move(Vector3.zero, false, false);
            Agent.isStopped = true;
        }
        else
        {
            //TODO Fix a bug : When you see an enemy you attack it and when the enemy you attacked dies , this code automaticly chooses another enemy and run to it. This should not supposed to happend because yhis hero never saw that enemy.
            if (inRange.Count == 0)
            {
                //This happens when there's no enemy in sight and never seen one before. So patrol.
            }
            StartCoroutine(FOVRoutine());
            ChooseTarget();
            if (closestTargetDistance < MainHero.Range && canSeePlayer)
            {
                Agent.SetDestination(gameObject.transform.position);
                if (!isAttacking && Enemy.Team.Count >= 0)
                {
                    StartCoroutine(Attack(Enemy.Team[TargetHero]));
                }
            }


            if (Agent.remainingDistance > Agent.stoppingDistance)
            {
                character.Move(Agent.desiredVelocity, false, false);
            }
            else
            {
                character.Move(Vector3.zero, false, false);
            }

            if (Enemy.Team.Count == 0)
            {
                character.Move(Vector3.zero, false, false);
                animator.SetBool("Win", true);
                Agent.isStopped = true;
            }

        }
    }

    public void ChooseTarget()
    {
        closestTargetDistance = float.MaxValue;
        NavMeshPath Path = null;
        NavMeshPath ShortestPath = null;

        for (int i = 0; i < Enemy.Team.Count; i++)
        {
            if (Enemy.Team[i] == null)
            {
                continue;
            }
            Path = new NavMeshPath();

            if (NavMesh.CalculatePath(transform.position, Enemy.Team[i].heroObject.transform.position, Agent.areaMask, Path))
            {
                float distance = Vector3.Distance(transform.position, Path.corners[0]);

                for (int j = 1; j < Path.corners.Length; j++)
                {
                    distance += Vector3.Distance(Path.corners[j - 1], Path.corners[j]);
                }

                if (distance < closestTargetDistance)
                {
                    closestTargetDistance = distance;
                    ShortestPath = Path;
                    TargetHero = i;
                    if(canSeePlayer)
                    {
                        transform.LookAt(Enemy.Team[TargetHero].heroObject.transform);
                    }
                    playerRef = Enemy.Team[TargetHero].heroObject;
                }
            }
            
        }


        if (ShortestPath != null)
        {
            Agent.SetPath(ShortestPath);
        }
    }

    IEnumerator Attack(Hero TargetHero)
    {
        isAttacking = true;
        TargetHero.effectedBy(MainHero.usedSkill(0));
        Debug.Log(Owner.Name +" "+MainHero.Name + " Attacked to " + TargetHero.Name +" with " + MainHero.usedSkill(0).Name + " and dealt " + MainHero.usedSkill(0).Power.ToString() + " Targe hero's remaining Health is "+  TargetHero.Health);
        yield return new WaitForSeconds(NormalAttackCooldown);
        isAttacking = false;
    }

    private IEnumerator FOVRoutine()
    {
        WaitForSeconds wait = new WaitForSeconds(0.2f);

        while (true)
        {
            yield return wait;
            FieldOfViewCheck();
        }
    }

    private void FieldOfViewCheck()
    {
        Collider[] rangeChecks = Physics.OverlapSphere(transform.position, radius, targetMask);
        if (rangeChecks.Length != 0)
        {
            Transform target = rangeChecks[0].transform;
            Vector3 directionToTarget = (target.position - transform.position).normalized;

            if (Vector3.Angle(transform.forward, directionToTarget) < angle / 2)
            {
                float distanceToTarget = Vector3.Distance(transform.position, target.position);
                if (!Physics.Raycast(transform.position, directionToTarget, distanceToTarget, obstructionMask))
                {
                    foreach (var element in rangeChecks)
                    {
                        if (!inRange.Contains(element.transform))
                        {
                            inRange.Add(element.transform);
                        }
                    }
                    canSeePlayer = true;
                }
                else
                {
                    canSeePlayer = false;
                }
            }
            else
            {
                canSeePlayer = false;
            }
        }
        else if (canSeePlayer)
        {
            canSeePlayer = false;
        }
    }
    //TODO Move , Normal Attack , Range , Ranged Attack , Find path ..
    //main things are going to be Attack() method , which will find and opponent to attack and use NormalAttack Card on it.
}
