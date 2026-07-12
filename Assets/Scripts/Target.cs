using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;
using System.Collections.Generic;
public class Target : MonoBehaviour
{
    private Rigidbody targetRb;
    private GameManager gameManager;
    public ParticleSystem explosionParticle;
    public AudioSource destroySoundGood;
    public AudioSource destroySoundBad;
    private float minSpeed = 8;
    private float maxSpeed = 12;
    private float maxTorque = 10;
    private float xRange = 6;
    private float ySpawnPos = 0;
    public int pointValue;

    void Awake()
    {
        destroySoundGood = GetComponent<AudioSource>();
        destroySoundBad = GetComponent<AudioSource>();
        targetRb = GetComponent<Rigidbody>(); //getting the rigid body from the game
        gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
    }
    void OnEnable(){
        targetRb.linearVelocity = Vector3.zero;
        targetRb.angularVelocity = Vector3.zero;
        transform.position = RandomSpawnPos();
        targetRb.AddForce(RandomForce(), ForceMode.Impulse); // adding force to rigid body
        targetRb.AddTorque(RandomTorque(), RandomTorque(), RandomTorque(), ForceMode.Impulse); // adding torque to rigid body 
    }

    Vector3 RandomForce(){
        return Vector3.up * Random.Range(minSpeed,maxSpeed);
    }

    float RandomTorque(){
        return Random.Range(-maxTorque,maxTorque);
    }

    Vector3 RandomSpawnPos(){
        return new Vector3(Random.Range(-xRange,xRange),-ySpawnPos, 0);
    }

    void Update()
    {
        if (Touchscreen.current != null && Touchscreen.current.primaryTouch.press.wasPressedThisFrame && gameManager.isGameActive){ // to execute this only when pressed and when game is active
            //Debug.Log("Mouse was clicked");
            Ray ray = Camera.main.ScreenPointToRay(Touchscreen.current.primaryTouch.position.ReadValue()); 
            //Debug.DrawRay(ray.origin, ray.direction*100f, Color.red, 2f);
            if (Physics.Raycast(ray, out RaycastHit hit)){
                if (hit.transform == transform){
                    if (gameObject.CompareTag("Bad")){
                        AudioSource.PlayClipAtPoint(destroySoundBad.clip, transform.position);
                        //destroySoundBad.Play();
                        gameObject.SetActive(false);
                        // Destroy(gameObject, destroySoundBad.clip.length);
                    }
                    else{
                        destroySoundGood.Play();
                        AudioSource.PlayClipAtPoint(destroySoundGood.clip, transform.position);
                        gameObject.SetActive(false);
                        // Destroy(gameObject, destroySoundGood.clip.length);
                    }
                    Instantiate(explosionParticle, transform.position, explosionParticle.transform.rotation);
                    gameManager.UpdateScore(pointValue);
                }
            }
        }
    }
    private void OnTriggerEnter(Collider other){
        if (other.CompareTag("DestroyZone")){
            //Destroy(gameObject);
            gameObject.SetActive(false);
            if (!gameObject.CompareTag("Bad")){
                gameManager.LoseLife();
        }
        }
    }
}