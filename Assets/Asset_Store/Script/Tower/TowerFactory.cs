using UnityEngine;

public class TowerFactory : MonoBehaviour
{
    public GameObject basicTowerPrefab;
    public GameObject advancedTowerPrefab;

    // Fungsi untuk membuat menara berdasarkan tipe
    public GameObject CreateTower(string towerType)
    {
        if (towerType == "Basic")
        {
            return Instantiate(basicTowerPrefab); // Menghasilkan prefab BasicTower
        }
        else if (towerType == "Advanced")
        {
            return Instantiate(advancedTowerPrefab); // Menghasilkan prefab AdvancedTower
        }
        else
        {
            return null; // Menangani kasus jika tipe menara tidak valid
        }
    }
}
