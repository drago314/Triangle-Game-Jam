using UnityEngine;
using TMPro;

public class ReloadText : MonoBehaviour
{
    private TextMeshProUGUI reloadText;
    public float flashSpeed = 1f;
    private float t = 0;

    void Awake()
    {
        reloadText = GetComponent<TextMeshProUGUI>();
        if (reloadText == null) Debug.LogError("ReloadText: No TextMeshProUGUI found on " + gameObject.name);
        gameObject.SetActive(false);
    }

    void Update()
    {
        if (gameObject.activeSelf)
        {
            t += Time.deltaTime * flashSpeed;
            reloadText.color = Color.Lerp(new Color(0.5f, 0, 0), Color.red, (Mathf.Sin(t * Mathf.PI * 2) + 1) / 2);
        }
    }

    public void UpdateWarning(int ammo)
    {
        gameObject.SetActive(ammo == 0);
        if (ammo == 0) t = 0; // Reset timer to sync animation on re-enable
    }
}
