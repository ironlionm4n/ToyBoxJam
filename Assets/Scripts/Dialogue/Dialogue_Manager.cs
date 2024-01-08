using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Ink.Runtime;

[RequireComponent(typeof(Dialogue_Holder))]
public class Dialogue_Manager : MonoBehaviour
{
    //Panel all dialogue UI is housed on
    [SerializeField] private GameObject dialogue_canvas;

    //Text field where dialogue will be displayed
    [SerializeField] private TMP_Text dialogue_text;

    //Image field where character sprite will be displayed
    [SerializeField] private Image sprite_location;

    //Ink story variable
    private Story currentStory;

    
    [SerializeField] private bool dialogueIsPlaying = false;

    // Start is called before the first frame update
    void Start()
    {
        //Testing
        DialogueStart();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void DialogueStart()
    {
        Dialogue_Holder holder = GetComponent<Dialogue_Holder>();

        TextAsset story_json = holder.InkJson;
        Sprite character_image = holder.Speaker_Sprite;

        if (sprite_location != null)
        {
            sprite_location.sprite = character_image;
        }
        else
        {
            Debug.LogWarning("No Sprite Location Assigned");
        }

        currentStory = new Story(story_json.text);

        dialogueIsPlaying = true;
        dialogue_canvas.SetActive(true);

    }
}
