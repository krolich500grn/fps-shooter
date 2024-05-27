using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour
{
  [Header("Player Health & Damage")]
  private int maxHealth = 100;
  public int currentHealth;
  public Slider HealthSlider; 
  public DeathScreen deathScreen;

  [Header("Player Movement & Gravity")]
  public float movementSpeed = 5f;
  public float jumpForce = 1f;
  private CharacterController controller;
  public float gravity = -9.81f;
  public Transform groundCheck;
  public LayerMask groundMask;
  public float groundDistance = 0.4f;
  private bool isGrounded;
  private Vector3 velocity;

  [Header("Foot Steps")]
  public AudioSource leftFootAudioSource;
  public AudioSource rightFootAudioSource;
  public AudioClip[] footstepSounds;
  public float footstepInterval = 0.5f;
  private float nextFootstepTime;
  private bool isLeftFootstep = true;

  void Start()
  {
    currentHealth = maxHealth;
    HealthSlider.minValue = 1;
    HealthSlider.maxValue = currentHealth;
    HealthSlider.value = currentHealth;
    controller = GetComponent<CharacterController>();
  }

  void Update()
  {
    isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

    if (isGrounded && velocity.y < 0)
    {
      velocity.y = -2f;
    }

    HandleMovement();
    HandleGravity();
    HandleJump();


    // Handle Footstep Sounds
    if (isGrounded && controller.velocity.magnitude > 0.1f && Time.time >= nextFootstepTime)
    {
      PlayFootstepSound();
      nextFootstepTime = Time.time + footstepInterval;
    }

    controller.Move(velocity * Time.deltaTime);
  }

  void HandleMovement()
  {
    float horizontalInput = Input.GetAxis("Horizontal");
    float verticalInput = Input.GetAxis("Vertical");

    Vector3 movement = transform.right * horizontalInput + transform.forward * verticalInput;

    movement.y = 0;
    controller.Move(movement * movementSpeed *  Time.deltaTime);
  }

  void HandleGravity()
  {
    velocity.y += gravity * Time.deltaTime;
  }

  void HandleJump()
  {
    if (Input.GetButtonDown("Jump") && isGrounded) 
    {
      velocity.y = Mathf.Sqrt(jumpForce * -2 * gravity);
    }
  }

  void PlayFootstepSound()
  {
    AudioClip footstepClip = footstepSounds[Random.Range(0, footstepSounds.Length)];

    if (isLeftFootstep) 
    {
      leftFootAudioSource.PlayOneShot(footstepClip);
    } 
    else 
    {
      rightFootAudioSource.PlayOneShot(footstepClip);
    }

    isLeftFootstep = !isLeftFootstep;
  }

  public void TakeDamage(int damageAmount)
  {
    currentHealth -= damageAmount;
    HealthSlider.value -= damageAmount;

    if (currentHealth <= 0) 
    {
      currentHealth = 0;
      Die();
    }
  }

  private void Die()
  {
    deathScreen.showDeathScreen = true;
  }
}
