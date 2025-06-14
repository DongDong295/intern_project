using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using Firebase.Firestore;
using Unity.VisualScripting;
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

    public Dictionary<string, Hero> OwnedHeroDict = new Dictionary<string, Hero>();

    public int PlayerGem;

    public bool FinishLoadData;

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
        if (!PlayerPrefs.HasKey(PlayerPref.PLAYER_ID))
        {
            PlayerID = GenerateGuestPlayerID();
            PlayerPrefs.SetString(PlayerPref.PLAYER_ID, PlayerID);
            PlayerPrefs.SetInt(PlayerPref.IS_AUTHENTICATED, 1);
            PlayerPrefs.Save();
        }
#endif

        LoadLanguageData();
        IsAuthenticated = PlayerPrefs.GetInt(PlayerPref.IS_AUTHENTICATED) == 1;
        PlayerID = PlayerPrefs.GetString(PlayerPref.PLAYER_ID, PlayerID);
        Debug.Log("Loaded PlayerID: " + PlayerID);
        Pubsub.Subscriber.Scope<PlayerEvent>().Subscribe<OnFinishInitializeEvent>(LoadPlayerData);
        Pubsub.Subscriber.Scope<PlayerEvent>().Subscribe<OnGachaEvent>(GenerateNewHero);
        await UniTask.CompletedTask;
    }

    public async UniTask LoadPlayerData(OnFinishInitializeEvent e)
    {
        FinishLoadData = false;
        Debug.Log("Loading Player Data");
        PlayerID = PlayerPrefs.GetString(PlayerPref.PLAYER_ID);

        Debug.Log("Loaded from PlayerPref: " + PlayerID);
        await SyncHeroesWithDatabase();
        await LoadHeroesFromJSON();
        await LoadPlayerGemFromFirebase();
        FinishLoadData = true;
        await UniTask.CompletedTask;
    }

    public void SetPlayerID(string value)
    {
        PlayerID = value;
        OwnedHero.Clear(); 
        PlayerPrefs.SetString(PlayerPref.PLAYER_ID, PlayerID);
        PlayerPrefs.Save();
    }

    public void SetAuthenticateStatus(bool status)
    {
        IsAuthenticated = status;
        PlayerPrefs.SetInt(PlayerPref.IS_AUTHENTICATED, IsAuthenticated ? 1 : 0);
        PlayerPrefs.Save();
    }

    void OnApplicationQuit()
    {
        Debug.Log("Save!");
        PlayerPrefs.SetInt(PlayerPref.IS_AUTHENTICATED, IsAuthenticated ? 1 : 0);
        //SaveHeroesToJSON();
        SavePlayerGemToFirebase().Forget();
        SaveHeroes().Forget();
        PlayerPrefs.Save();
    }

    private async UniTask SaveHeroes(){
        await SaveHeroesToFirebase();
    }
    public async UniTask GenerateNewHero(OnGachaEvent e)
    {
        if(PlayerGem < 500){
            Pubsub.Publisher.Scope<UIEvent>().Publish(new ShowModalEvent(ModalUI.GEM_NOTIFY, false));
            return;
        }
        else{
            PlayerGem -= 500;
            Pubsub.Publisher.Scope<PlayerEvent>().Publish(new OnUpdateGem(-500));
            var data = await Singleton.Of<DataManager>().Load<HeroData>(Data.HERO_DATA);
            var heroData = data.heroDataItems;
            var heroID = GenerateUniqueHeroID();
            var hero = new Hero(heroID, heroData[UnityEngine.Random.Range(0, heroData.Length)]);
            OwnedHero[heroID] = hero;
            Debug.Log($"Generated new hero with ID: {heroID}");

            // Save the updated hero dictionary to PlayerPrefs as JSON after generating a new hero
            await SaveHeroesToFirebase();
            Pubsub.Publisher.Scope<PlayerEvent>().Publish(new OnGenerateHero());
            await UniTask.CompletedTask;
        }
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
        //Debug.Log("Hero Data to Save: " + jsonHeroData);

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
    public async UniTask LoadHeroesFromJSON()
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
        if (!OwnedHeroDict.ContainsKey(heroToAdd.heroID) && OwnedHeroDict.Count < 5)
        {

            heroToAdd.isEquipped = true;
            OwnedHeroDict.Add(heroToAdd.heroID, heroToAdd);
        }
        else
        {
            if (OwnedHero.ContainsKey(heroToAdd.heroID)){
                //EquippedHero.Remove(heroToAdd);
                OwnedHeroDict.Remove(heroToAdd.heroID);
            }
            heroToAdd.isEquipped = false;
        }
        Pubsub.Publisher.Scope<PlayerEvent>().Publish(new OnPlayerEquipHero(heroToAdd.heroID, heroToAdd.isEquipped, heroToAdd));
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
        //Debug.Log("Loading Global Data from PlayerPrefs: " + globalJson);

        if (!string.IsNullOrEmpty(globalJson))
        {
            return JsonUtility.FromJson<GlobalData>(globalJson);
        }
        Debug.Log("No global data found in PlayerPrefs. Returning new GlobalData.");
        return new GlobalData();
    }

    public async UniTask SavePlayerGemToFirebase()
    {
        var db = SingleBehaviour.Of<FirebaseDatabase>().Database;
        CollectionReference playerCollectionRef = db.Collection(PlayerID);
        DocumentReference gemDocRef = playerCollectionRef.Document("PlayerData");

        Dictionary<string, object> playerData = new Dictionary<string, object>
        {
            { "PlayerGem", PlayerGem }
        };

        await gemDocRef.SetAsync(playerData, SetOptions.MergeAll);
    }

    public async UniTask LoadPlayerGemFromFirebase()
    {
        var db = SingleBehaviour.Of<FirebaseDatabase>().Database;
        CollectionReference playerCollectionRef = db.Collection(PlayerID);
        DocumentReference gemDocRef = playerCollectionRef.Document("PlayerData");

        DocumentSnapshot docSnapshot = await gemDocRef.GetSnapshotAsync();

        if (docSnapshot.Exists && docSnapshot.TryGetValue("PlayerGem", out int gemValue))
        {
            PlayerGem = gemValue;
            Pubsub.Publisher.Scope<PlayerEvent>().Publish(new OnUpdateGem(PlayerGem));
        }
        else
        {
            Debug.Log("No PlayerGem data found in Firebase.");
        }
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
        //Debug.Log("ID is PlayerID" + PlayerID);
        CollectionReference playerCollectionRef = db.Collection(PlayerID); // PlayerID is the collection name

        // Reference to the OwnedHero document inside the PlayerID collection
        DocumentReference ownedHeroDocRef = playerCollectionRef.Document("OwnedHero");

        // Set the hero data under the OwnedHero document
        await ownedHeroDocRef.SetAsync(heroData, SetOptions.Overwrite);

        //Debug.Log("Saved hero data to Firestore successfully");

        // Optionally save locally to maintain sync
        SaveHeroesToJSON();
    }

    public async UniTask SyncHeroesWithDatabase()
    {
        Debug.Log("Syncing with database");
        var db = SingleBehaviour.Of<FirebaseDatabase>().Database;

        // Reference to the player's collection (PlayerID is the collection name)
        CollectionReference playerCollectionRef = db.Collection(PlayerPrefs.GetString(PlayerPref.PLAYER_ID));

        // Reference to the OwnedHero document inside the PlayerID collection
        DocumentReference ownedHeroDocRef = playerCollectionRef.Document("OwnedHero");

        // Fetch the document from Firestore
        DocumentSnapshot docSnapshot = await ownedHeroDocRef.GetSnapshotAsync();

        if (docSnapshot.Exists)
        {
            Dictionary<string, object> heroDataFromFirestore = docSnapshot.ToDictionary();
            SyncLocalDataWithFirestore(heroDataFromFirestore);
        }
        else
        {
            // If the document doesn't exist, you might want to initialize it with the local data
            //Debug.Log("No hero data found in Firestore for PlayerID: " + PlayerID);
        }
    }

    private void SyncLocalDataWithFirestore(Dictionary<string, object> heroDataFromFirestore)
    {
        OwnedHero = new Dictionary<string, Hero>();

        if(heroDataFromFirestore == null)
            return;

        foreach (var kvp in heroDataFromFirestore)
        {
            Debug.Log("Hero Key" + kvp.Key);
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
                    //Debug.Log("Updated hero from Firestore: " + hero.heroID);
                }
            }
        }
        //SaveHeroesToJSON();
    }
}
