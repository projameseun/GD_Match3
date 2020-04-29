using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    private void Awake()
    {
        StorageManager.instance = new StorageManager();
        ShopManager.instance = new ShopManager();
        PlayerManager.instance = new PlayerManager();
    }
    private void Start()
    {
        PlayerManager.instance.AddItem("AAA");
        PlayerManager.instance.AddItem("BBB");
    }
}
