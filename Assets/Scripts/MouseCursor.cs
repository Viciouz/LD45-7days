﻿using UnityEngine;
using System.Collections;

public class MouseCursor : MonoBehaviour {
    public Texture2D cursorTexture;
    public CursorMode cursorMode = CursorMode.Auto;
    public Vector2 hotSpot = Vector2.zero;

    private void Start() {
        Cursor.SetCursor(cursorTexture, hotSpot, cursorMode);
    }
}