using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class AimAssist : MonoBehaviour
{
    [Header("Inscribed")]
    [Tooltip("Number of points along the predicted arc")]
    public int arcPoints = 30;
    [Tooltip("How far ahead in time to simulate (seconds)")]
    public float simDuration = 2.0f;
    [Tooltip("Time step per simulation sample")]
    public float timeStep = 0.05f;

    private LineRenderer _line;
    private Slingshot _slingshot;
    private bool _enabled = false;

    void Awake()
    {
        _line = GetComponent<LineRenderer>();

        _slingshot = GetComponent<Slingshot>();

        _enabled = DifficultyManager.AimAssistEnabled();

        _line.enabled = false;

        _line.positionCount = arcPoints;

        _line.startWidth = 0.05f;

        _line.endWidth = 0.01f;

        _line.startColor = new Color(0f, 1f, 1f, 0.8f);

        _line.endColor = new Color(0f, 1f, 1f, 0f);
    }

    void Update()
    {
        if (!_enabled || _slingshot == null) 
        {
            return;
        }

        if (_slingshot.aimingMode && _slingshot.projectile != null)
        {
            _line.enabled = true;

            DrawArc();
        }

        else
        {
            _line.enabled = false;
        }
    }

    void DrawArc()
    {
        Vector3 mouseDelta = _slingshot.projectile.transform.position - _slingshot.launchPos;
        Vector3 launchVelocity = -mouseDelta * _slingshot.velocityMult;
        Vector3 pos = _slingshot.projectile.transform.position;
        Vector3 vel = launchVelocity;
        Vector3 grav = Physics.gravity;
        Vector3 windAccel = Vector3.zero;

        if (WindManager.S != null)
        {
            windAccel = new Vector3(WindManager.S.currentWind, 0, 0);
        }

        _line.positionCount = arcPoints;

        for (int i = 0; i < arcPoints; i++)
        {
            _line.SetPosition(i, pos);

            vel += (grav + windAccel) * timeStep;
            
            pos += vel * timeStep;
        }
    }
}