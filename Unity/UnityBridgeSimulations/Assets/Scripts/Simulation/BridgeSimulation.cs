using System;
using System.Collections.Generic;
using System.Threading;
//using UnityEditor.VersionControl;
using UnityEngine;
using Task = System.Threading.Tasks.Task;

namespace Simulation
{
    public abstract class BridgeSimulation:Simulation
    {
        public EnvironmentController EnvironmentController;



        protected async Task BuildBridgeAsync(Chromosome c, Vector3 space, GameObject blockPrevab,  List<PoolableBlock> blocks,SimulationController controller,CancellationToken token)
        {
            while (controller.averagePerf > 0.10f || controller.spike > 0.20f||!controller.canSpawn)
            {
                await Task.Delay(300, token);
            }
            
            controller.canSpawn = false;
            EnvironmentController.Setup(c.size[0],c.size[1],c.size[2]);
            for (var i = 0; i < c.size[0]; i++)
            {
                for (var j = 0; j < c.size[1]; j++)
                {
                    for (var k = 0; k < c.size[2]; k++)
                    {
                        var position = new Vector3(i + space.x, j + space.y-Mathf.Ceil(c.size[1]/2f), k + space.z+j%2f/2f);
                        if (c.values[i, j, k] ==1)
                        {
                            if (PoolableBlock.blockPool.Count > 0)
                            {
                                var block = PoolableBlock.blockPool.Pop();
                                block.BlockData.ApplyToRigidbody(block.BlockRigidbody);
                                block.BlockRenderer.enabled = !controller.Hide;
                                block.BlockRigidbody.transform.position = position; 
                                block.BlockObject.SetActive(true);
                                blocks.Add(block);
                            }
                            else
                            {
                                var obj = GameObject.Instantiate(blockPrevab, position, Quaternion.identity);
                                var block = new PoolableBlock
                                {
                                    BlockObject = obj,
                                    BlockRigidbody = obj.GetComponent<Rigidbody>()
                                };
                                block.BlockData = new BlockData(block.BlockRigidbody);
                                block.BlockRenderer = obj.GetComponent<Renderer>();
                                block.BlockRenderer.enabled = !controller.Hide;
                                blocks.Add(block);
                            }

                            
                        }
                    }
                }
            }

        }
    }
}