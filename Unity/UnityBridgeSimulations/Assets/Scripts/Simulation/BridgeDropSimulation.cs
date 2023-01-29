using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Enums;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Simulation
{
    public class BridgeDropSimulation:BridgeSimulation
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
            await Task.Delay(100);
            Vector3 rayPos = new Vector3(space.x, space.y, space.z);
            rayPos.x += c.size[0]/2f;
            rayPos.y += c.size[1]*1.5f;
            rayPos.z += c.size[2]/2f;
            var score = 0;
            for (var i = 1; i < 16; i *= 2)
            {
                BuildBridge(c, space,blockPrevab,blocks);
                await Task.Delay(2000);
                var result = false;
                Debug.DrawRay(rayPos, Vector3.down*c.size[1]*2f, Color.blue, 3);
                if (Physics.Raycast(rayPos, Vector3.down, out var hit, c.size[1]*2f))
                {
                    var cubeSpawnPos = hit.point + Vector3.up;
                    result = await DropBlock(i, cubeSpawnPos, token);
                }
                

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
            c.simulationResults.Add("dropblock",score);
            
            simulating = true;
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