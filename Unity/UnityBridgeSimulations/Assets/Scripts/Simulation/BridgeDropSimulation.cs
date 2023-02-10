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
        private List<GameObject> blocks= new List<GameObject>();
        private Transform droppedBlock;
        public Material mat1;
        public Material mat2;
        public bool dropBlockInBounds;
        public Camera camera;
        private RenderTexture renderTexture;
        public int resWidth;
        public int resHeight;
        private IImageSubmitter submitter;
        public override async Task Simulate(Chromosome c, Vector3 space,CancellationToken token)
        {
            await Task.Delay(50);
            Vector3 rayPos = new Vector3(space.x, space.y, space.z);
            rayPos.x += c.size[0] / 2f - 0.5f;
            rayPos.y += c.size[1]*1.5f;
            rayPos.z += c.size[2]/2f;
            submitter = new SubmitImageToWebApi(GameObject.FindObjectOfType<CoroutineRunner>());
            var score = 0;
            renderTexture = new RenderTexture(resWidth, resHeight, 24);
            camera.targetTexture = renderTexture;
            var fs1 = new byte[] { };
            var fs2 = new byte[] { };
            var fs3 = new byte[] { };


            for (var i = 1; i < 16; i *=2)
            {
                BuildBridge(c, space,blockPrevab,blocks);
                var s1 = TakeScreenshot();
                await Task.Delay(10000);
                var result = false;
                Debug.DrawRay(rayPos, Vector3.down*c.size[1]*2f, Color.blue, 3);
                var s2 = TakeScreenshot();
                if (Physics.Raycast(rayPos, Vector3.down, out var hit, c.size[1]*2f))
                {
                    var cubeSpawnPos = hit.point + Vector3.up;
                    result = await DropBlock(i, cubeSpawnPos, token);
                    
                }
                var s3 = TakeScreenshot();

                EndSimulation();
                
                if (result)
                {
                    score = i;
                    fs1 = s1;
                    fs2 = s2;
                    fs3 = s3;
                }
                else
                {
                    

                    if (i == 1)
                    {
                        fs1 = s1;
                        fs2 = s2;
                        fs3 = s3;
                    }
                    i = 16;
                }
            }
            c.simulationResults.Add("dropblock",score);

            c.imageResults.Add("initial",await submitter.SubmitImageAsync(token,fs1));
            c.imageResults.Add("preblock",await submitter.SubmitImageAsync(token,fs2));
            c.imageResults.Add("block",await submitter.SubmitImageAsync(token,fs3));


            simulating = true;
        }


        private byte[] TakeScreenshot()
        {
            Texture2D screenShot = new Texture2D(resWidth, resHeight, TextureFormat.RGB24, false);
            camera.Render();
            RenderTexture.active = renderTexture;
            screenShot.ReadPixels(new Rect(0, 0, resWidth, resHeight), 0, 0);
            RenderTexture.active = null;
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
            await Task.Delay(10000,token);
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