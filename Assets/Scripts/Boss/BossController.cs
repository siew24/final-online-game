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

    [Header("Boss Died Event")]
    [SerializeField] UnityEvent bossDiedEvent;

    BossDiedListener bossDiedListener;

    Animator animator;

    float radius = 0.825f;
    float duration = 0;

    int bossEnragedAnimation;
    int bossDiedAnimation;
    int laserGunAssembleAnimation;

    bool isStarted = false;
    bool isAttacking = false;
    bool isDied = false;

    Coroutine hitCoroutine = null;
    Coroutine laserCoroutine = null;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();

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

        if (!isAttacking)
        {
            // Roll a pattern
            int randomInt = Random.Range(0, 100);

            // 40% chance to unleash laser if not unleashed
            if (randomInt < 40 && !laserGunAnimator.GetCurrentAnimatorStateInfo(0).IsName("Laser Gun Assemble"))
            {
                Laser();
                return;
            }

            // 50% chance to hit
            if (randomInt >= 40 && randomInt < 90)
            {
                StartCoroutine(nameof(Hit));
                isAttacking = true;
            }
        }
    }

    void FixedUpdate()
    {

    }

    Vector3 RandomPosition()
    {
        float theta = UnityEngine.Random.Range(0, 2 * Mathf.PI);
        float targetRadius = UnityEngine.Random.Range(.25f, radius);
        return new(targetRadius * Mathf.Cos(theta), 1, targetRadius * Mathf.Sin(theta));
    }

    /*
        Rotates towards the target, and then hits at their instant position
    */
    IEnumerator Hit()
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");

        float rotateDuration = 0f;
        Vector3 lookPosition = RandomPosition();
        Quaternion originalRotation = transform.rotation;
        while (true)
        {
            if (!xDirection)
                transform.rotation = Quaternion.Lerp(originalRotation,
                    Quaternion.LookRotation((new Vector3(lookPosition.x, transform.position.y, lookPosition.z) - transform.position).normalized), rotateDuration / 1.5f);
            else
                transform.rotation = Quaternion.Lerp(originalRotation,
                    Quaternion.LookRotation((new Vector3(lookPosition.x, transform.position.y, lookPosition.z) - transform.position).normalized)
                    * Quaternion.Euler(0, 90, 0), rotateDuration / 1.5f);
            rotateDuration += Time.deltaTime;
            if (rotateDuration >= 1.5f)
                break;

            yield return null;
        }

        rightTarget.position = lookPosition;
        isAttacking = false;
    }

    /*
        Builds up, and then shoots 4 lasers that orbits around the boss
    */
    void Laser()
    {
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
}
