﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ShowTime : MonoBehaviour
{
	TextMeshProUGUI text;
    // Start is called before the first frame update
    void Start()
    {
		text = GetComponentInChildren<TextMeshProUGUI> ();
    }

    // Update is called once per frame
    void Update()
    {
		text.text = Mathf.Floor(GameManager.instance.countdown) + "";
    }
}