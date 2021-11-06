using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using DuloGames.UI;

public class GUITownTemporary : MonoBehaviour
{
    public GameObject PnlLogin;
    public GameObject PnlRegister;
    public GameObject PnlLoading;
    public GameObject PnlGame;
    public GameObject WinCity;
    public GameObject WinGuild;
    public float TransitionTime = 0.75f;
    public float NotificationTime = 1.75f;
    public Auth Authentication;
    public InputField UserNameLogin;
    public InputField PasswordLogin;
    public InputField UserNameRegister;
    public InputField PasswordRegister;
    public InputField PasswordRetypeRegister;
    public GameObject NotifyRed;
    public GameObject NotifyGreen;
    public DataSource Data;

    // Start is called before the first frame update
    void Start()
    {
        PnlLogin.GetComponent<CanvasGroup>().alpha = 0;
        PnlRegister.GetComponent<CanvasGroup>().alpha = 0;
        PnlRegister.SetActive(false);
        PnlLoading.SetActive(false);
        PnlGame.SetActive(false);
        WinGuild.GetComponent<UIWindow>().Hide();
        WinCity.GetComponent<UIWindow>().Show();

        NotifyGreen.SetActive(false);
        NotifyRed.SetActive(false);

        if (!Authentication.LoggedIn())
        {
            DisplayLogin();
        } else
        {
            DisplayLoading("Reconnecting to last session...");
        }
    }

    public void DisplayLoading(string message)
    {
        PnlLoading.SetActive(true);
        PnlLoading.GetComponent<CanvasGroup>().alpha = 0;
        PnlLoading.transform.Find("Window/Text").GetComponent<Text>().text = message;
        PnlLoading.GetComponent<CanvasGroup>().DOFade(1, TransitionTime).SetEase(Ease.Linear);
    }

    public void HideLoading()
    {
        StartCoroutine(HideLoadingNow());
    }

    IEnumerator HideLoadingNow()
    {
        PnlLoading.GetComponent<CanvasGroup>().DOFade(0, TransitionTime).SetEase(Ease.Linear);

        yield return new WaitForSeconds(TransitionTime);

        PnlLoading.SetActive(false);
    }

    public void DisplayLoading()
    {
        DisplayLoading("Loading...");
    }

    public void HideLogin()
    {
        StartCoroutine(HideLoginNow());
    }

    IEnumerator HideLoginNow()
    {
        PnlLogin.GetComponent<CanvasGroup>().DOFade(0, TransitionTime).SetEase(Ease.Linear);

        yield return new WaitForSeconds(1);

        PnlLogin.SetActive(false);
    }

    public void DisplayLogin()
    {
        PnlLogin.SetActive(true);
        PnlLogin.GetComponent<CanvasGroup>().DOFade(1, TransitionTime).SetEase(Ease.Linear);
    }

    public void DisplayCity()
    {
        if (VariableManager.Instance.VariableExists("firstVisit") && (bool)VariableManager.Instance.GetVariable("firstVisit"))
        {
            VariableManager.Instance.SetVariable("firstVisit", false);
            transform.Find("/Managers").GetComponent<DialogManager>().Launch();

            string userId = VariableManager.Instance.GetLocal("userid").ToString();
            Data.GetProvider().SetUserFirstVisitProperty(userId);
        }

        SwitchToGame();
    }

    public void HideRegister()
    {
        StartCoroutine(HideRegisterNow());
    }

    IEnumerator HideRegisterNow()
    { 
        PnlRegister.GetComponent<CanvasGroup>().DOFade(0, TransitionTime).SetEase(Ease.Linear);

        yield return new WaitForSeconds(1);

        PnlRegister.SetActive(false);
    }

    public void DisplayRegister()
    {
        PnlRegister.SetActive(true);
        PnlRegister.GetComponent<CanvasGroup>().DOFade(1, TransitionTime).SetEase(Ease.Linear);
    }

    public void SwitchToRegister()
    {
        StartCoroutine(SwitchToRegisterNow());
    }

    public void SwitchToLogin()
    {
        StartCoroutine(SwitchToLoginNow());
    }

    public void SwitchToGame()
    {
        StartCoroutine(SwitchToGameNow());
    }

    IEnumerator SwitchToGameNow()
    {
        HideLogin();
        HideRegister();

        yield return new WaitForSeconds(TransitionTime);

        DisplayGamePanel();
    }

    public void DisplayGamePanel()
    {
        PnlGame.GetComponent<CanvasGroup>().alpha = 0;
        PnlGame.SetActive(true);
        PnlGame.GetComponent<CanvasGroup>().DOFade(1, TransitionTime);
    }

    public void Register()
    {
        string userName = UserNameRegister.text.Trim();
        string password = PasswordRegister.text.Trim();
        string passwordRetype = PasswordRetypeRegister.text.Trim();

        if (string.IsNullOrEmpty(userName))
        {
            NotifyError("ERROR", "User name cannot be left blank");

            EventSystem.current.SetSelectedGameObject(UserNameRegister.gameObject, null);
        }
        else if (string.IsNullOrEmpty(password))
        {
            NotifyError("ERROR", "Password cannot be left blank");

            EventSystem.current.SetSelectedGameObject(PasswordRegister.gameObject, null);
        }
        else if (password.Length < 8)
        {
            NotifyError("ERROR", "Password has to be at least 8 characters long");

            EventSystem.current.SetSelectedGameObject(PasswordRegister.gameObject, null);
        }
        else if (password != passwordRetype)
        {
            NotifyError("ERROR", "Password and retype must be the same");

            EventSystem.current.SetSelectedGameObject(PasswordRetypeRegister.gameObject, null);
        }
        else
        {
            HideRegister();
            DisplayLoading("Connecting to server...");

            StartCoroutine(HandleRegister(userName, password, passwordRetype));
        }
    }

    IEnumerator HandleRegister(string userName, string password, string passwordRetype)
    {
        yield return new WaitForSeconds(1);

        Authentication.Register(userName, password);
    }

    public void Login()
    {
        string userName = UserNameLogin.text.Trim();
        string password = PasswordLogin.text.Trim();

        if (string.IsNullOrEmpty(userName))
        {
            NotifyError("ERROR", "User name cannot be left blank");

            EventSystem.current.SetSelectedGameObject(UserNameLogin.gameObject, null);
        }
        else if (string.IsNullOrEmpty(password))
        {
            NotifyError("ERROR", "Password cannot be left blank");

            EventSystem.current.SetSelectedGameObject(PasswordLogin.gameObject, null);
        }
        else
        {
            HideLogin();
            DisplayLoading("Logging in...");

            StartCoroutine(HandleLogin(userName, password));
        }
    }

    IEnumerator HandleLogin(string userName, string password)
    {
        yield return new WaitForSeconds(1);

        Authentication.Login(userName, password);
    }

    IEnumerator SwitchToRegisterNow()
    {
        HideLogin();

        yield return new WaitForSeconds(1f);

        DisplayRegister();
    }

    IEnumerator SwitchToLoginNow()
    {
        HideRegister();

        yield return new WaitForSeconds(1f);

        DisplayLogin();
    }

    public void Notify(string title, string message)
    {
        NotifyGreen.transform.Find("Headline Text").GetComponent<Text>().text = title;
        NotifyGreen.transform.Find("Text").GetComponent<Text>().text = message;
        NotifyGreen.GetComponent<CanvasGroup>().alpha = 0;
        NotifyGreen.SetActive(true);
        NotifyGreen.GetComponent<CanvasGroup>().DOFade(1, TransitionTime);

        StartCoroutine(GoodbyeNotify());
    }

    IEnumerator GoodbyeNotify()
    {
        yield return new WaitForSeconds(NotificationTime);

        NotifyGreen.GetComponent<CanvasGroup>().DOFade(0, TransitionTime);

        yield return new WaitForSeconds(0.5f);

        NotifyGreen.SetActive(false);
    }

    public void NotifyError(string title, string message)
    {
        NotifyRed.transform.Find("Headline Text").GetComponent<Text>().text = title;
        NotifyRed.transform.Find("Text").GetComponent<Text>().text = message;
        NotifyRed.GetComponent<CanvasGroup>().alpha = 0;
        NotifyRed.SetActive(true);
        NotifyRed.GetComponent<CanvasGroup>().DOFade(1, TransitionTime);

        StartCoroutine(GoodbyeNotifyRed());
    }

    IEnumerator GoodbyeNotifyRed()
    {
        yield return new WaitForSeconds(NotificationTime);

        NotifyRed.GetComponent<CanvasGroup>().DOFade(0, TransitionTime);

        yield return new WaitForSeconds(0.5f);

        NotifyRed.SetActive(false);
    }

    public void DisplayCityWindow()
    {
        WinCity.GetComponent<UIWindow>().Show();
    }

    public void DisplayGuildWindow()
    {
        WinGuild.GetComponent<UIWindow>().Show();
    }

    public void HideCityWindow()
    {
        WinCity.GetComponent<UIWindow>().Hide();
    }

    public void HideGuildWindow()
    {
        WinGuild.GetComponent<UIWindow>().Hide();
    }

    public void SwitchFromGuildToCity()
    {
        SwitchBetweenWindows(WinGuild.GetComponent<UIWindow>(), WinCity.GetComponent<UIWindow>());
    }

    public void SwitchFromCityToGuild()
    {
        SwitchBetweenWindows(WinCity.GetComponent<UIWindow>(), WinGuild.GetComponent<UIWindow>());
    }

    public void SwitchBetweenWindows(UIWindow window1, UIWindow window2)
    {
        StartCoroutine(SwitchBetweenWindowsNow(window1, window2));
    }

    IEnumerator SwitchBetweenWindowsNow(UIWindow window1, UIWindow window2)
    {
        window1.Hide();

        yield return new WaitForSeconds(TransitionTime);

        window2.Show();
    }
}
