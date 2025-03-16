using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using ZBase.Foundation.Singletons;

public class BuffSphere : MonoBehaviour, IDispose
{
    private BuffDataItems data;
    private StageManager _stageManager;

    [SerializeField] private TextMeshProUGUI _displayBuff;
    public async UniTask InitiateBuff(BuffDataItems data){
        _stageManager = SingleBehaviour.Of<StageManager>();
        this.data = data;
        DisplayBuffName();
        _stageManager.DisposeList.Add(this);
        await UniTask.CompletedTask;
    }

    void OnMouseDown()
    {
        ActiveBuff().Forget();
        SingleBehaviour.Of<PoolingManager>().Return(gameObject);
    }
    public async UniTask ActiveBuff(){
        foreach(var i in _stageManager.DisposeList){
        if(i is MiniHeroBehaviour){
            var a = i as MiniHeroBehaviour;
            a.IncreaseStats(data.buffName, data.buffValue, data.buffTime).Forget();
        }
        await UniTask.CompletedTask;
    }
    }

    private void DisplayBuffName(){
        _displayBuff.transform.position = transform.position + new Vector3(0, 2, 0);
        switch(data.buffName){
            case BuffType.AttackSpeed:
                _displayBuff.text = "Increase Attack Speed!";
                break;
            case BuffType.MovementSpeed:
                _displayBuff.text = "Increase Movement Speed!";
                break;
        }
    }

    public void Dispose(){
        SingleBehaviour.Of<PoolingManager>().Return(gameObject);
    }
}
