using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayButton : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    private bool isMouseOver = false;
    public GameObject signature;
    public GameObject blink;
    bool signed;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        CheckMouseOver();

        if (Input.GetMouseButtonDown(0) && isMouseOver && !signed) 
        {
            signed = true;
            signature.SetActive(true);
            Invoke("CloseEyes", 1.5f);
        }
    }

    void CheckMouseOver()
    {
        // Ray from camera to mouse position
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            if (hit.collider != null && hit.collider.gameObject == gameObject)
            {
                if (!isMouseOver)
                {
                    isMouseOver = true;
                    OnMouseEnterSprite();
                }
                return;
            }
        }

        // If we reach here, the cursor is no longer over the sprite
        if (isMouseOver)
        {
            isMouseOver = false;
            OnMouseExitSprite();
        }
    }

    private void CloseEyes() { blink.SetActive(true); Invoke("NewScene", 3); }
    private void NewScene() 
    {
        if (PlayerPrefs.GetInt("Scene") == 0) SceneManager.LoadScene("Intro");
        else { SceneManager.LoadScene(PlayerPrefs.GetInt("Scene")); }
    }

    void OnMouseEnterSprite()
    {
        Debug.Log($"{gameObject.name} - Mouse Entered!");
        //spriteRenderer.color = Color.white; // Example: Change color on hover
    }

    void OnMouseExitSprite()
    {
        Debug.Log($"{gameObject.name} - Mouse Exited!");
        //spriteRenderer.color = color; // Reset color
    }
}