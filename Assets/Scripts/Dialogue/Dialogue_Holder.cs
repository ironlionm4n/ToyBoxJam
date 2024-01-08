using UnityEngine;

/// <summary>
/// File that holds the Ink JSON File and the Sprite of the current speaker for the dialogue manager.
/// </summary>
public class Dialogue_Holder : MonoBehaviour
{
    [SerializeField] private TextAsset inkJSON;

    public TextAsset InkJson { get { return inkJSON; } }

    [SerializeField] private Sprite speaker_sprite;

    public Sprite Speaker_Sprite { get { return speaker_sprite; } }
}
