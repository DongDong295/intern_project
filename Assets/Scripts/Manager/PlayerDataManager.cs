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

    [System.Serializable]
    public class PlayerData
    {
        public HeroDataDict ownedHeroData;
    }

    [System.Serializable]
    public class GlobalData
    {
        public List<PlayerDataEntry> playersData = new List<PlayerDataEntry>();
    }

    [System.Serializable]
    public class PlayerDataEntry
    {
        public string playerID;
        public PlayerData playerData;
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

        // Check if PlayerID is null or empty, if so generate a new guest PlayerID
        PlayerID = PlayerPrefs.GetString("PlayerID", "");
        if (string.IsNullOrEmpty(PlayerID))
        {
            // Generate a new PlayerID as guestPlayerXXXX
            PlayerID = GenerateGuestPlayerID();
            PlayerPrefs.SetString("PlayerID", PlayerID); // Save PlayerID immediately
            PlayerPrefs.Save(); // Ensure it persists
        }

        // Load the global player data
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

    // Generate a new guest PlayerID
    private string GenerateGuestPlayerID()
    {
        int randomNumber = Random.Range(1000, 9999); // Random number for guest player ID
        return $"guestPlayer{randomNumber}";
    }

    // Method to save the hero dictionary to JSON in PlayerPrefs under PlayerID
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

        string jsonHeroData = JsonUtility.ToJson(heroDataDict);
        Debug.Log("Hero Data to Save: " + jsonHeroData);

        // Load global data
        GlobalData globalData = LoadGlobalData();

        // If PlayerID doesn't exist in global data, create a new entry
        var existingPlayer = globalData.playersData.Find(player => player.playerID == PlayerID);

        if (existingPlayer == null)
        {
            globalData.playersData.Add(new PlayerDataEntry { playerID = PlayerID, playerData = new PlayerData() });
            existingPlayer = globalData.playersData.Find(player => player.playerID == PlayerID);
        }

        // Update the player's data (e.g., ownedHeroData)
        existingPlayer.playerData.ownedHeroData = heroDataDict;

        // Save back to PlayerPrefs
        string globalJson = JsonUtility.ToJson(globalData);
        Debug.Log("Global Data to Save: " + globalJson);

        // Ensure data is saved correctly
        PlayerPrefs.SetString("GlobalPlayerData", globalJson);
        PlayerPrefs.Save();

        Debug.Log("Saved Heroes for PlayerID: " + PlayerID);
    }

    // Method to load the global data from PlayerPrefs and extract the player's data
    public void LoadHeroesFromJSON()
    {
        // Load global data from PlayerPrefs
        string globalJson = PlayerPrefs.GetString("GlobalPlayerData");

        Debug.Log("Loading Global Data: " + globalJson);

        if (!string.IsNullOrEmpty(globalJson))
        {
            GlobalData globalData = JsonUtility.FromJson<GlobalData>(globalJson);

            // Check if the player data exists
            var playerEntry = globalData.playersData.Find(player => player.playerID == PlayerID);

            if (playerEntry != null)
            {
                PlayerData playerData = playerEntry.playerData;
                OwnedHero = new Dictionary<string, Hero>();

                // Load the owned heroes
                if (playerData.ownedHeroData != null)
                {
                    foreach (var heroEntry in playerData.ownedHeroData.heroes)
                    {
                        OwnedHero[heroEntry.heroID] = heroEntry.hero;
                        if (heroEntry.hero.isEquipped)
                        {
                            EquipHero(OwnedHero[heroEntry.heroID]);
                        }
                    }
                }
                Debug.Log("Loaded Heroes for PlayerID: " + PlayerID);
            }
            else
            {
                Debug.Log("No data found for PlayerID: " + PlayerID);
            }
        }
        else
        {
            Debug.Log("No global data found in PlayerPrefs.");
        }
    }

    public void EquipHero(Hero heroToAdd)
    {
        if (!EquippedHero.Contains(heroToAdd) && EquippedHero.Count < 5)
        {
            EquippedHero.Add(heroToAdd);
            heroToAdd.isEquipped = true;
        }
        else
        {
            if (EquippedHero.Contains(heroToAdd))
                EquippedHero.Remove(heroToAdd);
            heroToAdd.isEquipped = false;
        }
        Pubsub.Publisher.Scope<PlayerEvent>().Publish(new OnPlayerEquipHero(heroToAdd.heroID, heroToAdd.isEquipped));
    }

<<<<<<< Updated upstream
<<<<<<< Updated upstream
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
=======
=======
>>>>>>> Stashed changes
    // Method to load global data from PlayerPrefs
    public GlobalData LoadGlobalData()
    {
        string globalJson = PlayerPrefs.GetString("GlobalPlayerData");
        Debug.Log("Loading Global Data from PlayerPrefs: " + globalJson);

        if (!string.IsNullOrEmpty(globalJson))
        {
            return JsonUtility.FromJson<GlobalData>(globalJson);
        }

        // If no global data exists, return a new instance
        Debug.Log("No global data found in PlayerPrefs. Returning new GlobalData.");
        return new GlobalData();
<<<<<<< Updated upstream
>>>>>>> Stashed changes
=======
>>>>>>> Stashed changes
    }
}
