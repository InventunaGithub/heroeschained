using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
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
    private List<Hero> enemyTeam;
    private List<Hero> team;
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
    private bool interwal = false;
    private bool agentEnabled = false;
    public float Distance = 0;
    public GameObject HealthBarGO;
    public GameObject EnergyBarGO;
    private GameObject HeroHealthBar;
    private GameObject HeroEnergyBar;
    private Slider HealthBar;
    private Slider EnergyBar;
    private BattlefieldManager BM;
    private SpellManager SM;
    private Rigidbody RB;
    private CapsuleCollider CC;
    private Camera mainCam;

    void Start()
    {
        BM = GameObject.Find("Managers").GetComponent<BattlefieldManager>();
        SM = GameObject.Find("Managers").GetComponent<SpellManager>();
        RB = GetComponent<Rigidbody>();
        CC = GetComponent<CapsuleCollider>();
        MainHero = GetComponent<Hero>();
        agent = gameObject.GetComponent<NavMeshAgent>();
        heroAnimator = transform.GetChild(0).GetComponent<Animator>();
        MainHero.HeroObject = this.gameObject;
        if(gameObject.tag == "Team1")
        {
            team = BM.Team1;
            enemyTeam = BM.Team2;
        }
        else if(gameObject.tag == "Team2")
        {
            team = BM.Team2;
            enemyTeam = BM.Team1;
        }
        HeroHealthBar = Instantiate(HealthBarGO, gameObject.transform.position , gameObject.transform.rotation);
        HeroHealthBar.transform.SetParent(GameObject.Find("Canvas").transform);
        HeroEnergyBar = Instantiate(EnergyBarGO, gameObject.transform.position + -(Vector3.up * 0.1f), gameObject.transform.rotation);
        HeroEnergyBar.transform.SetParent(GameObject.Find("Canvas").transform);
        obstructionMask = LayerMask.GetMask("Obstacle");
        NormalAttackCooldown = 2 - (MainHero.Dexterity * 0.5f);
        if (NormalAttackCooldown < 0.7f)
        {
            NormalAttackCooldown = 0.7f;
        }
        HealthBar = HeroHealthBar.GetComponent<Slider>();
        HealthBar.maxValue = MainHero.MaxHealth;
        EnergyBar = HeroEnergyBar.GetComponent<Slider>();
        EnergyBar.maxValue = MainHero.MaxEnergy;
        agent.enabled = false;
        RB.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ;
        mainCam = Camera.main;

    }

    // Update is called once per frame
    void Update()
    {
        if (BM.GameStarted)
        {
            if (!agentEnabled)
            {
                agentEnabled = true;
                agent.enabled = true;
                Physics.IgnoreLayerCollision(LayerMask.NameToLayer("HeroLayer"), LayerMask.NameToLayer("HeroLayer"), false);
            }


            if (MainHero.Health <= 0 && enemyTeam.Count != 0  && !isDead) //Death state
            {
                HeroHealthBar.transform.position = mainCam.WorldToScreenPoint(MainHero.HeroObject.transform.position);
                HealthBar.value = MainHero.Health;
                HeroEnergyBar.transform.position = mainCam.WorldToScreenPoint(MainHero.HeroObject.transform.position + -(Vector3.up * 0.1f));
                EnergyBar.value = 0;
                isDead = true;
                DyingAnimation();
                team.Remove(MainHero);
                if (agent.enabled)
                {
                    agent.isStopped = true;
                }
                RB.constraints = RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezePositionZ;
                RB.useGravity = false;
                agent.enabled = false;
                CC.enabled = false;
            }
            else if (enemyTeam.Count == 0 && !victory && !isDead) //Victory state
            {
                HeroHealthBar.transform.position = mainCam.WorldToScreenPoint(MainHero.HeroObject.transform.position);
                HealthBar.value = MainHero.Health;
                HeroEnergyBar.transform.position = mainCam.WorldToScreenPoint(MainHero.HeroObject.transform.position + -(Vector3.up * 0.1f));
                EnergyBar.value = MainHero.Energy;
                if (MainHero.Health == 0)
                {
                    MainHero.Health = 1;
                }
                victory = true;
                VictoryAnimation();
                if (agent.enabled)
                {
                    agent.isStopped = true;
                }
                RB.constraints = RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezePositionZ;
                agent.enabled = false;
                CC.enabled = false;
            }
            else if(!victory && !isDead)
            {
                HeroHealthBar.transform.position = mainCam.WorldToScreenPoint(MainHero.HeroObject.transform.position);
                HealthBar.value = MainHero.Health;
                HeroEnergyBar.transform.position = mainCam.WorldToScreenPoint(MainHero.HeroObject.transform.position + - (Vector3.up * 0.1f));
                EnergyBar.value = MainHero.Energy;
                if (isRunning)
                {
                    RB.constraints = RigidbodyConstraints.None;
                    RB.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ;
                }
                else
                {
                    RB.constraints = RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezePositionZ;
                    RB.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ;
                }
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
                if (TargetHeroGO != null)
                {
                    SeeingTarget = CanSeeTarget(TargetHeroGO);
                    if (NavMesh.CalculatePath(transform.position, TargetHeroGO.transform.position, agent.areaMask, path))
                    {
                        float distance = Vector3.Distance(transform.position, path.corners[0]);
                        for (int j = 1; j < path.corners.Length; j++)
                        {
                            distance += Vector3.Distance(path.corners[j - 1], path.corners[j]);
                        }
                        Distance = distance;
                        if ((distance > MainHero.Range && distance > agent.stoppingDistance) || !CanSeeTarget(TargetHeroGO))
                        {
                            agent.isStopped = false;
                            agent.SetPath(path);
                            if(agent.velocity != Vector3.zero)
                            {
                                transform.rotation = Quaternion.LookRotation(agent.velocity, Vector3.up);
                            }
                            if (!isRunning)
                            {
                                RunningAnimation();
                                isRunning = true;
                            }
                        }
                        else
                        {
                            transform.DOLookAt(TargetHeroGO.transform.position , 0.1f);
                            isRunning = false;
                            agent.isStopped = true;
                            if (!onCooldown && enemyTeam.Count >= 0 && !isAttacking && MainHero.Energy < 10)
                            {
                                if (TargetHero < enemyTeam.Count)
                                {
                                    StartCoroutine(CooldownTimer(NormalAttackCooldown));
                                    SM.Cast(MainHero.Skills[0] , MainHero.HeroObject , enemyTeam[TargetHero].HeroObject);
                                }
                            }
                            else if(!onCooldown && enemyTeam.Count >= 0 && !isAttacking && MainHero.Energy >= 10)
                            {
                                if (TargetHero < enemyTeam.Count)
                                {
                                    StartCoroutine(CooldownTimer(NormalAttackCooldown));
                                    SM.Cast(MainHero.Skills[1], MainHero.HeroObject, enemyTeam[TargetHero].HeroObject);
                                    MainHero.Energy -= 10;
                                }
                            }
                        }
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

        for (int i = 0; i < enemyTeam.Count; i++)
        {
            if (enemyTeam[i] == null)
            {
                continue;
            }
            path = new NavMeshPath();

            if (NavMesh.CalculatePath(transform.position, enemyTeam[i].HeroObject.transform.position, agent.areaMask, path))
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
                    Temp = enemyTeam[TargetHero].HeroObject;
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

    IEnumerator CooldownTimer(float cooldown)
    {
        onCooldown = true;
        yield return new WaitForSeconds(cooldown);
        onCooldown = false;
    }

    public void DyingAnimation()
    {
        onCooldown = false;

        heroAnimator.CrossFade("Death", 0.1f);
    }

    public void RunningAnimation()
    {
        heroAnimator.CrossFade("Run", 0.1f);
     
    }
    public void IdleAnimation()
    {
        heroAnimator.CrossFade("Idle", 0.1f);
    }

   
    public void VictoryAnimation()
    {
        isDead = false;
        onCooldown = false;
        heroAnimator.CrossFade("Victory", 0.1f);
    }

    public void setIsAttacking(bool given)
    {
        isAttacking = given;
    }


}
