// using Mirror;
// using UnityEngine;

// public class GridSystem : NetworkBehaviour
// {
//     [SerializeField] private Mouse3D mouse3D;
//     private Grid<int> grid;
//     private GameObject previewPrefab;

//     void Awake()
//     {
//         grid = new Grid<int>(25, 25, 5f, new Vector3(0, 0, 0));
//     }

//     private void ShowBuildPreview()
//     {
//         var mousePosition =  mouse3D.GetMousePositionIn3DWorld();
//         grid.GetXZ(mousePosition, out int x, out int z);
//         var v = grid.GetWorldPosition(x, z);

//         if (previewPrefab is null)
//         {
//             previewPrefab = Instantiate(BuldingManager.Instance.SelectedBuilding.buildingPrefab, v, Quaternion.identity);
//             // chabge color
//             var renderer = previewPrefab.GetComponentInChildren<Renderer>();
//             renderer.material.color = new Color(1, 1, 1, 0.5f);
//         }
//         else
//         {
//             previewPrefab.transform.position = v;
//         }
//     }

//     void Update()
//     {
//         if (BuldingManager.Instance.SelectedBuilding is not null) ShowBuildPreview();
//         if (Input.GetMouseButtonDown(0) && BuldingManager.Instance.SelectedBuilding is not null)
//         {
//             var mousePosition =  mouse3D.GetMousePositionIn3DWorld();
            
//             grid.GetXZ(mousePosition, out int x, out int z);
//             Debug.Log($"x: {x}, z: {z}");
//             var v = grid.GetWorldPosition(x, z);

//             var building = Instantiate(BuldingManager.Instance.SelectedBuilding.buildingPrefab, v, Quaternion.identity);
//             NetworkServer.Spawn(building, connectionToClient);
//             BuldingManager.Instance.SetSelectedBuilding(null);
//         }
//     }
// }
