using UnityEngine;
using System.Collections.Generic;  // Untuk menggunakan HashSet

public class TowerPlacement : MonoBehaviour
{
    public TowerFactory towerFactory;  // Referensi ke TowerFactory
    public Camera mainCamera;  // Kamera utama untuk menangani klik raycasting
    public LayerMask placementLayer;  // Layer untuk mendeteksi area tempat menara bisa dibangun

    private string currentTowerType = "Basic";  // Menara default yang dipilih
    public float gridSize = 1.0f;  // Ukuran grid (1 unit x 1 unit)

    // Menyimpan posisi grid yang sudah terisi oleh menara
    private HashSet<Vector3> occupiedPositions = new HashSet<Vector3>();

    void Update()
    {
        if (Input.GetMouseButtonDown(0))  // Ketika pemain mengklik mouse
        {
            PlaceTower();  // Tempatkan menara di area yang diklik
        }
    }

    public void PlaceTower()
    {
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);  // Membuat ray dari posisi klik mouse
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, Mathf.Infinity, placementLayer))  // Pastikan ray mengenai area yang valid
        {
            Vector3 placementPosition = hit.point;  // Posisi tempat menara akan dibangun

            // Snap posisi ke grid dengan pembulatan ke kelipatan gridSize
            placementPosition.x = Mathf.Round(placementPosition.x / gridSize) * gridSize;
            placementPosition.z = Mathf.Round(placementPosition.z / gridSize) * gridSize;
            placementPosition.y = 99.52f;  // Sesuaikan dengan tinggi menara, jika perlu

            // Cek apakah posisi grid sudah terisi oleh menara
            if (occupiedPositions.Contains(placementPosition))
            {
                Debug.Log("Posisi ini sudah terisi menara!");
                return;  // Jangan menempatkan menara jika sudah terisi
            }

            // Buat menara berdasarkan tipe yang dipilih
            GameObject newTower = towerFactory.CreateTower(currentTowerType);

            if (newTower != null)
            {
                // Tempatkan menara di posisi yang dipilih pemain
                newTower.transform.position = placementPosition;

                // Tandai posisi ini sebagai sudah terisi oleh menara
                occupiedPositions.Add(placementPosition);
            }
        }
    }

    // Fungsi untuk mengubah tipe menara yang akan dibangun
    public void SetTowerType(string towerType)
    {
        currentTowerType = towerType;
    }
}
