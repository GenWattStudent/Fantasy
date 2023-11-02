using Mirror;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;

public class UnitMovement : NetworkBehaviour
{
    [SerializeField] private NavMeshAgent agent = null;
    private Camera mainCamera;

    [Command]
    private void CmdMove(Vector3 position)
    {
        Debug.Log("MoveCommand");
        if (!NavMesh.SamplePosition(position, out NavMeshHit hit, 1f, NavMesh.AllAreas)) return;
        Debug.Log("MoveCommand2 " + hit.position);
        agent.SetDestination(hit.position);
    }

    [ClientCallback]
    void Update()
    {
        if (!isOwned) return;

        if (Mouse.current.rightButton.wasPressedThisFrame && UnitSelection.Instance.SelectedUnits.Count > 0)
        {
            Ray ray = mainCamera.ScreenPointToRay(Mouse.current.position.ReadValue());
            if (!Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity)) return;
            Debug.Log("Move it!"); //
            Debug.Log("Clien: " + hit.point);
            CmdMove(hit.point);
        }
    }

    public override void OnStartAuthority()
    {
        mainCamera = Camera.main;
    }
}
