using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class GameManager : MonoBehaviour
{
    public bool ActiveGame = true;
    public int CoinCount = 0;
    public bool gameWin = false;
    private string CurrentSceen;
    public Animator transition;

    public void Start()
    {
        CurrentSceen = SceneManager.GetActiveScene().name;
    }

    public void NextSceen()
    {
        gameWin = true;
        StartCoroutine(LoadLevel());
    }

    IEnumerator LoadLevel()
    {
        transition.SetTrigger("fadeIn");
        yield return new WaitForSeconds(1);
        SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex + 1);
        transition.SetTrigger("fadeOut");
        gameWin = false;
    }
}
