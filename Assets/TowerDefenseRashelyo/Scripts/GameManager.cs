using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [Space(7)]
    public int levelID;

    [Space(7)]
    // Defenders game objects
    public GameObject[] defenders;
    // Defender's instances
    List<GameObject> createdDefenders;
    int createdDefCounts = 0;

    // Coins management
    // Coins management
    [Header("Coins Management")]
    public int startingGameplayCoins = 5000; // Jumlah koin awal untuk gameplay, diatur di Inspector
    [HideInInspector] public int totalCoins;
    private int initialCoins; // Melacak koin awal untuk menghitung koin yang dihasilkan
    public int[] defendersPrice;

    [Space(7)]
    [Header("UI")]
    public Text coinsText;

    // Tower settings
    public Slider towerHealthSlider;
    public Text towerHealthText;

    // Display when Tower's health become zero (the game lost screens)
    public GameObject gameLostWindow;
    // Display when player passed all waves
    public GameObject gameWinWindow;

    [Space(7)]
    [Header("Tower Settings")]
    // Tower settings
    public int towerHealth = 100;
    public int towerDamage = 1;

    [Space(7)]
    [Header("Preview System")]
    public LayerMask groundLayer = 1; // Layer untuk ground
    public float placementRadius = 1f; // Radius untuk cek collision dengan defender lain
    [Header("Selling System")]
    public GameObject sellConfirmationUI; // UI popup untuk konfirmasi jual
    public Text sellInfoText; // Text untuk info defender yang mau dijual
    private GameObject selectedDefenderForSelling;

    [Header("Hover Animation")]
    public float hoverScaleMultiplier = 1.2f; // Scale ketika di-hover
    public float hoverAnimationSpeed = 5f; // Kecepatan animasi scale
    private GameObject currentHoveredDefender; // Defender yang sedang di-hover
    private Vector3 originalScale; // Scale asli defender
    private bool isAnimatingHover = false;

    [Header("Audio SFX")]
    public AudioSource audioSource; // AudioSource component
    public AudioClip invalidPlacementSFX; // SFX ketika placement invalid
    [Range(0f, 1f)]
    public float sfxVolume = 0.7f; // Volume SFX

    // Internal variables
    [HideInInspector] public int currentDefender;
    [HideInInspector] public bool canInstantiate = true;
    [HideInInspector] public bool isSelectingDefender = false;

    // Preview system variables
    private GameObject previewDefender;
    private List<Renderer> previewRenderers = new List<Renderer>();
    private HashSet<Vector3> occupiedPositions = new HashSet<Vector3>();
    private bool isValidPlacement = false;

    void Start()
    {
        // Setup AudioSource jika belum ada
        if (audioSource == null)
        {
            audioSource = GetComponent<AudioSource>();
            if (audioSource == null)
            {
                audioSource = gameObject.AddComponent<AudioSource>();
            }
        }

        // Setup AudioSource settings
        audioSource.playOnAwake = false;
        audioSource.loop = false;

        // Inisialisasi koin untuk gameplay
        totalCoins = startingGameplayCoins;
        initialCoins = startingGameplayCoins; // Simpan koin awal untuk perhitungan
        if (totalCoins < PlayerPrefs.GetInt("Minimum Coins"))
            totalCoins = PlayerPrefs.GetInt("Minimum Coins");

        coinsText.text = totalCoins.ToString();

        createdDefenders = new List<GameObject>();
        canInstantiate = true;
        currentDefender = 1;
    }

    void Update()
    {
        HandlePreviewMovement();
        HandleDefenderPlacement();
        HandleDefenderSelling();
        HandleCancelSelection();
        HandleDefenderHover();
    }
    // Method baru untuk handle hover detection
    void HandleDefenderHover()
    {
        if (!isSelectingDefender) // Hanya aktif ketika tidak sedang place defender
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, 1000))
            {
                // Cek apakah hit defender
                if (hit.transform.CompareTag("Defender"))
                {
                    GameObject hoveredDefender = hit.transform.gameObject;

                    // Jika defender berbeda dari yang sebelumnya di-hover
                    if (currentHoveredDefender != hoveredDefender)
                    {
                        // Reset defender sebelumnya
                        ResetPreviousHoveredDefender();

                        // Set defender baru
                        SetHoveredDefender(hoveredDefender);
                    }
                }
                else
                {
                    // Mouse tidak di defender, reset hover
                    ResetPreviousHoveredDefender();
                }
            }
            else
            {
                // Tidak hit apa-apa, reset hover
                ResetPreviousHoveredDefender();
            }
        }
        else
        {
            // Sedang selecting defender, reset semua hover
            ResetPreviousHoveredDefender();
        }
    }

    // Set defender yang sedang di-hover
    void SetHoveredDefender(GameObject defender)
    {
        if (createdDefenders.Contains(defender))
        {
            currentHoveredDefender = defender;
            originalScale = defender.transform.localScale;
            isAnimatingHover = true;

            // Play hover SFX
            AudioEventSystem.PlayAudio("hoverDefender");

            // Mulai coroutine untuk animasi scale up
            StartCoroutine(AnimateHoverScale(defender, originalScale, originalScale * hoverScaleMultiplier));
        }
    }

    // Reset defender yang sebelumnya di-hover
    void ResetPreviousHoveredDefender()
    {
        if (currentHoveredDefender != null)
        {
            // Mulai coroutine untuk animasi scale down
            StartCoroutine(AnimateHoverScale(currentHoveredDefender, currentHoveredDefender.transform.localScale, originalScale));
            currentHoveredDefender = null;
        }
    }

    // Coroutine untuk animasi scale smooth
    System.Collections.IEnumerator AnimateHoverScale(GameObject defender, Vector3 fromScale, Vector3 toScale)
    {
        if (defender == null) yield break;

        float timer = 0f;
        float duration = 1f / hoverAnimationSpeed;

        while (timer < duration)
        {
            if (defender == null) yield break; // Safety check

            timer += Time.deltaTime;
            float progress = timer / duration;

            // Smooth lerp dengan ease out
            progress = Mathf.SmoothStep(0f, 1f, progress);

            defender.transform.localScale = Vector3.Lerp(fromScale, toScale, progress);
            yield return null;
        }

        // Ensure final scale
        if (defender != null)
        {
            defender.transform.localScale = toScale;
        }

        isAnimatingHover = false;
    }

    public void SetDefenderID(int id)
    {
        if (totalCoins >= defendersPrice[id - 1])
        {
            currentDefender = id;
            isSelectingDefender = true;

            // Play select SFX
            AudioEventSystem.PlayAudio("ChoseCharacter");
            CreatePreviewDefender();

            // Set preview langsung ke tengah layar
            // SetPreviewToScreenCenter();
        }
        else
        {
            Debug.Log("Not enough coins to select this defender!");
            // Play invalid SFX
            PlaySFX(invalidPlacementSFX);
        }
    }


    void CreatePreviewDefender()
    {
        if (previewDefender != null)
            DestroyPreviewDefender();

        // Buat preview defender
        previewDefender = Instantiate(defenders[currentDefender - 1]);

        // Dapatkan semua renderer untuk diubah jadi transparan
        previewRenderers.Clear();
        Renderer[] renderers = previewDefender.GetComponentsInChildren<Renderer>();

        foreach (Renderer renderer in renderers)
        {
            previewRenderers.Add(renderer);

            // Buat material baru untuk preview (transparan)
            foreach (Material mat in renderer.materials)
            {
                // Ubah ke rendering mode transparent
                mat.SetFloat("_Mode", 3); // Transparent mode
                mat.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
                mat.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
                mat.SetInt("_ZWrite", 0);
                mat.DisableKeyword("_ALPHATEST_ON");
                mat.EnableKeyword("_ALPHABLEND_ON");
                mat.DisableKeyword("_ALPHAPREMULTIPLY_ON");
                mat.renderQueue = 3000;

                // Set alpha untuk transparan - cek property dulu
                if (mat.HasProperty("_Color"))
                {
                    Color color = mat.color;
                    color.a = 0.6f;
                    mat.color = color;
                }
                else if (mat.HasProperty("_TintColor"))
                {
                    Color tintColor = mat.GetColor("_TintColor");
                    tintColor.a = 0.6f;
                    mat.SetColor("_TintColor", tintColor);
                }
                else if (mat.HasProperty("_MainTex"))
                {
                    // Untuk beberapa shader particle, bisa set alpha via main texture
                    // Atau skip jika tidak perlu transparansi untuk particle effects
                    Debug.Log($"Material {mat.name} menggunakan shader {mat.shader.name} - skip transparency");
                }
            }
        }

        // Disable collider dan script lain yang tidak diperlukan untuk preview
        Collider[] colliders = previewDefender.GetComponentsInChildren<Collider>();
        foreach (Collider col in colliders)
        {
            col.enabled = false;
        }

        // Disable scripts yang tidak perlu (misal AI, shooting, dll)
        MonoBehaviour[] scripts = previewDefender.GetComponentsInChildren<MonoBehaviour>();
        foreach (MonoBehaviour script in scripts)
        {
            if (script != null && script.GetType() != typeof(Transform))
            {
                script.enabled = false;
            }
        }
    }

    void HandlePreviewMovement()
    {
        if (isSelectingDefender && previewDefender != null)
        {
            // Raycast ke SEMUA layer untuk posisi preview (tidak hanya groundLayer)
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            // Raycast tanpa layer mask (atau gunakan ~0 untuk semua layer)
            if (Physics.Raycast(ray, out hit, 1000))
            {
                // Update posisi preview di mana pun ada collider
                previewDefender.transform.position = hit.point;

                // Cek apakah posisi valid untuk placement (hanya ground yang valid)
                isValidPlacement = IsValidPlacement(hit.point);

                // Update warna preview berdasarkan validitas
                UpdatePreviewColor(isValidPlacement);

                // Rotate preview to face center
                GameObject centerPoint = GameObject.FindWithTag("Center Point");
                if (centerPoint != null)
                {
                    Vector3 lookPos = centerPoint.transform.position - previewDefender.transform.position;
                    lookPos.y = 0;
                    previewDefender.transform.rotation = Quaternion.LookRotation(lookPos);
                }
            }
            else
            {
                // Jika tidak ada collider, proyeksikan ke plane virtual
                Plane groundPlane = new Plane(Vector3.up, Vector3.zero);
                float distance;
                if (groundPlane.Raycast(ray, out distance))
                {
                    previewDefender.transform.position = ray.GetPoint(distance);
                }
                isValidPlacement = false;
                UpdatePreviewColor(isValidPlacement);
            }
        }
    }
    bool IsValidPlacement(Vector3 position)
    {
        // Cek apakah ada defender lain dalam radius tertentu
        foreach (Vector3 occupiedPos in occupiedPositions)
        {
            if (Vector3.Distance(position, occupiedPos) < placementRadius)
            {
                return false;
            }
        }

        // Cek apakah di area ground yang valid
        Collider[] groundColliders = Physics.OverlapSphere(position, 0.1f, groundLayer);
        return groundColliders.Length > 0;
    }

    void UpdatePreviewColor(bool isValid)
    {
        Color previewColor = isValid ? Color.green : Color.red;
        previewColor.a = 0.6f; // Tetap transparan

        foreach (Renderer renderer in previewRenderers)
        {
            if (renderer != null)
            {
                foreach (Material mat in renderer.materials)
                {
                    mat.color = previewColor;
                }
            }
        }
    }


    void PlaceDefender()
    {
        if (previewDefender == null) return;

        canInstantiate = false;

        Vector3 placePosition = previewDefender.transform.position;
        Quaternion placeRotation = previewDefender.transform.rotation;

        // Hapus preview
        DestroyPreviewDefender();

        // Buat defender asli
        GameObject newDefender = Instantiate(defenders[currentDefender - 1], placePosition, placeRotation);
        createdDefenders.Add(newDefender);
        createdDefCounts++;
        occupiedPositions.Add(placePosition);

        // Play deploy SFX
        AudioEventSystem.PlayAudio("deploy_Defender");

        // Deduct coins
        totalCoins -= defendersPrice[currentDefender - 1];
        PlayerPrefs.SetInt("Total Buys", PlayerPrefs.GetInt("Total Buys") + defendersPrice[currentDefender - 1]);
        PlayerPrefs.SetInt("Total Coins", totalCoins);
        coinsText.text = totalCoins.ToString();

        // Reset selection
        isSelectingDefender = false;
        canInstantiate = true;
    }
    private void HandleDefenderPlacement()
    {
        if (Input.GetMouseButtonDown(0)) // Left click
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, 1000))
            {
                // Cek apakah klik defender yang sudah ada
                if (hit.transform.CompareTag("Defender") && !isSelectingDefender)
                {
                    // Klik defender untuk jual
                    HandleDefenderClick(hit.transform.gameObject);
                }
                else if (isSelectingDefender)
                {
                    // Mode placement defender baru
                    if (isValidPlacement && canInstantiate)
                    {
                        PlaceDefender();
                    }
                    else
                    {
                        Debug.Log("Tidak bisa menempatkan defender di sini!");
                        // Play invalid placement SFX
                        PlaySFX(invalidPlacementSFX);
                    }
                }
            }
        }
    }


    // Method baru untuk handle klik defender
    void HandleDefenderClick(GameObject clickedDefender)
    {
        if (createdDefenders.Contains(clickedDefender))
        {
            selectedDefenderForSelling = clickedDefender;

            // Hitung refund amount
            int defenderType = GetDefenderType(clickedDefender);
            int refundAmount = defendersPrice[defenderType - 1] / 2;

            // Tampilkan konfirmasi UI
            if (sellConfirmationUI != null)
            {
                sellConfirmationUI.SetActive(true);
                if (sellInfoText != null)
                {
                    sellInfoText.text = $"Sell this defender for {refundAmount} gold?";
                }
            }
            else
            {
                // Langsung jual tanpa konfirmasi jika tidak ada UI
                SellDefender(clickedDefender);
            }
        }
    }

    // Method untuk konfirmasi jual (dipanggil dari UI button)
    public void ConfirmSellDefender()
    {
        if (selectedDefenderForSelling != null)
        {
            SellDefender(selectedDefenderForSelling);
            selectedDefenderForSelling = null;
            AudioEventSystem.PlayAudio("Sell_Defender");
        }

        if (sellConfirmationUI != null)
            sellConfirmationUI.SetActive(false);
    }

    // Method untuk cancel jual (dipanggil dari UI button)
    public void CancelSellDefender()
    {
        selectedDefenderForSelling = null;
        if (sellConfirmationUI != null)
            sellConfirmationUI.SetActive(false);
    }

    // Method untuk jual defender (dipanggil dari berbagai tempat)
    void SellDefender(GameObject defender)
    {
        // Reset hover jika defender yang dijual sedang di-hover
        if (currentHoveredDefender == defender)
        {
            ResetPreviousHoveredDefender();
        }

        int defenderIndex = createdDefenders.IndexOf(defender);
        if (defenderIndex >= 0)
        {
            // Play sell SFX
            //AudioEventSystem.PlayAudio("ChoseCharacter");

            // Refund 50% dari harga defender
            int defenderType = GetDefenderType(defender);
            int refundAmount = defendersPrice[defenderType - 1] / 2;
            AddCoins(refundAmount);

            // Remove dari occupied positions
            occupiedPositions.Remove(defender.transform.position);

            // Remove dari lists dan destroy
            createdDefenders.RemoveAt(defenderIndex);
            createdDefCounts--;
            Destroy(defender);

            Debug.Log($"Defender sold for {refundAmount} coins!");
        }
    }

    // Method untuk play SFX
    void PlaySFX(AudioClip clip)
    {
        if (audioSource != null && clip != null)
        {
            audioSource.PlayOneShot(clip, sfxVolume);
        }
    }

    // Update method HandleDefenderSelling untuk tetap support right-click
    private void HandleDefenderSelling()
    {
        if (Input.GetMouseButtonDown(1) && !isSelectingDefender) // Right-click to sell
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 1000))
            {
                if (hit.transform.CompareTag("Defender"))
                {
                    // Langsung jual dengan right-click (tanpa konfirmasi)
                    SellDefender(hit.transform.gameObject);
                }
            }
        }
    }

    private void HandleCancelSelection()
    {
        if (isSelectingDefender && (Input.GetKeyDown(KeyCode.Q) || Input.GetMouseButtonDown(1)))
        {
            isSelectingDefender = false;
            DestroyPreviewDefender();
        }
    }

    void DestroyPreviewDefender()
    {
        if (previewDefender != null)
        {
            Destroy(previewDefender);
            previewDefender = null;
            previewRenderers.Clear();
        }
    }

    public void ReduceCoins(int value)
    {
        totalCoins -= value;
        coinsText.text = totalCoins.ToString();
    }

    public void AddCoins(int value)
    {
        totalCoins += value;
        coinsText.text = totalCoins.ToString();
    }


    public void Reduce_Tower_Health(int value)
    {
        towerHealth -= value;
        towerHealthSlider.value = towerHealth;
        towerHealthText.text = towerHealth.ToString();

        if (towerHealth <= 0)
            Game_Lost();
    }
    private void SaveCoinsAndScore()
    {
        // Hitung koin yang dihasilkan selama gameplay
        int earnedCoins = totalCoins - initialCoins;
        if (earnedCoins > 0) // Hanya tambahkan jika koin bertambah
        {
            int currentTotalCoins = PlayerPrefs.GetInt("Total Coins", 0);
            PlayerPrefs.SetInt("Total Coins", currentTotalCoins + earnedCoins);
            PlayerPrefs.SetInt("Total Scores", PlayerPrefs.GetInt("Total Scores") + earnedCoins);
            Debug.Log($"Added {earnedCoins} coins to Total Coins. New Total Coins: {PlayerPrefs.GetInt("Total Coins")}");
        }
    }

    // Helper function to get defender type
    private int GetDefenderType(GameObject defender)
    {
        for (int i = 0; i < defenders.Length; i++)
        {
            if (defender.name.Contains(defenders[i].name))
                return i + 1;
        }
        return 1;
    }

    void OnDestroy()
    {
        // Cleanup preview saat script dihancurkan
        if (previewDefender != null)
            DestroyPreviewDefender();
    }
    public void Game_Lost()
    {
        ResetPreviousHoveredDefender();
        // Hapus preview saat game over
        if (previewDefender != null)
            DestroyPreviewDefender();

        SaveCoinsAndScore(); // Simpan koin dan skor sebelum game berakhir
        Time.timeScale = 0;
        gameLostWindow.SetActive(true);
    }

    public void You_Win()
    {
        ResetPreviousHoveredDefender();
        // Hapus preview saat menang
        if (previewDefender != null)
            DestroyPreviewDefender();

        PlayerPrefs.SetInt("Level Unlocked" + (levelID + 1).ToString(), 1);
        SaveCoinsAndScore(); // Simpan koin dan skor sebelum game berakhir
        Time.timeScale = 0;
        gameWinWindow.SetActive(true);
    }
}