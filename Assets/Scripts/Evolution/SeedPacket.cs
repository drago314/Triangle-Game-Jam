using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeedPacket : MonoBehaviour
{
    public Transform center;
    public float scaleSpeed, distanceScaleUnit, distanceTravelUnit;
    float mult;

    public float horizontalSpeed;
    public int myId;

    public Evolution myEvolution;
    public EvolutionMenu em;

    public float deathTimer;

    public bool openAnim;

    private RectTransform rectTransform;
    private RectTransform centerRect;

    private void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        centerRect = center as RectTransform;

        Vector2 myPos = rectTransform != null ? rectTransform.anchoredPosition : (Vector2)transform.localPosition;
        Vector2 centerPos = centerRect != null ? centerRect.anchoredPosition : (Vector2)center.localPosition;

        mult = Mathf.Clamp(
            1 - (Mathf.Abs(myPos.x - centerPos.x) / (distanceScaleUnit * (Screen.width / 1920f))),
            0,
            1
        );

        transform.localScale = new Vector3(mult, mult, mult);

        if (openAnim)
        {
            transform.localScale = Vector3.zero;
            scaleSpeed /= 3;
        }
    }

    private void Update()
    {
        Vector2 myPos = rectTransform != null ? rectTransform.anchoredPosition : (Vector2)transform.localPosition;
        Vector2 centerPos = centerRect != null ? centerRect.anchoredPosition : (Vector2)center.localPosition;

        mult = Mathf.Clamp(
            1 - (Mathf.Abs(myPos.x - centerPos.x) / (distanceScaleUnit * (Screen.width / 1920f))),
            0,
            10
        );

        if (openAnim) mult = (Screen.width / 1920f) * 1.3f;
        if (openAnim && deathTimer < 0.5f && deathTimer > 0)
        {
            mult = 0;
            scaleSpeed = 9;
        }

        transform.localScale = Vector3.Lerp(
            transform.localScale,
            new Vector3(mult, mult, mult),
            scaleSpeed * Time.unscaledDeltaTime
        );

        if (deathTimer > 0)
        {
            deathTimer -= Time.unscaledDeltaTime;
            if (deathTimer <= 0) { Destroy(gameObject); }
        }
        else if (!openAnim)
        {
            int x = Mathf.Abs(myId - em.currentlySelectedSeedPacket);
            int neg = em.currentlySelectedSeedPacket > myId ? -1 : 1;
            float eval = Mathf.Log(x + 1) * 1.45f;

            Vector2 currentPos = rectTransform != null
                ? rectTransform.anchoredPosition
                : (Vector2)transform.localPosition;

            Vector2 targetPos = new Vector2(
                distanceTravelUnit * eval * neg,
                currentPos.y
            );

            Vector2 newPos = Vector2.Lerp(
                currentPos,
                targetPos,
                horizontalSpeed * Time.unscaledDeltaTime * (Screen.width / 1920f)
            );

            if (rectTransform != null)
                rectTransform.anchoredPosition = newPos;
            else
                transform.localPosition = new Vector3(newPos.x, newPos.y, 0);
        }
    }

    public void AddMenu(EvolutionMenu menu)
    {
        em = menu;
        center = menu.transform;
        centerRect = center as RectTransform;
    }
}