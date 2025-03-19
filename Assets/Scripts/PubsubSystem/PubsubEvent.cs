using System.Collections;
using System.Collections.Generic;
using UnityEditor.SceneManagement;
using UnityEngine;
using ZBase.Foundation.PubSub;

public struct PlayerEvent{}
public struct OnFinishInitializeEvent : IMessage{}
public struct OnPlayerLoginEvent : IMessage{}

public struct OnPlayerFinishAuthentication : IMessage{}


public struct OnEnterGamePlayScene : IMessage{
    public int StageIndex;
    public OnEnterGamePlayScene(int stageIndex){
        StageIndex = stageIndex;
    }
}

public struct OnShowHeroInformationEvent : IMessage{
    public Hero HeroRef;
    public OnShowHeroInformationEvent(Hero hero){
        HeroRef = hero;
    }
}

public struct OnGachaEvent : IMessage{
    
}

public struct OnBossTakeDamageEvent : IMessage{

}

public struct OnPlayerEquipHero : IMessage{
    public string HeroID;
    public bool IsEquip;
    public OnPlayerEquipHero(string id, bool isEquip){
        HeroID = id;
        this.IsEquip = isEquip;
    }
}

public struct OnSelectMaterialHeroForUpgrade : IMessage{
    public Hero MaterialHero;
    public bool IsSelected;
    public OnSelectMaterialHeroForUpgrade(Hero hero, bool isSelected){
        MaterialHero = hero;
        IsSelected = isSelected;
    }
}

public struct OnUpgradeHero : IMessage{
    
}

public struct OnFinishStage : IMessage{
    public bool Result;
    public OnFinishStage(bool result){
        Result = result;
    }
}

public struct UIEvent{}
public struct ShowModalEvent<T> : IMessage
{
    public T Parameter; 

    public ShowModalEvent(T parameter)
    {
        Parameter = parameter;
    }
}

public struct ShowScreenEvent : IMessage{
    public string Path;
    public bool LoadAsync;
    public ShowScreenEvent(string path, bool loadAsync){
        Path = path;
        LoadAsync = loadAsync;
    }
}

public struct ShowModalEvent : IMessage{
    public string Path;
    public bool LoadAsync;
    public ShowModalEvent(string path, bool loadAsync){
        Path = path;
        LoadAsync = loadAsync;
    }
}

public struct CloseModalEvent : IMessage{
    public bool IsPlayAnim;
    public CloseModalEvent(bool isPlayAnim){
        IsPlayAnim = isPlayAnim;
    }
}

public struct CloseScreenEvent : IMessage{
    public bool IsPlayAnim;
    public CloseScreenEvent(bool isPlayAnim){
        IsPlayAnim = isPlayAnim;
    }
}
