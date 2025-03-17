using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using Unity.Mathematics;
using UnityEngine;
using ZBase.Foundation.Singletons;

public class PlayerDataManager : MonoBehaviour
{
    // Local data
    public Dictionary<string, Hero> OwnedHero;
    public bool IsAuthenticated;
    public string PlayerID;
    public List<Hero> EquippedHero;

    [System.Serializable]
    public class HeroDataDict
    {
        public List<HeroDataEntry> heroes;
    }

    [System.Serializable]
    public class HeroDataEntry
    {
        public string heroID;
        public Hero hero;
    }

    public async UniTask OnStartApplication()
    {
        OwnedHero = new Dictionary<string, Hero>();
        EquippedHero = new List<Hero>();
        await LoadPlayerData();
        Pubsub.Subscriber.Scope<PlayerEvent>().Subscribe<OnGachaEvent>(GenerateNewHero);
    }

    public async UniTask LoadPlayerData()
    {
        IsAuthenticated = PlayerPrefs.GetInt("IsAuthenticated") == 1;
        PlayerID = PlayerPrefs.GetString("PlayerID");
        LoadHeroesFromJSON();
        await UniTask.CompletedTask;
    }

    public void SetAuthenticateStatus(string value)
    {
        PlayerID = value;
        PlayerPrefs.SetString("PlayerID", PlayerID);
        PlayerPrefs.Save();
    }

    public void SetAuthenticateStatus(bool status)
    {
        IsAuthenticated = status;
        PlayerPrefs.SetInt("IsAuthenticated", IsAuthenticated ? 1 : 0);
        PlayerPrefs.Save();
    }

    void OnApplicationQuit()
    {
        Debug.Log("Save!");
        PlayerPrefs.SetInt("IsAuthenticated", IsAuthenticated ? 1 : 0);
        SaveHeroesToJSON();
        PlayerPrefs.Save();
    }

    public async UniTask GenerateNewHero(OnGachaEvent e)
    {
        var data = await Singleton.Of<DataManager>().Load<HeroData>(Data.HERO_DATA);
        var heroData = data.heroDataItems;
        var heroID = GenerateUniqueHeroID();
        var hero = new Hero(heroID, heroData[UnityEngine.Random.Range(0, heroData.Length)]);
        OwnedHero[heroID] = hero;
        Debug.Log($"Generated new hero with ID: {heroID}");

        // Save the updated hero dictionary to PlayerPrefs as JSON after generating a new hero
        SaveHeroesToJSON();
        await UniTask.CompletedTask;
    }

    private string GenerateUniqueHeroID()
    {
        var timestamp = System.DateTime.UtcNow.ToString("yyyyMMddHHmmssfff");
        var randomPart = UnityEngine.Random.Range(1000, 9999);
        return $"{timestamp}{randomPart}";
    }

    // Method to save the hero dictionary to JSON in PlayerPrefs
    public void SaveHeroesToJSON()
    {
        HeroDataDict heroDataDict = new HeroDataDict { heroes = new List<HeroDataEntry>() };

        foreach (var kvp in OwnedHero)
        {
            heroDataDict.heroes.Add(new HeroDataEntry
            {
                heroID = kvp.Key,
                hero = kvp.Value
            });
        }

        string json = JsonUtility.ToJson(heroDataDict);
        Debug.Log(json);

        PlayerPrefs.SetString("OwnedHeroes", json);
        PlayerPrefs.Save();

        Debug.Log("Saved Heroes to JSON: " + json);
    }

    // Method to load the hero dictionary from PlayerPrefs (deserialize JSON)
    public void LoadHeroesFromJSON()
    {
        string json = PlayerPrefs.GetString("OwnedHeroes");

        if (!string.IsNullOrEmpty(json))
        {
            HeroDataDict heroDataDict = JsonUtility.FromJson<HeroDataDict>(json);
            OwnedHero = new Dictionary<string, Hero>();

            foreach (var heroEntry in heroDataDict.heroes)
            {
                OwnedHero[heroEntry.heroID] = heroEntry.hero;
                if(heroEntry.hero.isEquipped){
                    EquipHero(OwnedHero[heroEntry.heroID]);
                }
            }

            Debug.Log("Loaded Heroes from JSON" + json);
        }
        else
        {
            Debug.Log("No heroes found in PlayerPrefs");
        }
    }
    
    public void EquipHero(Hero heroToAdd)
    {
        if(!EquippedHero.Contains(heroToAdd) && EquippedHero.Count < 5){
            EquippedHero.Add(heroToAdd);
            heroToAdd.isEquipped = true;
    }
        else{
            if(EquippedHero.Contains(heroToAdd))
                EquippedHero.Remove(heroToAdd);
            heroToAdd.isEquipped = false;
    }
        Pubsub.Publisher.Scope<PlayerEvent>().Publish(new OnPlayerEquipHero(heroToAdd.heroID, heroToAdd.isEquipped));
    }

    public List<Hero> GetUnequippedHeroList()
    {
        List<Hero> unequippedHeroes = new List<Hero>();

        foreach (var kvp in OwnedHero)
        {
            Hero hero = kvp.Value;
            if (!hero.isEquipped)
            {
                unequippedHeroes.Add(hero);
            }
        }
        return unequippedHeroes;
    }

    public Dictionary<string, Hero> GetUnequippedHeroDict()
    {
        Dictionary<string, Hero> unequippedHeroes = new Dictionary<string, Hero>();

        foreach (var kvp in OwnedHero)
        {
            Hero hero = kvp.Value;
            if (!hero.isEquipped)
            {
                unequippedHeroes.Add(kvp.Key, hero); // Using heroID (kvp.Key) as the dictionary key
            }
        }

        return unequippedHeroes;
    }

    public Dictionary<string, Hero> GetUnequippedHeroDict(Hero currentHero)
    {
        Dictionary<string, Hero> unequippedHeroes = new Dictionary<string, Hero>();

        foreach (var kvp in OwnedHero)
        {
            Hero hero = kvp.Value;
            if (!hero.isEquipped)
            {
                unequippedHeroes.Add(kvp.Key, hero); // Using heroID (kvp.Key) as the dictionary key
            }
        }
        Debug.Log(currentHero.heroID);
        unequippedHeroes.Remove(currentHero.heroID);
        return unequippedHeroes;
    }

    public void RemoveHero(Hero heroRef){
        if(OwnedHero.ContainsKey(heroRef.heroID)){
             OwnedHero.Remove(heroRef.heroID);
        }
        SaveHeroesToJSON();
    }
}
