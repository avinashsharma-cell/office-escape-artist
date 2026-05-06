using UnityEngine;
using System.Collections.Generic;

public class CharacterCustomization : MonoBehaviour
{
    public enum SkinTone { Light, Medium, Tan, Dark }
    public enum HairStyle { Short, Long, Curly, Bald }
    public enum OutfitType { Formal, BusinessCasual, CasualFriday, Pyjamas }

    public SkinTone selectedSkin = SkinTone.Light;
    public HairStyle selectedHair = HairStyle.Short;
    public OutfitType selectedOutfit = OutfitType.Formal;

    public string fakeJobTitle = "Senior Synergy Consultant";

    public SpriteRenderer skinRenderer;
    public SpriteRenderer hairRenderer;
    public SpriteRenderer outfitRenderer;

    public List<GameObject> skinOptions;
    public List<GameObject> hairOptions;
    public List<GameObject> outfitOptions;

    void Start()
    {
        ApplyCustomization();
    }

    public void SelectSkin(SkinTone tone)
    {
        selectedSkin = tone;
        ApplyCustomization();
    }

    public void SelectHair(HairStyle style)
    {
        selectedHair = style;
        ApplyCustomization();
    }

    public void SelectOutfit(OutfitType outfit)
    {
        selectedOutfit = outfit;
        ApplyCustomization();
    }

    void ApplyCustomization()
    {
        // Apply skin
        if (skinRenderer != null)
        {
            int skinIndex = (int)selectedSkin;
            if (skinIndex < skinOptions.Count)
                skinRenderer.sprite = skinOptions[skinIndex].GetComponent<SpriteRenderer>().sprite;
        }

        // Apply hair
        if (hairRenderer != null)
        {
            int hairIndex = (int)selectedHair;
            if (hairIndex < hairOptions.Count)
                hairRenderer.sprite = hairOptions[hairIndex].GetComponent<SpriteRenderer>().sprite;
        }

        // Apply outfit
        if (outfitRenderer != null)
        {
            int outfitIndex = (int)selectedOutfit;
            if (outfitIndex < outfitOptions.Count)
                outfitRenderer.sprite = outfitOptions[outfitIndex].GetComponent<SpriteRenderer>().sprite;
        }
    }
}
