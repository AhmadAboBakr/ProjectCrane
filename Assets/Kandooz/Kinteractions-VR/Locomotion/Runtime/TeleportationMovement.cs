using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;

namespace Kinteractions_VR.Locomotion.Runtime
{
    public class TeleportationMovement : MonoBehaviour
    {
        [SerializeField] private int maxIterations = 30;
        [SerializeField] private LineRenderer lineRenderer;
        [SerializeField] private float gravity = 1;
        [SerializeField] private float dt = .1f;
        [SerializeField] private float speed = 2;
        [SerializeField] private float groundLevel = 0;
        [SerializeField] private LayerMask layerMask;
        [SerializeField] private Transform leftHandPointer;
        [SerializeField] private Transform rightHandPointer;
        [SerializeField] private Transform pointer;
        [SerializeField] private string teleportationTag;
        [SerializeField] private UnityEvent onTeleportationStarted;
        [SerializeField] private UnityEvent onTeleportationEnded;

        private Vector3 gravityVector = Vector3.down;
        private bool showCurve = false;
        private bool canTeleport;

        private readonly List<Vector3> points = new List<Vector3>();
        private Vector3 teleportationPoint;

        private void Awake()
        {
            lineRenderer.transform.parent = null;
            lineRenderer.transform.rotation = Quaternion.identity;
            lineRenderer.transform.position = Vector3.zero;
        }

        private void OnTeleportationButtonDown(Transform pointer)
        {
            this.pointer = pointer;
            showCurve = true;
            lineRenderer.enabled = true;
            onTeleportationStarted.Invoke();
        }

        private async void OnTeleportationButtonUp()
        {
            showCurve = false;
            lineRenderer.enabled = false;
            this.transform.position = teleportationPoint;
            await Task.Delay(200);
            onTeleportationEnded.Invoke();
        }

        private void Update()
        {
            HandleInput();
            if (showCurve)
            {
                gravityVector = Vector3.down * gravity;
                CalculateTeleportationCurve();
                lineRenderer.positionCount = points.Count;
                lineRenderer.SetPositions(points.ToArray());
            }
        }

        private void HandleInput()
        {
            if (Input.GetKeyDown(KeyCode.JoystickButton8))
            {
                OnTeleportationButtonDown(leftHandPointer);
            }

            if (Input.GetKeyUp(KeyCode.JoystickButton8))
            {
                OnTeleportationButtonUp();
            }

            if (Input.GetKeyDown(KeyCode.K))
            {
                OnTeleportationButtonDown(leftHandPointer);
            }

            if (Input.GetKeyUp(KeyCode.K))
            {
                OnTeleportationButtonUp();
            }
        }

        private void CalculateTeleportationCurve()
        {
            points.Clear();
            var (velocity, point) = InitializeCurveParameters();
            points.Add(point);
            int counter = 0;
            lineRenderer.startColor = lineRenderer.endColor = Color.red;
            while (point.y > groundLevel && counter < maxIterations)
            {
                StepCurve(ref velocity, ref point);
                points.Add(point);
                if (RayCast(point, velocity, 1))
                {
                    return;
                }

                counter++;
            }

            if (point.y > groundLevel)
            {
                RayCast(point, velocity, 20);
            }
        }

        private bool RayCast(Vector3 point, Vector3 velocity, float distance)
        {
            canTeleport = false;
            if (velocity.y >= 0)
            {
                return false;
            }

            Ray ray = new Ray(point, velocity);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, distance, layerMask))
            {
                if (!hit.transform.CompareTag(teleportationTag) && string.IsNullOrEmpty(teleportationTag))
                {
                    return false;
                }

                lineRenderer.startColor = lineRenderer.endColor = Color.green;
                points.Add(hit.point);
                teleportationPoint = hit.point;
                canTeleport = true;
                if (hit.point.y > groundLevel)
                {
                    var extraPoint = hit.point;
                    extraPoint.y = groundLevel;
                    points.Add(extraPoint);
                }

                return true;
            }

            return false;
        }

        private void StepCurve(ref Vector3 velocity, ref Vector3 point)
        {
            velocity += gravityVector * dt;
            point += velocity * dt;
        }

        private (Vector3 velocity, Vector3 point) InitializeCurveParameters()
        {
            var velocity = pointer.forward * speed;
            var point = pointer.position + pointer.forward * .3f;
            return (velocity, point);
        }
    }
}