﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MoveByTouch : MonoBehaviour {

    public float moveSpeed;
    public float jumpHeight;

    public GameObject row;
    public GameObject menu;
    public GameObject ControlesMoviles;

    public Joystick joystick;
    public ParticleSystem dust;

    private bool canJump;
    private bool canDoubleJump;
    private bool rotacionA = false;
    private bool rotacionD = false;

    void Update() {
        if (ControlesMoviles.active) {
            canJump = GetComponentInChildren<PlayerControllerUP>().getJump();
            canDoubleJump = GetComponentInChildren<PlayerControllerUP>().getDoubleJump();

            Rigidbody2D rb = GetComponent<Rigidbody2D>();
            float input = joystick.Horizontal;

            if (input < -0.1f) {
                CreateDust();
                rb.linearVelocity = new Vector2(-moveSpeed * Mathf.Abs(input), rb.linearVelocity.y);
                GetComponent<Animator>().SetBool("moving", true);
                GetComponent<SpriteRenderer>().flipX = true;

                rotacionA = true;
                if (rotacionD) {
                    row.transform.Rotate(0f, 180f, 0f);
                    rotacionD = false;
                }
            }

            if (input > 0.1f) {
                CreateDust();
                rb.linearVelocity = new Vector2(moveSpeed * Mathf.Abs(input), rb.linearVelocity.y);
                GetComponent<Animator>().SetBool("moving", true);
                GetComponent<SpriteRenderer>().flipX = false;

                rotacionD = true;
                if (rotacionA) {
                    row.transform.Rotate(0f, 180f, 0f);
                    rotacionA = false;
                }
            }

            if (Mathf.Abs(input) <= 0.1f) {
                GetComponent<Animator>().SetBool("moving", false);
            }

            // 🔍 Mostrar velocidad horizontal en consola
            Debug.Log("Joystick Horizontal: " + input + " | Rigidbody2D.velocity.x: " + rb.linearVelocity.x);
        }
    }

    public void saltar() {
        if (ControlesMoviles.active) {
            Rigidbody2D rb = GetComponent<Rigidbody2D>();
            if (canJump) {
                CreateDust();
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpHeight);
                GetComponentInChildren<PlayerControllerUP>().setJump(false);
            } else if (canDoubleJump) {
                CreateDust();
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpHeight);
                GetComponentInChildren<PlayerControllerUP>().setDoubleJump(false);
                canDoubleJump = false;
            }
        }
    }

    public void settings() {
        if (ControlesMoviles.active) {
            if (!menu.active) {
                Stats.instance.gameObject.SetActive(false);
                menu.SetActive(true);
                Time.timeScale = 0;
            } else {
                menu.SetActive(false);
                Stats.instance.gameObject.SetActive(true);
                Time.timeScale = 1;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (ControlesMoviles.active && collision.CompareTag("Chest")) {
            collision.GetComponent<Chest>().openChest();
        }
    }

    private void OnTriggerStay2D(Collider2D collision) {
        if (ControlesMoviles.active && collision.CompareTag("Instructor")) {
            collision.GetComponent<DialogueManager>().DialogueMobile();
        }
    }

    void CreateDust() {
        dust.Play();
    }
}