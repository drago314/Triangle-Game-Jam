using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class RhythmGame : MonoBehaviour
{
    public Transform cam;
    public float camRot, camPos, lerpSpeed;
    bool entered;

    public Player player;
    public PlayerWeapon weapon;

    public GameObject musicManager, health, clip, weaponSprite, canvas;

    public TextMeshProUGUI scoreText;
    public int score, scoreToWin;

    public GameObject[] arrows;
    public Transform[] arrowSpawns, inputs;
    public float avgSpawnPerArrow;
    public AudioSource[] sources;
    public AudioSource victory;

    private Animator[] anims;
    private SpriteFlip[] flips;

    public Image[] uiImages;
    float alpha;
    
    private void FixedUpdate()
    {
        if (entered)
        {
            cam.localPosition = Vector3.Lerp(cam.localPosition, new(cam.localPosition.x, cam.localPosition.y, camPos), lerpSpeed * Time.fixedDeltaTime);
            cam.localEulerAngles = Vector3.Lerp(cam.localEulerAngles, new(camRot, cam.localEulerAngles.y, cam.localEulerAngles.z), lerpSpeed * Time.fixedDeltaTime);
            avgSpawnPerArrow = Mathf.Clamp(avgSpawnPerArrow - Time.fixedDeltaTime / 50, 0.2f, avgSpawnPerArrow);
        }

        if (alpha < 1 && entered && score < scoreToWin) 
        {
            alpha = Mathf.Clamp(alpha + Time.fixedDeltaTime, 0, 1); 
            foreach (Image i in uiImages)
            {
                i.color = new(i.color.r, i.color.g, i.color.b, alpha);
            }
        }

        if (score >= scoreToWin && alpha > 0)
        {
            alpha = Mathf.Clamp(alpha - Time.fixedDeltaTime, 0, 1);
            foreach (Image i in uiImages)
            {
                i.color = new(i.color.r, i.color.g, i.color.b, alpha);
            }
        }

        victory.volume = Mathf.Clamp(victory.volume - Time.fixedDeltaTime/10, 0, 0.2f);

        anims = GetComponentsInChildren<Animator>();
        flips = GetComponentsInChildren<SpriteFlip>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !entered)
        {
            entered = true;
            player.disableInput = true;
            weapon.disableInput = true;
            GameManager.Inst.SwitchDimension(Dimension.Extroversion);
            musicManager.SetActive(false);
            health.SetActive(false);
            clip.SetActive(false);
            weaponSprite.SetActive(false);
            GetComponent<AudioSource>().Play();
            Invoke("SpawnArrow", avgSpawnPerArrow);
        }
    }

    private void SpawnArrow()
    {
        if (score >= scoreToWin) return;
        int toSpawn = Random.Range(0, 4);
        GameObject go = Instantiate(arrows[toSpawn], canvas.transform);
        Arrow arrow = go.GetComponent<Arrow>();
        arrow.speed *= Random.Range(0.7f, 1.3f);
        arrow.myInput = inputs[toSpawn];
        arrow.rg = this;
        go.transform.position = arrowSpawns[toSpawn].position;
        Destroy(go, 15);
        Invoke("SpawnArrow", avgSpawnPerArrow * Random.Range(0, 1.2f));
    }

    public void PushScore(float dis, int id)
    {
        score += Mathf.RoundToInt(100 - dis);
        victory.volume = Mathf.Clamp(victory.volume + 0.1f, 0, 0.2f);
        scoreText.text = "" + score;
        for (int i = 0; i < Mathf.RoundToInt((100 - dis) / 10); i++)
        {
            anims[Random.Range(0, anims.Length)].Play("Jump");
        }
        for (int i = 0; i < Mathf.RoundToInt((100 - dis) / 20); i++)
        {
            flips[Random.Range(0, flips.Length)].Flip(0);
        }
        sources[id].pitch = 1 - (dis / 100);
        sources[id].Play();
    }
}
