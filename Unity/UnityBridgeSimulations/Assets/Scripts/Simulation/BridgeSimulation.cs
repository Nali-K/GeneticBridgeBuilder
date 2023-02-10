using System;
using System.Collections.Generic;
using UnityEngine;

namespace Simulation
{
    public abstract class BridgeSimulation:Simulation
    {
        public EnvironmentController EnvironmentController;



        protected void BuildBridge(Chromosome c, Vector3 space, GameObject blockPrevab,  List<GameObject> blocks)
        {
            EnvironmentController.Setup(c.size[0],c.size[1],c.size[2]);
            for (var i = 0; i < c.size[0]; i++)
            {
                for (var j = 0; j < c.size[1]; j++)
                {
                    for (var k = 0; k < c.size[2]; k++)
                    {
                        var position = new Vector3(i + space.x, j + space.y, k + space.z);
                        if (c.values[i, j, k] == 1)
                        {
                            var obj = GameObject.Instantiate(blockPrevab, position, Quaternion.identity);
                            
                            blocks.Add(obj);
                        }
                    }
                }
            }
        }
    }
}