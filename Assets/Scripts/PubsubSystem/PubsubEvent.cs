using System.Collections;
using System.Collections.Generic;
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

public struct UIEvent{}
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
