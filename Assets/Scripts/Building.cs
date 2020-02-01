﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(AudioSource))]
public class Building : MonoBehaviour
{

	private float maxLife = 100f;
	private float life = 100f;
    private int clicks = 0;
	private Vector2 position;
	private float repair = 12.5f;
	private float damage = 8f;
	private AudioSource som;
	private SpriteRenderer sp;
	private Slider bar;
	public Sprite[] states;


	// Start is called before the first frame update
	void Start()
	{
		bar = GetComponentInChildren<Slider>();
		sp  = GetComponentInChildren<SpriteRenderer>();
		som = GetComponentInChildren<AudioSource>();
	}

	// Update is called once per frame
	void Update()
	{
		bar.value = life/maxLife;
		if(life < 30){
			sp.sprite = states[2];
		}else if (life < 60){
			sp.sprite = states[1];
		}else{
			sp.sprite = states[0];
		}
	}

    public void hakai()
    {
        
		life -= damage;
        if (life <= 0f)
        {
			gameObject.SetActive(false);
        }
        Debug.Log(this.gameObject.name + " " + life);

    }

    private void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(0))
        {

            if (life < 100f)
            {
				life = Mathf.Min(life + repair, maxLife);
				som.Play();
				if (clicks < 2 && (clicks == 0 || GameManager.instance.lastClicked == this))
				{
					GameManager.instance.lastClicked = this;
					clicks++;
				}	else if (GameManager.instance.lastClicked != this)
				{
					clicks = 0;
				}	else
				{
					GameManager.instance.spawnCoin(this.gameObject);
					clicks = 0;
				}
            }
        }
    }
}