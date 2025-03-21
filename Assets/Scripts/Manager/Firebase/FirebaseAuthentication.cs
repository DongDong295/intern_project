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
    }

    public void SignInWithGoogle(OnPlayerLoginEvent e) {
        GoogleSignIn.Configuration = configuration;
        GoogleSignIn.Configuration.UseGameSignIn = false;
        GoogleSignIn.Configuration.RequestIdToken = true;
        GoogleSignIn.Configuration.RequestEmail = true;

        GoogleSignIn.DefaultInstance.SignIn().ContinueWith(OnGoogleAuthenticatedFinished);
    }

    private void OnGoogleAuthenticatedFinished(Task<GoogleSignInUser> task) {
        if (task.IsFaulted) {
            Debug.LogError("Faulted");
        } else if (task.IsCanceled) {
            Debug.LogError("Cancelled");
        } else {
            Firebase.Auth.Credential credential = Firebase.Auth.GoogleAuthProvider.GetCredential(task.Result.IdToken, null);

            auth.SignInWithCredentialAsync(credential).ContinueWithOnMainThread(task => {
                if (task.IsCanceled) {
                    return;
                }

                if (task.IsFaulted) {
                    Debug.LogError("SignInWithCredentialAsync encountered an error: " + task.Exception);
                    return;
                }

                user = auth.CurrentUser;
                SingleBehaviour.Of<PlayerDataManager>().SetAuthenticateStatus(true);
                SingleBehaviour.Of<PlayerDataManager>().SetPlayerID(user.UserId);
                Pubsub.Publisher.Scope<PlayerEvent>().Publish(new OnFinishInitializeEvent());
            });
        }
    }

    public void SignOut(){
        auth.SignOut();
        GoogleSignIn.DefaultInstance.SignOut();
        user = null;
        SingleBehaviour.Of<PlayerDataManager>().SetAuthenticateStatus(false);
        SingleBehaviour.Of<PlayerDataManager>().SetPlayerID("");
    }
}
