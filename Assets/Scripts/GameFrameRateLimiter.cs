using UnityEngine;

public class GameFrameRateLimiter : MonoBehaviour {
    [SerializeField][Range(24, 144)] int _targetFrameRate = 60;
    
    void Awake() {
        Application.targetFrameRate = _targetFrameRate;
    }
}
