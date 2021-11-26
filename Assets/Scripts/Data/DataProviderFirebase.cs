using Firebase.Extensions;
using Firebase.Firestore;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using static MessagingManager;

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

    [FirestoreData]
    class FirebaseUserMessage
    {
        [FirestoreProperty]
        public string title { get; set; }

        [FirestoreProperty]
        public string senderId { get; set; }

        [FirestoreProperty]
        public string senderUserName { get; set; }

        [FirestoreProperty]
        public string senderName { get; set; }

        [FirestoreProperty]
        public string content { get; set; }

        [FirestoreProperty]
        public DateTime sentAt { get; set; }

        [FirestoreProperty]
        public DateTime readAt { get; set; }
    }


    [FirestoreData]
    class FirebaseUserResource
    {
        [FirestoreProperty]
        public string name { get; set; }

        [FirestoreProperty]
        public int tier { get; set; }

        [FirestoreProperty]
        public int amount { get; set; }
    }

    [FirestoreData]
    class FirebaseUserResourceCollected
    {
        [FirestoreProperty]
        public string name { get; set; }

        [FirestoreProperty]
        public int tier { get; set; }

        [FirestoreProperty]
        public int amount { get; set; }

        [FirestoreProperty]
        public string id { get; set; }

        [FirestoreProperty]
        public DateTime dateTime { get; set; }
    }

    [FirestoreData]
    class FirebaseUserBeacon
    {
        [FirestoreProperty]
        public float locX { get; set; }

        [FirestoreProperty]
        public float locY { get; set; }

        [FirestoreProperty]
        public float locZ { get; set; }

        [FirestoreProperty]
        public int index { get; set; }

        [FirestoreProperty]
        public DateTime dateTime { get; set; }
    }

    [FirestoreData]
    class FirebaseDungeonResult
    {
        [FirestoreProperty]
        public DateTime clearedAt { get; set; }

        [FirestoreProperty]
        public int result { get; set; }

        [FirestoreProperty]
        public string dungeonId { get; set; }

        [FirestoreProperty]
        public string landId { get; set; }
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

        FirebaseUserMessage welcomeMessage = new FirebaseUserMessage
        {
            readAt = DateTime.MinValue,
            senderId = "0",
            senderUserName = "system",
            senderName = "System",
            sentAt = DateTime.Now,
            title = "Welcome to Heroes Chained!",
            content = "Hey new player, welcome to the world of Inventuna Games! We hope you enjoy Heroes Chained"
        };

        Debug.Log("Firebase: User created");
        var addTask = doc.Collection("games").AddAsync(game);
        yield return new WaitUntil(() => addTask.IsCompleted);

        addTask = doc.Collection("messages").AddAsync(welcomeMessage);
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
                user.UserName = data["userName"].ToString();
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
                }
                else
                {
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
        StartCoroutine(SetUserLastVisitDateNow(userId));
    }

    IEnumerator SetUserLastVisitDateNow(string userId)
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
            Debug.Log("Fault 114");
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
                        { new FieldPath("lastVisitedAt"), DateTime.Now }
                    };

                    string subGameId = dsg.Id;
                    firebase.Collection("users").Document(topUserId).Collection("games").Document(subGameId).UpdateAsync(updates);
                    break;
                }
            }
        }
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

        if (string.IsNullOrEmpty(topUserId))
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
                Dictionary<string, object> gameData;
                foreach (DocumentSnapshot dsg in task.Result.Documents)
                {
                    gameData = dsg.ToDictionary();
                    if (gameData.ContainsKey("restrictedUntil"))
                    {
                        string dateStr = gameData["restrictedUntil"].ToString().Replace("Timestamp:", "").Trim();
                        DateTime date = DateTime.Parse(dateStr);
                        if (date >= DateTime.Now)
                        {
                            onRestriction?.Invoke(date, gameData["restrictionReason"].ToString());
                        }
                        else
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

    public override void GetNewMessageCount(string userId, OnCompletionDelegateWithParameter onComplete)
    {
        StartCoroutine(GetNewMessageCountNow(userId, onComplete));
    }

    IEnumerator GetNewMessageCountNow(string userId, OnCompletionDelegateWithParameter onComplete)
    {
        var taskUsr = firebase.Collection("users").WhereEqualTo("userId", userId).GetSnapshotAsync();
        yield return new WaitUntil(() => taskUsr.IsCompleted);

        string topUserId = "";
        foreach (DocumentSnapshot dsg in taskUsr.Result.Documents)
        {
            var gameData = dsg.ToDictionary();
            topUserId = dsg.Id;
            break;
        }

        var task = firebase.Collection("users").Document(topUserId).Collection("messages").WhereEqualTo("readAt", DateTime.MinValue).GetSnapshotAsync();
        yield return new WaitUntil(() => task.IsCompleted);

        if (task.IsFaulted)
        {
            Debug.Log("Fault 111");
            onComplete?.Invoke(0);
        }
        else
        {
            var snapshot = task.Result;
            if (snapshot != null)
            {
                onComplete?.Invoke(snapshot.Count);
            }
            else
            {
                onComplete?.Invoke(0);
            }
        }

    }

    public override void GetMessageCount(string userId, OnCompletionDelegateWithParameter onComplete)
    {
        StartCoroutine(GetMessageCountNow(userId, onComplete));
    }

    IEnumerator GetMessageCountNow(string userId, OnCompletionDelegateWithParameter onComplete)
    {
        var taskUsr = firebase.Collection("users").WhereEqualTo("userId", userId).GetSnapshotAsync();
        yield return new WaitUntil(() => taskUsr.IsCompleted);

        string topUserId = "";
        foreach (DocumentSnapshot dsg in taskUsr.Result.Documents)
        {
            var gameData = dsg.ToDictionary();
            topUserId = dsg.Id;
            break;
        }

        var task = firebase.Collection("users").Document(topUserId).Collection("messages").GetSnapshotAsync();
        yield return new WaitUntil(() => task.IsCompleted);

        if (task.IsFaulted)
        {
            Debug.Log("Fault 112");
            onComplete?.Invoke(0);
        }
        else
        {
            var snapshot = task.Result;
            if (snapshot != null)
            {
                onComplete?.Invoke(snapshot.Count);
            }
            else
            {
                onComplete?.Invoke(0);
            }
        }
    }

    public override void GetMessageHeaders(string userId, int messageCount, OnCompletionDelegateWithParameter onComplete)
    {
        StartCoroutine(GetMessageHeadersNow(userId, messageCount, onComplete));
    }

    IEnumerator GetMessageHeadersNow(string userId, int messageCount, OnCompletionDelegateWithParameter onComplete)
    {
        var taskUsr = firebase.Collection("users").WhereEqualTo("userId", userId).GetSnapshotAsync();
        yield return new WaitUntil(() => taskUsr.IsCompleted);

        string topUserId = "";
        foreach (DocumentSnapshot dsg in taskUsr.Result.Documents)
        {
            var gameData = dsg.ToDictionary();
            topUserId = dsg.Id;
            break;
        }

        var task = firebase.Collection("users").Document(topUserId).Collection("messages").GetSnapshotAsync();
        yield return new WaitUntil(() => task.IsCompleted);

        if (task.IsFaulted)
        {
            Debug.Log("Fault 113");
            onComplete?.Invoke(null);
        }
        else
        {
            var snapshot = task.Result;
            if (snapshot != null)
            {
                List<InGameMessage> result = new List<InGameMessage>();
                Dictionary<string, object> gameData;
                InGameMessage header;
                int index = 0;

                foreach (DocumentSnapshot dsg in task.Result.Documents)
                {
                    if (++index > messageCount)
                    {
                        break;
                    }

                    gameData = dsg.ToDictionary();
                    header = new InGameMessage(dsg.Id, gameData["title"].ToString(), gameData["senderId"].ToString(), (!gameData.ContainsKey("senderUserName") || gameData["senderUserName"] == null) ? "" : gameData["senderUserName"].ToString(), gameData["senderName"].ToString(), DateTime.Parse(gameData["sentAt"].ToString().Replace("Timestamp:", "").Trim()));

                    result.Add(header);
                }

                onComplete?.Invoke(result.ToArray());
            }
            else
            {
                onComplete?.Invoke(null);
            }
        }
    }

    public override void GetSentMessageHeaders(string userId, int messageCount, OnCompletionDelegateWithParameter onComplete)
    {
    }

    public override void SendMessage(string from, string senderName, string to, string title, string content)
    {
        StartCoroutine(SendMessageNow(from, senderName, to, title, content));
    }

    IEnumerator SendMessageNow(string from, string senderName, string to, string title, string content)
    {
        var taskUsr = firebase.Collection("users").WhereEqualTo("userName", to).GetSnapshotAsync();
        yield return new WaitUntil(() => taskUsr.IsCompleted);

        FirebaseUserMessage newMessage = new FirebaseUserMessage
        {
            readAt = DateTime.MinValue,
            senderId = from,
            senderUserName = VariableManager.Instance.GetVariable("username").ToString(),
            senderName = senderName,
            sentAt = DateTime.Now,
            title = title,
            content = content
        };

        foreach (DocumentSnapshot dsg in taskUsr.Result.Documents)
        {
            var addTask = dsg.Reference.Collection("messages").AddAsync(newMessage);
            yield return new WaitUntil(() => addTask.IsCompleted);

            Debug.Log("Firebase: Message has been delivered");
            break;
        }
    }

    public override void DeleteMessage(string sentTo, string messageId, OnCompletionDelegate onComplete)
    {
        StartCoroutine(DeleteMessageNow(sentTo, messageId, onComplete));
    }

    IEnumerator DeleteMessageNow(string sentTo, string messageId, OnCompletionDelegate onComplete)
    {
        var taskUsr = firebase.Collection("users").WhereEqualTo("userId", sentTo).GetSnapshotAsync();
        yield return new WaitUntil(() => taskUsr.IsCompleted);

        string topUserId = "";
        foreach (DocumentSnapshot dsg in taskUsr.Result.Documents)
        {
            var gameData = dsg.ToDictionary();
            topUserId = dsg.Id;
            break;
        }

        var task = firebase.Collection("users").Document(topUserId).Collection("messages").Document(messageId);
        task.DeleteAsync().ContinueWithOnMainThread(task =>
        {
            onComplete?.Invoke();
        });
    }
    public override void GetMessageBody(string sentTo, string messageId, OnCompletionDelegateWithParameter onComplete)
    {
        StartCoroutine(GetMessageBodyNow(sentTo, messageId, onComplete));
    }

    IEnumerator GetMessageBodyNow(string sentTo, string messageId, OnCompletionDelegateWithParameter onComplete)
    {
        var taskUsr = firebase.Collection("users").WhereEqualTo("userId", sentTo).GetSnapshotAsync();
        yield return new WaitUntil(() => taskUsr.IsCompleted);

        string topUserId = "";
        foreach (DocumentSnapshot dsg in taskUsr.Result.Documents)
        {
            var gameData = dsg.ToDictionary();
            topUserId = dsg.Id;
            break;
        }

        var task = firebase.Collection("users").Document(topUserId).Collection("messages").Document(messageId).GetSnapshotAsync();
        yield return new WaitUntil(() => task.IsCompleted);

        DocumentSnapshot ds = task.Result;
        if (ds != null)
        {
            var messageData = ds.ToDictionary();

            onComplete?.Invoke(messageData["content"].ToString());
        }
        else
        {
            onComplete?.Invoke(null);
        }
    }

    public override void MarkMessageAsRead(string sentTo, string messageId)
    {
        StartCoroutine(MarkMessageAsReadNow(sentTo, messageId));
    }

    IEnumerator MarkMessageAsReadNow(string sentTo, string messageId)
    {
        var taskUsr = firebase.Collection("users").WhereEqualTo("userId", sentTo).GetSnapshotAsync();
        yield return new WaitUntil(() => taskUsr.IsCompleted);

        string topUserId = "";
        foreach (DocumentSnapshot dsg in taskUsr.Result.Documents)
        {
            topUserId = dsg.Id;
            break;
        }

        var updates = new Dictionary<FieldPath, object>
        {
            { new FieldPath("readAt"), DateTime.Now }
        };

        firebase.Collection("users").Document(topUserId).Collection("messages").Document(messageId).UpdateAsync(updates);
    }

    public override void TestIfUserNameExists(string userName, OnCompletionDelegateWithParameter onComplete)
    {
        StartCoroutine(TestIfUserNameExistsNow(userName, onComplete));

    }

    IEnumerator TestIfUserNameExistsNow(string userName, OnCompletionDelegateWithParameter onComplete)
    {
        var taskUsr = firebase.Collection("users").WhereEqualTo("userName", userName).GetSnapshotAsync();
        yield return new WaitUntil(() => taskUsr.IsCompleted);

        int totalUsers = 0;
        foreach (DocumentSnapshot dsg in taskUsr.Result.Documents)
        {
            totalUsers += 1;
            break;
        }

        onComplete?.Invoke(totalUsers != 0);
    }

    public override void SaveUserName(string userId, string userName, string mailAddress)
    {
        StartCoroutine(SaveUserNameNow(userId, userName, mailAddress));
    }

    IEnumerator SaveUserNameNow(string userId, string userName, string mailAddress)
    {
        var taskUsr = firebase.Collection("users").WhereEqualTo("userId", userId).GetSnapshotAsync();
        yield return new WaitUntil(() => taskUsr.IsCompleted);

        string topUserId = "";
        foreach (DocumentSnapshot dsg in taskUsr.Result.Documents)
        {
            topUserId = dsg.Id;
            break;
        }

        var updates = new Dictionary<FieldPath, object>
        {
            { new FieldPath("userName"), userName },
            { new FieldPath("mailAddress"), mailAddress },
        };

        firebase.Collection("users").Document(topUserId).UpdateAsync(updates);
    }

    public override void SaveNickName(string userId, string nickName)
    {
        StartCoroutine(SaveNickNameNow(userId, nickName));
    }

    IEnumerator SaveNickNameNow(string userId, string nickName)
    {
        var taskUsr = firebase.Collection("users").WhereEqualTo("userId", userId).GetSnapshotAsync();
        yield return new WaitUntil(() => taskUsr.IsCompleted);

        string topUserId = "";
        foreach (DocumentSnapshot dsg in taskUsr.Result.Documents)
        {
            topUserId = dsg.Id;
            break;
        }

        var updates = new Dictionary<FieldPath, object>
        {
            { new FieldPath("nickName"), nickName }
        };

        firebase.Collection("users").Document(topUserId).UpdateAsync(updates);
    }

    public override void AddResource(string userId, string name, int tier, int amount, OnCompletionDelegateWithParameter onComplete)
    {
        StartCoroutine(AddResourceNow(userId, name, tier, amount, onComplete));
    }

    IEnumerator AddResourceNow(string userId, string name, int tier, int amount, OnCompletionDelegateWithParameter onComplete)
    {
        var taskUsr = firebase.Collection("users").WhereEqualTo("userId", userId).GetSnapshotAsync();
        yield return new WaitUntil(() => taskUsr.IsCompleted);

        // update the record if it is present, or add if it isn't
        string topUserId = "";
        foreach (DocumentSnapshot dsg in taskUsr.Result.Documents)
        {
            var gameData = dsg.ToDictionary();
            topUserId = dsg.Id;
            break;
        }

        var task = firebase.Collection("users").Document(topUserId).Collection("resources").WhereEqualTo("name", name).WhereEqualTo("tier", tier).GetSnapshotAsync();
        yield return new WaitUntil(() => task.IsCompleted);

        int recordCount = 0;
        foreach (DocumentSnapshot dsg in task.Result.Documents)
        {
            recordCount += 1;
            break;
        }

        if (recordCount == 0)
        {
            // record for this resource is not present, add it for the first time
            FirebaseUserResource newRecord = new FirebaseUserResource
            {
                name = name,
                amount = amount,
                tier = tier
            };

            foreach (DocumentSnapshot dsg in taskUsr.Result.Documents)
            {
                var addTask = dsg.Reference.Collection("resources").AddAsync(newRecord);
                yield return new WaitUntil(() => addTask.IsCompleted);

                onComplete?.Invoke(amount);
                break;
            }
        }
        else
        {
            // update the amount
            foreach (DocumentSnapshot dsg in task.Result.Documents)
            {
                var data = dsg.ToDictionary();
                var updates = new Dictionary<FieldPath, object>
                {
                    { new FieldPath("amount"), amount + int.Parse(data["amount"].ToString()) }
                };

                firebase.Collection("users").Document(topUserId).Collection("resources").Document(dsg.Id).UpdateAsync(updates);
                onComplete?.Invoke(amount + int.Parse(data["amount"].ToString()));

                break;
            }
        }
    }

    public override void GetResourceAmount(string userId, string name, int tier, OnCompletionDelegateWithParameter onComplete)
    {
        StartCoroutine(GetResourceAmountNow(userId, name, tier, onComplete));
    }

    IEnumerator GetResourceAmountNow(string userId, string name, int tier, OnCompletionDelegateWithParameter onComplete)
    {
        var taskUsr = firebase.Collection("users").WhereEqualTo("userId", userId).GetSnapshotAsync();
        yield return new WaitUntil(() => taskUsr.IsCompleted);


        // update the record if it is present, or add if it isn't
        string topUserId = "";
        foreach (DocumentSnapshot dsg in taskUsr.Result.Documents)
        {
            var gameData = dsg.ToDictionary();
            topUserId = dsg.Id;
            break;
        }

        var task = firebase.Collection("users").Document(topUserId).Collection("resources").WhereEqualTo("name", name).WhereEqualTo("tier", tier).GetSnapshotAsync();
        yield return new WaitUntil(() => task.IsCompleted);

        foreach (DocumentSnapshot dsg in task.Result.Documents)
        {
            var gameData = dsg.ToDictionary();
            onComplete?.Invoke(int.Parse(gameData["amount"].ToString()));

            yield break;
        }

        onComplete?.Invoke(0);
    }

    public override void GetResourceAmount(string userId, string name, OnCompletionDelegateWithParameter onComplete)
    {
        StartCoroutine(GetResourceAmountNow(userId, name, onComplete));
    }

    IEnumerator GetResourceAmountNow(string userId, string name, OnCompletionDelegateWithParameter onComplete)
    {
        var taskUsr = firebase.Collection("users").WhereEqualTo("userId", userId).GetSnapshotAsync();
        yield return new WaitUntil(() => taskUsr.IsCompleted);


        // update the record if it is present, or add if it isn't
        string topUserId = "";
        foreach (DocumentSnapshot dsg in taskUsr.Result.Documents)
        {
            var gameData = dsg.ToDictionary();
            topUserId = dsg.Id;
            break;
        }

        var task = firebase.Collection("users").Document(topUserId).Collection("resources").WhereEqualTo("name", name).GetSnapshotAsync();
        yield return new WaitUntil(() => task.IsCompleted);

        List<string> results = new List<string>();
        foreach (DocumentSnapshot dsg in task.Result.Documents)
        {
            var gameData = dsg.ToDictionary();
            results.Add(gameData["name"] + "|" + gameData["tier"] + "|" + gameData["amount"]);
        }

        onComplete?.Invoke(results);
    }

    public override void GetResources(string userId, OnCompletionDelegateWithParameter onComplete)
    {
        StartCoroutine(GetResourcesNow(userId, onComplete));
    }

    IEnumerator GetResourcesNow(string userId, OnCompletionDelegateWithParameter onComplete)
    {
        var taskUsr = firebase.Collection("users").WhereEqualTo("userId", userId).GetSnapshotAsync();
        yield return new WaitUntil(() => taskUsr.IsCompleted);

        // update the record if it is present, or add if it isn't
        string topUserId = "";
        foreach (DocumentSnapshot dsg in taskUsr.Result.Documents)
        {
            var gameData = dsg.ToDictionary();
            topUserId = dsg.Id;

            break;
        }

        var task = firebase.Collection("users").Document(topUserId).Collection("resources").GetSnapshotAsync();
        yield return new WaitUntil(() => task.IsCompleted);

        List<string> results = new List<string>();
        foreach (DocumentSnapshot dsg in task.Result.Documents)
        {
            var gameData = dsg.ToDictionary();
            results.Add(gameData["name"] + "|" + gameData["tier"] + "|" + gameData["amount"]);
        }

        onComplete?.Invoke(results);
    }

    public override void FetchLandResources(string landId, string userId, OnCompletionDelegateWithParameter onComplete)
    {
        StartCoroutine(FetchLandResourcesNow(landId, userId, onComplete));
    }

    IEnumerator FetchLandResourcesNow(string landId, string userId, OnCompletionDelegateWithParameter onComplete)
    {
        var taskLand = firebase.Collection("lands").WhereEqualTo("landId", landId).GetSnapshotAsync();
        yield return new WaitUntil(() => taskLand.IsCompleted);

        // update the record if it is present, or add if it isn't
        string topLandId = "";
        foreach (DocumentSnapshot dsg in taskLand.Result.Documents)
        {
            topLandId = dsg.Id;
            break;
        }

        var task = firebase.Collection("lands").Document(topLandId).Collection("resources").GetSnapshotAsync();
        yield return new WaitUntil(() => task.IsCompleted);

        List<string> results = new List<string>();
        foreach (DocumentSnapshot dsg in task.Result.Documents)
        {
            var gameData = dsg.ToDictionary();
            results.Add(dsg.Id + "|" + gameData["name"] + "|" + gameData["tier"] + "|" + gameData["amountMin"] + "|" + gameData["amountMax"] + "|" + gameData["locX"] + "|" + gameData["locY"] + "|" + gameData["locZ"]);
        }

        onComplete?.Invoke(results);
    }

    public override void CollectResource(string resourceId, string userId, string resourceType, int resourceTier, int amount, OnCompletionDelegateWithParameter onComplete)
    {
        StartCoroutine(CollectResourceNow(resourceId, userId, resourceType, resourceTier, amount, onComplete));
    }

    IEnumerator CollectResourceNow(string resourceId, string userId, string resourceType, int resourceTier, int amount, OnCompletionDelegateWithParameter onComplete)
    {
        var taskUsr = firebase.Collection("users").WhereEqualTo("userId", userId).GetSnapshotAsync();
        yield return new WaitUntil(() => taskUsr.IsCompleted);

        // update the record if it is present, or add if it isn't
        string topUserId = "";
        foreach (DocumentSnapshot dsg in taskUsr.Result.Documents)
        {
            var gameData = dsg.ToDictionary();
            topUserId = dsg.Id;

            break;
        }

        var task = firebase.Collection("users").Document(topUserId).Collection("collected_resources").GetSnapshotAsync();
        yield return new WaitUntil(() => task.IsCompleted);

        bool found = false;
        foreach (DocumentSnapshot dsg in task.Result.Documents)
        {
            var gameData = dsg.ToDictionary();
            if (gameData["id"].ToString() == resourceId)
            {
                found = true;
                break;
            }
        }

        if (found)
        {
            onComplete?.Invoke(false);
            yield break;
        }

        // add new record as collected
        var newRecord = new FirebaseUserResourceCollected
        {
            id = resourceId,
            name = resourceType,
            tier = resourceTier,
            amount = amount,
            dateTime = DateTime.Now
        };

        var addTask = firebase.Collection("users").Document(topUserId).Collection("collected_resources").AddAsync(newRecord);
        yield return new WaitUntil(() => addTask.IsCompleted);

        onComplete?.Invoke(true);
    }

    public override void IfResourceCollected(string resourceId, string userId, OnCompletionDelegateWithParameter onComplete)
    {
        StartCoroutine(IfResourceCollectedNow(resourceId, userId, onComplete));
    }

    IEnumerator IfResourceCollectedNow(string resourceId, string userId, OnCompletionDelegateWithParameter onComplete)
    {
        var taskUsr = firebase.Collection("users").WhereEqualTo("userId", userId).GetSnapshotAsync();
        yield return new WaitUntil(() => taskUsr.IsCompleted);

        // update the record if it is present, or add if it isn't
        string topUserId = "";
        foreach (DocumentSnapshot dsg in taskUsr.Result.Documents)
        {
            var gameData = dsg.ToDictionary();
            topUserId = dsg.Id;

            break;
        }

        var task = firebase.Collection("users").Document(topUserId).Collection("collected_resources").GetSnapshotAsync();
        yield return new WaitUntil(() => task.IsCompleted);

        bool collected = false;
        foreach (DocumentSnapshot dsg in task.Result.Documents)
        {
            var gameData = dsg.ToDictionary();
            if (gameData["id"].ToString() == resourceId)
            {
                collected = true;
                break;
            }
        }

        onComplete(collected);
    }

    public override void ResetResources()
    {
        StartCoroutine(ResetResourcesNew());
    }

    IEnumerator ResetResourcesNew()
    {
#if DEBUG
        Debug.LogWarning("If you are not sure what you are doing, DO NOT PROCEED!");

        var taskUsr = firebase.Collection("users").GetSnapshotAsync();
        yield return new WaitUntil(() => taskUsr.IsCompleted);

        string id;
        foreach (DocumentSnapshot snp in taskUsr.Result.Documents)
        {
            id = snp.Id;

            var docTask = snp.Reference.Collection("collected_resources").GetSnapshotAsync();
            yield return new WaitUntil(() => docTask.IsCompleted);

            foreach (DocumentSnapshot dsh in docTask.Result.Documents)
            {
                dsh.Reference.DeleteAsync();
            }
        }
#else
        Debug.Log("Can't execute this command");
        yield break;
#endif
    }

    public override void AddBeacon(Vector3 location, int index, string userId)
    {
        StartCoroutine(AddBeaconNow(location, index, userId));
    }

    IEnumerator AddBeaconNow(Vector3 location, int index, string userId)
    {
        var taskUsr = firebase.Collection("users").WhereEqualTo("userId", userId).GetSnapshotAsync();
        yield return new WaitUntil(() => taskUsr.IsCompleted);

        // update the record if it is present, or add if it isn't
        string topUserId = "";
        foreach (DocumentSnapshot dsg in taskUsr.Result.Documents)
        {
            var gameData = dsg.ToDictionary();
            topUserId = dsg.Id;

            break;
        }

        // add new record as collected
        var newRecord = new FirebaseUserBeacon
        {
            locX = location.x,
            locY = location.y,
            locZ = location.z,
            dateTime = DateTime.Now,
            index = index
        };

        var addTask = firebase.Collection("users").Document(topUserId).Collection("beacons").AddAsync(newRecord);
        yield return new WaitUntil(() => addTask.IsCompleted);
    }

    public override void GetUserBeacons(string userId, OnCompletionDelegateWithParameter onComplete)
    {
        StartCoroutine(GetUserBeaconsNow(userId, onComplete));
    }

    IEnumerator GetUserBeaconsNow(string userId, OnCompletionDelegateWithParameter onComplete)
    {
        var taskUsr = firebase.Collection("users").WhereEqualTo("userId", userId).GetSnapshotAsync();
        yield return new WaitUntil(() => taskUsr.IsCompleted);

        // update the record if it is present, or add if it isn't
        string topUserId = "";
        foreach (DocumentSnapshot dsg in taskUsr.Result.Documents)
        {
            var gameData = dsg.ToDictionary();
            topUserId = dsg.Id;

            break;
        }

        var task = firebase.Collection("users").Document(topUserId).Collection("beacons").GetSnapshotAsync();
        yield return new WaitUntil(() => task.IsCompleted);

        string result = "";
        foreach (DocumentSnapshot dsg in task.Result.Documents)
        {
            var gameData = dsg.ToDictionary();

            result += gameData["locX"] + ":" + gameData["locY"] + ":" + gameData["locZ"] + "|" + gameData["index"] + "~";
        }

        if (result.EndsWith("~"))
        {
            result = result.Substring(0, result.Length - 1);
        }

        onComplete?.Invoke(result);
    }

    public override void GetLandInfo(string landId, OnCompletionDelegateWithParameter onComplete)
    {
        StartCoroutine(GetLandInfoNow(landId, onComplete));
    }

    IEnumerator GetLandInfoNow(string landId, OnCompletionDelegateWithParameter onComplete)
    {
        var task = firebase.Collection("lands").WhereEqualTo("landId", landId).GetSnapshotAsync();
        yield return new WaitUntil(() => task.IsCompleted);

        foreach (DocumentSnapshot dsg in task.Result.Documents)
        {
            var gameData = dsg.ToDictionary();
            int width = int.Parse(gameData["width"].ToString());
            int height = int.Parse(gameData["height"].ToString());
            float locX = float.Parse(gameData["locX"].ToString());
            float locY = float.Parse(gameData["locY"].ToString());
            float locZ = float.Parse(gameData["locZ"].ToString());
            string theme = gameData["theme"].ToString();
            string landEast = gameData["landEast"].ToString();
            string landWest = gameData["landWest"].ToString();
            string landNorth = gameData["landNorth"].ToString();
            string landSouth = gameData["landSouth"].ToString();

            LandInfo land = new LandInfo(width, height, locX, locY, locZ, theme, landEast, landWest, landNorth, landSouth);

            onComplete?.Invoke(land);
            yield break;
        }

        onComplete?.Invoke(null);
    }

    public override void ResetBeacons(string userId)
    {
        StartCoroutine(ResetBeaconsNow(userId));
    }

    IEnumerator ResetBeaconsNow(string userId)
    {
        var taskUsr = firebase.Collection("users").WhereEqualTo("userId", userId).GetSnapshotAsync();
        yield return new WaitUntil(() => taskUsr.IsCompleted);

        // update the record if it is present, or add if it isn't
        string topUserId = "";
        foreach (DocumentSnapshot dsg in taskUsr.Result.Documents)
        {
            var gameData = dsg.ToDictionary();
            topUserId = dsg.Id;

            break;
        }

        var task = firebase.Collection("users").Document(topUserId).Collection("beacons").GetSnapshotAsync();
        yield return new WaitUntil(() => task.IsCompleted);

        foreach (DocumentSnapshot dsg in task.Result.Documents)
        {
            dsg.Reference.DeleteAsync();
        }
    }

    public override void GetNearbyLands(Vector3 origin, float distance, OnCompletionDelegateWithParameter onComplete)
    {
        StartCoroutine(GetNearbyLandsNow(origin, distance, onComplete));
    }

    IEnumerator GetNearbyLandsNow(Vector3 origin, float distance, OnCompletionDelegateWithParameter onComplete)
    {
        var taskLands = firebase.Collection("lands").GetSnapshotAsync();
        yield return new WaitUntil(() => taskLands.IsCompleted);

        List<string> result = new List<string>();
        Vector3 v;
        foreach (DocumentSnapshot dsg in taskLands.Result.Documents)
        {
            var gameData = dsg.ToDictionary();
            v = new Vector3(float.Parse(gameData["locX"].ToString()), float.Parse(gameData["locY"].ToString()), float.Parse(gameData["locZ"].ToString()));
            if (Vector3.Distance(v, origin) <= distance)
            {
                result.Add(gameData["landId"].ToString());
            }
        }

        onComplete?.Invoke(result.ToArray());
    }

    public override void GetLandDungeons(string landId, OnCompletionDelegateWithParameter onComplete)
    {
        StartCoroutine(GetLandDungeonsNow(landId, onComplete));
    }

    IEnumerator GetLandDungeonsNow(string landId, OnCompletionDelegateWithParameter onComplete)
    {
        var taskLand = firebase.Collection("lands").WhereEqualTo("landId", landId).GetSnapshotAsync();
        yield return new WaitUntil(() => taskLand.IsCompleted);

        string topLandId = "";
        foreach (DocumentSnapshot dsg in taskLand.Result.Documents)
        {
            var gameData = dsg.ToDictionary();
            topLandId = dsg.Id;

            break;
        }

        var dungeons = firebase.Collection("lands").Document(topLandId).Collection("dungeons");
        if (dungeons != null)
        {
            var taskDungeons = dungeons.GetSnapshotAsync();
            yield return new WaitUntil(() => taskDungeons.IsCompleted);

            List<string> result = new List<string>();
            string line;
            foreach (DocumentSnapshot dsg in taskDungeons.Result.Documents)
            {
                var gameData = dsg.ToDictionary();
                line = gameData["dungeonId"].ToString() + "|" + gameData["locX"].ToString() + "|" + gameData["locY"].ToString() + "|" + gameData["locZ"].ToString() + "|" + gameData["prefab"] + "|" + gameData["possibleThemes"].ToString();

                for (int i = 0; i < (int)(float.Parse(gameData["possibleThemes"].ToString())); i++)
                {
                    line += "|" + gameData["possibleTheme" + (i + 1)].ToString();
                }

                result.Add(line);
            }

            onComplete?.Invoke(result.ToArray());
        }
        else
        {
            onComplete?.Invoke(null);
        }
    }

    public override void IfDungeonCleaned(string landId, string dungeonId, string userId, OnCompletionDelegateWithParameter onComplete)
    {
        StartCoroutine(IfDungeonCleanedNow(landId, dungeonId, userId, onComplete));
    }

    IEnumerator IfDungeonCleanedNow(string landId, string dungeonId, string userId, OnCompletionDelegateWithParameter onComplete)
    {
        var taskUser = firebase.Collection("users").WhereEqualTo("userId", userId).GetSnapshotAsync();
        yield return new WaitUntil(() => taskUser.IsCompleted);

        string topUserId = "";
        foreach (DocumentSnapshot dsg in taskUser.Result.Documents)
        {
            var gameData = dsg.ToDictionary();
            topUserId = dsg.Id;

            break;
        }

        var taskDungeon = firebase.Collection("users").Document(topUserId).Collection("dungeons").WhereEqualTo("dungeonId", dungeonId).WhereEqualTo("landId", landId).GetSnapshotAsync();
        yield return new WaitUntil(() => taskDungeon.IsCompleted);

        string topDungeonId = "";
        foreach (DocumentSnapshot dsg in taskDungeon.Result.Documents)
        {
            var gameData = dsg.ToDictionary();
            topDungeonId = dsg.Id;

            break;
        }

        if (!string.IsNullOrEmpty(topDungeonId))
        {
            var task = firebase.Collection("users").Document(topUserId).Collection("dungeons").Document(topDungeonId).GetSnapshotAsync();
            yield return new WaitUntil(() => task.IsCompleted);

            var gameData = task.Result.ToDictionary();
            DateTime completedAt = DateTime.Parse(gameData["clearedAt"].ToString().Substring(11));
            string result = completedAt.ToOADate() + "|" + gameData["result"].ToString();

            onComplete?.Invoke(result);
        }
        else
        {
            onComplete?.Invoke(null);
        }
    }

    public override void VisitDungeon(int result, string landId, string dungeonId, string userId)
    {
        StartCoroutine(VisitDungeonNow(result, landId, dungeonId, userId));
    }

    IEnumerator VisitDungeonNow(int result, string landId, string dungeonId, string userId)
    {
        // test if such a dungeon exists
        var taskLand = firebase.Collection("lands").WhereEqualTo("landId", landId).GetSnapshotAsync();
        yield return new WaitUntil(() => taskLand.IsCompleted);

        string topLandId = "";
        foreach (DocumentSnapshot dsg in taskLand.Result.Documents)
        {
            var gameData = dsg.ToDictionary();
            topLandId = dsg.Id;

            break;
        }

        var taskDungeon = firebase.Collection("lands").Document(topLandId).Collection("dungeons").WhereEqualTo("dungeonId", dungeonId).GetSnapshotAsync();
        yield return new WaitUntil(() => taskDungeon.IsCompleted);

        string topDungeonId = "";
        foreach (DocumentSnapshot dsg in taskDungeon.Result.Documents)
        {
            var gameData = dsg.ToDictionary();
            topDungeonId = dsg.Id;

            break;
        }

        if (string.IsNullOrEmpty(topDungeonId))
        {
            // no such dungeon exists
            yield break;
        }

        // add new dungeon record
        var taskUser = firebase.Collection("users").WhereEqualTo("userId", userId).GetSnapshotAsync();
        yield return new WaitUntil(() => taskUser.IsCompleted);

        string topUserId = "";
        foreach (DocumentSnapshot dsg in taskUser.Result.Documents)
        {
            var gameData = dsg.ToDictionary();
            topUserId = dsg.Id;

            break;
        }

        FirebaseDungeonResult dungeonResult = new FirebaseDungeonResult
        {
            dungeonId = dungeonId,
            landId = landId,
            clearedAt = DateTime.Now,
            result = result
        };

        var taskUserDungeon = firebase.Collection("users").Document(topUserId).Collection("dungeons").AddAsync(dungeonResult);
        yield return new WaitUntil(() => taskUserDungeon.IsCompleted);
    }
}
