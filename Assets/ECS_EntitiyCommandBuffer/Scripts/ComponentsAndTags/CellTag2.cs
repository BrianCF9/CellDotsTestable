using Unity.Entities;

namespace TMG.ECS_CommandBuffer2
{
    
    public struct CellTag2 : IComponentData {
        public Entity selfEntity;
        public float maxSize;
        public bool maxSizeReached;
        public bool readyToSpawn;
        public float timeToGrowth;
        public float actualTimeToGrowth;
        public float deltaTime;
    }
}