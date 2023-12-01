using Unity.Entities;

namespace TMG.ECS_CommandBuffer2
{
    [GenerateAuthoringComponent]
    public struct EntitySpawnData2 : IComponentData
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