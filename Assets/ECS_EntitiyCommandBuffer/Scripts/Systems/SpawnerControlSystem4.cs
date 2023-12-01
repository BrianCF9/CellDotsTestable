using Unity.Entities;
using UnityEngine;

namespace TMG.ECS_CommandBuffer4
{
    [UpdateInGroup(typeof(InitializationSystemGroup))]
    public class SpawnerControlSystem4 : SystemBase
    {
        private EndInitializationEntityCommandBufferSystem _endInitializationEntityCommandBufferSystem4;
        protected override void OnCreate()
        {
            _endInitializationEntityCommandBufferSystem4 =
                World.GetOrCreateSystem<EndInitializationEntityCommandBufferSystem>();
        }

        protected override void OnUpdate()
        {
            if (!Input.GetKeyDown(KeyCode.Y) && !Input.GetKeyDown(KeyCode.N))
            {
                return;
            }

            var ecb = _endInitializationEntityCommandBufferSystem4.CreateCommandBuffer();
            var spawnerQuery = EntityManager.CreateEntityQuery(typeof(EntitySpawnData4));

            if (Input.GetKeyDown(KeyCode.Y))
            {
                ecb.AddComponent<ShouldSpawnTag4>(spawnerQuery);
            }

            if (Input.GetKeyDown(KeyCode.N))
            {
                ecb.RemoveComponent<ShouldSpawnTag4>(spawnerQuery);
            }
        }
    }
}