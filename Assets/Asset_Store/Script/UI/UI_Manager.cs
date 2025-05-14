using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Manager : MonoBehaviour
{
    public TowerPlacement towerPlacement;
    public Button basicTowerButton;
    public Button AdvancedTowerButton;

    void Start()
    {
        basicTowerButton.onClick.AddListener(() => towerPlacement.SetTowerType("Basic"));
        AdvancedTowerButton.onClick.AddListener(() => towerPlacement.SetTowerType("Advanced"));
    }
}
