using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CatalogController : MonoBehaviour
{
    public Biome[] biomes;
    
    public ScrollRect scroll;
    
    [SerializeField]
    private Transform pages, animals;

    [SerializeField]
    private GameObject CatalogItemRef;
    public void ToAnimal(string kind)
    {
        for (int b = 0; b < biomes.Length; b++)
        {
            for(int i = 0;i< biomes[b].kinds.Length;i++)
            {
                if(biomes[b].kinds[i]== kind)
                {
                    SetUp(b);
                    scroll.horizontalNormalizedPosition = i / (biomes[b].kinds.Length - 1f);
                    return;
                }
            }
        }
    }
    public void SetUp(int page = 0)
    {
        gameObject.SetActive(true);
        for (int i = 0; i < biomes.Length; i++)
        {
            pages.GetChild(i).gameObject.SetActive(true);
            pages.GetChild(i).GetComponent<Image>().sprite = biomes[i].catalogIcon;
        }
        SetUpPage(page);
    }
    public void SetUpPage(int pageIndex)
    {
        for(int i = 0;i< biomes.Length;i++)
        {
            pages.GetChild(i).GetComponent<Image>().sprite = biomes[i].catalogIcon;
            pages.GetChild(i).GetComponent<Button>().interactable = true;
        }
        pages.GetChild(pageIndex).GetComponent<Image>().sprite = biomes[pageIndex].catalogSelectedIcon;
        pages.GetChild(pageIndex).GetComponent<Button>().interactable = false;
        for (int i = 0; i < animals.childCount; i++)
            Destroy(animals.GetChild(i).gameObject);
        for (int i = 0; i < biomes[pageIndex].kinds.Length; i++)
        {
            Instantiate(CatalogItemRef, animals).GetComponent<CatalogItemController>().SetUp(biomes[pageIndex].kinds[i]);
        }
    }

    
}
