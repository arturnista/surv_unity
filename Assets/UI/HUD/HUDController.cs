using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class HUDController : MonoBehaviour {

	public static HUDController main;
	public GameObject actionItemPrefab;
	public GameObject buildingItemPrefab;
	public GameObject inventoryItemPrefab;
	public GameObject floatingTextPrefab;

	public List<Building> buildings;

	private TextMeshProUGUI mDebugText;

	private Canvas mMenuCanvas;
	private Canvas mActionCanvas;
	private Canvas mBuildingCanvas;
	private Canvas mInventoryCanvas;

	void Awake () {
		main = this;

		mActionCanvas = transform.Find("ActionCanvas").GetComponent<Canvas>();

		mMenuCanvas = transform.Find("MenuCanvas").GetComponent<Canvas>();
		mBuildingCanvas = mMenuCanvas.transform.Find("BuildingsListCanvas").GetComponent<Canvas>();
		mInventoryCanvas = mMenuCanvas.transform.Find("InventoryCanvas").GetComponent<Canvas>();

		mDebugText = transform.Find("DebugCanvas/Text").GetComponent<TextMeshProUGUI>();
		mDebugText.text = "";

		foreach (Building build in buildings) {
			BuildingItemHUD bHUD = Instantiate(buildingItemPrefab, transform.position, Quaternion.identity).GetComponent<BuildingItemHUD>();
			bHUD.Configure(build);
			bHUD.transform.SetParent(mBuildingCanvas.transform);
		}

	}
	
	// Update is called once per frame
	void Update () {
		// mDebugText.text = "Inventory: \n";
		// foreach(InventoryItem invItem in mInventory.inventory) {
		// 	mDebugText.text += "[" + invItem.index + "]: " + invItem.item.name + " ( " + invItem.amount + " )\n";
		// }
		
		// mDebugText.text += "Tasks: \n";
		// if(mBehaviour.activeTask != null) mDebugText.text += "[C]: " + mBehaviour.activeTask;
		// List<Task> tasks = mBehaviour.taskList;
		// for (int i = 0; i < tasks.Count; i++) {
		// 	mDebugText.text += " => [" + i + "]: " + tasks[i];
		// }
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
}
