
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;




public class BlueMeteorShower : MonoBehaviour
{
    [SerializeField] private Transform[] meteorSpawnPoint;
    [SerializeField] private GameObject meteorPrefab;
    [SerializeField] private float meteorSpawnInterval = 0.5f;
    [SerializeField] private Image cooldonwnImage;
    [SerializeField] private Text cooldownText;

    public float cooldownTime;
    private float currentCooldownTime;
    private bool isCooldown = false;


    void Start()
    {
        StartCooldown();
    }
    void Update()
    {
        if (isCooldown)
        {
            currentCooldownTime -= Time.deltaTime;
            cooldonwnImage.fillAmount = currentCooldownTime / cooldownTime;
            cooldownText.text = Mathf.Ceil(currentCooldownTime).ToString();
            if (currentCooldownTime <= 0)
            {
                isCooldown = false;
                cooldonwnImage.fillAmount = 0f;
                cooldownText.text = "";
            }
        }
    }
    public void CastSkill()
    {
        if (!isCooldown)
        {
            ActiveUltimateSkill();
            ActiveUltimateSkill();
            StartCooldown();

        }
    }
    private void ActiveUltimateSkill()
    {
        if (meteorSpawnPoint != null)
        {
            StartCoroutine(SpawnMeteor());
        }
    }
    private void StartCooldown()
    {
        isCooldown = true;
        currentCooldownTime = cooldownTime;
    }

    private IEnumerator SpawnMeteor()
    {
        for (int i = 0; i < meteorSpawnPoint.Length; i++)
        {
            int randomIndex = UnityEngine.Random.Range(0, meteorSpawnPoint.Length);
            Transform spawnPoint = meteorSpawnPoint[randomIndex];
            GameObject meteor = Instantiate(meteorPrefab, spawnPoint.position, Quaternion.identity);


            yield return new WaitForSeconds(meteorSpawnInterval);
        }
    }

}
