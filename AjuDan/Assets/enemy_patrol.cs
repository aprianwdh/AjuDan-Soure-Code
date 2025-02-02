using UnityEngine;
using System.Collections;
using UnityEditor;

public class EnemyPatrol : MonoBehaviour
{
    public GameObject pointA;
    public GameObject pointB;
    public float speed = 2f;
    public float waitTime = 2f;
    public Animator anim;
    public float rangeDistance = 2f;
    public GameObject player;
    public float attackDistance = 2f;
    public float chaseSpeed = 3f;
    public Transform ground_check;
    public float ground_check_distance = 0.1f;
    public LayerMask layer_mask;
    public LayerMask attack_layer;

    public int maxHealt = 5;

    private Rigidbody2D Rb;
    private Transform currentPoint;
    private bool isWaiting = false;
    private bool inRange = false;
    private bool facingRight = true;

    void Start()
    {
        Rb = GetComponent<Rigidbody2D>();
        currentPoint = pointB.transform;
    }

    void FixedUpdate()
    {
        bool hit = Physics2D.Raycast(ground_check.position, Vector2.down, ground_check_distance, layer_mask);

        // Menentukan apakah player berada dalam jangkauan musuh
        inRange = Vector2.Distance(transform.position, player.transform.position) <= rangeDistance;

        if (inRange && hit) // Mengejar dan menyerang player
        {
            //Debug.Log("Player dalam jangkauan");
            ChasePlayer();
        }
        else if (!isWaiting) // Jika tidak mengejar, lakukan patroli
        {
            MoveEnemy();
        }
    }

    void ChasePlayer()
    {
        // Flip musuh sesuai arah player
        if (player.transform.position.x > transform.position.x && !facingRight)
        {
            Flip();
        }
        else if (player.transform.position.x < transform.position.x && facingRight)
        {
            Flip();
        }

        // Jika jarak lebih jauh dari attackDistance, kejar player
        if (Vector2.Distance(transform.position, player.transform.position) > attackDistance)
        {
            anim.SetBool("attack", false);
            transform.position = Vector2.MoveTowards(transform.position, player.transform.position, chaseSpeed * Time.fixedDeltaTime);
        }
        else // Jika dalam attackDistance, lakukan serangan
        {
            anim.SetBool("attack", true);
        }
    }

    void MoveEnemy()
    {
        // Flip musuh sesuai arah gerakan
        if (currentPoint.position.x > transform.position.x && !facingRight)
        {
            Flip();
        }
        else if (currentPoint.position.x < transform.position.x && facingRight)
        {
            Flip();
        }

        // Gerakkan musuh menuju titik tujuan
        Vector2 direction = ((Vector2)currentPoint.position - (Vector2)transform.position).normalized;
        Rb.linearVelocity = new Vector2(direction.x * speed, 0); // Pastikan hanya bergerak horizontal

        // Jika musuh hampir sampai ke titik tujuan
        if (Vector2.Distance(transform.position, currentPoint.position) < 0.5f)
        {
            StartCoroutine(WaitAtPoint());
        }
    }

    IEnumerator WaitAtPoint()
    {
        isWaiting = true;
        anim.SetBool("isWaiting", true);
        Rb.linearVelocity = Vector2.zero;
        yield return new WaitForSeconds(waitTime);

        // Berpindah ke titik tujuan berikutnya
        currentPoint = (currentPoint == pointA.transform) ? pointB.transform : pointA.transform;

        isWaiting = false;
        anim.SetBool("isWaiting", false);
    }

    void Flip()
    {
        facingRight = !facingRight;
        transform.localScale = new Vector3(facingRight ? 1 : -1, 1, 1);
    }

    public void attack()
    {
        // Lakukan serangan
        Collider2D inRangeAttack =  Physics2D.OverlapCircle(transform.position, attackDistance, attack_layer);
        if (inRangeAttack)
        {
            if (inRangeAttack.GetComponent<Player>() != null)
            {
                inRangeAttack.GetComponent<Player>().Take_Damage(1);
                Debug.Log("Serang player");
            }
            else
            {
                Debug.Log("Tidak ada script player");
            }

        }
    }

    public void TakeDamage(int damage)
    {
        if (maxHealt >= 0)
        {
            maxHealt -= damage;
            if (maxHealt <= 0)
            {
                die();
            }
        }
    }

    public void die()
    {
        Destroy(this.gameObject);
        Debug.Log("Enemy mati");
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawSphere(pointA.transform.position, 0.5f);
        Gizmos.DrawSphere(pointB.transform.position, 0.5f);
        Gizmos.DrawLine(pointA.transform.position, pointB.transform.position);

        // Gizmo chase area
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, rangeDistance);

        // Gizmo attack area
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, attackDistance);

        // Gizmo ground check
        Gizmos.color = Color.yellow;
        Gizmos.DrawRay(ground_check.position, Vector2.down * ground_check_distance);
    }
}
