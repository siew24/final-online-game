using System.Collections;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Animator),
                  typeof(BossDiedListener))]
public class BossController : MonoBehaviour
{
    [SerializeField] Transform rightTarget;
    [SerializeField] float frequency;
    [SerializeField] bool xDirection = false;

    [SerializeField] Animator laserGunAnimator;

    [Header("Boss Hit Attributes")]
    [SerializeField] float hitRange = 2f;
    [SerializeField] int hitDamage = 3;

    [Header("Boss Died Event")]
    [SerializeField] UnityEvent bossDiedEvent;

    BossDiedListener bossDiedListener;

    Animator animator;

    float radius = 0.825f;
    float duration = 0;

    int bossHitAnimation;
    int bossEnragedAnimation;
    int bossDiedAnimation;
    int laserGunAssembleAnimation;

    bool isStarted = false;
    bool isAttacking = false;
    bool isHitReady = false;
    bool isDied = false;

    Coroutine hitCoroutine = null;
    Coroutine laserCoroutine = null;
    Coroutine attackChoose = null;

    Vector3 targetedPosition = new();

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();

        bossHitAnimation = Animator.StringToHash("Hit Sequence");
        bossEnragedAnimation = Animator.StringToHash("Enraged");
        bossDiedAnimation = Animator.StringToHash("Died");

        Utils.GetListener(this, out bossDiedListener);
        bossDiedListener.Register(StartBossDieSequence);
    }

    // Update is called once per frame
    void Update()
    {
        if (!isStarted)
            return;

        if (!isAttacking && attackChoose == null)
        {
            attackChoose = StartCoroutine(nameof(ChooseAttackPattern));
        }
    }

    IEnumerator ChooseAttackPattern()
    {
        while (true)
        {
            if (isAttacking)
                yield return new WaitForSeconds(5f);

            // Roll a pattern
            int randomInt = Random.Range(0, 100);

            // 40% chance to unleash laser
            if (randomInt < 40)
            {
                Laser();
                continue;
            }

            // 50% chance to hit
            if (randomInt >= 40 && randomInt < 90)
            {
                animator.Play(bossHitAnimation);
                isAttacking = true;
                continue;
            }

            yield return new WaitForSeconds(5f);
        }
    }

    /*
        Rotates towards the target, and then hits at their instant position
    */
    IEnumerator HitCoroutine()
    {
        isHitReady = false;
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");

        // Find player with least health
        float minHealth = float.MaxValue;
        GameObject targetedPlayer = null;
        targetedPosition = new();

        foreach (GameObject player in players)
            if (player.GetComponent<Health>().currentHealth < minHealth)
            {
                targetedPlayer = player;
                minHealth = player.GetComponent<Health>().currentHealth;
            }

        // We found the least-health player - track its position until we encounter stop tracking event
        // Rotate to the targeted player by 1.5 seconds, then rigidly follow the player

        Vector3 lookPosition;
        float rotateDuration = 0f;
        Quaternion originalRotation = transform.rotation;
        while (true)
        {
            lookPosition = targetedPlayer.transform.position;

            if (!xDirection)
                transform.rotation = Quaternion.Lerp(originalRotation,
                    Quaternion.LookRotation((new Vector3(lookPosition.x, transform.position.y, lookPosition.z) - transform.position).normalized), rotateDuration / 1.5f);
            else
                transform.rotation = Quaternion.Lerp(originalRotation,
                    Quaternion.LookRotation((new Vector3(lookPosition.x, transform.position.y, lookPosition.z) - transform.position).normalized)
                    * Quaternion.Euler(0, 90, 0), rotateDuration / 1.5f);

            rotateDuration += Time.deltaTime;

            rotateDuration = Mathf.Clamp(rotateDuration, 0, 1.5f);

            if (isHitReady)
                break;

            yield return null;
        }

        rightTarget.position = lookPosition;
        targetedPosition = lookPosition;
    }

    /*
        Builds up, and then shoots 4 lasers that orbits around the boss
    */
    void Laser()
    {
        if (laserGunAnimator.GetCurrentAnimatorStateInfo(0).IsName("Laser Gun Assemble"))
        {
            foreach (Transform child in gameObject.transform.parent)
            {
                if (child.name != "Laser Gun")
                    continue;

                child.GetComponent<BossLaserGunController>().SpeedUpLaser();
                return;
            }
        }

        laserGunAnimator.Play("Laser Gun Assemble");

        return;
    }

    /*
        Encountering players for the first time 
    */
    IEnumerator BootUp()
    {

        yield return null;
    }

    public void StartSequence()
    {
        isStarted = true;
    }

    void StartBossDieSequence()
    {
        if (isDied)
            return;

        animator.Play(bossDiedAnimation);
        isDied = true;
    }

    void Died()
    {
        bossDiedEvent.Invoke();
    }

    // Triggers when boss Hit animation is started
    void OnBossHitAnimationStart()
    {
        StartCoroutine(nameof(HitCoroutine));
    }

    // Triggers when boss is ready to Hit
    void OnBossHitPlayerReady()
    {
        isHitReady = true;
    }

    // Triggers when boss hits the targeted position
    void OnBossHitTarget()
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");

        // Check if players are within a specific range
        foreach (GameObject player in players)
        {
            if (Vector3.Distance(targetedPosition, player.transform.position) > hitRange)
                continue;

            player.GetComponent<Health>().TakeDamage(hitDamage);
        }
    }

    // Triggers when boss Hit animation is finished
    void OnBossHitAnimationFinished()
    {
        isAttacking = false;
    }
}
