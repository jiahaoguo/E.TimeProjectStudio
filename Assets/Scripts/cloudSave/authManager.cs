using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Services.Core;
using Unity.Services.Authentication;
using System.Threading.Tasks;
using UnityEngine.UI;
using Assets.SimpleSignIn.Google;

public class authManager : MonoBehaviour
{
    [HideInInspector] public Text logTxt;
    [HideInInspector] static public string googleIdToken = "329035547293-h86s58ve856q1kqcjm3oi9iomrvhu02m.apps.googleusercontent.com"; // You will set the Google ID token here
    public Text playerid;
    public Example googleExample;

    async void Start()
    {
        // Initialize Unity services and sign in anonymously
        await UnityServices.InitializeAsync();
        //SignIn();
    }

    // This method is responsible for signing in
    public async void SignIn(string id)
    {
        //await signInAnonymous();
        await signInWithGoogle(id);
    }

    // Anonymous sign-in method
    async Task signInAnonymous()
    {
        try
        {
            await AuthenticationService.Instance.SignInAnonymouslyAsync();

            Debug.Log("Sign in Success");
            Debug.Log("Player Id:" + AuthenticationService.Instance.PlayerId);
            playerid.text = AuthenticationService.Instance.PlayerId;
        }
        catch (AuthenticationException ex)
        {
            Debug.LogError("Sign in failed!!");
            Debug.LogException(ex);
        }
        catch (RequestFailedException ex)
        {
            Debug.LogError("Sign in request failed!!");
            Debug.LogException(ex);
        }
    }


    async Task signInWithGoogle(string googleIdToken)
    {
        try
        {
            await AuthenticationService.Instance.SignInWithGoogleAsync(googleIdToken);

            Debug.Log("Sign in with Google success");
            Debug.Log("Player Id:" + AuthenticationService.Instance.PlayerId);
            playerid.text = AuthenticationService.Instance.PlayerId;
        }
        catch (AuthenticationException ex)
        {
            Debug.LogError("Sign in with Google failed!!");
            Debug.LogException(ex);
        }
        catch (RequestFailedException ex)
        {
            Debug.LogError("Request failed!!");
            Debug.LogException(ex);
        }
    }

    // Facebook and Steam sign-in methods (optional, from your original code)
}
