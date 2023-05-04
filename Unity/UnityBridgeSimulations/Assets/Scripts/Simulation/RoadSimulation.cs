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
    public class RoadSimulation:BridgeSimulation
    {

        private Vector3 simulationSpace;
        public bool simulating;
        public GameObject blockPrevab;
        private List<PoolableBlock> blocks= new List<PoolableBlock>();
        private Transform droppedBlock;
        public Material mat1;
        public Material mat2;
        public bool dropBlockInBounds;
        public Camera camera;
        private RenderTexture renderTexture;
        public int resWidth;
        public int resHeight;
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
            //camera.targetTexture = renderTexture;
         //   BackupPos = new Dictionary<Rigidbody, BlockData>();
         //   endPosses = new Dictionary<Rigidbody, BlockData>();
             done = false;
             started = false;
             await BuildBridgeAsync(c, space,blockPrevab,blocks,simulationController, token);
             started = true;
      
             renderTexture = new RenderTexture(resWidth, resHeight, 24);
             camera.targetTexture = renderTexture;
             Texture2D fs1 = TakeScreenshot(simulationController);

            var furthest = 0f;
            var furthest2 = 0f;
            var dropBlockPositions = new List<Vector3>();
            await Task.Delay(7000, token);

            for (var i = 0; i < c.size[0]; i++)
            {
                var roadScore = 0f;
                var roadPass = true;
                for (var j = 0; j < c.size[2]; j++)
                {
                    Vector3 rayPos = new Vector3(space.x, space.y, space.z);
                    var prevY = -0.50f;
                    rayPos.y += Mathf.Floor(c.size[1]/2f)+1;
                    rayPos.z = space.z+  j;
                    rayPos.x = space.x+  i;
                    //Debug.DrawRay(rayPos, Vector3.down * (Mathf.Floor(c.size[1]/2f)+1.5f), Color.blue, 5);
                    if (Physics.Raycast(rayPos, Vector3.down, out var hit, Mathf.Floor(c.size[1]/2f)+1.5f))
                    {
                        var dif = Mathf.Abs(hit.point.y - prevY);
                        if (dif < 1.5f)
                        {
                            dropBlockPositions.Add(hit.point+Vector3.up);
                            roadScore -= dif;
                            prevY = hit.point.y;
                        }
                        else
                        {
                            if (j > furthest)
                            {
                                furthest = j;
                                furthest2 = roadScore;
                            }
                            roadPass = false;
                            break;
                        }


                    }
                    else
                    {
                        if (j > furthest)
                        {
                            furthest = j;
                            furthest2 = roadScore;
                        }
                        roadPass = false;
                        break;
                    }


                }
                if (roadPass)
                {
                    score += 1;
                    score2 += ( 1.5f) + roadScore/c.size[2];
                }
            }

            if (score < 1)
            {
                score = furthest / c.size[2];
                score2 =score*1.5f+ furthest2 / c.size[2];
            }
            /*foreach (var blockRB in blocksRB)
                {
                    score -= blockRB.velocity.magnitude;
                }


            foreach (var blockRB in blocksRB)
            {

                endPosses.Add(blockRB, new BlockData(blockRB));
                BackupPos[blockRB].ApplyToRigidbody(blockRB);
            }
            for (var i = 0; i < 100; i++)
            {
                await Task.Delay(100);
                foreach (var blockRB in blocksRB)
                {
                    score -= blockRB.velocity.magnitude;
                }

            }*/
            
            var blockDataBeforeDrop = new Dictionary<PoolableBlock, BlockData>();
            
            foreach (var VARIABLE in blocks)
            {

                var bd = new BlockData(VARIABLE.BlockRigidbody);
                try
                {
                    blockDataBeforeDrop.Add(VARIABLE,bd);
                }
                catch (Exception e)
                {
                    Debug.Log(e);
                    throw;
                }


            }

            
            var droppedBlocks = new List<Transform>();
            foreach (var dropBlockPosition in dropBlockPositions)
            {
               var droppedBlock= Instantiate(blockPrevab,dropBlockPosition,Quaternion.identity).transform;
                droppedBlock.transform.localScale = new Vector3(0.7f, 0.7f, 0.7f);
                droppedBlock.GetComponent<Rigidbody>().mass = 4;
                droppedBlock.GetComponent<MeshRenderer>().material = mat1;
                droppedBlock.gameObject.layer = 7;
                droppedBlocks.Add(droppedBlock);


            }

            await Task.Delay(5000);

            var score3 = 0f;
            foreach (var VARIABLE in blockDataBeforeDrop)
            {
                score3 += Mathf.Pow(VARIABLE.Value.Compare(VARIABLE.Key.BlockRigidbody), 2);
            }
            if (dropBlockPositions.Count > 0)
            {
                score3 /= dropBlockPositions.Count;
            }
            else
            {
                score3 = 50;
            }

            Texture2D fs2 = TakeScreenshot(simulationController);
           foreach (var VARIABLE in droppedBlocks)
            {
                Destroy(VARIABLE.gameObject);
                
            }
            droppedBlocks.Clear();
            blockDataBeforeDrop.Clear();
            dropBlockPositions.Clear();

            EndSimulation();
                
            c.simulationResults.Add("roads",score);
            c.simulationResults.Add("flatness",score2);
            c.simulationResults.Add("dropblock",score3);

            c.imageResults.Add("dropblock",await submitter.SubmitImageAsync(token,MergeScreenshot(new List<Texture2D>(){fs1,fs2}),assignmentID.ToString()));
            await Task.Delay(100);

            done = true;
            simulating = true;
        }


        private Texture2D TakeScreenshot(SimulationController controller)
        {
            if (controller.Hide)
            {
                foreach (var block in blocks)
                {
                    block.BlockRenderer.enabled = true;
                }
            }
            Texture2D screenShot = new Texture2D(resWidth, resHeight, TextureFormat.RGB24, false);
            camera.Render();
            RenderTexture.active = renderTexture;
            screenShot.ReadPixels(new Rect(0, 0, resWidth, resHeight), 0, 0);
            if (controller.Hide)
            {
                foreach (var block in blocks)
                {
                    block.BlockRenderer.enabled = false;
                }
            }
            return screenShot;
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
                //Debug.DrawRay(rayPos, Vector3.down * c.size[1] * 2f, Color.blue, 3);
                if (Physics.Raycast(rayPos, Vector3.down, out var hit, c.size[1] * 1.5f))
                {
                    
                    var cubeSpawnPos = hit.point + Vector3.up;
                    return cubeSpawnPos;

                }
            }

            return null;
        }



    }
}