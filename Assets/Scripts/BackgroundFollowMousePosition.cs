using UnityEngine;

public class BackgroundFollowMousePosition : MonoBehaviour
{
    public float maxOffsetX = 100f;
    public float maxOffsetY = 100f;
    public float smoothSpeed = 5f;

    private Vector3 initialPosition;

    void Start()
    {
        initialPosition = transform.position;
    }

    void Update()
    {
        // Fare konumunu ekranın merkezine göre -1 ile 1 arasında normalize et
        float mouseX = (Input.mousePosition.x / Screen.width - 0.5f) * 2f;
        float mouseY = (Input.mousePosition.y / Screen.height - 0.5f) * 2f;

        // Ters yönlü offset hesapla
        float offsetX = -mouseX * maxOffsetX;
        float offsetY = -mouseY * maxOffsetY;

        // Hedef pozisyonu hesapla ve sınırla
        Vector3 targetPosition = initialPosition + new Vector3(offsetX, offsetY, 0);
        targetPosition.x = Mathf.Clamp(targetPosition.x, initialPosition.x - maxOffsetX, initialPosition.x + maxOffsetX);
        targetPosition.y = Mathf.Clamp(targetPosition.y, initialPosition.y - maxOffsetY, initialPosition.y + maxOffsetY);

        // Yumuşak geçiş
        transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * smoothSpeed);
    }
}
