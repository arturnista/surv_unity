using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HUDController : MonoBehaviour {

	public static HUDController main;
	public GameObject actionItemPrefab;
	public GameObject buildingItemPrefab;
	public GameObject inventoryItemPrefab;
	public GameObject dwarfItemPrefab;
	public GameObject floatingTextPrefab;
	public GameObject selectPrefab;

	public List<Building> buildings;

	private TextMeshProUGUI mDebugText;

	private Canvas mMenuCanvas;
	private Canvas mActionCanvas;
	private Canvas mBuildingCanvas;
	private Canvas mInventoryCanvas;
	private Canvas mDwarfsCanvas;

	private GameObject mSelectSprite;

	private Vector3 mSelectInitial;
	private bool mIsSelecting;

	void Awake () {
		main = this;

		mActionCanvas = transform.Find("ActionCanvas").GetComponent<Canvas>();

		mMenuCanvas = transform.Find("MenuCanvas").GetComponent<Canvas>();
		mBuildingCanvas = mMenuCanvas.transform.Find("BuildingsListCanvas").GetComponent<Canvas>();
		mInventoryCanvas = mMenuCanvas.transform.Find("InventoryCanvas").GetComponent<Canvas>();
		mDwarfsCanvas = mMenuCanvas.transform.Find("DwarfsList").GetComponent<Canvas>();
		
		mSelectSprite = Instantiate(selectPrefab);
		mSelectSprite.SetActive(false);

		mDebugText = transform.Find("DebugCanvas/Text").GetComponent<TextMeshProUGUI>();
		mDebugText.text = "";

	}

	void Start () {
		foreach (Building build in buildings) {
			BuildingItemHUD bHUD = Instantiate(buildingItemPrefab, transform.position, Quaternion.identity).GetComponent<BuildingItemHUD>();
			bHUD.Configure(build);
			bHUD.transform.SetParent(mBuildingCanvas.transform);
		}

		foreach (DwarfStatus dwarf in GameObject.FindObjectsOfType<DwarfStatus>()) {
			DwarfItemHUD dHUD = Instantiate(dwarfItemPrefab, transform.position, Quaternion.identity).GetComponent<DwarfItemHUD>();
			dHUD.Configure(dwarf);
			dHUD.transform.SetParent(mDwarfsCanvas.transform);
		}
	}
	
	void Update () {
		mDebugText.text = "";
		
		if(GameController.main.dwarf != null) {
			mDebugText.text += " Health: " + Mathf.Round( GameController.main.dwarf.status.health );
			mDebugText.text += " Hungry: " + Mathf.Round( GameController.main.dwarf.status.hungry );
			mDebugText.text += " Fatigue: " + Mathf.Round( GameController.main.dwarf.status.fatigue );
			mDebugText.text += " Tasks: " + GameController.main.dwarf.taskList.Count;
		} else {
			mDebugText.text += "Tasks: " + GameController.main.taskList.Count;
		}

		if(mIsSelecting) {
			Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			float xDiff = pos.x - mSelectInitial.x;
			float yDiff = pos.y - mSelectInitial.y;
			mSelectSprite.transform.localScale = new Vector3(xDiff, yDiff, 1f);
			mSelectSprite.transform.position = new Vector3(mSelectInitial.x + (xDiff / 2), mSelectInitial.y + (yDiff / 2), 0f);
		}
	}

	public void UpdateInventory() {
		if(GameController.main.dwarf == null) return;

		foreach(Transform c in mInventoryCanvas.transform) Destroy(c.gameObject);

		foreach (InventoryItem iItem in GameController.main.dwarf.inventory.inventory) {
			InventoryItemHUD iHUD = Instantiate(inventoryItemPrefab, transform.position, Quaternion.identity).GetComponent<InventoryItemHUD>();
			iHUD.Configure(iItem);
			iHUD.transform.SetParent(mInventoryCanvas.transform);
		}
	}

	// Buildings list menu
	public void OpenBuildingsListMenu() {
		OpenMenu(mBuildingCanvas);
	}

	public void CloseBuildingsListMenu() {
		CloseMenu(mBuildingCanvas);
	}

	public void ToggleBuildingsListMenu() {
		ToggleMenu(mBuildingCanvas);
	}

	// Inventory menu
	public void OpenInventoryMenu() {
		OpenMenu(mInventoryCanvas);
	}

	public void CloseInventoryMenu() {
		CloseMenu(mInventoryCanvas);
	}

	public void ToggleInventoryMenu() {
		ToggleMenu(mInventoryCanvas);
	}

	void CloseAllMenus() {
		CloseMenu(mInventoryCanvas);
		CloseMenu(mBuildingCanvas);
	}

	void OpenMenu(Canvas menu) {
		CloseAllMenus();

		menu.gameObject.SetActive( true );
	}

	void CloseMenu(Canvas menu) {
		menu.gameObject.SetActive( false );
	}

	void ToggleMenu(Canvas menu) {
		if(menu.gameObject.activeSelf) CloseMenu(menu);
		else OpenMenu(menu);
	}

	public void OpenActionCanvas(Vector3 position, List<string> actions, Action<int> callback) {
		this.CloseActionCanvas();
		mActionCanvas.transform.position = position;

		for (int i = 0; i < actions.Count; i++) {
			CreateAction(actions[i], i, callback);
		}
		CreateAction("Cancel", -1, callback);
	}

	public void CloseActionCanvas() {
		foreach(Transform c in mActionCanvas.transform) Destroy(c.gameObject);
	}

	public void CreateAction(string text, int index, Action<int> callback) {
		GameObject t = Instantiate(actionItemPrefab, Vector3.zero, Quaternion.identity) as GameObject;
		t.transform.SetParent(mActionCanvas.transform);
		t.GetComponentInChildren<TextMeshProUGUI>().text = text;
		t.GetComponent<Button>().onClick.AddListener(() => {
			callback(index);
			this.CloseActionCanvas();
		});
	}

	public void CreateFloatingText(string text, Vector3 position, Color color) {
		FloatingText ft = Instantiate(floatingTextPrefab, position, Quaternion.identity).GetComponent<FloatingText>();
		ft.Configure(text, color);
		ft.transform.position = position;
	}


	public void StartSelecting() {
		mSelectInitial = Camera.main.ScreenToWorldPoint(Input.mousePosition);	
		mIsSelecting = true;
		mSelectSprite.SetActive(mIsSelecting);
	}

	public void StopSelecting() {
		mIsSelecting = false;
		mSelectSprite.SetActive(mIsSelecting);
	}

}
