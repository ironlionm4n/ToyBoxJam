using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class InputController : ScriptableObject
{
    /// <summary>
    /// If you highlight over this in another file you will read this text and that's really fucking useful.
    /// </summary>
    /// <returns>The value will be in the range -1...1 for keyboard and joystick input. Since input is not smoothed, keyboard input will always be either -1, 0 or 1.</returns>
    public abstract float RetrieveMovementInput();
    public abstract bool RetrieveJumpInput();
    public abstract bool RetrieveJumpInputHeld();
}