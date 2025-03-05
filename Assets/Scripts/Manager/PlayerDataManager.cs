using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class PlayerDataManager : MonoBehaviour
{
    //Local data
    public bool IsAuthenticated;
    public string PlayerID;

    public async UniTask OnStartApplication(){
        await LoadPlayerData();
        Debug.Log("Loaded " + IsAuthenticated);
        Debug.Log("Loaded " + PlayerID);
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
}
