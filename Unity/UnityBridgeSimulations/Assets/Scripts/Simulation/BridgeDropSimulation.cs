using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Enums;
using UnityEngine;

namespace Simulation
{
    public class BridgeDropSimulation:Simulation
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
            Vector3 cubeSpawnPos = new Vector3(space.x, space.y, space.z);
            cubeSpawnPos.x += c.size[0]/2f;
            cubeSpawnPos.y += c.size[1]/2f;
            cubeSpawnPos.z += c.size[2]/2f;
            var score = 0;
            for (var i = 0; i < 16; i *= 2)
            {
                BuildBridge(c, space);
                var result=await DropBlock(i,cubeSpawnPos,token);
                EndSimulation();
                
                if (!result)
                {
                    i = 16;
                }
                else
                {
                    score = i;
                }
            }
            c.simulationResults.Add(Simulations.Dropblock,score);
            
            simulating = true;
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
            if (droppedBlock!=null)
                Destroy(droppedBlock.gameObject);

        }

        public async Task<bool> DropBlock(int weight, Vector3 cubeSpawnPos,CancellationToken token)
        {
            
            droppedBlock = Instantiate(blockPrevab,cubeSpawnPos,Quaternion.identity).transform;
            droppedBlock.GetComponent<Rigidbody>().mass = 0.1f + weight;
            droppedBlock.GetComponent<MeshRenderer>().material = mat1;
            droppedBlock.gameObject.layer = 7;
            await Task.Delay(8300,token);
            var result = dropBlockInBounds;
            return result;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (droppedBlock == null) return;
            if (other.gameObject != droppedBlock.gameObject) return;
            droppedBlock.GetComponent<MeshRenderer>().material = mat1;
            dropBlockInBounds = true;

        }

        private void OnTriggerExit(Collider other)
        {
            if (droppedBlock == null) return;
            if (other.gameObject != droppedBlock.gameObject) return;
            droppedBlock.GetComponent<MeshRenderer>().material = mat2;
            dropBlockInBounds = false;
        }




    }
}