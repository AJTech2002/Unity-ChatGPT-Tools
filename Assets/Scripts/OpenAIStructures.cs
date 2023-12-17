using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Secrets
{
    public static string secretKey = "[PUT YOUR SECRET KEY HERE]";
}

[System.Serializable]
public struct ResponseChoice
{
    public int index;
    public Message message;
}

[System.Serializable]
public struct Response
{
    public string id;
    public ResponseChoice[] choices;
}

[System.Serializable]
public struct Message
{
    public string role;
    public string content;
}

[System.Serializable]
public struct Request
{
    public string model;
    public Message[] messages;
}