using UnityEngine;
using System.Collections;
using Unity.VisualScripting;

public class EnemyPatrol : MonoBehaviour
{
    public GameObject pointA;
    public GameObject pointB;
    public float speed = 2f;
    public float waitTime = 2f; // Waktu berhenti di tiap titik
    public SpriteRenderer flip;
    public Animator anim;
    public float rangeDistance = 2f;
    public GameObject player;
    public float attackDistance = 2f;
    public float chaseSpeed = 3f;

    private Rigidbody2D Rb;
    private Transform currentPoint;
    private bool isWaiting = false; // Cek apakah musuh sedang berhenti
    private bool inRange = false;
    private bool inGround = true;


    void Start()
    {
        Rb = GetComponent<Rigidbody2D>();
        currentPoint = pointB.transform;
    }

    void Update()
    {
        //menentukan apakah ada di dalam range atau tidak
        if (Vector2.Distance(transform.position, player.transform.position) <= rangeDistance) 
        {
            inRange = true;
        }
        else 
        { 
            inRange= false;
        }

        if (inRange) // jika di dalam range akan mengejar danmenyerang player
        {
            //fungsi untuk flip enemy saat mengejar player
            if (player.transform.position.x > transform.position.x)
            {
                flip.flipX = false;
            }
            else 
            {
                flip.flipX = true;
            }

            //fungsi agar musuh bergerak ke arah player dan menyerang player
            if (Vector2.Distance(transform.position, player.transform.position) >  attackDistance)
            {
                anim.SetBool("attack",false);
                transform.position = Vector2.MoveTowards(transform.position, player.transform.position, chaseSpeed * Time.fixedDeltaTime);
            }

            else
            {
                anim.SetBool("attack",true);
            }
        }
        else // jika diluar range akan patroli
        {
            if (!isWaiting) // Hanya bergerak jika tidak sedang menunggu
            {
                MoveEnemy();
            }
        }

    }

    void MoveEnemy()
    {
        if (currentPoint == pointB.transform)
        {
            flip.flipX = false;
        }
        else
        {
            flip.flipX = true;
        }

        Vector2 targetPosition = currentPoint.position;
        Vector2 direction = (targetPosition - (Vector2)transform.position).normalized;

        Rb.linearVelocity = new Vector2(direction.x * speed, Rb.linearVelocity.y);

        // Jika musuh hampir sampai ke titik tujuan
        if (Vector2.Distance(transform.position, currentPoint.position) < 0.5f)
        {
            StartCoroutine(WaitAtPoint()); // Mulai delay
        }
    }

    IEnumerator WaitAtPoint()
    {
        isWaiting = true; // Berhenti bergerak
        anim.SetBool("isWaiting", isWaiting);
        Rb.linearVelocity = Vector2.zero; // Set kecepatan jadi 0
        yield return new WaitForSeconds(waitTime); // Tunggu beberapa detik

        // Pindah ke titik tujuan berikutnya
        if (currentPoint == pointA.transform)
        {
            currentPoint = pointB.transform;
            flip.flipX = false;
        }
        else
        {
            currentPoint = pointA.transform;
            flip.flipX = true;
        }

        isWaiting = false; // Mulai bergerak lagi
        anim.SetBool("isWaiting", isWaiting);

    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawSphere(pointA.transform.position, 0.5f);
        Gizmos.DrawSphere(pointB.transform.position, 0.5f);
        Gizmos.DrawLine(pointA.transform.position, pointB.transform.position);

        //gizmo chase area
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, rangeDistance);

        //gizmo untuk attack area
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackDistance);
    }


}
