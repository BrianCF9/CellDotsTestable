using Unity.Entities;

namespace TMG.ECS_CommandBuffer3
{
    [GenerateAuthoringComponent]
    public struct EntitySpawnData3 : IComponentData
    {
        public Entity EntityToSpawn;
        public bool isInitialized;

        public float deltaTime;
        // public float Timer;
        // public bool FirstCell;
        // public bool ReadyToSpawn;
        // public float MaxSize;
        // public bool MaxSizeReached;
    }
}