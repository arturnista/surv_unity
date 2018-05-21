using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DwarfItemHUD : MonoBehaviour {

	private DwarfStatus mDwarfStatus;
	private DwarfSelect mDwarfSelect;
	private DwarfBehaviour mDwarfBehaviour;

	private TextMeshProUGUI mNameText;
	private Image mHealthImage;
	private Image mHungryImage;
	private Image mFatigueImage;
	private Image mSelectImage;
	private Button mSelectButton;

	void Awake() {
		mSelectButton = GetComponent<Button>();
		mSelectButton.onClick.AddListener(OnSelect);

		mNameText = transform.Find("Name").GetComponent<TextMeshProUGUI>();
		mHealthImage = transform.Find("Health").GetComponent<Image>();
		mHungryImage = transform.Find("Hungry").GetComponent<Image>();
		mFatigueImage = transform.Find("Fatigue").GetComponent<Image>();

		mSelectImage = transform.Find("Select").GetComponent<Image>();
		mSelectImage.gameObject.SetActive(false);
	}

	public void Configure (DwarfStatus dwarf) {
		mDwarfStatus = dwarf;
		mDwarfSelect = dwarf.GetComponent<DwarfSelect>();
		mDwarfBehaviour = dwarf.GetComponent<DwarfBehaviour>();
		mNameText.text = mDwarfStatus.dwarfName;
	}
	
	void Update () {
		mHealthImage.transform.localScale = new Vector3(mDwarfStatus.healthPerc, 1f, 1f);
		mHungryImage.transform.localScale = new Vector3(mDwarfStatus.hungryPerc, 1f, 1f);
		mFatigueImage.transform.localScale = new Vector3(mDwarfStatus.fatiguePerc, 1f, 1f);

		mSelectImage.gameObject.SetActive(mDwarfSelect.isSelected);
	}

	void OnSelect() {
		GameController.main.dwarf = mDwarfBehaviour;
	}
}
