using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slingshot : MonoBehaviour
{
    [Header("Inscribed")]
    public GameObject projectilePrefab;
    public float velocityMult = 10f;
    public GameObject projLinePrefab;

    [Header("Slow-Mo (Easy only)")]
    [Tooltip("Time scale during slow-mo launch")]
    public float slowMoScale = 0.3f;
    [Tooltip("Duration (real seconds) of slow-mo effect")]
    public float slowMoDuration = 1.2f;

    [Header("Dynamic")]
    public GameObject launchPoint;
    public Vector3 launchPos;
    public GameObject projectile;
    public bool aimingMode;

    void Awake()
    {
        Transform launchPointTrans = transform.Find("LaunchPoint");

        launchPoint = launchPointTrans.gameObject;

        launchPoint.SetActive(false);

        launchPos = launchPointTrans.position;
    }

    void Update()
    {
        if (!aimingMode) return;

        Vector3 mousePos2D = Input.mousePosition;

        mousePos2D.z = -Camera.main.transform.position.z;

        Vector3 mousePos3D = Camera.main.ScreenToWorldPoint(mousePos2D);
        Vector3 mouseDelta = mousePos3D - launchPos;
        float maxMagnitude = this.GetComponent<SphereCollider>().radius;

        if (mouseDelta.magnitude > maxMagnitude)
        {
            mouseDelta.Normalize();

            mouseDelta *= maxMagnitude;
        }

        projectile.transform.position = launchPos + mouseDelta;

        if (Input.GetMouseButtonUp(0))
        {
            aimingMode = false;

            Rigidbody projRB = projectile.GetComponent<Rigidbody>();

            projRB.isKinematic = false;

            projRB.collisionDetectionMode = CollisionDetectionMode.Continuous;

            projRB.velocity = -mouseDelta * velocityMult;

            WindManager.ROLL();

            WindManager.REGISTER_PROJECTILE(projRB);

            FollowCam.SWITCH_VIEW(FollowCam.eView.slingshot);

            FollowCam.POI = projectile;

            Instantiate<GameObject>(projLinePrefab, projectile.transform);

            SoundManager.PLAY_LAUNCH();

            projectile = null;

            MissionDemolition.SHOT_FIRED();

            if (DifficultyManager.SlowMoEnabled())
            {
                StartCoroutine(SlowMoCoroutine());
            }
        }
    }

    void OnMouseEnter()
    {
        launchPoint.SetActive(true);
    }

    void OnMouseExit()
    {
        launchPoint.SetActive(false);
    }

    void OnMouseDown()
    {
        if (MissionDemolition.OUT_OF_SHOTS()) 
        {
            return;
        }

        aimingMode = true;

        projectile = Instantiate(projectilePrefab) as GameObject;

        projectile.transform.position = launchPos;

        projectile.GetComponent<Rigidbody>().isKinematic = true;
    }

    IEnumerator SlowMoCoroutine()
    {
        Time.timeScale = slowMoScale;

        Time.fixedDeltaTime = 0.02f * Time.timeScale;

        yield return new WaitForSecondsRealtime(slowMoDuration);

        Time.timeScale = 1f;

        Time.fixedDeltaTime = 0.02f;
    }
}