using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Auth : MonoBehaviour
{
    public enum Infrastructure { Firebase, Amazon };
    public Infrastructure AuthInfrastructure;
    public DataSource Data;
    public GUITownTemporary GUI;

    AuthProvider authProvider;

    public bool LoggedIn()
    {
        return VariableManager.Instance.LocalExists("mail_address") && VariableManager.Instance.LocalExists("password") &&
                !string.IsNullOrEmpty(VariableManager.Instance.GetLocal("mail_address")) &&
                !string.IsNullOrEmpty(VariableManager.Instance.GetLocal("password"));
    }
    void Start()
    {
        authProvider = null;

        switch (AuthInfrastructure)
        {
            case Infrastructure.Amazon:
                authProvider = GetComponent<AuthProviderAmazon>();
                ((AuthProviderAmazon)authProvider).Init();
                break;
            case Infrastructure.Firebase:
                authProvider = GetComponent<AuthProviderFirebase>();
                ((AuthProviderFirebase)authProvider).Init();
                break;
        }

        if(authProvider != null)
        {
            Debug.Log("Authentication provider: " + authProvider.Vendor());

            if(LoggedIn())
            {
                Login(VariableManager.Instance.GetLocal("mail_address"), VariableManager.Instance.GetLocal("password"));
            }
        }
    }

    public void Register(string userName, string password)
    {
        if (authProvider != null)
        {
            authProvider.OnRegisterCompleted = (succeeded, message) => {
                if (!succeeded)
                {
                    GUI.NotifyError("REGISTRATION FAILED", message);
                    StartCoroutine(BackToLoginSequence());
                }
                else
                {
                    // notify user about the operation
                    GUI.Notify("REGISTRATION SUCCEEDED", "You are being redirected now");

                    string[] elements = message.Split('|');
                    VariableManager.Instance.AddOrSetLocal("userid", elements[0]);
                    VariableManager.Instance.AddOrSetLocal("display_name", elements[1]);
                    VariableManager.Instance.AddOrSetLocal("mail_address", userName);
                    VariableManager.Instance.AddOrSetLocal("password", password);

                    // save additional user data
                    Data.GetProvider().CreateUser(new DOUser(elements[0], elements[1]), () => {
                        StartCoroutine(PostLoginSequence());
                    });
                }
            };

            authProvider.Register(userName, password);
        }
        else
        {
            GUI.NotifyError("ERROR", "Authentication provider not set");
        }
    }

    public void Login(string userName, string password)
    {
        if (authProvider != null)
        {
            authProvider.OnLoginCompleted = (succeeded, message) => {
                if(!succeeded)
                {
                    GUI.NotifyError("LOGIN FAILED", message);
                    StartCoroutine(BackToLoginSequence());
                }
                else
                {
                    GUI.Notify("LOGIN SUCCEEDED", "You are being redirected now");

                    string[] elements = message.Split('|');
                    VariableManager.Instance.AddOrSetLocal("userid", elements[0]);
                    VariableManager.Instance.AddOrSetLocal("display_name", elements[1]);
                    VariableManager.Instance.AddOrSetLocal("mail_address", userName);
                    VariableManager.Instance.AddOrSetLocal("password", password);

                    StartCoroutine(PostLoginSequence());
                }
            };

            authProvider.Login(userName, password);
        } else
        {
            GUI.NotifyError("ERROR", "Authentication provider not set");
        }
    }

    IEnumerator BackToLoginSequence()
    {
        yield return new WaitForSeconds(GUI.TransitionTime);

        GUI.HideLoading();
        yield return new WaitForSeconds(GUI.TransitionTime);

        GUI.DisplayLogin();
    }

    IEnumerator PostLoginSequence()
    {
        yield return new WaitForSeconds(GUI.TransitionTime);

        GUI.HideLogin();
        yield return new WaitForSeconds(GUI.TransitionTime);

        GUI.DisplayLoading("Loading user settings...");
        Data.GetProvider().SetUserLastVisitDate(VariableManager.Instance.GetLocal("userid").ToString());
        Data.GetProvider().GetUser(VariableManager.Instance.GetLocal("userid"), (user) =>
        {
            if (user == null)
            {
                StartCoroutine(PostLoginFailed());
            }
            else
            {
                StartCoroutine(PostLoginSucceeded(user));
            }
        });
    }

    IEnumerator PostLoginFailed()
    {
        GUI.NotifyError("ERROR", "User information could not be retreived");
        VariableManager.Instance.ResetLocals();
        VariableManager.Instance.ResetVariables();
        GUI.HideLoading();
        yield return new WaitForSeconds(GUI.TransitionTime);

        GUI.DisplayLogin();
    }

    IEnumerator PostLoginSucceeded(DOUser user)
    {
        yield return new WaitForSeconds(GUI.TransitionTime);

        GUI.Notify("PLAYER NICK NAME", user.NickName);
        GUI.HideLoading();

        yield return new WaitForSeconds(GUI.TransitionTime);

        GUI.DisplayCity();
    }
}
