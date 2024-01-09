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

    //Current text read from the ink file
    private string current_Text;
    
    [SerializeField] private bool dialogueIsPlaying = false;

    private const string SPEAKER_IDENTIFIER = "speaker";

    private const string PLAYER_VALUE = "player";
    private const string OTHER_SPEAKER_VALUE = "other";

    private Dialogue_Holder holder;

    // Start is called before the first frame update
    void Start()
    {
        holder = GetComponent<Dialogue_Holder>();

        //Testing
        DialogueStart();

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if(dialogueIsPlaying)
            {
                Continue();
            }
        }
    }

    public void DialogueStart()
    {
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
        Continue();
    }

    public void ExitDialogue()
    {
        dialogueIsPlaying=false;
        dialogue_canvas.SetActive(false);
    }

    //Continuse reading the story from the ink file
    public void Continue()
    {
        if (currentStory == null)
        {
            Debug.Log("Current story is null?");
            return;
        }

        if (currentStory.canContinue)
        {
            current_Text = "";
            //printingText = true;
            current_Text = currentStory.Continue();

            //Handles tags
            HandleTags(currentStory.currentTags);

            dialogue_text.text = current_Text;
            //lastCoroutine = StartCoroutine(PrintText(currentText));

        }
        else
        {
            ExitDialogue();
        }
    }

    /// <summary>
    /// Handles all tag interactions
    /// </summary>
    /// <param name="currentTags">Should only contain one ':' character per set in List</param>
    public void HandleTags(List<string> currentTags)
    {
        foreach(string tag in currentTags)
        {
            string[] split_tags = tag.Split(':');

            //Should only be an identifer and tag
            if (split_tags.Length != 2 ) {
                Debug.LogWarning("Error with parsing tags from current Ink Story");
            }

            string tag_identifier = split_tags[0].ToLower();
            string tag_value = split_tags[1].ToLower();

            switch(tag_identifier)
            {
                case SPEAKER_IDENTIFIER:
                    {
                        switch (tag_value)
                        {
                            case PLAYER_VALUE:
                                {
                                    if (sprite_location != null)
                                    {
                                        sprite_location.sprite = holder.Player_Speaking_Sprite;
                                    }
                                    else
                                    {
                                        Debug.LogWarning("No Sprite Location Assigned");
                                    }

                                    break;
                                }
                            case OTHER_SPEAKER_VALUE:
                                {
                                    if (sprite_location != null)
                                    {
                                        sprite_location.sprite = holder.Speaker_Sprite;
                                    }
                                    else
                                    {
                                        Debug.LogWarning("No Sprite Location Assigned");
                                    }

                                    break;
                                }
                            default:
                                {
                                    Debug.LogWarning("Error: Invalid value tag for speaker identifier");
                                    break;
                                }
                        }
                        break;
                    }
                default:
                    {
                        Debug.LogWarning("Error: Invalid tag identifier found in current ink story");
                        break;
                    }
            }


        }
    }
}
