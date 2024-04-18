using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MemberDTO 
{
    private string id = string.Empty; 
    private string password = string.Empty;
    private string name = string.Empty;
    private string birthday = string.Empty;

    public string Id { get => id; set => id = value; }
    public string Password { get => password; set => password = value; }
    public string Name { get => name; set => name = value; }
    public string Birthday { get => birthday; set => birthday = value; }

}
