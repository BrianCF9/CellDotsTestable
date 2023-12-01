using Unity.Entities;
using UnityEngine;

namespace TMG.ECS_CommandBuffer2
{
    [UpdateInGroup(typeof(InitializationSystemGroup))]
    public class SpawnerControlSystem2 : SystemBase
    {
        private EndInitializationEntityCommandBufferSystem _endInitializationEntityCommandBufferSystem2;
        protected override void OnCreate()
        {
            _endInitializationEntityCommandBufferSystem2 =
                World.GetOrCreateSystem<EndInitializationEntityCommandBufferSystem>();
        }

        protected override void OnUpdate()
        {
            if (!Input.GetKeyDown(KeyCode.Y) && !Input.GetKeyDown(KeyCode.N))
            {
                return;
            }

            var ecb = _endInitializationEntityCommandBufferSystem2.CreateCommandBuffer();
            var spawnerQuery = EntityManager.CreateEntityQuery(typeof(EntitySpawnData2));

            if (Input.GetKeyDown(KeyCode.Y))
            {
                ecb.AddComponent<ShouldSpawnTag2>(spawnerQuery);
            }

            if (Input.GetKeyDown(KeyCode.N))
            {
                ecb.RemoveComponent<ShouldSpawnTag2>(spawnerQuery);
            }
        }
    }
}