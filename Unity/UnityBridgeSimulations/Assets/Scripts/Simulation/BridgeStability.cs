using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Controller;
using Controllers;
using Enums;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Simulation
{
    public class BridgeStability:BridgeSimulation
    {

        private Vector3 simulationSpace;
        public bool simulating;
        public GameObject blockPrevab;
        private List<PoolableBlock> blocks= new List<PoolableBlock>();
        private Transform droppedBlock;
        public Material mat1;
        public Material mat2;
        public bool dropBlockInBounds;
        //public Camera camera;
        //private RenderTexture renderTexture;
        //public int resWidth;
        //public int resHeight;
        private IImageSubmitter submitter;
        private Dictionary<Rigidbody,BlockData> BackupPos;
        private Dictionary<Rigidbody, BlockData> endPosses;
        public override async Task Simulate(Chromosome c, Vector3 space,CancellationToken token, int assignmentID,SimulationController simulationController,int delay)
        {
            await Task.Delay(delay, token);

            submitter = new SubmitImageToWebApi(GameObject.FindObjectOfType<CoroutineRunner>());
            var score = 0f;
            var score2 = 0f;
            //renderTexture = new RenderTexture(resWidth, resHeight, 24);
           // camera.targetTexture = renderTexture;
            BackupPos = new Dictionary<Rigidbody, BlockData>();
            endPosses = new Dictionary<Rigidbody, BlockData>();
            var blocksRB = new List<Rigidbody>();
            done = false;
            started = false;
            await BuildBridgeAsync(c, space,blockPrevab,blocks,simulationController, token);
            started = true;
            await Task.Delay(10);
            foreach (var block in blocks)
            {

                blocksRB.Add(block.BlockRigidbody);
                BackupPos.Add(block.BlockRigidbody,new BlockData(block.BlockRigidbody));
                
            }

            for (var i = 0; i < 50; i++)
            {
                await Task.Delay(100);
                foreach (var blockRB in blocksRB)
                {
                    score -= blockRB.velocity.magnitude;
                }

            }
            foreach (var blockRB in blocksRB)
            {

                endPosses.Add(blockRB, new BlockData(blockRB));
                BackupPos[blockRB].ApplyToRigidbody(blockRB);
            }
            for (var i = 0; i < 50; i++)
            {
                await Task.Delay(100);
                foreach (var blockRB in blocksRB)
                {
                    score -= blockRB.velocity.magnitude;
                }

            }
            foreach (var endPos in endPosses)
            {
                score2 -= endPos.Value.Compare(endPos.Key);
            }
            c.simulationResults.Add("stability_velocity",score/blocks.Count/50f);
            c.simulationResults.Add("stability_difference",score2/blocks.Count);

            EndSimulation();
            blocksRB.Clear();
            BackupPos.Clear();
            endPosses.Clear();
            
            done = true;   
            await Task.Delay(100); 
        }


        private byte[] MergeScreenshot(List<Texture2D> screenshots)
        {
            var w = 0;
            var h = 0;
            foreach (var VARIABLE in screenshots)
            {
                w += VARIABLE.width;
                if (VARIABLE.height > h) h = VARIABLE.height;
            }
            Texture2D screenShot = new Texture2D(w, h, TextureFormat.RGB24, false);
            var cw = 0;
            foreach (var s in screenshots)
            {
                
                screenShot.SetPixels(cw,0,s.width,s.height,s.GetPixels(0,0,s.width,s.height));
                cw += s.width;
            }

            byte[] bytes = screenShot.EncodeToPNG();
            return bytes;
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
                block.BlockObject.SetActive(false);
                PoolableBlock.blockPool.Push(block);
            }
            blocks.Clear();
            if (droppedBlock!=null)
                Destroy(droppedBlock.gameObject);

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

        private Vector3? FindDropPos(Vector3 space,Chromosome c)
        {
            Vector3 rayPos = new Vector3(space.x, space.y, space.z);
            
            rayPos.y += c.size[1]*1.5f;
            rayPos.z += c.size[2]/2f;


            for (var i = 0; i < c.size[0]; i++)
            {
                rayPos.x = space.x+  i;
                Debug.DrawRay(rayPos, Vector3.down * c.size[1] * 2f, Color.blue, 3);
                if (Physics.Raycast(rayPos, Vector3.down, out var hit, c.size[1] * 2f))
                {
                    
                    var cubeSpawnPos = hit.point + Vector3.up;
                    return cubeSpawnPos;

                }
            }

            return null;
        }



    }
}