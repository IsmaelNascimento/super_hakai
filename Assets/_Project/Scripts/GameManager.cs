﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
	#region SINGLETON PATTERN
	public static GameManager _instance;
	public static GameManager instance
	{
		get {
			if (_instance == null)
			{
				_instance = GameObject.FindObjectOfType<GameManager>();

				if (_instance == null)
				{
					GameObject container = new GameObject("Arceus");
					_instance = container.AddComponent<GameManager>();
				}
			}

			return _instance;
		}
	}
	#endregion

	public Building[] buildings;
	public Building lastClicked;
	private Vector3 position;
	private Coroutine timer;
	public Transform kaiju;
	public Transform robot;
	private Vector3 kaijuPos;
	private Vector3 robotPos;
	private float count = 0;
    private bool animate = true;
    private const float repair = 12.5f;
    private const float damage = 8f;
    private const float bonusRepair = 100;
    private const float bonusDamage = 5f;
    private bool repairBonusActive = false;
    private bool damageBonusActive = false;
    private bool ClickerPowerOn = false;
	public float countdown = 30;

	public GameObject dialog;
	public GameObject shop;
	public Button shop0;
	public Button shop1;
	public Button shop2;
    public GameObject Wave1FinishDialogue;

    public GameObject Wave3FinishDialogue;


    public bool _canClicker = false;
    public bool _canShield = false;
    public bool _canFreeze = false;
    public bool _canMakeTheKO = false;
    public bool _canDontGiveUp = false;
	public bool _canZordTime = false;
	public Button[] buttons;
		
    // Start is called before the first frame update
    void Start()
    {
        PlayerPrefs.SetInt("wave", 1);
        PlayerPrefs.SetInt("coin", 0);

        buildings = FindObjectsOfType<Building>();
		timer = StartCoroutine(HakaiBuildingChoice());
		kaijuPos = kaiju.position;
		robotPos = robot.position;
	}

	void Update(){
        if (animate)
        {
            count += Time.deltaTime * 8;
            if (count > Mathf.PI * 2) count -= Mathf.PI * 2;
            kaiju.position = kaijuPos +
                Vector3.right * -Mathf.Cos(count) / 2 +
                Vector3.up * Mathf.Abs(Mathf.Sin(count)) / 2;
            robot.position = robotPos +
                Vector3.right * Mathf.Cos(count) / 2 +
                Vector3.up * Mathf.Abs(Mathf.Sin(count)) / 2;
        }

		if (!dialog.activeSelf)
			countdown -= Time.deltaTime;

		if (countdown <= 0){
			//StopAllCoroutines();
            _canClicker = false;
            _canShield = false;
            _canFreeze = false;
            _canMakeTheKO = false;
            _canDontGiveUp = false;
            _canZordTime = false;
            setWave();
		}

		if(_canFreeze){
			buttons[0].interactable = false;
		}else{
			buttons[0].interactable = true;
		}

		bool test = true;
		foreach (var building in buildings){
			test = test & building.life < 1;
		}
		if (test){
			SceneManager.LoadScene(0);
		}
	}

	public void spawnCoin(GameObject building)
	{
		for (int i = 0; i < 4; i++)
		{
			Vector2 buildingSize = building.GetComponent<SpriteRenderer>().bounds.size;
			position = new Vector3(Random.Range(-buildingSize.x/2, buildingSize.x/2), Random.Range(-buildingSize.y/2, buildingSize.y/2), -1) + building.transform.position;
			Instantiate(Resources.Load("Coin") as GameObject, position, Quaternion.identity);
		}
	}

	private IEnumerator HakaiBuildingChoice()
	{
		int buildingNumber = Random.Range( 0, buildings.Length);
		buildings[buildingNumber].hakai();
		yield return new WaitForSeconds(1);
		timer = StartCoroutine(HakaiBuildingChoice());
    }

    public void ClickerPowerupOn()
    {
        _canClicker = false;
        ClickerPowerOn = true;
    }

    public void ShieldPowerupOn()
    {
        _canShield = false;
        damageBonusActive = true;

    }



    public void FreezePowerupOn()
    {
        if (_canFreeze == true)
        {
            _canFreeze = false;
            animate = false;
            StopAllCoroutines();
            StartCoroutine(FreezePowerDownRoutime());
        }
        
    }

    public IEnumerator FreezePowerDownRoutime()
    {
        
        yield return new WaitForSeconds(4f);
        timer = StartCoroutine(HakaiBuildingChoice());
        animate = true;
    }

    public void MakeTheKOPowerupOn()
    {
        if (_canMakeTheKO == true)
        {
            _canMakeTheKO = false;
            countdown = 1;
        }
    }

    public void DontGiveUpPowerupOn()
    {
        if (_canDontGiveUp == true)
        {
            _canDontGiveUp = false;
            countdown += 20;
        }
        
    }

    public void ZordTimePowerupOn()
    {
        if (_canZordTime == true)
        {
            _canZordTime = false;
            repairBonusActive = true;
        }
       
    }

    public float getRepair() {
        float r = repairBonusActive ? bonusRepair : repair;
        r *= ClickerPowerOn ? 1.5f : 1; 
        repairBonusActive = false;
        return r;
    }
    public float getDamage() {
        
        float r = damageBonusActive ? bonusDamage : damage;
        if (PlayerPrefs.GetInt("wave", 1) >= 3 && r == 8)
        {
            r = 10;
        }
        return r;

    }

	public void setWave(){
		foreach (var building in buildings){
			building.life = building.maxLife;
		}
		PlayerPrefs.SetInt("wave", PlayerPrefs.GetInt("wave") + 1);
		shop0.onClick.RemoveAllListeners ();
		shop1.onClick.RemoveAllListeners ();
		shop2.onClick.RemoveAllListeners ();
		switch(PlayerPrefs.GetInt("wave")){
		case 2:
            dialog.SetActive(true);
            Wave1FinishDialogue.gameObject.SetActive(true);
			shop0.onClick.AddListener (() => PowerUpsShop.buy (0));
			shop1.onClick.AddListener (() => PowerUpsShop.buy (1));
			shop2.onClick.AddListener (() => PowerUpsShop.buy (4));
			countdown = 42;
			break;
		case 3:
			shop0.onClick.AddListener (() => PowerUpsShop.buy (1));
			shop1.onClick.AddListener (() => PowerUpsShop.buy (2));
			shop2.onClick.AddListener (() => PowerUpsShop.buy (3));
			countdown = 56;
			break;
		case 4:
            dialog.SetActive(true);
            Wave3FinishDialogue.gameObject.SetActive(true);
            shop0.onClick.AddListener (() => PowerUpsShop.buy (4));
			shop1.onClick.AddListener (() => PowerUpsShop.buy (5));
			shop2.onClick.AddListener (() => PowerUpsShop.buy (0));
			countdown = 68;
			break;
		case 5:
		default:
			shop0.onClick.AddListener (() => PowerUpsShop.buy (Random.Range(0,6)));
			shop1.onClick.AddListener (() => PowerUpsShop.buy (Random.Range(0,6)));
			shop2.onClick.AddListener (() => PowerUpsShop.buy (Random.Range(0,6)));
			countdown = 80;
			break;
		}
	}
}