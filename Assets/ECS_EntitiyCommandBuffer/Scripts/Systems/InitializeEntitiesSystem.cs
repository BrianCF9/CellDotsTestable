using Unity.Entities;
using Unity.Transforms;
using Unity.Mathematics;
using UnityEngine;
using Unity.Physics;


namespace TMG.ECS_CommandBuffer
{
    public class InitializeEntitiesSystem : SystemBase
    {
        private EndSimulationEntityCommandBufferSystem _endSimulationEntityCommandBufferSystem;

        protected override void OnCreate()
        {
            _endSimulationEntityCommandBufferSystem = World.GetOrCreateSystem<EndSimulationEntityCommandBufferSystem>();


        }

        protected override void OnUpdate()
        {
            if (SimulatorManager.developmentDataset == null){
                return;
            }else{
                Debug.Log("Dataset is not null");
                Debug.Log(SimulatorManager.developmentDataset);
            }
            var ecb = _endSimulationEntityCommandBufferSystem.CreateCommandBuffer().ToConcurrent();
            Entities.WithAll<ShouldSpawnTag>().ForEach((Entity _, int entityInQueryIndex, ref EntitySpawnData spawnData, in Translation translation) =>
            {
                if (!spawnData.isInitialized)
                {


                    var entity = ecb.Instantiate(entityInQueryIndex, spawnData.EntityToSpawn);
                    ecb.AddComponent<CellTag>(entityInQueryIndex, entity);
                    ecb.AddComponent<NonUniformScale>(entityInQueryIndex, entity);
                    ecb.SetComponent(entityInQueryIndex, entity, new Translation
                    {
                        Value = new float3(1f,0.5f,0)
                    });
                    ecb.SetComponent(entityInQueryIndex, entity, new CellTag
                    {
                        selfEntity = spawnData.EntityToSpawn,
                        maxSize = 0.5f,
                        readyToSpawn = false,
                        maxSizeReached = false,
                        timeToGrowth = 10f,
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


            _endSimulationEntityCommandBufferSystem.AddJobHandleForProducer(this.Dependency);
        }
    }
}



