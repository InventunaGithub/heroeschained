using Firebase.Extensions;
using Firebase.Firestore;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class DataProviderFirebase : DataProvider
{
    // Firebase specific definitions
    [FirestoreData]
    class FirebaseUser
    {
        [FirestoreProperty]
        public string userId { get; set; }

        [FirestoreProperty]
        public string nickName { get; set; }

        [FirestoreProperty]
        public DateTime restrictedUntil { get; set; }

        [FirestoreProperty]
        public string restrictionReason { get; set; }

        [FirestoreProperty]
        public DateTime lastLogon { get; set; }

        [FirestoreProperty]
        public string custodialWalletId { get; set; }
    }

    [FirestoreData]
    class FirebaseUserGame
    {

        [FirestoreProperty]
        public string gameId { get; set; }

        [FirestoreProperty]
        public string gameName { get; set; }

        [FirestoreProperty]
        public bool firstVisit { get; set; }

        [FirestoreProperty]
        public DateTime firstVisitedAt { get; set; }

        [FirestoreProperty]
        public DateTime restrictedUntil { get; set; }

        [FirestoreProperty]
        public string restrictionReason { get; set; }

        [FirestoreProperty]
        public DateTime lastVisitedAt { get; set; }

        [FirestoreProperty]
        public string connectdWallet { get; set; }

        [FirestoreProperty]
        public bool cityTavernOpen { get; set; }

        [FirestoreProperty]
        public bool cityArenaOpen { get; set; }

        [FirestoreProperty]
        public bool cityMarketOpen { get; set; }

        [FirestoreProperty]
        public bool cityRoyalPalaceOpen { get; set; }

        [FirestoreProperty]
        public bool citySlumsOpen { get; set; }

        [FirestoreProperty]
        public bool cityGateOpen { get; set; }

        [FirestoreProperty]
        public bool guildTavernOpen { get; set; }

        [FirestoreProperty]
        public bool guildSmithOpen { get; set; }

        [FirestoreProperty]
        public bool guildGarageOpen { get; set; }

        [FirestoreProperty]
        public bool guildScoutOpen { get; set; }

        [FirestoreProperty]
        public bool guildCourtOpen { get; set; }

        [FirestoreProperty]
        public bool guildAdvisorOpen { get; set; }

        [FirestoreProperty]
        public bool guildHealingOpen { get; set; }

        [FirestoreProperty]
        public bool guildTrainingGroundsOpen { get; set; }

        [FirestoreProperty]
        public bool guildPetOpen { get; set; }
    }

    FirebaseFirestore firebase;

    // abstract method implementations
    public override bool Connect(string gameId)
    {
        firebase = FirebaseFirestore.DefaultInstance;
        this.gameId = gameId;

        return true;
    }

    public override bool Connect(string gameId, string[] args)
    {
        return Connect(gameId);
    }

    public override bool Disonnect()
    {
        return true;
    }

    public override void CreateUser(DOUser user, OnCompletionDelegate onComplete)
    {
        StartCoroutine(CreateUserNow(user, onComplete));
    }

    IEnumerator CreateUserNow(DOUser user, OnCompletionDelegate onComplete)
    {
        FirebaseUser fbUser = new FirebaseUser
        {
            userId = user.ID,
            nickName = user.NickName,
            lastLogon = DateTime.Now,
            custodialWalletId = "",
            restrictedUntil = DateTime.MinValue,
            restrictionReason = ""
        };

        var userAddTask = firebase.Collection("users").AddAsync(fbUser);
        yield return new WaitUntil(() => userAddTask.IsCompleted);
        
        DocumentReference doc = userAddTask.Result;

        FirebaseUserGame game = new FirebaseUserGame
        {
            gameId = this.gameId,
            gameName = "Heroes Chained",
            firstVisit = true,
            firstVisitedAt = DateTime.Now,
            restrictedUntil = DateTime.MinValue,
            restrictionReason = "",
            lastVisitedAt = DateTime.MinValue,
            connectdWallet = "",

            cityArenaOpen = false,
            cityGateOpen = false,
            cityMarketOpen = false,
            cityRoyalPalaceOpen = false,
            citySlumsOpen = false,
            cityTavernOpen = false,
            guildTavernOpen = false,
            guildAdvisorOpen = false,
            guildCourtOpen = false,
            guildGarageOpen = false,
            guildHealingOpen = false,
            guildPetOpen = false,
            guildScoutOpen = false,
            guildSmithOpen = false,
            guildTrainingGroundsOpen = false
        };

        Debug.Log("Firebase: User created");
        var addTask = doc.Collection("games").AddAsync(game);
        yield return new WaitUntil(() => addTask.IsCompleted);

        Debug.Log("Firebase: Game has been added to user's document");
        onComplete?.Invoke();
    }

    public override void GetUser(string userId, OnQueryUserCompletionDelegate onComplete)
    {
        StartCoroutine(GetUserInternal(userId, onComplete));
    }

    IEnumerator GetUserInternal(string userId, OnQueryUserCompletionDelegate onComplete)
    {
        DOUser user = null;
        var task = firebase.Collection("users").WhereEqualTo("userId", userId).GetSnapshotAsync();
        QuerySnapshot snapshot = null;

        yield return new WaitUntil(() => task.IsCompleted);

        if (task.IsFaulted)
        {
            Debug.Log("Fault 101");
            // REAL ERROR HANDLING INSTEAD
        }
        else
        {
            snapshot = task.Result;
        }

        if (snapshot != null && snapshot.Count > 0)
        {
            Dictionary<string, object> data;
            Dictionary<string, object> gameData;

            foreach (DocumentSnapshot ds in snapshot.Documents)
            {
                data = ds.ToDictionary();

                user = new DOUser(userId);
                user.NickName = data["nickName"].ToString();
                user.CustodialWalletId = data["custodialWalletId"].ToString();
                user.RestrictedUntil = DateTime.Parse(data["restrictedUntil"].ToString().Substring(11));
                user.LastLogon = DateTime.Parse(data["lastLogon"].ToString().Substring(11));
                user.RestrictionReason = data["restrictionReason"].ToString();

                user.CityArenaOpen = data.ContainsKey("cityArenaOpen") ? (bool)data["cityArenaOpen"] : false;
                user.CityGateOpen = data.ContainsKey("cityGateOpen") ? (bool)data["cityGateOpen"] : false;
                user.CityMarketOpen = data.ContainsKey("cityMarketOpen") ? (bool)data["cityMarketOpen"] : false;
                user.CityRoyalPalaceOpen = data.ContainsKey("cityRoyalPalaceOpen") ? (bool)data["cityRoyalPalaceOpen"] : false;
                user.CitySlumsOpen = data.ContainsKey("citySlumsOpen") ? (bool)data["citySlumsOpen"] : false;
                user.CityTavernOpen = data.ContainsKey("cityTavernOpen") ? (bool)data["cityTavernOpen"] : false;

                user.GuildTavernOpen = data.ContainsKey("guildTavernOpen") ? (bool)data["guildTavernOpen"] : false;
                user.GuildCourtOpen = data.ContainsKey("guildCourtOpen") ? (bool)data["guildCourtOpen"] : false;
                user.GuildGarageOpen = data.ContainsKey("guildGarageOpen") ? (bool)data["guildGarageOpen"] : false;
                user.GuildHealingOpen = data.ContainsKey("guildHealingOpen") ? (bool)data["guildHealingOpen"] : false;
                user.GuildPetOpen = data.ContainsKey("guildPetOpen") ? (bool)data["guildPetOpen"] : false;
                user.GuildScoutOpen = data.ContainsKey("guildScoutOpen") ? (bool)data["guildScoutOpen"] : false;
                user.GuildSmithOpen = data.ContainsKey("guildSmithOpen") ? (bool)data["guildSmithOpen"] : false;
                user.GuildTrainingGroundsOpen = data.ContainsKey("guildTrainingGroundsOpen") ? (bool)data["guildTrainingGroundsOpen"] : false;

                task = ds.Reference.Collection("games").WhereEqualTo("gameId", gameId).GetSnapshotAsync();
                yield return new WaitUntil(() => task.IsCompleted);

                if (task.IsFaulted)
                {
                    Debug.Log("Fault 102");
                    // REAL ERROR HANDLING INSTEAD
                } else {
                    foreach (DocumentSnapshot dsg in task.Result.Documents)
                    {
                        gameData = dsg.ToDictionary();

                        DOUserGameMatch userGame = new DOUserGameMatch(userId, gameId);

                        userGame.ConnectedWallet = gameData["connectdWallet"].ToString();
                        userGame.FirstVisit = (bool)gameData["firstVisit"];
                        userGame.FirstPlayedAt = DateTime.Parse(gameData["firstVisitedAt"].ToString().Substring(11));

                        VariableManager.Instance.SetOrAddVariable("firstVisit", userGame.FirstVisit);

                        user.Games.Add(userGame);
                    }
                }

                break;
            }
        }

        onComplete?.Invoke(user);
    }

    public override void SetUserFirstVisitDate(string userId)
    {
        var updates = new Dictionary<FieldPath, object>
        {
            { new FieldPath("firstVisitedAt"), DateTime.Now }
        };

        firebase.Collection("users").Document(userId).Collection("games").Document(gameId).UpdateAsync(updates);
    }

    public override void SetUserFirstVisitProperty(string userId)
    {
        StartCoroutine(SetUserFirstVisitPropertyNow(userId));
    }

    IEnumerator SetUserFirstVisitPropertyNow(string userId)
    {
        var taskUsr = firebase.Collection("users").WhereEqualTo("userId", userId).GetSnapshotAsync();
        yield return new WaitUntil(() => taskUsr.IsCompleted);

        string topUserId = "";
        foreach (DocumentSnapshot dsg in taskUsr.Result.Documents)
        {
            topUserId = dsg.Id;
            break;
        }

        var task = firebase.Collection("users").Document(topUserId).Collection("games").WhereEqualTo("gameId", gameId).GetSnapshotAsync();

        yield return new WaitUntil(() => task.IsCompleted);

        if (task.IsFaulted)
        {
            Debug.Log("Fault 110");
            // REAL ERROR HANDLING INSTEAD
        }
        else
        {
            var snapshot = task.Result;

            if (snapshot != null && snapshot.Count > 0)
            {
                foreach (DocumentSnapshot dsg in task.Result.Documents)
                {
                    var gameData = dsg.ToDictionary();
                    var updates = new Dictionary<FieldPath, object>
                    {
                        { new FieldPath("firstVisit"), false }
                    };

                    string subGameId = dsg.Id;
                    firebase.Collection("users").Document(topUserId).Collection("games").Document(subGameId).UpdateAsync(updates);
                    break;
                }
            }
        }
    }
    public override void SetUserLastVisitDate(string userId)
    {
        var updates = new Dictionary<FieldPath, object>
        {
            { new FieldPath("lasstVisitedAt"), DateTime.Now }
        };

        firebase.Collection("users").Document(userId).Collection("games").Document(gameId).UpdateAsync(updates);
    }

    public override void GetUserRestricted(string userId, OnRestrictionDelegate onRestriction)
    {
        StartCoroutine(GetUserRestrictedNow(userId, onRestriction));
    }

    public IEnumerator GetUserRestrictedNow(string userId, OnRestrictionDelegate onRestriction)
    {
        var taskUsr = firebase.Collection("users").WhereEqualTo("userId", userId).GetSnapshotAsync();
        yield return new WaitUntil(() => taskUsr.IsCompleted);

        string topUserId = "";
        foreach (DocumentSnapshot dsg in taskUsr.Result.Documents)
        {
            var gameData = dsg.ToDictionary();
            string dateStr = gameData["restrictedUntil"].ToString().Replace("Timestamp:", "").Trim();
            DateTime date = DateTime.Parse(dateStr);
            if (date >= DateTime.Now)
            {
                onRestriction?.Invoke(date, gameData["restrictionReason"].ToString());
                yield break;
            }

            topUserId = dsg.Id;
            break;
        }

        if(string.IsNullOrEmpty(topUserId))
        {
            onRestriction?.Invoke(DateTime.MinValue, "");
            yield break;
        }

        var task = firebase.Collection("users").Document(topUserId).Collection("games").WhereEqualTo("gameId", gameId).GetSnapshotAsync();

        yield return new WaitUntil(() => task.IsCompleted);

        if (task.IsFaulted)
        {
            Debug.Log("Fault 110");
            // REAL ERROR HANDLING INSTEAD
        }
        else
        {
            var snapshot = task.Result;

            if (snapshot != null && snapshot.Count > 0)
            {
                foreach (DocumentSnapshot dsg in task.Result.Documents)
                {
                    var gameData = dsg.ToDictionary();
                    if (gameData.ContainsKey("restrictedUntil"))
                    {
                        string dateStr = gameData["restrictedUntil"].ToString().Replace("Timestamp:", "").Trim();
                        DateTime date = DateTime.Parse(dateStr);
                        if (date >= DateTime.Now)
                        {
                            onRestriction?.Invoke(date, gameData["restrictionReason"].ToString());
                        } else
                        {
                            onRestriction?.Invoke(DateTime.MinValue, "");
                        }

                        yield break;
                    }
                    else
                    {
                        onRestriction?.Invoke(DateTime.MinValue, "");
                    }

                    break;
                }
            }
        }
    }

    public override string Vendor()
    {
        return "Google Firestore";
    }
}
