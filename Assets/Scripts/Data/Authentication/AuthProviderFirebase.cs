using Firebase;
using Firebase.Auth;
using Firebase.Firestore;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AuthProviderFirebase : AuthProvider
{
    // Firebase specific
    FirebaseFirestore db;
    FirebaseAuth auth;
    FirebaseUser user;

    DependencyStatus dependencyStatus;

    public void Init()
    {
        db = FirebaseFirestore.DefaultInstance;
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task =>
        {
            dependencyStatus = task.Result;
            if (dependencyStatus == DependencyStatus.Available)
            {
                //If they are avalible Initialize Firebase
                InitializeFirebase();

            }
            else
            {
                Debug.LogError("Could not resolve all Firebase dependencies: " + dependencyStatus);
            }
        });
    }

    private void InitializeFirebase()
    {
        Debug.Log("Setting up Firebase Auth");
        //Set the authentication instance object
        auth = FirebaseAuth.DefaultInstance;
    }

    public override string Vendor()
    {
        return "Firebase";
    }

    public override void Init(string args)
    {
        Init();
    }

    public override void Login(string userName, string password)
    {
        StartCoroutine(LoginNow(userName, password));
    }

    IEnumerator LoginNow(string userName, string password)
    {
        var LoginTask = auth.SignInWithEmailAndPasswordAsync(userName, password);

        yield return new WaitUntil(predicate: () => LoginTask.IsCompleted);

        if (LoginTask.Exception != null)
        {
            FirebaseException firebaseEx = LoginTask.Exception.GetBaseException() as FirebaseException;
            AuthError errorCode = (AuthError)firebaseEx.ErrorCode;
            string message = "Login Failed!";

            switch (errorCode)
            {
                case AuthError.MissingEmail:
                    message = "Missing Email";
                    break;
                case AuthError.MissingPassword:
                    message = "Missing Password";
                    break;
                case AuthError.WrongPassword:
                    message = "Wrong Password";
                    break;
                case AuthError.InvalidEmail:
                    message = "Invalid Email";
                    break;
                case AuthError.UserNotFound:
                    message = "Account does not exist";
                    break;
            }

            OnLoginCompleted?.Invoke(false, message);
        }
        else
        {
            var user = LoginTask.Result;

            OnLoginCompleted?.Invoke(true, user.UserId + "|" + user.DisplayName);
        }
    }

    public override void Register(string userName, string password)
    {
        StartCoroutine(RegisterNow(userName, password));
    }

    IEnumerator RegisterNow(string userName, string password)
    {
        var RegisterTask = auth.CreateUserWithEmailAndPasswordAsync(userName, password);

        yield return new WaitUntil(predicate: () => RegisterTask.IsCompleted);

        if (RegisterTask.Exception != null)
        {
            Debug.LogWarning(message: $"Failed to register task with {RegisterTask.Exception}");
            FirebaseException firebaseEx = RegisterTask.Exception.GetBaseException() as FirebaseException;
            AuthError errorCode = (AuthError)firebaseEx.ErrorCode;

            string message = "Register Failed!";
            switch (errorCode)
            {
                case AuthError.MissingEmail:
                    message = "Missing Email";
                    break;
                case AuthError.MissingPassword:
                    message = "Missing Password";
                    break;
                case AuthError.WeakPassword:
                    message = "Weak Password";
                    break;
                case AuthError.EmailAlreadyInUse:
                    message = "Email Already In Use";
                    break;
            }

            OnRegisterCompleted?.Invoke(false, message);
        } else
        {
            user = RegisterTask.Result;
            createPlayer(user.UserId);

            OnRegisterCompleted?.Invoke(true, user.UserId + "|" + user.DisplayName);
        }
    }

    void createPlayer(string userId)
    {
        // adjust palyer specific settings
    }
}
