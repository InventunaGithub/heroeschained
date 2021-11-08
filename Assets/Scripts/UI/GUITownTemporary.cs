using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using DuloGames.UI;
using UnityEngine.SceneManagement;

public class GUITownTemporary : MonoBehaviour
{
    public GameObject PnlLogin;
    public GameObject PnlRegister;
    public GameObject PnlLoading;
    public GameObject PnlGame;
    public GameObject WinCity;
    public GameObject WinGuild;
    public GameObject WinQuest;
    public GameObject WinDialog;
    public GameObject WinCityMarketPlayer;
    public GameObject WinCityMarketShop;
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
    public QuestManager Quests;
    public GameObject QuestItem;
    public GameObject QuestObjective;

    public delegate void OnAfterEvent();
    OnAfterEvent AfterDialogEvent = null;

    public enum GuildBuildings { GuildHouse, Tavern };

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
        WinQuest.GetComponent<UIWindow>().Hide();
        WinDialog.GetComponent<UIWindow>().Hide();

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
        bool cityRoyalPalaceOpen = (bool)VariableManager.Instance.GetVariable("city_royal_palace_open");
        bool cityTavernOpen = (bool)VariableManager.Instance.GetVariable("city_tavern_open");
        bool cityArenaOpen = (bool)VariableManager.Instance.GetVariable("city_arena_open");
        bool cityMarketOpen = (bool)VariableManager.Instance.GetVariable("city_market_open");
        bool citySlumsOpen = (bool)VariableManager.Instance.GetVariable("city_slums_open");
        bool cityGateOpen = (bool)VariableManager.Instance.GetVariable("city_gate_open");
        //bool guildTavernOpen = (bool)VariableManager.Instance.GetVariable("guild_tavern_open");

        var palace = WinCity.transform.Find("Content/Scroll Rect/Viewport/Content/RoyalPalace");
        palace.GetComponent<UIHighlightTransition>().enabled = cityRoyalPalaceOpen;
        palace.GetComponent<UIPressTransition>().enabled = cityRoyalPalaceOpen;

        var tavern = WinCity.transform.Find("Content/Scroll Rect/Viewport/Content/HeroTavern");
        tavern.GetComponent<UIHighlightTransition>().enabled = cityTavernOpen;
        tavern.GetComponent<UIPressTransition>().enabled = cityTavernOpen;

        var arena = WinCity.transform.Find("Content/Scroll Rect/Viewport/Content/Arena");
        arena.GetComponent<UIHighlightTransition>().enabled = cityArenaOpen;
        arena.GetComponent<UIPressTransition>().enabled = cityArenaOpen;

        var market = WinCity.transform.Find("Content/Scroll Rect/Viewport/Content/Market");
        market.GetComponent<UIHighlightTransition>().enabled = cityMarketOpen;
        market.GetComponent<UIPressTransition>().enabled = cityMarketOpen;

        var slums = WinCity.transform.Find("Content/Scroll Rect/Viewport/Content/Slums");
        slums.GetComponent<UIHighlightTransition>().enabled = citySlumsOpen;
        slums.GetComponent<UIPressTransition>().enabled = citySlumsOpen;

        var gate = WinCity.transform.Find("Content/Scroll Rect/Viewport/Content/Gate");
        gate.GetComponent<UIHighlightTransition>().enabled = cityGateOpen;
        gate.GetComponent<UIPressTransition>().enabled = cityGateOpen;

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

        if (window2 == WinCity.GetComponent<UIWindow>())
        {
            DisplayCityWindow();
        }
        else if (window2 == WinGuild.GetComponent<UIWindow>())
        {
            DisplayGuildWindow();
        }
        else
        {
            window2.Show();
        }
    }

    public void DisplayQuests()
    {
        // Display the list of available quests on the left
        var questList = WinQuest.transform.Find("Content/Quest List/Scroll Rect/Viewport/Content").gameObject;
        for (int i = 0; i < questList.transform.childCount; i++)
        {
            Destroy(questList.transform.GetChild(i).gameObject);
        }

        GameObject newElement;
        for (int i = 0; i < Quests.QuestList.Length; i++)
        {
            if(Quests.QuestList[i].State == Quest.QuestState.Available || Quests.QuestList[i].State == Quest.QuestState.Attempted)
            {
                newElement = Instantiate(QuestItem, questList.transform);
                newElement.transform.Find("Text").GetComponent<Text>().text = Quests.QuestList[i].Title;
                newElement.GetComponent<QuestListElement>().QuestOnSelection = Quests.QuestList[i];
                newElement.GetComponent<Toggle>().onValueChanged.RemoveAllListeners();
                newElement.GetComponent<Toggle>().onValueChanged.AddListener((result) =>
                {
                    if(result)
                    {
                        DisplayQuest(Quests.QuestList[i].ID);
                    }
                });

                newElement.SetActive(true);
            }
        }

        // Display the window
        WinQuest.GetComponent<UIWindow>().Show();

        if (Quests.QuestList.Length > 0)
        {
            DisplayQuest(Quests.QuestList[0].ID);
        }
    }

    public void DisplayQuest(int id)
    {
        var questList = WinQuest.transform.Find("Content/Quest List/Scroll Rect/Viewport/Content").gameObject;
        Quest itemQuest;
        //Toggle toggle;
        QuestListElement element;
        for (int i = 0; i < questList.transform.childCount; i++)
        {
            //toggle = questList.transform.GetChild(i).GetComponent<Toggle>();
            element = questList.transform.GetChild(i).GetComponent<QuestListElement>();
            if (element != null)
            {
                itemQuest = element.QuestOnSelection;
                var content = WinQuest.transform.Find("Content/Quest Info/Scroll Rect/Viewport/Content");
                content.Find("Title Group/Title Text").GetComponent<Text>().text = itemQuest.TitleLong;
                content.Find("Description Text").GetComponent<Text>().text = itemQuest.Description.Replace("\\n", "\n");

                var objGroup = content.Find("Objectives Group");
                for (int g = 0; g < objGroup.childCount; g++)
                {
                    Destroy(objGroup.GetChild(g).gameObject);
                }

                for (int a = 0; a < itemQuest.RequirementsListRight.Length; a++)
                {
                    var questObjective = Instantiate(QuestObjective, objGroup);

                    questObjective.transform.Find("Amount Group/Current Amount Text").GetComponent<Text>().text = itemQuest.RequirementsListSatisfiedAmount[a].ToString();
                    questObjective.transform.Find("Amount Group/Required Amount Text").GetComponent<Text>().text = itemQuest.RequirementsListRequiredAmount[a].ToString();
                    questObjective.transform.Find("Objective Text").GetComponent<Text>().text = itemQuest.RequirementsListRight[a];

                    questObjective.SetActive(true);
                }

                //toggle.isOn = itemQuest.ID == id;
            }
            else
            {
                //toggle.isOn = false;
            }
        }
    }

    public void MakeGuildBuildingAvailable(GuildBuildings building)
    {
        Transform element = null;
        switch (building)
        {
            case GuildBuildings.Tavern:
                element = WinGuild.transform.Find("Content/Scroll Rect/Viewport/Content/BuildingTavern");
                break;
        }

        if (element != null)
        {
            element.GetComponent<UIHighlightTransition>().enabled = true;
            element.GetComponent<UIPressTransition>().enabled = true;
        }
    }

    public void DisplayTavern()
    {
        //SceneManager.LoadScene(0);
    }

    public void DisplayMessage(string title, string message)
    {
        WinDialog.transform.Find("Header/Text").GetComponent<Text>().text = title;
        WinDialog.transform.Find("Content/Scroll Rect/Viewport/Content/Description Text").GetComponent<Text>().text = message;
        WinDialog.GetComponent<UIWindow>().Show();

        AfterDialogEvent = null;
    }

    public void DisplayMessage(string title, string message, OnAfterEvent afterEvent)
    {
        WinDialog.transform.Find("Header/Text").GetComponent<Text>().text = title;
        WinDialog.transform.Find("Content/Scroll Rect/Viewport/Content/Description Text").GetComponent<Text>().text = message;
        WinDialog.GetComponent<UIWindow>().Show();

        AfterDialogEvent = afterEvent;
    }

    public void HideMessage()
    {
        WinDialog.GetComponent<UIWindow>().Hide();

        AfterDialogEvent?.Invoke();
        AfterDialogEvent = null;
    }

    public void DisplayCityMarket()
    {
        WinCityMarketPlayer.GetComponent<UIWindow>().Show();
        WinCityMarketShop.GetComponent<UIWindow>().Show();
    }

    public void HideCityMarket()
    {
        WinCityMarketPlayer.GetComponent<UIWindow>().Hide();
        WinCityMarketShop.GetComponent<UIWindow>().Hide();
    }
}
