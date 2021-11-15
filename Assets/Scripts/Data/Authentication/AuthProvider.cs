using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AuthProvider : MonoBehaviour
{
    public delegate void OnAuthCompletedHandler(bool succeeded, string message);
    public OnAuthCompletedHandler OnLoginCompleted = null;
    public OnAuthCompletedHandler OnRegisterCompleted = null;
    public abstract string Vendor();

    public abstract void Init(string args);

    public abstract void Login(string userName, string password);

    public abstract void Register(string userName, string password);

    public abstract void Logout();
}
