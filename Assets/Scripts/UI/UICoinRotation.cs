using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UICoinRotation : MonoBehaviour
{
    [SerializeField] private float duration;
    [SerializeField] private Sprite[] sprites;

    private Image _image;
    private int _index = 0;
    private float _timer = 0f;

    private void Start()
    {
        _image = GetComponent<Image>();
    }

    private void Update()
    {
        if ((_timer += Time.deltaTime) >= (duration / sprites.Length))
        {
            _timer = 0f;
            _image.sprite = sprites[_index];
            _index = (_index + 1) % sprites.Length;
        }
    }
}
