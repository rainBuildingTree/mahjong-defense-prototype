using UnityEngine;

public class AimController : MonoBehaviour {
    /* Loaded Components *///==================================================
    RectTransform rectTransform;
    EnemyObjectPool enemyObjectPool;
    Canvas userControlCanvas;
    Camera mainCam;



    /* Cached Variables *///==================================================
    Vector2 cachedMousePosition;
    Vector2 closestEnemyPositionOnCanvas;
    Vector2 currentMousePositionOnCanvas;
    Transform closestEnemy;
    float closestEnemyDistanceOnCanvas;



    /* Member Variables *///==================================================
    float maxAttachDistance;



    /* Unity Event Functions *///==================================================
    void Awake() {
        rectTransform = GetComponent<RectTransform>();
        userControlCanvas = GetComponentInParent<Canvas>();
        enemyObjectPool = FindObjectOfType<EnemyObjectPool>();
        mainCam = FindObjectOfType<Camera>();
        maxAttachDistance = rectTransform.rect.width;
    }
    void Start() {
        gameObject.SetActive(false);
    }
    void OnEnable() {
        Cursor.visible = false;
        JumpToMousePosition();
    }
    void OnDisable() {
        Cursor.visible = true;
    }
    void Update() {
        JumpToMousePosition();
        AttachToClosestEnemy();
    }



    /* Private Methods *///==================================================
    void JumpToMousePosition() {
        cachedMousePosition = Input.mousePosition;
        rectTransform.anchoredPosition = cachedMousePosition / userControlCanvas.scaleFactor;
    }
    void AttachToClosestEnemy() {
        closestEnemy = enemyObjectPool.ClosestEnemy(mainCam.ScreenToWorldPoint(cachedMousePosition));
        if (closestEnemy == null)
            return;

        closestEnemyPositionOnCanvas = mainCam.WorldToScreenPoint(closestEnemy.position) / userControlCanvas.scaleFactor;
        currentMousePositionOnCanvas = cachedMousePosition / userControlCanvas.scaleFactor;
        closestEnemyDistanceOnCanvas = Mathf.Abs(Vector2.Distance(closestEnemyPositionOnCanvas, currentMousePositionOnCanvas));

        if (closestEnemyDistanceOnCanvas < maxAttachDistance) {
            rectTransform.anchoredPosition = closestEnemyPositionOnCanvas;
        }
    }

}
