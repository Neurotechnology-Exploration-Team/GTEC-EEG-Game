using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using DG.Tweening;

public class GunBehavior : MonoBehaviour
{
    [Header("Gun Settings")]
    [Range(0f, 2.0f)]
    [SerializeField] private float reloadTime;
    [SerializeField] bool reloading;
    [SerializeField] GameObject projectile;
    [SerializeField] Transform projectileSpawn;

    //Stat trackers
    private float shots = 0f;

    private InputManager inputManager;
    private Coroutine reloadRoutine;

    [Header("UI")]
    public Image crossHair;

    public void Awake()
    {
        inputManager = InputManager.Instance;
    }

    private void Update()
    {
        if (inputManager.fire)
        {
            Fire();
        }
    }

    /// <summary>
    /// A method which upon activationn will simulate a bullet with no drop
    /// will be fired from the forward direction of the camera. This bullet
    /// will travel for the set range and return a hit if it intersects
    /// with an object witin that range. It will then itereate the shots and if
    /// it hits the hits fields.
    /// </summary>
    public void Fire()
    {
        if (!reloading)
        {
            GameObject bullet = Instantiate(projectile, projectileSpawn.position, projectileSpawn.transform.rotation);
            shots++;

            reloading = true;
            reloadRoutine = StartCoroutine(Reload());

            // Effects
            Camera.main.transform.DOComplete();
            Camera.main.transform.DOShakePosition(.2f, .01f, 10, 90, false, true).SetUpdate(true);
            transform.DOLocalMoveZ(-.1f, .05f).OnComplete(() => transform.DOLocalMoveZ(0.020f, .2f));
        }
    }

    IEnumerator Reload()
    {
        ReloadUI(reloadTime);
        yield return new WaitForSeconds(reloadTime);
        reloading = false;
        StopCoroutine(reloadRoutine);
    }

    public void ReloadUI(float time)
    {
        crossHair.transform.DORotate(new Vector3(0, 0, 90), time, RotateMode.LocalAxisAdd).SetEase(Ease.Linear).OnComplete(() => crossHair.transform.DOPunchScale(Vector3.one / 3, .2f, 10, 1).SetUpdate(true));
    }
}
