using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EvolutionMenu : MonoBehaviour
{
    public Player player;
    public GameObject placedPlant;
    List<int> idList = new List<int>();
    public List<SeedPacket> seedPackets;
    public int currentlySelectedSeedPacket;

    public Button evolveButton;
    public TextMeshProUGUI descriptionText;

    private void Start()
    {
        idList = new List<int>();
        for (int i = 0; i < seedPackets.Count; i++)
        {
            seedPackets[i].myId = i;
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E)) { ClickEvolution(seedPackets[currentlySelectedSeedPacket].myEvolution); }

        if ((Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D)) && currentlySelectedSeedPacket < seedPackets.Count - 1) currentlySelectedSeedPacket++;
        if ((Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A)) && currentlySelectedSeedPacket > 0) currentlySelectedSeedPacket--;

        evolveButton.interactable = seedPackets.Count > 0 && player.water >= seedPackets[currentlySelectedSeedPacket].myEvolution.price;
        descriptionText.text = seedPackets.Count > 0 ? seedPackets[currentlySelectedSeedPacket].myEvolution.description : "";
    }

    public void ClickEvolution(Evolution evolution)
    {
        if ((!idList.Contains(evolution.requires) && evolution.requires != -1) || player.water < evolution.price) return;

        // heal
        if (evolution.id == -1) 
        {
            //Debug.Log("healt");
            if (player.health.GetHealth() >= player.health.GetMaxHealth()) return;
            player.health.Heal(1); 
            player.UpdateWater(-evolution.price);
            return; 
        }
        // place plant
        else if (evolution.id == -2)
        {
            Instantiate(placedPlant, player.transform.position, Quaternion.identity);
            player.UpdateWater(-evolution.price);
            return;
        }

        idList.Add(evolution.id);
        player.Evolve(evolution);

        seedPackets[currentlySelectedSeedPacket].GetComponent<Animator>().enabled = true;
        seedPackets[currentlySelectedSeedPacket].transform.parent = transform.parent.parent;
        seedPackets[currentlySelectedSeedPacket].deathTimer = 0.8f;

        seedPackets.RemoveAt(currentlySelectedSeedPacket);
        if (currentlySelectedSeedPacket >= seedPackets.Count) currentlySelectedSeedPacket--;
        currentlySelectedSeedPacket = Mathf.Clamp(currentlySelectedSeedPacket, 0, seedPackets.Count);

        for (int i = 0; i < seedPackets.Count; i++)
        {
            seedPackets[i].myId = i;
        }
    }

    public void PressEvolveButton()
    {
        ClickEvolution(seedPackets[currentlySelectedSeedPacket].myEvolution);
    }

    public void SpawnNewPacket(Evolution evolution)
    {
        // spawn the selectable part
        GameObject seedPacket = Instantiate(evolution.prefab, transform.GetChild(0));
        seedPackets.Add(seedPacket.GetComponent<SeedPacket>());
        seedPacket.GetComponent<SeedPacket>().myId = seedPackets.Count - 1;
        seedPacket.GetComponent<SeedPacket>().AddMenu(this);
    }

    public bool PlayerHasPacket(Evolution evolution)
    {
        if (idList.Contains(evolution.id) && evolution.id >= 0) return true;
        foreach (SeedPacket packet in seedPackets)
        {
            if (packet.myEvolution == evolution) return true;
        }

        return false;
    }
}
