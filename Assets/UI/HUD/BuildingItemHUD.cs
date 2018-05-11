using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BuildingItemHUD : MonoBehaviour {

	private Image mSprite;
	private TextMeshProUGUI mText;
	private Button mButton;

	private Building mBuilding;

	void Awake () {
		mSprite = GetComponentInChildren<Image>();
		mText = GetComponentInChildren<TextMeshProUGUI>();
		mButton = GetComponentInChildren<Button>();
	}

	public void Configure(Building building) {
		mBuilding = building;

		mText.text = mBuilding.name;
		mSprite.sprite = mBuilding.sprite;
		mButton.onClick.AddListener(() => {
			GameController.main.activeBuilding = mBuilding;
		});
	}
	
}
