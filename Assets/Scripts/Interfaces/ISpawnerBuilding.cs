
public interface ISpawnerBuilding
{
    void AddUnitToQueue(SoUnit unit);
    float GetSpawnTimer();
    SoUnit GetCurrentSpawningUnit();
    int GetUnitQueueCountByName(string unitName);
}
