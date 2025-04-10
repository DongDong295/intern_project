using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using Firebase.Firestore;
using UnityEngine;
using UnityEngine.Localization.Settings;
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

    private void LoadLanguageData(){
        var index = PlayerPrefs.GetInt("LanguageOption");
        LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[index];
    }

    public async UniTask OnStartApplication()
    {
#if UNITY_EDITOR
        OwnedHero = new Dictionary<string, Hero>();
        EquippedHero = new List<Hero>();
        if (!PlayerPrefs.HasKey("PlayerID"))
        {
            PlayerID = GenerateGuestPlayerID();
            PlayerPrefs.SetString("PlayerID", PlayerID);
            PlayerPrefs.SetInt("IsAuthenticated", 1);
            PlayerPrefs.Save();
        }
#endif

        LoadLanguageData();
        IsAuthenticated = PlayerPrefs.GetInt("IsAuthenticated") == 1;
        PlayerID = PlayerPrefs.GetString("PlayerID");
        Debug.Log("Loaded PlayerID: " + PlayerID);
        Pubsub.Subscriber.Scope<PlayerEvent>().Subscribe<OnFinishInitializeEvent>(LoadPlayerData);
        Pubsub.Subscriber.Scope<PlayerEvent>().Subscribe<OnGachaEvent>(GenerateNewHero);
        await UniTask.CompletedTask;
    }

    public async UniTask LoadPlayerData(OnFinishInitializeEvent e)
    {
        if(PlayerID != ""){
            Debug.Log("Null ID");
            await SyncHeroesWithDatabase();
            await LoadHeroesFromJSON();
            Debug.Log(PlayerID);
        }
        else{
            Pubsub.Publisher.Scope<UIEvent>().Publish(new ShowScreenEvent(ScreenUI.LOGIN_SCREEN, false));
        }
        await UniTask.CompletedTask;
    }

    public void SetPlayerID(string value)
    {
        PlayerID = value;
        OwnedHero.Clear(); 
        PlayerPrefs.SetString("PlayerID", PlayerID);
        PlayerPrefs.Save();
    }

    public void SetAuthenticateStatus(bool status)
    {
        IsAuthenticated = status;
        PlayerPrefs.SetInt("IsAuthenticated", IsAuthenticated ? 1 : 0);
        Debug.Log("Im " + IsAuthenticated);
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
        await SaveHeroesToFirebase();
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
        int randomNumber = UnityEngine.Random.Range(1000, 9999); // Random number for guest player ID
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
    public async Task LoadHeroesFromJSON()
    {
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
                            await EquipHero(OwnedHero[heroEntry.heroID]);
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

    public async UniTask EquipHero(Hero heroToAdd)
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
        await SaveHeroesToFirebase();
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

    public async UniTask RemoveHero(Hero heroRef){
        if(OwnedHero.ContainsKey(heroRef.heroID)){
             OwnedHero.Remove(heroRef.heroID);
        }
        await SaveHeroesToFirebase();
    }

    public GlobalData LoadGlobalData()
    {
        string globalJson = PlayerPrefs.GetString("GlobalPlayerData");
        Debug.Log("Loading Global Data from PlayerPrefs: " + globalJson);

        if (!string.IsNullOrEmpty(globalJson))
        {
            return JsonUtility.FromJson<GlobalData>(globalJson);
        }
        Debug.Log("No global data found in PlayerPrefs. Returning new GlobalData.");
        return new GlobalData();
    }

    public async UniTask SaveHeroesToFirebase()
    {
        var db = SingleBehaviour.Of<FirebaseDatabase>().Database;

        // Convert OwnedHero dictionary to a dictionary that Firestore can accept
        Dictionary<string, object> heroData = new Dictionary<string, object>();

        foreach (var kvp in OwnedHero)
        {
            Hero hero = kvp.Value;
            
            // Create a dictionary for each hero's data
            Dictionary<string, object> heroFields = new Dictionary<string, object>
            {
                { "heroID", hero.heroID },
                { "heroVisualID", hero.heroVisualID },
                { "hpStep", hero.hpStep },
                { "attackDamageStep", hero.attackDamageStep },
                { "critChance", hero.critChance },
                { "cooldownGenerate", hero.cooldownGenerate },
                { "attackSpeed", hero.attackSpeed },
                { "moveSpeed", hero.moveSpeed },
                { "killDamage", hero.killDamage },
                { "expStep", hero.expStep },
                { "expBasic", hero.expBasic },
                { "exp", hero.exp },
                { "level", hero.level },
                { "isEquipped", hero.isEquipped }
            };

            // Use the heroID as the key to represent each hero
            heroData[hero.heroID] = heroFields;
        }

        // Reference to the PlayerID collection
        Debug.Log("ID is PlayerID" + PlayerID);
        CollectionReference playerCollectionRef = db.Collection(PlayerID); // PlayerID is the collection name

        // Reference to the OwnedHero document inside the PlayerID collection
        DocumentReference ownedHeroDocRef = playerCollectionRef.Document("OwnedHero");

        // Set the hero data under the OwnedHero document
        await ownedHeroDocRef.SetAsync(heroData, SetOptions.Overwrite);

        Debug.Log("Saved hero data to Firestore successfully");

        // Optionally save locally to maintain sync
        SaveHeroesToJSON();
    }

    public async UniTask SyncHeroesWithDatabase()
    {
        var db = SingleBehaviour.Of<FirebaseDatabase>().Database;

        // Reference to the player's collection (PlayerID is the collection name)
        CollectionReference playerCollectionRef = db.Collection(PlayerID);

        // Reference to the OwnedHero document inside the PlayerID collection
        DocumentReference ownedHeroDocRef = playerCollectionRef.Document("OwnedHero");

        // Fetch the document from Firestore
        DocumentSnapshot docSnapshot = await ownedHeroDocRef.GetSnapshotAsync();

        if (docSnapshot.Exists)
        {
            // Document exists, so let's compare and sync data
            Dictionary<string, object> heroDataFromFirestore = docSnapshot.ToDictionary();
            SyncLocalDataWithFirestore(heroDataFromFirestore);
        }
        else
        {
            // If the document doesn't exist, you might want to initialize it with the local data
            Debug.Log("No hero data found in Firestore for PlayerID: " + PlayerID);
        }
    }

    private void SyncLocalDataWithFirestore(Dictionary<string, object> heroDataFromFirestore)
    {
        // Assuming OwnedHero is a Dictionary<string, Hero> in your local data
        Dictionary<string, Hero> newOwnedHeroes = new Dictionary<string, Hero>();

        foreach (var kvp in heroDataFromFirestore)
        {
            string heroID = kvp.Key;
            var heroFields = kvp.Value as Dictionary<string, object>;

            if (heroFields != null)
            {
                // Create a hero from the Firestore data
                Hero hero = new Hero
                {
                    heroID = heroFields["heroID"] as string,
                    heroVisualID = Convert.ToInt32(heroFields["heroVisualID"]),
                    hpStep = Convert.ToSingle(heroFields["hpStep"]),
                    attackDamageStep = Convert.ToSingle(heroFields["attackDamageStep"]),
                    critChance = Convert.ToSingle(heroFields["critChance"]),
                    cooldownGenerate = Convert.ToSingle(heroFields["cooldownGenerate"]),
                    attackSpeed = Convert.ToSingle(heroFields["attackSpeed"]),
                    moveSpeed = Convert.ToSingle(heroFields["moveSpeed"]),
                    killDamage = Convert.ToSingle(heroFields["killDamage"]),
                    expStep = Convert.ToSingle(heroFields["expStep"]),
                    expBasic = Convert.ToSingle(heroFields["expBasic"]),
                    exp = Convert.ToSingle(heroFields["exp"]),
                    level = Convert.ToInt32(heroFields["level"]),
                    isEquipped = Convert.ToBoolean(heroFields["isEquipped"])
                };

                // If the hero is not already in local data, add it
                if (!OwnedHero.ContainsKey(hero.heroID))
                {
                    OwnedHero.Add(hero.heroID, hero);
                    Debug.Log("Added new hero from Firestore: " + hero.heroID);
                }
                else
                {
                    // If the hero exists, update its data
                    OwnedHero[hero.heroID] = hero;
                    Debug.Log("Updated hero from Firestore: " + hero.heroID);
                }
            }
        }
        SaveHeroesToJSON();
    }
}
