using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Global 
{
    private readonly string url = "http://127.0.0.1/UnityPHP/"; 

    public string GetUrl()
    {
        return url; 
    }
}
