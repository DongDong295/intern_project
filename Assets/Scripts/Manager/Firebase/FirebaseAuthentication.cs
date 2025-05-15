using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using Firebase.Extensions;
using Google;
using UnityEngine;
using UnityEngine.UI;
using ZBase.Foundation.PubSub;
using ZBase.Foundation.Singletons;

public class FirebaseAuthentication : FirebaseModule
{
    private GoogleSignInConfiguration configuration;

    //private Firebase.DependencyStatus dependencyStatus = Firebase.DependencyStatus.UnavailableOther;
    private Firebase.Auth.FirebaseAuth auth;
    private Firebase.Auth.FirebaseUser user;

    public override UniTask InitModule()
    {
        base.InitModule();
        InitFirebaseAuthentication();
        Pubsub.Subscriber.Scope<PlayerEvent>().Subscribe<OnPlayerLoginEvent>(SignInWithGoogle);
        return UniTask.CompletedTask;
    }

    void InitFirebaseAuthentication() {
        configuration = new GoogleSignInConfiguration{
            WebClientId = Constants.GOOGLE_API_KEY,
            RequestIdToken = true,
        };

        auth = Firebase.Auth.FirebaseAuth.DefaultInstance;
        Debug.Log(auth);
    }

    public async void SignInWithGoogle(OnPlayerLoginEvent e)
    {
        GoogleSignIn.Configuration = configuration;
        GoogleSignIn.Configuration.UseGameSignIn = false;
        GoogleSignIn.Configuration.RequestIdToken = true;
        GoogleSignIn.Configuration.RequestEmail = true;

        try
        {
            Debug.Log("Starting Google sign-in...");
            var googleUser = await GoogleSignIn.DefaultInstance.SignIn();

            if (googleUser == null)
            {
                Debug.LogError("Google sign-in returned null user.");
                return;
            }

            Debug.Log("Google sign-in succeeded: " + googleUser.DisplayName);
            Debug.Log("ID Token: " + googleUser.IdToken);

            if (string.IsNullOrEmpty(googleUser.IdToken))
            {
                Debug.LogError("Google user ID token is null or empty.");
                return;
            }

            var credential = Firebase.Auth.GoogleAuthProvider.GetCredential(googleUser.IdToken, null);

            if (auth == null)
            {
                Debug.LogError("FirebaseAuth is not initialized.");
                return;
            }

            var authResult = await auth.SignInWithCredentialAsync(credential);
            Debug.Log("Firebase Auth success: " + authResult.UserId);
            Debug.Log(SingleBehaviour.Of<PlayerDataManager>());
            SingleBehaviour.Of<PlayerDataManager>().SetAuthenticateStatus(true);
            //SingleBehaviour.Of<PlayerDataManager>().SetPlayerID(authResult.UserId);
            //PlayerPrefs.SetInt(PlayerPref.IS_AUTHENTICATED, 1);
            PlayerPrefs.SetString(PlayerPref.PLAYER_ID, authResult.UserId);
            Debug.Log("Save player id " + authResult.UserId);
            PlayerPrefs.Save();
            Pubsub.Publisher.Scope<PlayerEvent>().Publish(new OnFinishInitializeEvent());
        }
        catch (System.Exception ex)
        {
            Debug.LogError("Google sign-in failed: " + ex.Message);
            Debug.LogException(ex);
        }
    }

    private void OnGoogleAuthenticatedFinished(Task<GoogleSignInUser> task) {
        Debug.Log("Continue login");
        if (task.IsFaulted) {
            Debug.LogError("Faulted");
        } else if (task.IsCanceled) {
            Debug.LogError("Cancelled");
        } else {
            Firebase.Auth.Credential credential = Firebase.Auth.GoogleAuthProvider.GetCredential(task.Result.IdToken, null);

            auth.SignInWithCredentialAsync(credential).ContinueWithOnMainThread(task => {
                if (task.IsCanceled) {
                    Debug.Log("Error so yo");
                    return;
                }

                if (task.IsFaulted) {
                    Debug.LogError("SignInWithCredentialAsync encountered an error: " + task.Exception);
                    return;
                }

                user = auth.CurrentUser;
                Debug.Log("Save player id " + user.UserId);
                Pubsub.Publisher.Scope<PlayerEvent>().Publish(new OnFinishInitializeEvent());
            });
        }
    }

    public void SignOut()
    {
        Debug.Log("Signing out...");

        try
        {
            // Sign out from Firebase
            if (auth != null)
            {
                auth.SignOut();
                Debug.Log("Signed out from Firebase.");
            }

            // Sign out from Google
            GoogleSignIn.DefaultInstance.SignOut();
            Debug.Log("Signed out from Google.");

            // Clear local user reference
            user = null;

            // Clear player-related PlayerPrefs
            PlayerPrefs.DeleteKey(PlayerPref.PLAYER_ID);
            PlayerPrefs.DeleteKey(PlayerPref.IS_AUTHENTICATED);
            PlayerPrefs.Save();

            // Reset PlayerDataManager state if needed
            if (SingleBehaviour.Of<PlayerDataManager>() != null)
            {
                SingleBehaviour.Of<PlayerDataManager>().SetAuthenticateStatus(false);
            }

            Debug.Log("User sign-out complete.");
        }
        catch (System.Exception ex)
        {
            Debug.LogError("Error during sign-out: " + ex.Message);
            Debug.LogException(ex);
        }
    }

}
