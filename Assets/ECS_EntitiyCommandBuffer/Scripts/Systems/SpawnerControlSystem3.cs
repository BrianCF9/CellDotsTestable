using Unity.Entities;
using UnityEngine;

namespace TMG.ECS_CommandBuffer3
{
    [UpdateInGroup(typeof(InitializationSystemGroup))]
    public class SpawnerControlSystem3 : SystemBase
    {
        private EndInitializationEntityCommandBufferSystem _endInitializationEntityCommandBufferSystem3;
        protected override void OnCreate()
        {
            _endInitializationEntityCommandBufferSystem3 =
                World.GetOrCreateSystem<EndInitializationEntityCommandBufferSystem>();
        }

        protected override void OnUpdate()
        {
            if (!Input.GetKeyDown(KeyCode.Y) && !Input.GetKeyDown(KeyCode.N))
            {
                return;
            }

            var ecb = _endInitializationEntityCommandBufferSystem3.CreateCommandBuffer();
            var spawnerQuery = EntityManager.CreateEntityQuery(typeof(EntitySpawnData3));

            if (Input.GetKeyDown(KeyCode.Y))
            {
                ecb.AddComponent<ShouldSpawnTag3>(spawnerQuery);
            }

            if (Input.GetKeyDown(KeyCode.N))
            {
                ecb.RemoveComponent<ShouldSpawnTag3>(spawnerQuery);
            }
        }
    }
}