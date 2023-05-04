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
    public class BridgeDropSimulation:BridgeSimulation
    {

        private Vector3 simulationSpace;
        public bool simulating;
        public GameObject blockPrevab;
        private List<PoolableBlock> blocks= new List<PoolableBlock>();
        private Transform droppedBlock;
        public Material mat1;
        public Material mat2;
        public BoxCollider Collider;
        public bool dropBlockInBounds;
        public Camera camera;
        private RenderTexture renderTexture;
        public int resWidth;
        public int resHeight;
        private IImageSubmitter submitter;
        public override async Task Simulate(Chromosome c, Vector3 space,CancellationToken token,int assignmentID,SimulationController simulationController,int delay)
        {
            await Task.Delay(delay, token);

            submitter = new SubmitImageToWebApi(GameObject.FindObjectOfType<CoroutineRunner>());
            var score = 0;
            renderTexture = new RenderTexture(resWidth, resHeight, 24);
            camera.targetTexture = renderTexture;
            Texture2D fs1 = null;
            Texture2D fs2 = null;
            Texture2D fs3 = null;
            var center = Collider.center;
            center = new Vector3(center.x, center.y, (c.size[2] - 2f)/2f+0.5f);
            Collider.center = center;
            var size = Collider.size;
            size = new Vector3(size.x, size.y, (c.size[2] - 2) <=1 ? 1:(c.size[2] - 2));
            Collider.size = size;
            done = false;
            started = false;
            await BuildBridgeAsync(c, space,blockPrevab,blocks,simulationController, token);
            started = true;
            var s1 = TakeScreenshot(simulationController);
            await Task.Delay(5000); 
            Dictionary<Rigidbody,BlockData> BackupPos = new Dictionary<Rigidbody, BlockData>();
            foreach (var block in blocks)
            {
                
                BackupPos.Add(block.BlockRigidbody,new BlockData(block.BlockRigidbody));
                
            }
            for (var i = 1; i < 16; i *=2)
            {

                var result = false;
                var cubeSpawnPos = FindDropPos(space,c);
                var s2 = TakeScreenshot(simulationController);
                if (cubeSpawnPos!=null)
                {
                    var cubeSpawnPos2 = (Vector3) cubeSpawnPos;
                    result = await DropBlock(i, cubeSpawnPos2, token);
                    
                }
                var s3 = TakeScreenshot(simulationController);
                Destroy(droppedBlock.gameObject);
                await Task.Delay(50,token);

                
                if (result)
                {
                    foreach (var blockRB in BackupPos)
                    {


                        blockRB.Value.ApplyToRigidbody(blockRB.Key);
                    }
                    score = i;
                    fs1=(s1);
                    fs2=(s2);
                    fs3=(s3);
                }
                else
                {
                    

                    if (i == 1)
                    {
                        fs1=(s1);
                        fs2=(s2);
                        fs3=(s3);
                    }
                    i = 16;
                }
            }
            EndSimulation();
            c.simulationResults.Add("dropblock",score);
           /* var picture = new byte[fs1.Length + fs2.Length + fs3.Length];
            fs1.CopyTo(picture,0);
            fs2.CopyTo(picture,fs1.Length);
            fs3.CopyTo(picture,fs1.Length + fs2.Length);*/
            c.imageResults.Add("dropblock",await submitter.SubmitImageAsync(token,MergeScreenshot(new List<Texture2D>(){fs1,fs2,fs3}),assignmentID.ToString()));

            done = true;
            await Task.Delay(100); 
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

        public async Task<bool> DropBlock(int weight, Vector3 cubeSpawnPos,CancellationToken token)
        {
            droppedBlock = Instantiate(blockPrevab,cubeSpawnPos,Quaternion.identity).transform;
            droppedBlock.transform.localScale = new Vector3(0.7f, 0.7f, 0.7f);
            droppedBlock.GetComponent<Rigidbody>().mass = 0.1f + weight;
            droppedBlock.GetComponent<MeshRenderer>().material = mat1;
            droppedBlock.gameObject.layer = 7;
            await Task.Delay(5000,token);
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