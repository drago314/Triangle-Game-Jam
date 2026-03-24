using UnityEngine;

[CreateAssetMenu(fileName = "NewEvolution", menuName = "Game/Evolution")]
public class Evolution : ScriptableObject
{
    public int id;
    public int price;
    public int requires;
    [TextArea]
    public string description;

    public Vector3 bodyOffset;
    public bool enableComponent;

    public float addSpeed;
    public float addDamage;
    public int addHealth;
    public int enableProj;

    public Animator animator;
    public GameObject prefab;
}