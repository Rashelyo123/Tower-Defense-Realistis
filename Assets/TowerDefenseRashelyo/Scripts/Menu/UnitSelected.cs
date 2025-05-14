using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UnitSelected : MonoBehaviour
{
    // Array untuk karakter-karakter
    public Button[] avatarButtons;
    public GameObject[] characters;

    // Array untuk tombol-tombol avatar

    private void Start()
    {
        // Pastikan semua karakter disembunyikan saat pertama kali
        foreach (var character in characters)
        {
            character.SetActive(false);
        }

        // Menambahkan listener untuk setiap tombol
        for (int i = 0; i < avatarButtons.Length; i++)
        {
            int index = i;  // Membuat salinan dari i agar tidak terpengaruh dalam loop
            avatarButtons[i].onClick.AddListener(() => SelectCharacter(index));
        }
    }

    // Fungsi untuk memilih karakter berdasarkan index
    private void SelectCharacter(int index)
    {
        // Menonaktifkan semua karakter terlebih dahulu
        foreach (var character in characters)
        {
            character.SetActive(false);
        }

        // Menyalakan karakter sesuai dengan index yang dipilih
        if (index >= 0 && index < characters.Length)
        {
            characters[index].SetActive(true);
        }
    }
}
