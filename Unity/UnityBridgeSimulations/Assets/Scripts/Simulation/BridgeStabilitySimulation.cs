using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Enums;
using UnityEngine;

namespace Simulation
{
    public class BridgeStablilitySimulation:BridgeSimulation
    {

        private Vector3 simulationSpace;
        public bool simulating;
        public GameObject blockPrevab;
        private List<PoolableBlock> blocks= new List<PoolableBlock>();
        private Transform droppedBlock;
        public Material mat1;
        public Material mat2;
        public bool dropBlockInBounds;


        public override async Task Simulate(Chromosome c, Vector3 space,CancellationToken token, int assignmentID,SimulationController simulationController,int delay)
        {
            await Task.Delay(delay, token);
            await BuildBridgeAsync(c, space,blockPrevab,blocks,simulationController, token);
            var result=await GetStability(token);
            EndSimulation();
            c.simulationResults.Add("stability",result);
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
            
            //Debug.Log(s);
        }
        public override void EndSimulation()
        {
            foreach (var block in blocks)
            {
                block.BlockObject.SetActive(false);
                PoolableBlock.blockPool.Push(block);
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
                    frameVelocity += block.BlockRigidbody.velocity.magnitude;
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