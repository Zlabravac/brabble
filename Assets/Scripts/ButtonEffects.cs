using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

[RequireComponent(typeof(Button))]
public class ButtonEffects : MonoBehaviour, IPointerClickHandler, IPointerDownHandler, IPointerUpHandler
{
    [Header("Sound Settings")]
    [SerializeField] private AudioClip buttonSound; // Sound for this button
    [SerializeField] private AudioSource audioSource; // AudioSource to play sounds

    [Header("Child Appearance")]
    [SerializeField] private Image childImage; // Image of the button's child to grey out
    [SerializeField] private Color normalColor = Color.white; // Default color of the child image
    [SerializeField] private Color pressedColor = Color.grey; // Color of the child image when pressed

    private Button button;

    private void Awake()
    {
        // Get references
        button = GetComponent<Button>();

        // Ensure an AudioSource is assigned or add one automatically
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
            audioSource.playOnAwake = false;
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        // Change child image color to pressed color
        if (childImage != null)
        {
            childImage.color = pressedColor;
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        // Revert child image to normal color when released
        if (childImage != null)
        {
            childImage.color = normalColor;
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        // Play sound when the button is clicked
        PlaySound();
    }

    private void PlaySound()
    {
        if (audioSource != null && buttonSound != null)
        {
            audioSource.clip = buttonSound;
            audioSource.Play();
        }
    }

    // Optional: Set the sound dynamically
    public void SetButtonSound(AudioClip sound)
    {
        buttonSound = sound;
    }
}
