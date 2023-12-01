using Unity.Entities;

namespace TMG.ECS_CommandBuffer
{
    [GenerateAuthoringComponent]
    public struct EntitySpawnData : IComponentData
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