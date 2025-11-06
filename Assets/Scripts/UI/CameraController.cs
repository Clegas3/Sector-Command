using UnityEngine;

namespace SectorCommand.UI
{
    /// <summary>
    /// Simple camera controller for scene navigation
    /// </summary>
    public class CameraController : MonoBehaviour
    {
        [Header("Movement Settings")]
        public float moveSpeed = 10f;
        public float fastMoveSpeed = 20f;
        public float rotationSpeed = 100f;
        public float zoomSpeed = 5f;
        
        [Header("Limits")]
        public float minHeight = 5f;
        public float maxHeight = 30f;
        public Vector2 mapBoundsMin = new Vector2(-5, -5);
        public Vector2 mapBoundsMax = new Vector2(15, 15);
        
        [Header("Controls")]
        public bool enableWASDMovement = true;
        public bool enableArrowKeyRotation = true;
        public bool enableMouseRotation = false;
        public bool enableScrollZoom = true;
        
        private float currentHeight;
        
        private void Start()
        {
            currentHeight = transform.position.y;
        }
        
        private void Update()
        {
            HandleMovement();
            HandleRotation();
            HandleZoom();
        }
        
        private void HandleMovement()
        {
            if (!enableWASDMovement) return;
            
            Vector3 movement = Vector3.zero;
            float speed = Input.GetKey(KeyCode.LeftShift) ? fastMoveSpeed : moveSpeed;
            
            if (Input.GetKey(KeyCode.W))
                movement += transform.forward;
            if (Input.GetKey(KeyCode.S))
                movement -= transform.forward;
            if (Input.GetKey(KeyCode.A))
                movement -= transform.right;
            if (Input.GetKey(KeyCode.D))
                movement += transform.right;
            
            movement.y = 0; // Keep movement horizontal
            movement = movement.normalized * speed * Time.deltaTime;
            
            Vector3 newPos = transform.position + movement;
            
            // Clamp to bounds
            newPos.x = Mathf.Clamp(newPos.x, mapBoundsMin.x, mapBoundsMax.x);
            newPos.z = Mathf.Clamp(newPos.z, mapBoundsMin.y, mapBoundsMax.y);
            newPos.y = currentHeight; // Maintain height
            
            transform.position = newPos;
        }
        
        private void HandleRotation()
        {
            if (enableArrowKeyRotation)
            {
                float rotation = 0f;
                
                if (Input.GetKey(KeyCode.Q) || Input.GetKey(KeyCode.LeftArrow))
                    rotation = -rotationSpeed * Time.deltaTime;
                if (Input.GetKey(KeyCode.E) || Input.GetKey(KeyCode.RightArrow))
                    rotation = rotationSpeed * Time.deltaTime;
                
                transform.Rotate(Vector3.up, rotation, Space.World);
            }
            
            if (enableMouseRotation && Input.GetMouseButton(2)) // Middle mouse
            {
                float rotX = Input.GetAxis("Mouse X") * rotationSpeed * Time.deltaTime;
                transform.Rotate(Vector3.up, rotX, Space.World);
            }
        }
        
        private void HandleZoom()
        {
            if (!enableScrollZoom) return;
            
            float scroll = Input.GetAxis("Mouse ScrollWheel");
            
            if (Mathf.Abs(scroll) > 0.01f)
            {
                currentHeight -= scroll * zoomSpeed;
                currentHeight = Mathf.Clamp(currentHeight, minHeight, maxHeight);
                
                Vector3 pos = transform.position;
                pos.y = currentHeight;
                transform.position = pos;
            }
        }
        
        /// <summary>
        /// Focus camera on a specific world position
        /// </summary>
        public void FocusOn(Vector3 position, float height = -1)
        {
            Vector3 targetPos = position;
            targetPos.y = height > 0 ? height : currentHeight;
            transform.position = targetPos;
        }
        
        /// <summary>
        /// Reset camera to default position
        /// </summary>
        public void ResetPosition()
        {
            transform.position = new Vector3(5, 10, -5);
            transform.rotation = Quaternion.Euler(45, 0, 0);
            currentHeight = 10;
        }
    }
}
