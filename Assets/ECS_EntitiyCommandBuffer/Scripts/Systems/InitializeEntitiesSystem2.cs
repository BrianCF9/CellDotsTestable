using Unity.Entities;
using Unity.Transforms;
using Unity.Mathematics;
using UnityEngine;
using Unity.Physics;


namespace TMG.ECS_CommandBuffer2
{
    public class InitializeEntitiesSystem2 : SystemBase
    {
        private EndSimulationEntityCommandBufferSystem _endSimulationEntityCommandBufferSystem2;

        protected override void OnCreate()
        {
            _endSimulationEntityCommandBufferSystem2 = World.GetOrCreateSystem<EndSimulationEntityCommandBufferSystem>();
        }

        protected override void OnUpdate()
        {
            var ecb = _endSimulationEntityCommandBufferSystem2.CreateCommandBuffer().ToConcurrent();
            Entities.WithAll<ShouldSpawnTag2>().ForEach((Entity _, int entityInQueryIndex, ref EntitySpawnData2 spawnData, in Translation translation) =>
            {
                if (!spawnData.isInitialized)
                {


                    var entity = ecb.Instantiate(entityInQueryIndex, spawnData.EntityToSpawn);
                    ecb.AddComponent<CellTag2>(entityInQueryIndex, entity);
                    ecb.AddComponent<NonUniformScale>(entityInQueryIndex, entity);
                    ecb.SetComponent(entityInQueryIndex, entity, new Translation
                    {
                        Value = new float3(1f,0.5f,1f)
                    });
                    ecb.SetComponent(entityInQueryIndex, entity, new CellTag2
                    {
                        selfEntity = spawnData.EntityToSpawn,
                        maxSize = 0.5f,
                        readyToSpawn = false,
                        maxSizeReached = false,
                        timeToGrowth = 20f,
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


            _endSimulationEntityCommandBufferSystem2.AddJobHandleForProducer(this.Dependency);
        }
    }
}



