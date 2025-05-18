using System.Collections;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.UI;

public class MeteorShower : MonoBehaviour
{
    [SerializeField] private GameObject ultimateskil;
    [SerializeField] private Image cooldonwnImage;
    [SerializeField] private Text cooldownText;
    [SerializeField] private ParticleSystem meteorShowerParticle;

    public float cooldownTime = 60f;
    private float currentCooldownTime = 0f;
    private bool isCooldown = false;
    public float ultimateDuration = 7f;
    void Start()
    {


        // cooldonwnImage.fillAmount = 0f;
        // cooldownText.text = "";
        meteorShowerParticle.Stop();
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
        Debug.Log("Ultimate Skill Casted");
        if (!isCooldown)
        {
            ActiveUltimateSkill();
            AudioEventSystem.PlayAudio("MeteorShower");

            StartCooldown();
        }
    }
    private void ActiveUltimateSkill()
    {
        // if (ultimateskil != null)
        // {
        //     ultimateskil.SetActive(true);
        //     StartCoroutine(UltimateDuration());

        // }

        if (meteorShowerParticle != null)
        {
            meteorShowerParticle.Play();
            StartCoroutine(UltimateDuration());
        }

    }
    private void StartCooldown()
    {
        isCooldown = true;
        currentCooldownTime = cooldownTime;
    }
    private IEnumerator UltimateDuration()
    {
        yield return new WaitForSeconds(ultimateDuration);
        if (meteorShowerParticle != null)
        {
            meteorShowerParticle.Stop();
            // ultimateskil.SetActive(false);
        }

    }
}