using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ZBase.Foundation.PubSub;

public struct PlayerEvent{}
public struct OnFinishInitializeEvent : IMessage{}
public struct OnPlayerLoginEvent : IMessage{}

public struct OnPlayerFinishAuthentication : IMessage{}

public struct UIEvent{}
public struct ShowScreenEvent : IMessage{
    public string Path;
    public bool LoadAsync;
    public ShowScreenEvent(string path, bool loadAsync){
        Path = path;
        LoadAsync = loadAsync;
    }
}
