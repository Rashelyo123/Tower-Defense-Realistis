using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnitSelected : MonoBehaviour
{
    public Button[] avatarButtons;
    public GameObject[] characters;

    // Tambahan untuk audio
    public AudioClip clickSound;
    private AudioSource audioSource;

    private void Start()
    {
        // Setup AudioSource
        audioSource = GetComponent<AudioSource>();

        // Pastikan semua karakter disembunyikan saat pertama kali
        foreach (var character in characters)
        {
            character.SetActive(false);
        }

        // Menambahkan listener untuk setiap tombol
        for (int i = 0; i < avatarButtons.Length; i++)
        {
            int index = i;
            avatarButtons[i].onClick.AddListener(() =>
            {
                PlayClickSound();      // Tambahkan pemanggilan sound di sini
                SelectCharacter(index);
            });
        }
    }

    private void PlayClickSound()
    {
        if (audioSource != null && clickSound != null)
        {
            audioSource.PlayOneShot(clickSound);
        }
    }

    // Fungsi untuk memilih karakter berdasarkan index
    private void SelectCharacter(int index)
    {
        foreach (var character in characters)
        {
            character.SetActive(false);
        }

        if (index >= 0 && index < characters.Length)
        {
            characters[index].SetActive(true);
        }
    }
}
