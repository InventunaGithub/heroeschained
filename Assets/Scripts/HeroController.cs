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
    private NavMeshAgent agent;
    public float NormalAttackCooldown;
    private bool onCooldown = false;
    public float Radius = 25.0f; //This is sight Radius
    [Range(0, 360)]
    public float angle = 180f; //This is view Angle
    public GameObject TargetHeroGO;
    public GameObject[] Projectiles;
    private LayerMask obstructionMask;
    private List<Transform> inRange = new List<Transform>();
    private Animator heroAnimator;
    private bool isDead = false;
    private bool isAttacking = false;
    private bool isRunning = false;
    private bool victory = false;
    public bool SeeingTarget = false;
    public bool interwal = false;
    public float Distance = 0;

    void Start()
    {
        GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ;
        heroAnimator = GetComponent<Animator>();
        obstructionMask = LayerMask.GetMask("Obstacle");
        agent = gameObject.GetComponent<NavMeshAgent>();
        if (Owner.Team.Count == 0)
        {
            throw new System.Exception("Team does not have any members");
        }
        MainHero = Owner.Team[ChildNo];
        NormalAttackCooldown = 2 - (MainHero.Dexterity * 0.5f);
        if (NormalAttackCooldown < 0.7f)
        {
            NormalAttackCooldown = 0.7f;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
        if (MainHero.Health <= 0) //Death state
        {
            if(!isDead)
            {
                isDead = true;
                DyingAnimation();
            }
            Owner.Team.Remove(MainHero);
            agent.isStopped = true;
        }
        else if (Enemy.Team.Count == 0) //Victory state
        {
            if (!victory)
            {
                victory = true;
                VictoryAnimation();
            }
            agent.isStopped = true;
        }
        else
        {
            if (MainHero.AIType == AITypes.Closest && !interwal)
            {
                StartCoroutine(SelectClosestEnemy());
            }
            else if (MainHero.AIType == AITypes.Lockon)
            {
                if (TargetHeroGO == null)
                {
                    TargetHeroGO = ClosestEnemy();
                }
                else
                {
                    if (TargetHeroGO.GetComponent<HeroController>().MainHero.Health <= 0)
                    {
                        TargetHeroGO = ClosestEnemy();
                    }
                }
            }

            NavMeshPath path = new NavMeshPath();
            SeeingTarget = CanSeeTarget(TargetHeroGO);
            if (NavMesh.CalculatePath(transform.position, TargetHeroGO.transform.position, agent.areaMask, path))
            {
                float distance = Vector3.Distance(transform.position, path.corners[0]);

                for (int j = 1; j < path.corners.Length; j++)
                {
                    distance += Vector3.Distance(path.corners[j - 1], path.corners[j]);
                }
                Distance = distance;
                if((distance > MainHero.Range && distance > agent.stoppingDistance) || !CanSeeTarget(TargetHeroGO))
                {
                    agent.isStopped = false;
                    agent.SetPath(path);
                    transform.rotation = Quaternion.LookRotation(agent.velocity, Vector3.up);
                    if(!isRunning)
                    {
                        RunningAnimation();
                        isRunning = true;
                    }
                }
                else
                {
                    isRunning = false;
                    agent.isStopped = true;
                    if (!onCooldown && Enemy.Team.Count >= 0 && !isAttacking)
                    {
                        StartCoroutine(Attack(Enemy.Team[TargetHero]));
                    }
                }
            }
        }
    }

    public GameObject ClosestEnemy()
    {
        float closestTargetDistance = float.MaxValue;
        NavMeshPath path = null;
        NavMeshPath shortestPath = null;
        GameObject Temp= null;

        for (int i = 0; i < Enemy.Team.Count; i++)
        {
            if (Enemy.Team[i] == null)
            {
                continue;
            }
            path = new NavMeshPath();

            if (NavMesh.CalculatePath(transform.position, Enemy.Team[i].HeroObject.transform.position, agent.areaMask, path))
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
                    Temp = Enemy.Team[TargetHero].HeroObject;
                }
            }

        }
        return Temp;
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

    IEnumerator SelectClosestEnemy()
    {
        interwal = true;
        TargetHeroGO = ClosestEnemy();
        yield return new WaitForSeconds(1);
        interwal = false;
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
        return heroAnimator.GetCurrentAnimatorStateInfo(0).length > heroAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime && heroAnimator.GetCurrentAnimatorStateInfo(0).IsName(stateName);
    }

    public void DyingAnimation()
    {
        onCooldown = false;

        if (MainHero.HeroType == HeroTypes.Warrior)
        {
            heroAnimator.CrossFade("SSDeath", 0.1f);
        }
        else if (MainHero.HeroType == HeroTypes.Archer)
        {
            heroAnimator.CrossFade("DeathLeft", 0.1f);
        }

        else if (MainHero.HeroType == HeroTypes.Mage)
        {
            heroAnimator.CrossFade("DeathLeft", 0.1f);
        }

        else if(MainHero.HeroType == HeroTypes.Human)
        {
            heroAnimator.CrossFade("DeathLeft", 0.1f);
        }       
    }

    public void RunningAnimation()
    {
        if (MainHero.HeroType == HeroTypes.Warrior)
        {
            heroAnimator.CrossFade("SSRun", 0.1f);
        }
        else if (MainHero.HeroType == HeroTypes.Archer)
        {
            heroAnimator.CrossFade("BowRun", 0.1f);
        }

        else if (MainHero.HeroType == HeroTypes.Mage)
        {
            heroAnimator.CrossFade("MageRun", 0.1f);
        }
        else if (MainHero.HeroType == HeroTypes.Human)
        {
            heroAnimator.CrossFade("MageRun", 0.1f);
        }
    }
    public void IdleAnimation()
    {
        if (MainHero.HeroType == HeroTypes.Warrior)
        {
            heroAnimator.CrossFade("WarrIdle", 0.1f);
        }
        else if (MainHero.HeroType == HeroTypes.Archer)
        {
            heroAnimator.CrossFade("BowIdle", 0.1f);
        }
        else if (MainHero.HeroType == HeroTypes.Mage)
        {
            heroAnimator.CrossFade("MageIdle", 0.1f);
        }
        else if (MainHero.HeroType == HeroTypes.Human)
        {
            heroAnimator.CrossFade("ReadyIdle", 0.1f);
        }
        
    }

    public void NormalAttackAnimation()
    {
        if (MainHero.HeroType == HeroTypes.Warrior)
        {
            heroAnimator.CrossFade("SSAttack", 0.1f);
            StartCoroutine(CloseRangeAttack(Projectiles[4], TargetHeroGO.transform, new Vector3(0, 1, 0) , "SSAttack"));
        }
        else if (MainHero.HeroType == HeroTypes.Archer)
        {
            heroAnimator.CrossFade("ArrowDraw", 0.1f);
            StartCoroutine(ShootProjectile(Projectiles[0], Projectiles[1], MainHero.HeroObject.transform, TargetHeroGO.transform , new Vector3(0,1,0), 0.5f , "ArrowDraw"));
        }
        else if (MainHero.HeroType == HeroTypes.Mage)
        {
            heroAnimator.CrossFade("MagicCast", 0.1f);
            StartCoroutine(ShootProjectile(Projectiles[2], Projectiles[3], MainHero.HeroObject.transform, TargetHeroGO.transform, new Vector3(0, 1, 0), 0.5f, "MagicCast"));
        }
        else if (MainHero.HeroType == HeroTypes.Human)
        {
            heroAnimator.CrossFade("SSAttack", 0.1f);
            StartCoroutine(CloseRangeAttack(Projectiles[4], TargetHeroGO.transform, new Vector3(0, 1, 0), "SSAttack"));
        }
        
    }

    public void VictoryAnimation()
    {
        isDead = false;
        onCooldown = false;
        if (MainHero.HeroType == HeroTypes.Warrior)
        {
            heroAnimator.CrossFade("Victory", 0.1f);
        }
        else if (MainHero.HeroType == HeroTypes.Archer)
        {
            heroAnimator.CrossFade("Victory", 0.1f);
        }

        else if (MainHero.HeroType == HeroTypes.Mage)
        {
            heroAnimator.CrossFade("Victory", 0.1f);
        }
        else if (MainHero.HeroType == HeroTypes.Human)
        {
            heroAnimator.CrossFade("Victory", 0.1f);
        }
        
    }

}
