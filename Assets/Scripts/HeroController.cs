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
    public float Radius = 25.0f; //This is sight Radius
    [Range(0, 360)]
    public float angle = 180f; //This is view Angle
    public GameObject PlayerRef;
    public LayerMask ObstructionMask;
    List<Transform> inRange = new List<Transform>();
    public Animator HeroAnimator;

    void Start()
    {
        character = gameObject.GetComponent<ThirdPersonCharacter>();
        HeroAnimator = gameObject.GetComponent<Animator>();
        ObstructionMask = LayerMask.GetMask("Obstacle");
        Agent = gameObject.GetComponent<NavMeshAgent>();
        Agent.updateRotation = false;
        if(Owner.Team.Count == 0)
        {
            throw new System.Exception("Team does not have any members");
        }
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
            HeroAnimator.SetBool("DeathTrigger" , true);
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
                    if(PlayerRef.GetComponent<HeroController>().MainHero.Health <= 0)
                    {
                        SetAgentPathToClosest();
                    }
                }

                SetAgentPath(PlayerRef.gameObject);
            }

            if (closestTargetDistance < MainHero.Range && CanSeeTarget(PlayerRef))
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
                HeroAnimator.SetBool("Win", true);
                Agent.isStopped = true;
            }

        }
    }

    public void SetAgentPathToClosest()
    {
        closestTargetDistance = float.MaxValue;
        NavMeshPath path = null;
        NavMeshPath shortestPath = null;

        for (int i = 0; i < Enemy.Team.Count; i++)
        {
            if(Enemy.Team[i] == null)
            {
                continue;
            }
            path = new NavMeshPath();

            if (NavMesh.CalculatePath(transform.position, Enemy.Team[i].HeroObject.transform.position, Agent.areaMask, path))
            {
                float distance = Vector3.Distance(transform.position, path.corners[0]);

                for (int j = 1; j < path.corners.Length; j++)
                {
                    distance += Vector3.Distance(path.corners[j - 1], path.corners[j]);
                }

                if (distance < closestTargetDistance)
                {
                    closestTargetDistance = distance;
                    shortestPath = path;
                    TargetHero = i;
                    PlayerRef = Enemy.Team[TargetHero].HeroObject;
                    if (CanSeeTarget(PlayerRef))
                    {
                        transform.LookAt(Enemy.Team[TargetHero].HeroObject.transform);
                    }
                    targetChosed = true;
                }
            }
            
        }

        if (shortestPath != null)
        {
            Agent.SetPath(shortestPath);
        }
    }

    public void SetAgentPath(GameObject target)
    {
        NavMeshPath path = new NavMeshPath();
   
        if (CanSeeTarget(PlayerRef))
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
        if(MainHero.Range <= 5)
        {
            HeroAnimator.CrossFade("Punch", 0.01f);
        }
        if(MainHero.Range <= 10)
        {
            HeroAnimator.CrossFade("ShootArrow", 0.01f);
        }
        if(MainHero.Range > 10)
        {
            HeroAnimator.CrossFade("SpellCast1", 0.01f);
        }

        //randominize pucnhing animations.
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
            if (!Physics.Raycast(transform.position, directionToTarget, distanceToTarget, ObstructionMask))
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
