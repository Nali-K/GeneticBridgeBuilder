using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using UnityEngine;

namespace Simulation
{
    public class BridgeSim:Simulation
    {
        private Chromosome Chromosome;
        private Vector3 simulationSpace;
        public bool simulating;
        public GameObject blockPrevab;
        private List<GameObject> blocks= new List<GameObject>();
        private Transform droppedBlock;
        public Material mat1;
        public Material mat2;
        public bool dropBlockInBounds;
        public override void Simulate(Chromosome c, Vector3 space)
        {
            var dict = c.GetValuesAndPositions(new[] {-1, -1, -1});

            foreach (var VARIABLE in dict)
            {
                if (VARIABLE.Value > 0.9f&&VARIABLE.Value<1.1f)
                {
                    var position = new Vector3(space.x, space.y, space.z);
                    position.x +=  VARIABLE.Key[0];
                    position.y += VARIABLE.Key[1];
                    position.z += VARIABLE.Key[2]+ ((float)(VARIABLE.Key[1]%2f)/2f);


                    var obj = GameObject.Instantiate(blockPrevab, position, Quaternion.identity);
                    blocks.Add(obj);
                }
            }

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