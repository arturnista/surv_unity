using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InventoryItemHUD : MonoBehaviour {

	private Image mSpriteText;
	private TextMeshProUGUI mAmountText;

	private InventoryItem mInventoryItem;

	void Awake () {
		mSpriteText = transform.Find("Sprite").GetComponent<Image>();
		mAmountText = transform.Find("Amount").GetComponent<TextMeshProUGUI>();
	}

	public void Configure(InventoryItem invItem) {
		mInventoryItem = invItem;

		mSpriteText.sprite = mInventoryItem.item.sprite;
		mAmountText.text = mInventoryItem.amount.ToString();
	}

}
