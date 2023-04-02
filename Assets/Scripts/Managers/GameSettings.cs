using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Manager/GameSettings")]
public class GameSettings : ScriptableObject
{
    [SerializeField] private static string _gameVersion = "0.0.1";
    public string GameVersion
    {
        get { return _gameVersion; }
    }
    [SerializeField] private string _nickName = "Guest";
    public string NickName
    {
        get 
        {
            int valeur = Random.Range(0,9999);
            return _nickName + valeur.ToString();
        }
    }
    [SerializeField] private int ship;
    public int Ship
    {
        get
        {
            return ship;
        }
    }
}
