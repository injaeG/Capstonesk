using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationToggle1 : MonoBehaviour
{
    private Animator animator; // Reference to the Animator component
    private AudioSource audioSource; // Reference to the AudioSource component
    private bool isAnimationPlaying = false; // Bool to track animation state

    void Start()
    {
        // Get the Animator and AudioSource components at the start
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        // Check if the E key is pressed
        if (Input.GetKeyDown(KeyCode.G))
        {
            // Toggle the animation state
            isAnimationPlaying = !isAnimationPlaying;

            // Update the Animator's bool parameter to start/stop the animation
            animator.SetBool("mirror", isAnimationPlaying);

            // Play or stop the sound based on the animation state
            if (isAnimationPlaying)
            {
                if (audioSource != null && !audioSource.isPlaying)
                {
                    audioSource.Play();
                }
            }
            else
            {
                if (audioSource != null && audioSource.isPlaying)
                {
                    audioSource.Stop();
                }
            }
        }
    }
}
