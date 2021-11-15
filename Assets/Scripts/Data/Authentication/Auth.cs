using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

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
                    var provider = Data.GetProvider();
                    provider.CreateUser(new DOUser(elements[0], elements[1]), () => {
                        GUI.HideRegister();
                        StartCoroutine(PostRegisterSequence());
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

    IEnumerator PostRegisterSequence()
    {
        yield return new WaitForSeconds(GUI.TransitionTime);

        GUI.HideLoading();
        GUI.Input("USER NAME", "Please set a user name for yourself", "", false, (userName) =>
        {
            StartCoroutine(UserNameExistenceSequence((string)userName));
        });
    }

    IEnumerator UserNameExistenceSequence(string userName)
    {
        GUI.HideRegister();

        yield return new WaitForSeconds(GUI.TransitionTime);

        GUI.DisplayLoading();

        yield return new WaitForSeconds(GUI.TransitionTime);

        Data.GetProvider().TestIfUserNameExists(userName, (result) => {
            GUI.HideLoading();

            if ((bool)result)
            {
                // another user with the given user name exists
                GUI.NotifyError("USER NAME IS IN USE", "That user name has been taken already");
                GUI.HideRegister();

                StartCoroutine(PostRegisterSequence());
            }
            else
            {
                // no other user exists wth the given name
                string userId = VariableManager.Instance.GetLocal("userid");
                Data.GetProvider().SaveUserName(userId, userName, VariableManager.Instance.GetLocal("mail_address"));

                GUI.Input("NICK NAME", "How would you like to be called " + userName + "?", "", false, (nickName) =>
                {
                    Data.GetProvider().SaveNickName(userId, (string)nickName);
                    StartCoroutine(PostLoginSequence());
                });
            }
        });
    }

    public void Login(string userName, string password)
    {
        if (authProvider != null)
        {
            authProvider.OnLoginCompleted = (succeeded, message) => {
                if(!succeeded)
                {
                    //GUI.NotifyError("LOGIN FAILED", message);
                    GUI.DisplayMessage("LOGIN FAILED", message);
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
        Data.GetProvider().GetUserRestricted(VariableManager.Instance.GetLocal("userid").ToString(), (restrictedUntil, restrictionReason) =>
        {
            if(restrictedUntil < DateTime.Now)
            {
                // NOT restricted
                Data.GetProvider().SetUserLastVisitDate(VariableManager.Instance.GetLocal("userid").ToString());
                Data.GetProvider().GetUser(VariableManager.Instance.GetLocal("userid"), (user) =>
                {
                    if (user == null)
                    {
                        StartCoroutine(PostLoginFailed());
                    }
                    else
                    {
                        if (string.IsNullOrEmpty(user.UserName))
                        {
                            StartCoroutine(PostRegisterSequence());
                        }
                        else
                        {
                            SucceedUserLogin(user);
                        }
                    }
                });
            } else
            {
                GUI.DisplayMessage("USER RESTRICTED", "You account has been restricted from Inventuna network until " + restrictedUntil.ToShortDateString() + " due to the following reason: " + restrictionReason, () =>
                {
                    StartCoroutine(PostLoginFailed());
                });
            }
        });
    }

    private void SucceedUserLogin(DOUser user)
    {
        VariableManager.Instance.SetOrAddVariable("nickname", user.NickName);
        VariableManager.Instance.SetOrAddVariable("username", user.UserName);
        VariableManager.Instance.SetOrAddVariable("userid", VariableManager.Instance.GetLocal("userid"));

        VariableManager.Instance.SetOrAddVariable("city_tavern_open", user.CityTavernOpen);
        VariableManager.Instance.SetOrAddVariable("city_arena_open", user.CityArenaOpen);
        VariableManager.Instance.SetOrAddVariable("city_market_open", user.CityMarketOpen);
        VariableManager.Instance.SetOrAddVariable("city_royal_palace_open", user.CityRoyalPalaceOpen);
        VariableManager.Instance.SetOrAddVariable("city_slums_open", user.CitySlumsOpen);
        VariableManager.Instance.SetOrAddVariable("city_gate_open", user.CityGateOpen);

        VariableManager.Instance.SetOrAddVariable("guild_tavern_open", user.GuildTavernOpen);
        VariableManager.Instance.SetOrAddVariable("guild_court_open", user.GuildCourtOpen);
        VariableManager.Instance.SetOrAddVariable("guild_garage_open", user.GuildGarageOpen);
        VariableManager.Instance.SetOrAddVariable("guild_healing_open", user.GuildHealingOpen);
        VariableManager.Instance.SetOrAddVariable("guild_pet_open", user.GuildPetOpen);
        VariableManager.Instance.SetOrAddVariable("guild_scout_open", user.GuildScoutOpen);
        VariableManager.Instance.SetOrAddVariable("guild_smith_open", user.GuildSmithOpen);
        VariableManager.Instance.SetOrAddVariable("guild_training_grounds_open", user.GuildTrainingGroundsOpen);

        StartCoroutine(PostLoginSucceeded(user));
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

        GUI.Notify("WELCOME BACK", user.NickName);
        GUI.HideLoading();

        yield return new WaitForSeconds(GUI.TransitionTime);

        GUI.HideCityWindow();
        //GUI.Hide();

        StartCoroutine(PopupStartWindows());
    }

    IEnumerator PopupStartWindows()
    {
        yield return new WaitForSeconds(GUI.TransitionTime);

        GUI.SwitchToGame();
        GUI.DisplayCity();
        GUI.DisplaySettings();
    }

    public void Logout()
    {
        GUI.Confirm("Are you sure you want to logout?", "YES", "NO", () => {
            VariableManager.Instance.ResetLocals();
            VariableManager.Instance.ResetVariables();

            authProvider.Logout();
            GUI.LogoutSequence();
        }, null);
    }
}
