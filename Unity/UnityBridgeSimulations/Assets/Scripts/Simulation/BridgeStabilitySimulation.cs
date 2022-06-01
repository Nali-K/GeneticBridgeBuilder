using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Enums;
using UnityEngine;

namespace Simulation
{
    public class BridgeStablilitySimulation:Simulation
    {

        private Vector3 simulationSpace;
        public bool simulating;
        public GameObject blockPrevab;
        private List<GameObject> blocks= new List<GameObject>();
        private Transform droppedBlock;
        public Material mat1;
        public Material mat2;
        public bool dropBlockInBounds;


        public override async Task Simulate(Chromosome c, Vector3 space,CancellationToken token)
        {

            BuildBridge(c, space);
            var result=await GetStability(token);
            EndSimulation();
            c.simulationResults.Add(Simulations.Stability,result);
        }

        private void BuildBridge(Chromosome c, Vector3 space)
        {
            for (var i = 0; i < c.size[0]; i++)
            {
                for (var j = 0; j < c.size[1]; j++)
                {
                    for (var k = 0; k < c.size[2]; k++)
                    {
                        var position = new Vector3(i + space.x, j + space.y, k + space.z);
                        var obj = GameObject.Instantiate(blockPrevab, position, Quaternion.identity);
                        blocks.Add(obj);
                    }
                }
            }
        }
        private void printdict(Dictionary<int[],float> a)
        {
            var s = "";
            foreach (var VARIABLE in a)
            {
                s += "[";
                foreach (var keyint in VARIABLE.Key)
                {
                    s += keyint;
                    s += ",";
                }
                s += "] ";
                s += VARIABLE.Value;
                s += ",";
            }
            
            Debug.Log(s);
        }
        public override void EndSimulation()
        {
            foreach (var block in blocks)
            {
                Destroy(block);
            }

        }




        public async Task<float> GetStability(CancellationToken token)
        {
            var checks = 80;
            var totalVelocity = 0f;
            for (var i=0;i<checks;i++)
            {
                var frameVelocity = 0f;
                foreach (var block in blocks)
                {
                    frameVelocity += block.GetComponent<Rigidbody>().velocity.magnitude;
                }

                frameVelocity /= blocks.Count;
                totalVelocity += frameVelocity;
                await Task.Delay(100,token);

            }

            totalVelocity /= checks;
            return totalVelocity;
        }
    }
}