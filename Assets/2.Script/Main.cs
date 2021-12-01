using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Main : MonoBehaviour
{
    public static Main instance;

    public Web web;
    public UserInfo userinfo;
    public Login login;

    public GameObject userProfile;
    void Start()
    {
        instance = this;
        web = GetComponent<Web>();
        userinfo = GetComponent<UserInfo>();
    }

   


}
