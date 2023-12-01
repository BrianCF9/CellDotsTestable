using Unity.Entities;

namespace TMG.ECS_CommandBuffer4
{
    [GenerateAuthoringComponent]
    public struct EntitySpawnData4 : IComponentData
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