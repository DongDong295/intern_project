using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.Localization.Settings;
using UnityEngine.UI;
using ZBase.Foundation.Singletons;

public class SettingsModal : BasicModal
{
    [SerializeField] private Button _logoutButton;
    [SerializeField] private TMP_Dropdown _languageDropdown;
    public override UniTask Initialize(Memory<object> args)
    {
        base.Initialize(args);
        if (_languageDropdown != null)
        {
            PopulateDropdown();

            _languageDropdown.onValueChanged.AddListener(OnLanguageChanged);
        }

        _logoutButton.onClick.AddListener(() => {Logout();});
        return UniTask.CompletedTask;
    }

    public void Logout(){
        
        Pubsub.Publisher.Scope<UIEvent>().Publish(new CloseModalEvent());
        SingleBehaviour.Of<FirebaseAuthentication>().SignOut();
        Pubsub.Publisher.Scope<UIEvent>().Publish(new ShowScreenEvent(ScreenUI.LOGIN_SCREEN, false));
    }

    public override UniTask Cleanup(Memory<object> args)
    {
        _logoutButton.onClick.RemoveAllListeners();
        _languageDropdown.ClearOptions();
        
        return base.Cleanup(args);
    }

    void PopulateDropdown()
    {
        _languageDropdown.ClearOptions(); 

        List<string> languageOptions = new List<string>();

        foreach (var locale in LocalizationSettings.AvailableLocales.Locales)
        {
            languageOptions.Add(locale.LocaleName);
        }

        _languageDropdown.AddOptions(languageOptions);
    }

    public void OnLanguageChanged(int index)
    {
        ChangeLanguageSetting(index);
    }

    public void ChangeLanguageSetting(int index)
    {
        if (index >= 0 && index < LocalizationSettings.AvailableLocales.Locales.Count)
        {
            LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[index];
            PlayerPrefs.SetInt("LanguageOption", index);
            PlayerPrefs.Save();
        }
    }
}

