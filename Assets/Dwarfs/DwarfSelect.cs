using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DwarfSelect : MonoBehaviour, IPointerDownHandler, IPointerUpHandler {

	private GameController mGameController;
	private DwarfBehaviour mBehaviour;
	private SpriteRenderer mSelectedSprite;

	void Awake () {
		mSelectedSprite = transform.Find("SelectedSprite").GetComponent<SpriteRenderer>();
		mBehaviour = GetComponent<DwarfBehaviour>();
	}

	void Start () {
		mGameController = GameController.main;
	}

	void Update () {
		mSelectedSprite.gameObject.SetActive( mGameController.dwarf == mBehaviour );
	}

	public void OnPointerDown(PointerEventData eventData) {

	}

	public void OnPointerUp(PointerEventData eventData) {
		mGameController.dwarf = mBehaviour;
	}

}
