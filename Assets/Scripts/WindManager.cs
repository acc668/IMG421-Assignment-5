using UnityEngine;
using UnityEngine.UI;

public class WindManager : MonoBehaviour
{
    public static WindManager S;

    [Header("UI References")]
    public RectTransform windArrowRect;
    public Image windArrowImage;
    public Text windLabel;

    [Header("Dynamic")]
    public float currentWind = 0f;

    private System.Collections.Generic.List<Rigidbody> _activeRBs = new System.Collections.Generic.List<Rigidbody>();

    void Awake() 
    { 
        S = this; 
    }

    void Start()
    {
        bool windActive = DifficultyManager.WindRange() != Vector2.zero;

        if (windArrowRect != null) 
        {
            windArrowRect.gameObject.SetActive(windActive);
        }

        if (windLabel != null) 
        {
            windLabel.gameObject.SetActive(windActive);
        }

        if (windActive) 
        {
            RollWind();
        }
    }

    void FixedUpdate()
    {
        if (currentWind == 0f) 
        {
            return;
        }

        _activeRBs.RemoveAll(rb => rb == null);

        Vector3 windForce = new Vector3(currentWind, 0, 0);

        foreach (Rigidbody rb in _activeRBs)
        {
            if (!rb.isKinematic && !rb.IsSleeping())
            {
                rb.AddForce(windForce, ForceMode.Acceleration);
            }
        }
    }

    public static void REGISTER_PROJECTILE(Rigidbody rb)
    {
        if (S != null) 
        {
            S._activeRBs.Add(rb);
        }
    }

    public static void ROLL()
    {
        if (S != null) 
        {
            S.RollWind();
        }
    }

    void RollWind()
    {
        Vector2 range = DifficultyManager.WindRange();

        if (range == Vector2.zero) 
        { 
            currentWind = 0f; return; 
        }

        float magnitude = Random.Range(range.x, range.y);

        currentWind = Random.value > 0.5f ? magnitude : -magnitude;

        UpdateWindUI();
    }

    void UpdateWindUI()
    {
        if (windLabel != null)
        {
            string dir = currentWind > 0 ? "→" : "←";

            windLabel.text = $"WIND  {Mathf.Abs(currentWind):F1}  {dir}";
        }

        if (windArrowRect != null)
        {
            float angle = currentWind > 0 ? 0f : 180f;

            windArrowRect.localEulerAngles = new Vector3(0, 0, angle);

            if (windArrowImage != null)
            {
                float t = Mathf.InverseLerp(
                    DifficultyManager.WindRange().x,
                    DifficultyManager.WindRange().y,
                    Mathf.Abs(currentWind));

                windArrowImage.color = Color.Lerp(Color.green, Color.red, t);
            }
        }
    }
}