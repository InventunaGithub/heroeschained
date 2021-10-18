using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityStandardAssets.Characters.ThirdPerson;

//Author: Mert Karavural
//Date: 28 Sep 2020

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
    private bool isAttacking;
    private bool targetChosed;
    public float radius = 25.0f; //This is sight Radius
    [Range(0, 360)]
    public float angle = 180f; //This is view Angle
    public GameObject playerRef;
    public LayerMask targetMask;
    public LayerMask obstructionMask;
    List<Transform> inRange = new List<Transform>();
    public Animator animator;

    void Start()
    {
        character = gameObject.GetComponent<ThirdPersonCharacter>();
        animator = gameObject.GetComponent<Animator>();
        obstructionMask = LayerMask.GetMask("Obstacle");
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
            if(MainHero.AIType == AITypes.Closest)
            {
                SetAgentPathToClosest();
            }
            else if (MainHero.AIType == AITypes.Lockon)
            {
                if(!targetChosed)
                {
                    SetAgentPathToClosest();
                }
                else
                {
                    if(playerRef.GetComponent<HeroController>().MainHero.Health <= 0)
                    {
                        SetAgentPathToClosest();
                    }
                }

                SetAgentPath(playerRef.gameObject);
            }

            if (closestTargetDistance < MainHero.Range && CanSeeTarget(playerRef))
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

    public void SetAgentPathToClosest()
    {
        closestTargetDistance = float.MaxValue;
        NavMeshPath Path = null;
        NavMeshPath ShortestPath = null;

        for (int i = 0; i < Enemy.Team.Count; i++)
        {
            if(Enemy.Team[i] == null)
            {
                continue;
            }
            Path = new NavMeshPath();

            if (NavMesh.CalculatePath(transform.position, Enemy.Team[i].HeroObject.transform.position, Agent.areaMask, Path))
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
                    playerRef = Enemy.Team[TargetHero].HeroObject;
                    if (CanSeeTarget(playerRef))
                    {
                        transform.LookAt(Enemy.Team[TargetHero].HeroObject.transform);
                    }
                    targetChosed = true;
                }
            }
            
        }

        if (ShortestPath != null)
        {
            Agent.SetPath(ShortestPath);
        }
    }

    public void SetAgentPath(GameObject target)
    {
        NavMeshPath path = new NavMeshPath();
   
        if (CanSeeTarget(playerRef))
        {
            transform.LookAt(target.transform);
        }

        NavMesh.CalculatePath(transform.position, target.transform.position, Agent.areaMask, path);

        Agent.SetPath(path);
    }

    IEnumerator Attack(Hero TargetHero)
    {
        if(MainHero.Skills.Count == 0)
        {
            throw new System.Exception("Hero Does Not Have Any Skills.");
        }
        isAttacking = true;
        TargetHero.EffectedBy(MainHero.UsedSkill(MainHero.Skills[0]));
        Debug.Log(Owner.Name +" "+MainHero.Name + " Attacked to " + TargetHero.Name +" with " + MainHero.UsedSkill(MainHero.Skills[0]).Name + " and dealt " + MainHero.UsedSkill(MainHero.Skills[0]).Power.ToString() + " Targe hero's remaining Health is "+  TargetHero.Health);
        yield return new WaitForSeconds(NormalAttackCooldown);
        isAttacking = false;
    }

    private bool CanSeeTarget(GameObject targetGO)
    {
        Transform target = targetGO.transform;
        Vector3 directionToTarget = (target.position - transform.position).normalized;

        if (Vector3.Angle(transform.forward, directionToTarget) < angle / 2)
        {
            float distanceToTarget = Vector3.Distance(transform.position, target.position);
            if (!Physics.Raycast(transform.position, directionToTarget, distanceToTarget, obstructionMask))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        else
        {
            return false;
        }
    }
}
