using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*public enum Hat
{
    Default = 0, 
    Cowboy = 1,
    King = 2,
}

public enum Outfit
{
    Default = 0,
    Suit = 1,
    Tutu = 2,
}*/
public class OutfitPicker : MonoBehaviour
{
    public List<GameObject> Hats;
    public List<GameObject> Outfits;

    public int hatIndex = 0;
    public int outfitIndex = 0;


    private void Update()
    {
        
        for (int i = 0; i<=Hats.Count-1; i++)
        {
            if (i == hatIndex)
            {
                Hats[i].SetActive(true);
            }
            else 
                Hats[i].SetActive(false);
        }

        for (int i = 0; i <= Outfits.Count-1; i++)
        {
            if (i == outfitIndex)
            {
                Outfits[i].SetActive(true);
            }
            else
                Outfits[i].SetActive(false);
        }
    }

    public void CycleHats(bool up)
    {
        if (up)
        {
            hatIndex++;
            if (hatIndex > Hats.Count-1)
            {
                hatIndex = 0;
            }
        }
        if (!up)
        {
            hatIndex--;
            if (hatIndex < 0)
            {
                hatIndex = Hats.Count - 1;
            }
        }
    }

    public void CycleOutfit(bool up)
    {
        if (up)
        {
            outfitIndex++;
            if (outfitIndex > Outfits.Count - 1)
            {
                outfitIndex = 0;
            }
        }
        if (!up)
        {
            outfitIndex--;
            if (outfitIndex < 0)
            {
                outfitIndex = Outfits.Count - 1;
            }
        }
    }
}
