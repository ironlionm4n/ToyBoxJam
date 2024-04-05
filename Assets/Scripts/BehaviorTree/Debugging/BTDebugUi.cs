using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BTDebugUi : MonoBehaviour
{
    [SerializeField] BehaviorTree LinkedBT;
    [SerializeField] TextMeshProUGUI LinkedDebugText;
    // Start is called before the first frame update
    void Start()
    {
        LinkedDebugText.text = "";
    }

    // Update is called once per frame
    void Update()
    {
        LinkedDebugText.text = LinkedBT.GetDebugText();
    }
}
