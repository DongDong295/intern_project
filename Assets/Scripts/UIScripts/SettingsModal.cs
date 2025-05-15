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
    [SerializeField] private Toggle _audio;
    public override UniTask Initialize(Memory<object> args)
    {
        bool isAudioEnabled = SingleBehaviour.Of<AudioManager>().IsEnableAudio;
        _audio.isOn = isAudioEnabled;
        base.Initialize(args);
        if (_languageDropdown != null)
        {
            PopulateDropdown();

            _languageDropdown.onValueChanged.AddListener(OnLanguageChanged);
        }
        _audio.onValueChanged.AddListener(OnAudioToggleChanged);

        _logoutButton.onClick.AddListener(() => {Logout();});
        return UniTask.CompletedTask;
    }

    public void Logout(){
        
        Pubsub.Publisher.Scope<UIEvent>().Publish(new CloseModalEvent());
        SingleBehaviour.Of<FirebaseAuthentication>().SignOut();
        Pubsub.Publisher.Scope<UIEvent>().Publish(new ShowScreenEvent(ScreenUI.LOGIN_SCREEN, false));
    }

    private void OnAudioToggleChanged(bool isOn)
    {
        SingleBehaviour.Of<AudioManager>().ToggleAudio(isOn);
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
        int currentIndex = 0;

        var currentLocale = LocalizationSettings.SelectedLocale;

        for (int i = 0; i < LocalizationSettings.AvailableLocales.Locales.Count; i++)
        {
            var locale = LocalizationSettings.AvailableLocales.Locales[i];
            languageOptions.Add(locale.LocaleName);

            if (locale == currentLocale)
            {
                currentIndex = i;
            }
        }

        _languageDropdown.AddOptions(languageOptions);

        // Set to current language
        _languageDropdown.value = currentIndex;
        _languageDropdown.RefreshShownValue(); // Make sure the label updates
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

