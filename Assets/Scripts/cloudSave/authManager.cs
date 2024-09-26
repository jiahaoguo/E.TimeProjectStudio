using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Services.Core;
using Unity.Services.Authentication;
using System.Threading.Tasks;
using UnityEngine.UI;

public class authManager : MonoBehaviour
{
    [HideInInspector] public Text logTxt;
    [HideInInspector] public string facebookToken = "....";

    async void Start()
    {
        // Initialize Unity services and sign in anonymously
        await UnityServices.InitializeAsync();
        SignIn();
    }

    // This method is responsible for signing in
    public async void SignIn()
    {
        await signInAnonymous();
    }

    // Anonymous sign-in method
    async Task signInAnonymous()
    {
        try
        {
            await AuthenticationService.Instance.SignInAnonymouslyAsync();

            Debug.Log("Sign in Success");
            Debug.Log("Player Id:" + AuthenticationService.Instance.PlayerId);
            // You can update UI elements here to show successful sign-in
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

    // Facebook sign-in method (you may not need this, but it's here for reference)
    async Task signInWithFacebook(string token)
    {
        try
        {
            await AuthenticationService.Instance.SignInWithFacebookAsync(token);
            Debug.Log("Sign in with Facebook success");
        }
        catch (AuthenticationException ex)
        {
            Debug.LogError("Sign in with Facebook failed!!");
            Debug.LogException(ex);
        }
        catch (RequestFailedException ex)
        {
            Debug.LogError("Request failed!!");
            Debug.LogException(ex);
        }
    }

    // Steam sign-in method (you may not need this, but it's here for reference)
    async Task signInWithSteam(string token)
    {
        try
        {
            await AuthenticationService.Instance.SignInWithSteamAsync(token);
            Debug.Log("Sign in with Steam success");
        }
        catch (AuthenticationException ex)
        {
            Debug.LogError("Sign in with Steam failed!!");
            Debug.LogException(ex);
        }
        catch (RequestFailedException ex)
        {
            Debug.LogError("Request failed!!");
            Debug.LogException(ex);
        }
    }
}
