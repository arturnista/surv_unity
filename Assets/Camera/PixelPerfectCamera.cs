using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PixelPerfectCamera : MonoBehaviour {

	public float m_PPUScale = 1f;

	private Camera m_Camera;
	private int m_PPU = 32;

	void Awake () {
		m_Camera = GetComponent<Camera>();
		UpdateCameraSize();
	}

    public void IncreaseZoom() {
        m_PPUScale *= 2f;
        UpdateCameraSize();
	}

	public void DecreaseZoom() {
        m_PPUScale /= 2f;
        UpdateCameraSize();
	}

	void UpdateCameraSize() {
		float size = ( m_Camera.pixelHeight / (m_PPUScale * m_PPU) ) * 0.5f;
		m_Camera.orthographicSize = size;
	}
}
