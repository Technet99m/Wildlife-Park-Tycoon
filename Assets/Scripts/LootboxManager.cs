using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LootboxManager : MonoBehaviour
{
    private int[] moneyAmounts = { 1000, 2000, 5000, 20000 };
    private int[] gemsAmounts = { 10, 15, 25, 50 };
    private int[] potionsAmounts = { 50, 75, 100, 300 };

    private void Awake()
    {
        for(int i = 0;i<10;i++)
        {
            var type = (LootboxType)Random.Range(0, 5);
            var lOut = OpenLootbox(type);
            Debug.Log(type.ToString());
            Debug.Log(Newtonsoft.Json.JsonConvert.SerializeObject(lOut));
        }
    }

    public LootboxOut OpenLootbox(LootboxType type)
    {
        LootboxOut lOut = new LootboxOut();  
        if (type != LootboxType.Super)
        {
            Biome biome = Resources.Load<Biome>("Biomes/" + type.ToString());
            lOut.pieceKind = biome.kinds[Random.Range(0, biome.kinds.Length)];
            lOut.pieceId = Random.Range(0, 4);
        }
        else
        {
            do
            {
                int b = Random.Range(0, 4);
                Biome biome = Resources.Load<Biome>("Biomes/" + ((LootboxType)b).ToString());
                lOut.pieceKind = biome.kinds[Random.Range(0, biome.kinds.Length)];
                lOut.pieceId = Random.Range(0, 4);
            } while (AnimalUpgradesManager.upgrades[lOut.pieceKind].active[lOut.pieceId]);
        }
        lOut.prizes = new List<LootboxPrize>();
        for (int i = 0, count = type == LootboxType.Super ? 2 : (1 + (Random.value > 0.7f ? 1 : 0)); i < count; i++)
        {
            LootboxPrize prize = new LootboxPrize();
            do
                prize.prizeType = (LootboxPrizeType)(Random.Range(0, 2) + Random.value > 0.5f ? 1 : 0);
            while (lOut.prizes.Exists(x => x.prizeType == prize.prizeType));
            switch (prize.prizeType)
            {
                case LootboxPrizeType.Gems:
                    prize.amount = gemsAmounts[RandomTier()];
                    break;
                case LootboxPrizeType.Money:
                    prize.amount = moneyAmounts[RandomTier()];
                    break;
                case LootboxPrizeType.Potions:
                    prize.amount = potionsAmounts[RandomTier()];
                    break;
            }
            lOut.prizes.Add(prize);
        }
        return lOut;
    }
    private int RandomTier()
    {
        float tmp = Random.value;

        if (tmp > 0.95f)
            return 3;

        if (tmp > 0.7f)
            return 2;

        if (tmp > 0.4f)
            return 1;

        return 0;
    }
}
[System.Serializable]
public class LootboxPrize
{
    public int amount;
    public LootboxPrizeType prizeType;
}
[System.Serializable]
public class LootboxOut
{
    public string pieceKind;
    public int pieceId;
    public List<LootboxPrize> prizes;
}

public enum LootboxPrizeType
{
    Gems = 0,
    Money = 1,
    Potions = 2
}
public enum LootboxType
{
    Forest = 0,
    Savannah = 1,
    Arctic = 2,
    Jungle = 3,
    Super = 4
}
