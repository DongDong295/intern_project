using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;
using ZBase.Foundation.Singletons;

public class PlayerDataManager : MonoBehaviour
{
    //Local data
    public List<Hero> OwnedHero;
    public bool IsAuthenticated;
    public string PlayerID;

    public async UniTask OnStartApplication(){
        await LoadPlayerData();
        Debug.Log("Loaded " + IsAuthenticated);
        Debug.Log("Loaded " + PlayerID);
        OwnedHero = new List<Hero>();
        Pubsub.Subscriber.Scope<PlayerEvent>().Subscribe<OnGachaEvent>(GenerateNewHero);
    }

    public async UniTask LoadPlayerData(){
        IsAuthenticated = PlayerPrefs.GetInt("IsAuthenticated") == 1;
        PlayerID = PlayerPrefs.GetString("PlayerID");
        await UniTask.CompletedTask;
    }

    public void SetAuthenticateStatus(string value){
        PlayerID = value;
        PlayerPrefs.SetString("PlayerID", PlayerID);
        PlayerPrefs.Save();
    }

    public void SetAuthenticateStatus(bool status){
        IsAuthenticated = status;
        PlayerPrefs.SetInt("IsAuthenticated", IsAuthenticated ? 1 : 0);
        PlayerPrefs.Save();
    }

    void OnApplicationQuit()
    {
        PlayerPrefs.SetInt("IsAuthenticated", IsAuthenticated ? 1 : 0);
        PlayerPrefs.Save();
    }

    public async UniTask GenerateNewHero(OnGachaEvent e)
    {
        var data = await Singleton.Of<DataManager>().Load<HeroData>(Data.HERO_DATA);
        var heroData = data.heroDataItems;
        var heroID = GenerateUniqueHeroID();
        var hero = new Hero(heroID, heroData[Random.Range(0, heroData.Length)]);
        OwnedHero.Add(hero);
        Debug.Log($"Generated new hero with ID: {heroID}");
        await UniTask.CompletedTask;
    }

    private string GenerateUniqueHeroID()
    {
        var timestamp = System.DateTime.UtcNow.ToString("yyyyMMddHHmmssfff");
        var randomPart = Random.Range(1000, 9999);
        return $"{timestamp}{randomPart}";
    }
}
