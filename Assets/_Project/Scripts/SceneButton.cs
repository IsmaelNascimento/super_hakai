﻿using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SceneButton : MonoBehaviour
{
	public Button button;
	public string nomeCena = "";


	void Start ()
	{
		if(this.GetComponent<Button> ())
			button = this.GetComponent<Button> ();
		button.onClick = new Button.ButtonClickedEvent ();
		button.onClick.AddListener (() => Go ());
	}

	private void Go ()
	{
		SceneManager.LoadScene (nomeCena);
	}
}