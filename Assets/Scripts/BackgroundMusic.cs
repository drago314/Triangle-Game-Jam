using UnityEngine;
using System.Collections;

public class BackgroundMusic : MonoBehaviour
{
    public AudioSource[] sources;
    public float fadeTime;
    public float[] maxVolumes;

    int lastDimension = 0;
    int dimension = 0;

    // Use this for initialization
    private void Start()
    {
        GameManager.Inst.OnDimensionSwitch += SwitchMusic;
        dimension = (int)GameManager.Inst.dimension;
    }

    private void FixedUpdate()
    {
        if (sources[lastDimension].volume > 0)
            sources[lastDimension].volume -= (Time.fixedDeltaTime / fadeTime) * maxVolumes[lastDimension];
        if (sources[dimension].volume < maxVolumes[dimension])
            sources[dimension].volume += (Time.fixedDeltaTime / fadeTime) * maxVolumes[dimension];
    }

    private void SwitchMusic(Dimension dimension)
    {
        lastDimension = this.dimension;
        this.dimension = (int)dimension;
    }
}
