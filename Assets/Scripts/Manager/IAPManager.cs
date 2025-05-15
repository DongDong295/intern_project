using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Purchasing;
using UnityEngine.Purchasing.Extension;
using ZBase.Foundation.Singletons;
using ZBase.UnityScreenNavigator.Core.Controls;

[Serializable]
public class ConsumableItem{
    public string Name;
    public string ID;
    public string Price;

}
[Serializable]
public class NonConsumableItem{
    public string Name;
    public string ID;
    public string Price;
}
public class IAPManager : MonoBehaviour, IDetailedStoreListener
{
    public IStoreController Controller;
    public ConsumableItem _500GemPack;
    public NonConsumableItem _removeAd;

    void Awake() {
}

    public async UniTask OnStartApplication(){
        SetUpBuilder();
        Debug.Log("IAPManager Awake, is listener: " + (this is IDetailedStoreListener));
        Debug.Log("Builder setting up");
        await UniTask.CompletedTask;
    }

    public void SetUpBuilder(){
        Debug.Log("Start setting up IAP builder");
        var builder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance());
        Debug.Log("IDS " + _500GemPack.ID + " " + _removeAd.ID);
        builder.AddProduct("500_gem", ProductType.Consumable);
        builder.AddProduct("remove_ad", ProductType.NonConsumable);
        UnityPurchasing.Initialize(this, builder);
        Debug.Log("Called UnityPurchasing.Initialize");
    }

    public void OnInitialized(IStoreController controller, IExtensionProvider extensions)
    {
        Debug.Log("This is called");
        Controller = controller;
        Debug.Log(Controller);
    }

    public void BuyGemPack(){
        Controller.InitiatePurchase(_500GemPack.ID);
    }

    public void BuyRemoveAd(){
        Controller.InitiatePurchase(_removeAd.ID);

    }

    public void OnInitializeFailed(InitializationFailureReason error)
    {
    }

    public void OnInitializeFailed(InitializationFailureReason error, string message)
    {
    }

    public void OnPurchaseFailed(Product product, PurchaseFailureReason failureReason)
    {
    }

    private void IncreaseGemPack(){
        SingleBehaviour.Of<PlayerDataManager>().PlayerGem += 500;
        Pubsub.Publisher.Scope<PlayerEvent>().Publish(new OnUpdateGem(500));
    }

    private void RemoveAd(){

    }

    public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs purchaseEvent)
    {
        var prod = purchaseEvent.purchasedProduct;  
        if(prod.definition.id == _500GemPack.ID){
            IncreaseGemPack();
        }
        else if(prod.definition.id == _removeAd.ID){
            RemoveAd();
        }
        return PurchaseProcessingResult.Complete;
    }

    public void OnPurchaseFailed(Product product, PurchaseFailureDescription failureDescription)
    {
        //throw new NotImplementedException();
    }
}
