using UnityEngine;

public class AudioManager : MonoBehaviour
{

    public AudioSource AttackSfx;
    public AudioSource WalkSfx;
    public void PlayAttackSfx()
    {
        AttackSfx.Play();
    }

    public void PlayWalkSfx()
    {
        WalkSfx.Play();
    }

    public void StopWalkSfx()
    {
        WalkSfx.Stop();
    }
}
