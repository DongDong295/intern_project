using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ZBase.Foundation.Singletons;

public class Constants
{
    //Google API
    public const string GOOGLE_API_KEY = "171174664471-q49e8fbvpj6er083g2614i9glguvm0o4.apps.googleusercontent.com";

}

public class Data
{
    public const string STAGE_DATA = "StageData";
    public const string HERO_DATA = "HeroData";

    public const string BUFF_DATA = "BuffData";
}

public class ScreenUI{
    private string platform;
    public const string LOGIN_SCREEN = "ui-login-screen";
    public const string LOADING_SCREEN = "ui-loading-screen-{0}";

    public const string MAIN_MENU_SCREEN = "ui-main-menu-screen";
    public const string MAIN_GAMEPLAY_SCREEN = "ui-main-gameplay-screen";

    public static string GetLoadingScreen(string p){
        return string.Format(LOADING_SCREEN, p);
    }
}

public class ModalUI{

    public const string STAGE_SELECTION_MODAL = "ui-stage-selection-modal";
    public const string CHARACTERS_MODAL = "ui-characters-modal";

    public const string HERO_INFORMATION_MODAL = "ui-hero-information-modal";
    public const string STAGE_END_MODAL = "ui-stage-end-modal";

    public const string SETTINGS = "ui-settings-modal";
}

public class PlayerPref{
    public const string PLAYER_ID = "PlayerID";
    public const string IS_AUTHENTICATED = "IsAuthenticated";
}
