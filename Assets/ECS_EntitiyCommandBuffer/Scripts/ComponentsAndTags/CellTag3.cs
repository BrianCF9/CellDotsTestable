using Unity.Entities;

namespace TMG.ECS_CommandBuffer3
{
    
    public struct CellTag3 : IComponentData {
        public Entity selfEntity;
        public float maxSize;
        public bool maxSizeReached;
        public bool readyToSpawn;
        public float timeToGrowth;
        public float actualTimeToGrowth;
        public float deltaTime;
    }
}