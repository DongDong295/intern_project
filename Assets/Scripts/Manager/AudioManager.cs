using System.Collections;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class AudioManager : MonoBehaviour
{
    private AudioSource _audioSource;
    public bool IsEnableAudio;
    
    CancellationTokenSource _cts;
    public async UniTask OnStartApplication()
    {
        _cts = new CancellationTokenSource();
        IsEnableAudio = PlayerPrefs.GetInt("AudioEnabled", 1) == 1;
        _audioSource = Camera.main.GetComponent<AudioSource>();
        await UniTask.CompletedTask;
    }

    public async UniTask PlayMusic(string addressableKey)
    {
        Debug.Log("Playing music " + IsEnableAudio);
        if(!IsEnableAudio){
            return;
        }

        AsyncOperationHandle<AudioClip> handle = Addressables.LoadAssetAsync<AudioClip>(addressableKey);
        await handle.ToUniTask();

        if (handle.Status == AsyncOperationStatus.Succeeded)
        {
            AudioClip clip = handle.Result;
            _audioSource.clip = clip;
            _audioSource.Play();
        }
        else
        {
            Debug.LogError($"Failed to load AudioClip with key: {addressableKey}");
        }
    }

    public void ToggleAudio(bool isOn){
        IsEnableAudio = isOn;
        if(!isOn)
            _audioSource.Stop();
        else if(isOn)
            PlayMusic("music-menu").Forget();
        PlayerPrefs.SetInt("AudioEnabled", IsEnableAudio ? 1 : 0);
        PlayerPrefs.Save();
    }
}
