using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using DG.Tweening;

//Author: Mert Karavural
//Date: 28 Sep 2020

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(CapsuleCollider))]
[RequireComponent(typeof(Animator))]
public class HeroController : MonoBehaviour
{

    public Hero MainHero;
    public int TargetHero;
    public int ChildNo;
    public Player Owner;
    public Player Enemy;
    public NavMeshAgent Agent;
    float closestTargetDistance;
    public float NormalAttackCooldown;
    private bool onCooldown = false;
    private bool targetChosed;
    public float Radius = 25.0f; //This is sight Radius
    [Range(0, 360)]
    public float angle = 180f; //This is view Angle
    public GameObject PlayerRef;
    public GameObject[] Projectiles;
    public LayerMask ObstructionMask;
    List<Transform> inRange = new List<Transform>();
    public Animator HeroAnimator;
    private bool isDead = false;
    private bool isRunning = false;
    private bool isAttacking = false;

    void Start()
    {
        GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ;
        HeroAnimator = GetComponent<Animator>();
        ObstructionMask = LayerMask.GetMask("Obstacle");
        Agent = gameObject.GetComponent<NavMeshAgent>();
        if (Owner.Team.Count == 0)
        {
            throw new System.Exception("Team does not have any members");
        }
        MainHero = Owner.Team[ChildNo];
        closestTargetDistance = float.MaxValue;
        NormalAttackCooldown = 2 - (MainHero.Dexterity * 0.5f);
        if (NormalAttackCooldown < 0.7f)
        {
            NormalAttackCooldown = 0.7f;
        }
    }

    // Update is called once per frame
    void Update()
    {
       
        if (MainHero.Health <= 0)
        {
            if(!isDead)
            {
                isDead = true;
                DyingAnimation();
            }
            Owner.Team.Remove(MainHero);
            Agent.isStopped = true;
        }
        else
        {
            if (MainHero.AIType == AITypes.Closest)
            {
                SetAgentPathToClosest();
            }
            else if (MainHero.AIType == AITypes.Lockon)
            {
                if (!targetChosed)
                {
                    SetAgentPathToClosest();
                }
                else
                {
                    if (PlayerRef.GetComponent<HeroController>().MainHero.Health <= 0)
                    {
                        SetAgentPathToClosest();
                    }
                }

                SetAgentPath(PlayerRef.gameObject);
            }

            if (closestTargetDistance < MainHero.Range && CanSeeTarget(PlayerRef))
            {
                Agent.isStopped = true;
                if (!onCooldown && Enemy.Team.Count >= 0 && !isAttacking)
                {
                    StartCoroutine(Attack(Enemy.Team[TargetHero]));
                }
            }

            if (Agent.remainingDistance <= Agent.stoppingDistance)
            {
                Agent.SetDestination(gameObject.transform.position);
                isRunning= false;
            }

            if (Enemy.Team.Count == 0)
            {
                VictoryAnimation();
                Agent.isStopped = true;
            }

            if (isRunning && !isAttacking)
            {
                RunningAnimation();
            }

            if (!isAttacking && Enemy.Team.Count > 0 && !isRunning)
            {
                IdleAnimation();
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
            if (Enemy.Team[i] == null)
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
            
            if (Agent.remainingDistance > Agent.stoppingDistance || Agent.remainingDistance == 0)
            {
                Agent.isStopped = false;
                Agent.SetPath(shortestPath);
                if (!isRunning && !onCooldown)
                {
                    isRunning = true;
                }
            }
            
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

        if (Agent.remainingDistance > Agent.stoppingDistance || Agent.remainingDistance == 0)
        {
            Agent.isStopped = false;
            Agent.SetPath(path);
            if (!isRunning && !onCooldown)
            {
                isRunning = true;
            }
        }
       
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

    IEnumerator Attack(Hero TargetHero)
    {
        if (MainHero.Skills.Count == 0)
        {
            throw new System.Exception("Hero Does Not Have Any Skills.");
        }
        TargetHero.EffectedBy(MainHero.UsedSkill(MainHero.Skills[0]));
        Debug.Log(Owner.Name + " " + MainHero.Name + " Attacked to " + TargetHero.Name + " with " + MainHero.UsedSkill(MainHero.Skills[0]).Name + " and dealt " + MainHero.UsedSkill(MainHero.Skills[0]).Power.ToString() + " Targe hero's remaining Health is " + TargetHero.Health);
        onCooldown = true;
        NormalAttackAnimation();
        yield return new WaitForSeconds(NormalAttackCooldown);
        onCooldown = false;
    }
    IEnumerator ShootProjectile(GameObject projectile , GameObject splash, Transform shootingPosition , Transform targetPosition , Vector3 offset , float travelTime , string animation)
    {
        isAttacking = true;
        while(AnimatorIsPlaying(animation))
        {
            yield return new WaitForSeconds(0.01f);
        }
        GameObject projectileGO = Instantiate(projectile, offset + shootingPosition.position , shootingPosition.rotation);
        projectileGO.transform.DOMove(targetPosition.position + offset, 0.5f);
        yield return new WaitForSeconds(travelTime);
        Destroy(projectileGO);
        GameObject splashGO = Instantiate(splash, targetPosition.position + offset, targetPosition.rotation);
        Destroy(splashGO, 0.3f);
        isAttacking = false;
    }

    IEnumerator CloseRangeAttack(GameObject splash,Transform targetPosition, Vector3 offset, string animation)
    {
        isAttacking = true;
        while (AnimatorIsPlaying(animation))
        {
            yield return new WaitForSeconds(0.01f);
        }
        GameObject splashGO = Instantiate(splash, targetPosition.position + offset, targetPosition.rotation);
        Destroy(splashGO, 0.3f);
        isAttacking = false;
    }

    bool AnimatorIsPlaying(string stateName)
    {
        return HeroAnimator.GetCurrentAnimatorStateInfo(0).length > HeroAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime && HeroAnimator.GetCurrentAnimatorStateInfo(0).IsName(stateName);
    }

    public void DyingAnimation()
    {
        isRunning = false;
        onCooldown = false;

        if (MainHero.HeroType == HeroTypes.Warrior)
        {
            HeroAnimator.CrossFade("SSDeath", 0.1f);
        }
        else if (MainHero.HeroType == HeroTypes.Archer)
        {
            HeroAnimator.CrossFade("DeathLeft", 0.1f);
        }

        else if (MainHero.HeroType == HeroTypes.Mage)
        {
            HeroAnimator.CrossFade("DeathLeft", 0.1f);
        }

        else if(MainHero.HeroType == HeroTypes.Human)
        {
            HeroAnimator.CrossFade("DeathLeft", 0.1f);
        }

       
       
    }

    public void RunningAnimation()
    {
        if (MainHero.HeroType == HeroTypes.Warrior)
        {
            HeroAnimator.CrossFade("SSRun", 0.1f);
        }
        else if (MainHero.HeroType == HeroTypes.Archer)
        {
            HeroAnimator.CrossFade("BowRun", 0.1f);
        }

        else if (MainHero.HeroType == HeroTypes.Mage)
        {
            HeroAnimator.CrossFade("MageRun", 0.1f);
        }
        else if (MainHero.HeroType == HeroTypes.Human)
        {
            HeroAnimator.CrossFade("MageRun", 0.1f);
        }
    }
    public void IdleAnimation()
    {
        if (MainHero.HeroType == HeroTypes.Warrior)
        {
            HeroAnimator.CrossFade("WarrIdle", 0.1f);
        }
        else if (MainHero.HeroType == HeroTypes.Archer)
        {
            HeroAnimator.CrossFade("BowIdle", 0.1f);
        }
        else if (MainHero.HeroType == HeroTypes.Mage)
        {
            HeroAnimator.CrossFade("ReadyIdle", 0.1f);
        }
        else if (MainHero.HeroType == HeroTypes.Human)
        {
            HeroAnimator.CrossFade("ReadyIdle", 0.1f);
        }
        
    }

    public void NormalAttackAnimation()
    {
        isRunning = false;

        if (MainHero.HeroType == HeroTypes.Warrior)
        {
            HeroAnimator.CrossFade("SSAttack", 0.1f);
            StartCoroutine(CloseRangeAttack(Projectiles[4], PlayerRef.transform, new Vector3(0, 1, 0) , "SSAttack"));
        }
        else if (MainHero.HeroType == HeroTypes.Archer)
        {
            HeroAnimator.CrossFade("ArrowDraw", 0.1f);
            StartCoroutine(ShootProjectile(Projectiles[0], Projectiles[1], MainHero.HeroObject.transform, PlayerRef.transform , new Vector3(0,1,0), 0.5f , "ArrowDraw"));
        }
        else if (MainHero.HeroType == HeroTypes.Mage)
        {
            HeroAnimator.CrossFade("MagicCast", 0.1f);
            StartCoroutine(ShootProjectile(Projectiles[2], Projectiles[3], MainHero.HeroObject.transform, PlayerRef.transform, new Vector3(0, 1, 0), 0.5f, "MagicCast"));
        }
        else if (MainHero.HeroType == HeroTypes.Human)
        {
            HeroAnimator.CrossFade("SSAttack", 0.1f);
            StartCoroutine(CloseRangeAttack(Projectiles[4], PlayerRef.transform, new Vector3(0, 1, 0), "SSAttack"));
        }
        
    }

    public void VictoryAnimation()
    {
        isRunning = false;
        isDead = false;
        onCooldown = false;
        if (MainHero.HeroType == HeroTypes.Warrior)
        {
            HeroAnimator.CrossFade("Victory", 0.1f);
        }
        else if (MainHero.HeroType == HeroTypes.Archer)
        {
            HeroAnimator.CrossFade("Victory", 0.1f);
        }

        else if (MainHero.HeroType == HeroTypes.Mage)
        {
            HeroAnimator.CrossFade("Victory", 0.1f);
        }
        else if (MainHero.HeroType == HeroTypes.Human)
        {
            HeroAnimator.CrossFade("Victory", 0.1f);
        }
        
    }

}
