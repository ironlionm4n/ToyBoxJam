using UnityEngine;

[CreateAssetMenu(fileName = "PlayerController", menuName = "InputController/PlayerController")]
public class PlayerController : InputController
{

    public override float RetrieveMovementInput()
    {
        return Input.GetAxisRaw("Horizontal");
    }

    public override bool RetrieveJumpInput()
    {
        return Input.GetButtonDown("Jump");
    }

    public override bool RetrieveJumpInputHeld()
    {
        return Input.GetButton("Jump");
    }
}