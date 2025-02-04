using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Coin : MonoBehaviour
{
    public Animator anim;
    //public coinCounter 
    public Text coinCounter;

    private void Update()
    {
        coinCounter.text = FindAnyObjectByType<GameManager>().CoinCount.ToString();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Debug.Log("Player mengambil koin");
            FindAnyObjectByType<GameManager>().CoinCount++;
            anim.SetTrigger("collected");
            transform.position = new Vector3(transform.position.x, transform.position.y + 0.5f, transform.position.z);

            // Nonaktifkan Collider agar tidak menghalangi player
            GetComponent<Collider2D>().enabled = false;

            LeanTween.moveY(gameObject, transform.position.y + 1f, 0.5f).setOnComplete(() =>
            {
                Destroy(gameObject);
            });

        }
    }


}
