using Unity.Entities;
using Unity.Transforms;
using Unity.Mathematics;
using UnityEngine;
using Unity.Physics;


namespace TMG.ECS_CommandBuffer4
{
    public class InitializeEntitiesSystem4 : SystemBase
    {
        private EndSimulationEntityCommandBufferSystem _endSimulationEntityCommandBufferSystem4;

        protected override void OnCreate()
        {
            _endSimulationEntityCommandBufferSystem4 = World.GetOrCreateSystem<EndSimulationEntityCommandBufferSystem>();
        }

        protected override void OnUpdate()
        {
            var ecb = _endSimulationEntityCommandBufferSystem4.CreateCommandBuffer().ToConcurrent();
            Entities.WithAll<ShouldSpawnTag4>().ForEach((Entity _, int entityInQueryIndex, ref EntitySpawnData4 spawnData, in Translation translation) =>
            {
                if (!spawnData.isInitialized)
                {


                    var entity = ecb.Instantiate(entityInQueryIndex, spawnData.EntityToSpawn);
                    ecb.AddComponent<CellTag4>(entityInQueryIndex, entity);
                    ecb.AddComponent<NonUniformScale>(entityInQueryIndex, entity);
                    ecb.SetComponent(entityInQueryIndex, entity, new Translation
                    {
                        Value = new float3(0,0.5f,0)
                    });
                    ecb.SetComponent(entityInQueryIndex, entity, new CellTag4
                    {
                        selfEntity = spawnData.EntityToSpawn,
                        maxSize = 0.5f,
                        readyToSpawn = false,
                        maxSizeReached = false,
                        timeToGrowth = 17f,
                        actualTimeToGrowth = 0f,
                        deltaTime = spawnData.deltaTime

                    });
                    ecb.SetComponent(entityInQueryIndex, entity, new NonUniformScale
                    {
                        Value = new float3(0,0,0)
                    });
                    ecb.AddComponent(entityInQueryIndex, entity, new PhysicsCollider
                        {
                            Value = Unity.Physics.SphereCollider.Create(new SphereGeometry
                            {
                                Center = new float3(0, 0, 0),
                                Radius = 0.1f
                            })
                        });
                    spawnData.isInitialized = true;
                }
            }).ScheduleParallel();


            _endSimulationEntityCommandBufferSystem4.AddJobHandleForProducer(this.Dependency);
        }
    }
}



