using UnityEngine;

public class MovingGoal : MonoBehaviour
{
    [Header("Inscribed")]
    public float moveDistance = 3f;
    public float moveSpeed = 2f;

    private Vector3 _origin;
    private bool _active = false;

    void Start()
    {
        _origin = transform.position;

        _active = DifficultyManager.GoalMoves();

        Debug.Log("MovingGoal active: " + _active + " | DifficultyManager instance: " + DifficultyManager.Instance);
    }

    void Update()
    {
        if (!_active) 
        {
            return;
        }

        float offset = Mathf.Sin(Time.time * moveSpeed) * moveDistance;
        
        transform.position = _origin + new Vector3(offset, 0, 0);
    }
}