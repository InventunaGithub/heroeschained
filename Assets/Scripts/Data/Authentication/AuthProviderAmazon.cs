using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AuthProviderAmazon : AuthProvider
{
    public override string Vendor()
    {
        return "Amazon";
    }

    public void Init()
    {
    }

    public override void Login(string userName, string password)
    {
        StartCoroutine(LoginNow(userName, password));
    }

    IEnumerator LoginNow(string userName, string password)
    {
        yield break;
    }

    public override void Register(string userName, string password)
    {
        StartCoroutine(RegisterNow(userName, password));
    }

    IEnumerator RegisterNow(string userName, string password)
    {
        yield break;
    }

    public override void Init(string args)
    {
        Init();
    }

    public override void Logout()
    {
        throw new System.NotImplementedException();
    }
}
