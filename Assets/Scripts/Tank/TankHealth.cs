using System;
using UnityEngine;
using UnityEngine.UI;

public class TankHealth : MonoBehaviour
{
    public float m_StartingHealth = 100f;               // The amount of health each tank starts with.
    public Slider m_Slider;                             // The slider to represent how much health the tank currently has.
    public Image m_FillImage;                           // The image component of the slider.
    public Color m_FullHealthColor = Color.green;       // The color the health bar will be when on full health.
    public Color m_ZeroHealthColor = Color.red;         // The color the health bar will be when on no health.
    public GameObject m_ExplosionPrefab;                // A prefab that will be instantiated in Awake, then used whenever the tank dies.


    private AudioSource m_ExplosionAudio;               // The audio source to play when the tank explodes.
    private float m_CurrentHealth;                      // How much health the tank currently has.
    private bool m_Dead;                                // Has the tank been reduced beyond zero health yet?
    private Color m_FullHealthGamma;
    private Color m_ZeroHealthGamma;

    public float CurrentHealth
    {
        get
        {
            return m_CurrentHealth;
        }
    }

    private void Awake()
    {
        m_ZeroHealthGamma = new Color(m_ZeroHealthColor.r * m_ZeroHealthColor.r,
            m_ZeroHealthColor.g * m_ZeroHealthColor.g,
            m_ZeroHealthColor.b * m_ZeroHealthColor.b);

        m_FullHealthGamma = new Color(m_FullHealthColor.r * m_FullHealthColor.r,
            m_FullHealthColor.g * m_FullHealthColor.g,
            m_FullHealthColor.b * m_FullHealthColor.b);
    }

    private void OnEnable()
    {
        // When the tank is enabled, reset the tank's health and whether or not it's dead.
        m_CurrentHealth = m_StartingHealth;
        m_Dead = false;

        // Update the health slider's value and color.
        SetHealthUI();
    }


    public void TakeDamage(float amount)
    {
        // Reduce current health by the amount of damage done.
        m_CurrentHealth -= amount;

        // Change the UI elements appropriately.
        SetHealthUI();

        // If the current health is at or below zero and it has not yet been registered, call OnDeath.
        if (m_CurrentHealth <= 0f && !m_Dead)
        {
            OnDeath();
        }
    }


    private void SetHealthUI()
    {
        // Set the slider's value appropriately.
        m_Slider.value = m_CurrentHealth;

        // Interpolate the color of the bar between the choosen colours based on the current percentage of the starting health.
        var colorGamma = Color.Lerp(m_ZeroHealthGamma, m_FullHealthGamma, m_CurrentHealth / m_StartingHealth);
        m_FillImage.color = new Color((float)Math.Sqrt(colorGamma.r), (float)Math.Sqrt(colorGamma.g), (float)Math.Sqrt(colorGamma.b));
    }


    private void OnDeath()
    {
        // Set the flag so that this function is only called once.
        m_Dead = true;

        //Instantiate the explosion prefab and get a reference to the particle system on it.
        var explosion = Instantiate(m_ExplosionPrefab);
        var explosionParticles = explosion.GetComponent<ParticleSystem>();
        Destroy(explosion, explosionParticles.duration);

        //Get a reference to the audio source on the instantiated prefab.
        m_ExplosionAudio = explosionParticles.GetComponent<AudioSource>();

        // Move the instantiated explosion prefab to the tank's position and turn it on.
        explosionParticles.transform.position = transform.position;

        // Play the particle system of the tank exploding.
        explosionParticles.Play();

        // Play the tank explosion sound effect.
        m_ExplosionAudio.Play();

        // Turn the tank off.
        gameObject.SetActive(false);
    }
}