using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Stats : MonoBehaviour {
    public int health = 200;
    public int power = 0;
    public int attackDamage = 100;
    public int defense = 0;
    private const int maxHealth = 200;
    
    public GameObject camera;
    
    public Image hearts;
    public Sprite[] heartSprites; // Array para los sprites de vida
    public Image powers;
    public Sprite[] powerSprites; // Array para los sprites de poder
    
    public static Stats instance;
    public TextMeshProUGUI textDamage;
    public TextMeshProUGUI textDefense;
    public GameObject sonidoDaño;
    public GameObject sonidoMuerte;

    private bool once = false;
    private Animator _animator;
    private PlayerController _playerController;


    // Start is called before the first frame update
    void Start() {
        instance = this;

        _animator = GetComponent<Animator>();
        _playerController = GetComponentInParent<PlayerController>();

        // Cogemos los datos guardados de las otras escenas
        if ((int)PlayerPrefs.GetInt("AttackDamage", 0) >= 100) {
            this.attackDamage = (int)PlayerPrefs.GetInt("AttackDamage", 0);
            this.defense = (int)PlayerPrefs.GetInt("Defense", 0);
            // this.textDamage.text = "+" + attackDamage.ToString(); // Asumo que textDefense también existe y quieres actualizarlo.
            // this.textDefense.text = "+" + defense.ToString();
        }

        UpdateHealthUI();
        UpdatePowerUI();
    }

    // Update is called once per frame
    void Update() {
        if (health <= 0) {
            _animator.SetBool("die", true);
            _playerController.isDead();
            camera.SetActive(true);
            
            // Desactivamos los elementos de la UI de stats
            hearts.gameObject.SetActive(false);
            powers.gameObject.SetActive(false);
            textDamage.gameObject.SetActive(false);
            textDefense.gameObject.SetActive(false);

            if (!once) {
                Instantiate(sonidoMuerte);
                once = true;
            }
        }
    }

    public void takeDamage(int value) {
        if((value-defense) > 0) { 
            this.health -= (value-defense);
            UpdateHealthUI();
        }
        Instantiate(sonidoDaño);
    }

    public void takeTrueDamage(int value) {
        this.health -= value;
        UpdateHealthUI();
    }

    public void takePower(int value) {
        this.power += value;
        UpdatePowerUI();
    }

    public int GetHealth()
    {

        return this.health;
    }

    public void setHealth(int value) {
        this.health += value;
        if(this.health > maxHealth) {
            this.health = maxHealth;
        }
        UpdateHealthUI();
    }

    public void addAttackDamage(int value) {
        this.attackDamage += value;
        textDamage.text = "+" + attackDamage.ToString();
        PlayerPrefs.SetInt("AttackDamage", attackDamage);
    }

    public int getAttackDamage() {
        return this.attackDamage;
    }

    public void addDefense(int value) {
        this.defense += value;
        textDefense.text = "+" + defense.ToString();
        PlayerPrefs.SetInt("Defense", defense);
    }

    private void UpdateHealthUI() {
        if (heartSprites == null || heartSprites.Length == 0) return;

        // Calcula el índice basado en la vida. Asume 21 sprites (0-200 en pasos de 10).
        int spriteIndex = Mathf.Clamp(health / 10, 0, heartSprites.Length - 1);
        
        // El array está invertido respecto al cálculo, el sprite 0 es el corazón lleno.
        int invertedIndex = heartSprites.Length - 1 - spriteIndex;
        hearts.sprite = heartSprites[invertedIndex];
    }

    private void UpdatePowerUI() {
        if (powerSprites == null || powerSprites.Length == 0) return;
        int spriteIndex = Mathf.Clamp(power, 0, powerSprites.Length - 1);
        powers.sprite = powerSprites[spriteIndex];
    }
}
