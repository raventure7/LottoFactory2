using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class MenuView : MonoBehaviour {
	
	public GameView game;
	public RectTransform scrollPanel;
	public RectTransform levelRow;
	public GameObject menuGfx;
	public GameObject backBtn;

	TextAsset[] maps;
	int topLevel = 0;
	int currentLevel = 0;
	Vector2 scrollPosition = Vector2.zero;
	List<GameObject> levels;

	void Start () 
	{
		levels = new List<GameObject>();
		topLevel = PlayerPrefs.GetInt("level");
		maps = Resources.LoadAll<TextAsset>("Maps");

		ShowMenu();
	}
	
	void ShowMenu()
	{
		// refresh the menu - list each level as button
		backBtn.SetActive(false);
		menuGfx.SetActive(true);
		levelRow.gameObject.SetActive(true);

		foreach(GameObject l in levels)
			Destroy(l);

		levels = new List<GameObject>();

		for(int i = 0; i < maps.Length; i++)
		{
			scrollPanel.sizeDelta = new Vector2(scrollPanel.rect.width, (i+1)*levelRow.rect.height);

			RectTransform level = Instantiate(levelRow) as RectTransform;
			level.SetParent(scrollPanel);
			level.gameObject.name = i.ToString();
			level.anchoredPosition = new Vector2(0, i*-levelRow.rect.height);
			level.GetComponent<Button>().onClick.AddListener(() => { 
				OnLevelPress(int.Parse(level.gameObject.name)); 
			});

			Text t = level.GetComponent<Text>();
			t.text = "< LEVEL " + (i + 1) + " >";
			if(i > topLevel) t.color = Color.gray; // grey out levels that can't be played yet

			levels.Add(level.gameObject);
		}

		levelRow.gameObject.SetActive(false);
	}

	public void OnLevelPress(int lvl)
	{
		// when a level is pressed, hide the menu and create the level
		if(lvl <= topLevel)
		{
			menuGfx.SetActive(false);
			backBtn.SetActive(true);
			game.CreateLevel(lvl + 1);
			currentLevel = lvl;
		}
	}
	
	public void OnBackBtnPress()
	{
		// return to menu
		ShowMenu();
		game.DestroyLevel();
	}

	public void NextLevel ()
	{
		// level completed so increment the top available level in playerprefs
		if(currentLevel == topLevel && topLevel < maps.Length)
		{
			topLevel++;
			PlayerPrefs.SetInt("level", topLevel);
		}

		ShowMenu();
	}
}