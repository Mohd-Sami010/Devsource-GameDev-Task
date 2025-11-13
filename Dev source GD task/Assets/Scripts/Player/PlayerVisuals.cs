using UnityEngine;

public class PlayerVisuals :MonoBehaviour {

    private Animator playerAnimator;

    private void Start()
    {
        playerAnimator = GetComponent<Animator>();
        InputManager.Instance.OnJumpButtonPressed += InputManager_OnJumpButtonPressed;
    }

    private void InputManager_OnJumpButtonPressed()
    {
        playerAnimator.SetTrigger("Jump");
        SoundManager.Instance.PlayJumpSound();
    }
    private void Update()
    {
        if (Mathf.Abs(InputManager.Instance.GetMovementInput()) > 0.2f && !playerAnimator.GetBool("Moving"))
        {
            playerAnimator.SetBool("Moving", true);
            SoundManager.Instance.PlayMoveSound();
        }
        else if (Mathf.Abs(InputManager.Instance.GetMovementInput()) < 0.2f && playerAnimator.GetBool("Moving"))
        {
            playerAnimator.SetBool("Moving", false);
            SoundManager.Instance.StopMoveSound();
        }
    }
}
