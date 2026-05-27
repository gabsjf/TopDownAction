using UnityEngine;

public class PlayerAnimationController : MonoBehaviour
{
    public Animator playerAnimator;
    private bool morto;

    public void PlayAnimation(string animationName)
    {
        if (morto) return;

        playerAnimator.Play(animationName);
    }

    public void PlayDeath()
    {
        morto = true;
        playerAnimator.Play("PlayerDead", 0, 0f);
    }

    private void Start()
    {
        PlayAnimation("PlayerIdle");
    }
}