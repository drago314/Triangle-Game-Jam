using UnityEngine;
using System.Collections;

public class BackgroundMusic : MonoBehaviour
{
    public AudioSource[] sources;
    public float fadeTime;

    int lastDimension = 0;
    int dimension = 0;

    // Use this for initialization
    private void Start()
    {
        GameManager.Inst.OnDimensionSwitch += SwitchMusic;
        dimension = (int)GameManager.Inst.dimension;
    }

    private void Update()
    {
        if (sources[lastDimension].volume > 0)
            sources[lastDimension].volume -= Time.deltaTime / fadeTime;
        if (sources[dimension].volume < 1)
            sources[dimension].volume += Time.deltaTime / fadeTime;
    }

    private void SwitchMusic(Dimension dimension)
    {
        lastDimension = this.dimension;
        this.dimension = (int)dimension;
    }
}
