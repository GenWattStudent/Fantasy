using UnityEngine;

[CreateAssetMenu(fileName = "New Building", menuName = "Create Building")]
public class SOBuilding : ScriptableObject
{
    public enum BuildingType
    {
       Economy,
       Military,
       Defanse
    }

    public BuildingType buildingType;
    public string buildingName;
    public string buildingDescription;
    public Sprite buildingSprite;
    public GameObject buildingPrefab;
    public GameObject buildingValidPrefab;
    public GameObject buildingInvalidPrefab;
    public float buildingCost;
    public float buildingIncome;
    public float buildingHealth;
    public float buildingAttack;
    public SoUnit[] unitsToSpawn;
}
