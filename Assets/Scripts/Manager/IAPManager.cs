using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Purchasing;

public class IAPManager : MonoBehaviour, IStoreListener
{
    private Consumable _500GemPack;
    private NonConsumable _removeAd;
    public void SetUpBuilder(){
        var Builder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance());
        Builder.AddProduct(_500GemPack.ID, ProductType.Consumable);
        Builder.AddProduct(_removeAd.ID, ProductType.NonConsumable);
    }
    public void OnInitialized(IStoreController controller, IExtensionProvider extensions)
    {
        throw new System.NotImplementedException();
    }

    public void OnInitializeFailed(InitializationFailureReason error)
    {
        throw new System.NotImplementedException();
    }

    public void OnInitializeFailed(InitializationFailureReason error, string message)
    {
        throw new System.NotImplementedException();
    }

    public void OnPurchaseFailed(Product product, PurchaseFailureReason failureReason)
    {
        throw new System.NotImplementedException();
    }

    public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs purchaseEvent)
    {
        throw new System.NotImplementedException();
    }
}
