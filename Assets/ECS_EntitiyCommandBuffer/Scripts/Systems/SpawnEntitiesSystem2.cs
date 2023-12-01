using Unity.Entities;
using Unity.Transforms;
using Unity.Mathematics;
using UnityEngine;
using Unity.Physics;


namespace TMG.ECS_CommandBuffer2
{
    public class SpawnEntitiesSystem2 : SystemBase
    {
        private EndSimulationEntityCommandBufferSystem _endSimulationEntityCommandBufferSystem2;

        protected override void OnCreate()
        {
            _endSimulationEntityCommandBufferSystem2 = World.GetOrCreateSystem<EndSimulationEntityCommandBufferSystem>();
        }

        protected override void OnUpdate()
        {
            var ecb = _endSimulationEntityCommandBufferSystem2.CreateCommandBuffer().ToConcurrent();
            // var deltaTime = Time.DeltaTime;
            var deltaTime = UnityEngine.Time.unscaledDeltaTime;
            // var scaleTime = .05f;
            // deltaTime *= scaleTime;
            //convert to real time
            
            var upOrDown = UnityEngine.Random.Range(0, 2);
            var leftOrRight = UnityEngine.Random.Range(0, 2);
            var backOrForward = UnityEngine.Random.Range(0, 2);
            var xyz = UnityEngine.Random.Range(0, 3);




            
            Entities.ForEach((Entity _, int entityInQueryIndex, ref  CellTag2 cellTag, ref NonUniformScale scale, in Translation translation) =>
            {
                
                if (cellTag.readyToSpawn && cellTag.actualTimeToGrowth >= cellTag.timeToGrowth)
                {
                        var newEntity = ecb.Instantiate(entityInQueryIndex, cellTag.selfEntity);
                        ecb.AddComponent<CellTag2>(entityInQueryIndex, newEntity);
                        if (xyz == 0)
                        {
                            if (upOrDown == 0)
                        {
                            ecb.SetComponent(entityInQueryIndex, newEntity, new Translation
                            {
                                Value = new float3(translation.Value.x+ 0.25f, translation.Value.y+0.125f , translation.Value.z)
                            });
                        }
                        else
                        {
                            ecb.SetComponent(entityInQueryIndex, newEntity, new Translation
                            {
                                Value = new float3(translation.Value.x+0.25f, translation.Value.y-0.125f, translation.Value.z)
                            });
                        }
                        }
                        else if (xyz == 1)
                        {
                            if (leftOrRight == 0)
                        {
                            ecb.SetComponent(entityInQueryIndex, newEntity, new Translation
                            {
                                Value = new float3(translation.Value.x + 0.25f, translation.Value.y, translation.Value.z)
                            });
                        }
                        else
                        {
                            ecb.SetComponent(entityInQueryIndex, newEntity, new Translation
                            {
                                Value = new float3(translation.Value.x - 0.25f, translation.Value.y, translation.Value.z)
                            });
                        }
                        }
                        else
                        {
                            if (backOrForward == 0)
                        {
                            ecb.SetComponent(entityInQueryIndex, newEntity, new Translation
                            {
                                Value = new float3(translation.Value.x, translation.Value.y, translation.Value.z + 0.25f)
                            });
                        }
                        else
                        {
                            ecb.SetComponent(entityInQueryIndex, newEntity, new Translation
                            {
                                Value = new float3(translation.Value.x, translation.Value.y, translation.Value.z - 0.25f)
                            });
                        }
                        }

                        
                       
                        

                        ecb.AddComponent<NonUniformScale>(entityInQueryIndex, newEntity);
                        ecb.SetComponent(entityInQueryIndex, newEntity, new NonUniformScale
                        {
                            Value = new float3(0, 0, 0)
                        });
                        
                        ecb.SetComponent(entityInQueryIndex, newEntity, new CellTag2
                        {
                            selfEntity = cellTag.selfEntity,
                            maxSize = 0.5f,
                            readyToSpawn = false,
                            maxSizeReached = false,
                            timeToGrowth = 20f,
                            actualTimeToGrowth = 0f,
                            deltaTime = cellTag.deltaTime
 
                        });
                        cellTag.readyToSpawn = false;
                        cellTag.actualTimeToGrowth = 0f;
                }
                else
                {
                    if (scale.Value.x >= cellTag.maxSize && !cellTag.maxSizeReached)
                    {
                        cellTag.readyToSpawn = true;
                        cellTag.maxSizeReached = true;
                        //masa 10
                        ecb.AddComponent(entityInQueryIndex, _, new PhysicsMass
                        {
                            Transform = new RigidTransform(quaternion.identity, translation.Value),
                            InverseInertia = new float3(0, 0, 0),
                            InverseMass = 10f
                        });
                        ecb.AddComponent(entityInQueryIndex,_ , new PhysicsCollider
                        {
                            Value = Unity.Physics.SphereCollider.Create(new SphereGeometry
                            {
                                Center = new float3(0, 0, 0),
                                Radius = scale.Value.x/2.2f
                            })
                        });
                    }
                    else if (scale.Value.x  >= cellTag.maxSize && cellTag.maxSizeReached)
                    {
                        cellTag.readyToSpawn = true;
                    }
                    else{
                        
                        if (!cellTag.maxSizeReached)
                        {
                            var deltatiempo = cellTag.deltaTime;
                            var tasaDeCrecimiento = 1f / cellTag.timeToGrowth;
                            var newScale = scale.Value + new float3(1, 1, 1) * tasaDeCrecimiento * deltaTime;

                            scale.Value = newScale;
                            ecb.AddComponent(entityInQueryIndex,_ , new PhysicsCollider{
                                Value = Unity.Physics.SphereCollider.Create(new SphereGeometry
                                {
                                    Center = new float3(0, 0, 0),
                                    Radius = scale.Value.x/2f
                                })
                            });
                            //calcular cuanto tamaño debo aumentar en base al tiempo de cellTag.timeToGrowth
                            

                        if ((translation.Value.y - newScale.y) < -0.1f)
                        {
                            var newTranslation = translation.Value + new float3(0, 1, 0) * 0.02f * deltaTime;
                            ecb.SetComponent(entityInQueryIndex, _, new Translation
                            {
                                Value = newTranslation
                            });
                        }
                        } 
                    }
                    cellTag.actualTimeToGrowth += deltaTime;

                }

            }).ScheduleParallel();
            _endSimulationEntityCommandBufferSystem2.AddJobHandleForProducer(this.Dependency);
        }
    }
}



