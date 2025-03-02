using UnityEngine;
using System.Collections;

public class BackgroundMusic : MonoBehaviour
{
    public AudioSource[] sources;
    public float fadeTime;
    public float[] maxVolumes;

    int lastDimension = -1;
    int dimension = -1;

    // Use this for initialization
    private void Start()
    {
        GameManager.Inst.OnDimensionSwitch += SwitchMusic;
        dimension = (int)GameManager.Inst.dimension;
    }

    private void FixedUpdate()
    {
        if (GameManager.Inst.musicOff == true)
            Destroy(gameObject);

        if (lastDimension != -1 && lastDimension != dimension && sources[lastDimension].volume > 0)
            sources[lastDimension].volume -= (Time.fixedDeltaTime / fadeTime) * maxVolumes[lastDimension];
        if (dimension != -1 && sources[dimension].volume < maxVolumes[dimension])
            sources[dimension].volume += (Time.fixedDeltaTime / fadeTime) * maxVolumes[dimension];
    }

    private void SwitchMusic(Dimension dimension)
    {
        lastDimension = this.dimension;
        this.dimension = (int)dimension;
    }

    public void FinalBoss()
    {
        foreach (AudioSource s in sources)
        {
            s.time = 59f;
            s.volume = 0f;
        }

        SwitchMusic(GameManager.Inst.dimension);
    }
}
