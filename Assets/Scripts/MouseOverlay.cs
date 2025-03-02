using UnityEngine;
using System.Collections;

public class MouseOverlay : MonoBehaviour
{
    public Texture2D[] cursorTexture;
    public CursorMode cursorMode = CursorMode.Auto;
    public Vector2 hotSpot = Vector2.zero;
   
    private void Start()
    {
        GameManager.Inst.OnDimensionSwitch += SwitchMouse;
    }

    private void SwitchMouse(Dimension d)
    {
        Cursor.SetCursor(cursorTexture[(int)d], Vector2.zero, cursorMode);
    }
}
