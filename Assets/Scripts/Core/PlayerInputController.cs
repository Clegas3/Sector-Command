using UnityEngine;
using SectorCommand.Grid;
using SectorCommand.Core;
using SectorCommand.Projectiles;
using System.Collections.Generic;

namespace SectorCommand.Core
{
    /// <summary>
    /// Handles player input for sector selection and action planning
    /// </summary>
    public class PlayerInputController : MonoBehaviour
    {
        [Header("Input Settings")]
        public Camera mainCamera;
        public LayerMask sectorLayerMask = -1;
        
        [Header("Action Planning")]
        public GridSector selectedOrigin;
        public GridSector selectedTarget;
        public bool isDrawingPath = false;
        
        [Header("Components")]
        public FlightPathDrawer pathDrawer;
        public UI.ProjectileSelector projectileSelector;
        public UI.GameHUD gameHUD;
        
        private GridManager gridManager;
        private GameManager gameManager;
        
        private void Start()
        {
            if (mainCamera == null)
            {
                mainCamera = Camera.main;
            }
            
            gridManager = GridManager.Instance;
            gameManager = GameManager.Instance;
            
            if (pathDrawer == null)
            {
                pathDrawer = gameObject.AddComponent<FlightPathDrawer>();
            }
        }
        
        private void Update()
        {
            if (gameManager == null || gameManager.currentPhase != GameManager.GamePhase.Planning)
            {
                return;
            }
            
            HandleMouseInput();
            HandleKeyboardInput();
        }
        
        private void HandleMouseInput()
        {
            // Left click - select sectors
            if (Input.GetMouseButtonDown(0))
            {
                OnLeftClick();
            }
            
            // Right click - cancel selection
            if (Input.GetMouseButtonDown(1))
            {
                CancelSelection();
            }
            
            // Mouse movement for path drawing
            if (Input.GetMouseButton(0) && isDrawingPath)
            {
                OnMouseDrag();
            }
            
            if (Input.GetMouseButtonUp(0) && isDrawingPath)
            {
                FinishPathDrawing();
            }
        }
        
        private void HandleKeyboardInput()
        {
            // Escape - cancel
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                CancelSelection();
            }
            
            // Space - execute turn
            if (Input.GetKeyDown(KeyCode.Space))
            {
                if (gameManager.plannedActions.Count > 0)
                {
                    gameManager.ExecuteTurn();
                }
            }
            
            // Z - undo last action
            if (Input.GetKeyDown(KeyCode.Z))
            {
                gameManager.CancelLastAction();
            }
        }
        
        private void OnLeftClick()
        {
            GridSector clickedSector = GetSectorUnderMouse();
            
            if (clickedSector == null) return;
            
            if (selectedOrigin == null)
            {
                // Select origin sector
                SelectOrigin(clickedSector);
            }
            else if (selectedTarget == null)
            {
                // Select target sector and start path drawing
                SelectTarget(clickedSector);
            }
        }
        
        private void SelectOrigin(GridSector sector)
        {
            selectedOrigin = sector;
            gridManager.SelectSector(sector);
            
            // Show range for selected projectile
            if (projectileSelector != null)
            {
                ProjectileData selectedProjectile = projectileSelector.GetSelectedProjectile();
                if (selectedProjectile != null)
                {
                    int range = Mathf.CeilToInt(selectedProjectile.range);
                    gridManager.HighlightSectorsInRange(sector.gridPosition, range);
                }
            }
        }
        
        private void SelectTarget(GridSector sector)
        {
            selectedTarget = sector;
            sector.SetState(GridSector.SectorState.Targeted);
            
            // Start path drawing
            if (pathDrawer != null && projectileSelector != null)
            {
                ProjectileData projectile = projectileSelector.GetSelectedProjectile();
                
                if (projectile != null && projectile.allowsCustomPath)
                {
                    Vector3 start = selectedOrigin.transform.position;
                    Vector3 end = selectedTarget.transform.position;
                    pathDrawer.StartDrawing(start, end);
                    isDrawingPath = true;
                }
                else
                {
                    // Auto-plan with default path
                    PlanAction(null);
                }
            }
        }
        
        private void OnMouseDrag()
        {
            if (pathDrawer == null) return;
            
            Vector3 worldPoint = GetMouseWorldPosition();
            pathDrawer.AddPathPoint(worldPoint);
        }
        
        private void FinishPathDrawing()
        {
            if (!isDrawingPath || pathDrawer == null) return;
            
            isDrawingPath = false;
            List<Vector3> customPath = pathDrawer.FinishDrawing();
            PlanAction(customPath);
        }
        
        private void PlanAction(List<Vector3> customPath)
        {
            if (selectedOrigin == null || selectedTarget == null || projectileSelector == null)
            {
                return;
            }
            
            ProjectileData projectile = projectileSelector.GetSelectedProjectile();
            if (projectile == null)
            {
                Debug.LogWarning("No projectile selected!");
                CancelSelection();
                return;
            }
            
            // Calculate accuracy
            float distance = gridManager.CalculateDistance(selectedOrigin.gridPosition, selectedTarget.gridPosition);
            float pathQuality = 1.0f;
            
            if (customPath != null && customPath.Count > 0)
            {
                pathQuality = pathDrawer.EvaluatePathQuality(customPath);
            }
            
            float accuracy = projectile.CalculateAccuracy(distance, pathQuality);
            
            // Create planned action
            GameManager.PlannedAction action = new GameManager.PlannedAction
            {
                origin = selectedOrigin.gridPosition,
                target = selectedTarget.gridPosition,
                projectileData = projectile,
                customPath = customPath,
                estimatedAccuracy = accuracy
            };
            
            // Show info to player
            if (gameHUD != null)
            {
                gameHUD.ShowActionInfo(projectile, accuracy, distance);
            }
            
            // Plan the action
            bool success = gameManager.PlanAction(action);
            
            if (success)
            {
                Debug.Log($"Action planned: {projectile.projectileName} with {accuracy * 100f:F1}% accuracy");
            }
            
            // Reset selection
            CancelSelection();
        }
        
        private void CancelSelection()
        {
            if (selectedOrigin != null)
            {
                selectedOrigin.SetState(GridSector.SectorState.Normal);
                selectedOrigin = null;
            }
            
            if (selectedTarget != null)
            {
                selectedTarget.SetState(GridSector.SectorState.Normal);
                selectedTarget = null;
            }
            
            if (gridManager != null)
            {
                gridManager.ClearHighlights();
            }
            
            if (pathDrawer != null && isDrawingPath)
            {
                pathDrawer.CancelDrawing();
            }
            
            isDrawingPath = false;
            
            if (gameHUD != null)
            {
                gameHUD.HideActionInfo();
            }
        }
        
        private GridSector GetSectorUnderMouse()
        {
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            
            if (Physics.Raycast(ray, out RaycastHit hit, 1000f, sectorLayerMask))
            {
                return hit.collider.GetComponent<GridSector>();
            }
            
            return null;
        }
        
        private Vector3 GetMouseWorldPosition()
        {
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            Plane groundPlane = new Plane(Vector3.up, Vector3.zero);
            
            if (groundPlane.Raycast(ray, out float distance))
            {
                return ray.GetPoint(distance);
            }
            
            return Vector3.zero;
        }
    }
}
