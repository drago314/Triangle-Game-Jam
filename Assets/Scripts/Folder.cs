using UnityEngine;

public class SpriteMouseHover3D : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    private bool isMouseOver = false;
    Color color;
    public Sprite newSprite;

    public float lerpSpeed;
    public float offsetX, offsetY;
    public Camera cam;
    public Transform myLight;
    public bool flip;
    public Vector2 minValues, maxValues;

    public BoxCollider alt;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        color = spriteRenderer.color;
    }

    void Update()
    {
        CheckMouseOver();

        if (transform.localEulerAngles.y > 86)
        {
            spriteRenderer.flipX = true;
            spriteRenderer.sprite = newSprite;
        }

        if (flip)
        {
            cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, 22, lerpSpeed * Time.deltaTime);
            Vector3 pos = new(offsetX * myLight.position.x, offsetY * myLight.position.y, cam.transform.position.z);
            cam.transform.position = Vector3.Lerp(cam.transform.position, pos, lerpSpeed * Time.deltaTime * 6);
            cam.transform.position = new(Mathf.Clamp(cam.transform.position.x, minValues.x, maxValues.x), Mathf.Clamp(cam.transform.position.y, minValues.y, maxValues.y), cam.transform.position.z);
        }

        if (Input.GetMouseButtonDown(0) && isMouseOver) { flip = true; GetComponent<AudioSource>().Play(); GetComponent<SpriteFlip>().Flip(0); GetComponent<BoxCollider>().enabled = false; alt.enabled = true; ; }
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

    void OnMouseEnterSprite()
    {
        Debug.Log($"{gameObject.name} - Mouse Entered!");
        spriteRenderer.color = Color.white; // Example: Change color on hover
    }

    void OnMouseExitSprite()
    {
        Debug.Log($"{gameObject.name} - Mouse Exited!");
        spriteRenderer.color = color; // Reset color
    }
}