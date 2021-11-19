using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AStar
{
    public class MapVisual : MonoBehaviour
    {

        [SerializeField] GameObject defaultCube;
        [SerializeField] Material FreeCube;
        [SerializeField] Material BlockCube;
        [SerializeField] Material StartCube;
        [SerializeField] Material FinishCube;
        [SerializeField] Material PathCube;

        private const string FREE = "Free";
        private const string BLOCK = "Block";
        private const string START = "Start";
        private const string FINISH = "Finish";
        private const string PATH = "Path";

        private const int MaxXcube = 50;
        private const int MaxYcube = 50;
        public string GetStart() {
            return START;
        }
        public string GetFinish()
        {
            return FINISH;
        }
        public int GetMaxX() {
            return MaxXcube;
        }
        public int GetMaxY()
        {
            return MaxYcube;
        }

        static public Dictionary<string, Material> MaterialDict = new Dictionary<string, Material>(5);
        public List<CubeStateInfo> mapCubeVisual = new List<CubeStateInfo>(MaxXcube * MaxYcube);
        public List<CubeStateInfo> StartStop = new List<CubeStateInfo>(2);

        public CubeStateInfo GetCubeVisual(int x, int y) {
            return mapCubeVisual[(x * MaxXcube) + y];
        }
        public void SetMAPColorPath(int x, int y) {
            GetCubeVisual(x, y).SetPATHcube(FREE, MaterialDict[PATH]);
        }
        public void SetMAPColorStart(int x, int y)
        {
            GetCubeVisual(x, y).SetSTARTcube(START, MaterialDict[START]);
        }
        public void SetMAPColorFinish(int x, int y)
        {
            GetCubeVisual(x, y).SetFINISHcube(FINISH, MaterialDict[FINISH]);
        }
        public void SetMAPColorBlock(int x, int y)
        {
            GetCubeVisual(x, y).SetBLOCKcube(BLOCK, MaterialDict[BLOCK]);
        }
        public void SetMAPColorFree(int x, int y)
        {
            GetCubeVisual(x, y).SetFREEcube(FREE, MaterialDict[FREE]);
        }
        private void StartFinishChanged(int x, int y)
        {
            switch (StartStop.Count)
            {
                case 0:
                    SetMAPColorStart(x, y);
                    StartStop.Add(GetCubeVisual(x, y));
                    break;
                case 1:
                    if (StartStop.Exists(a => a.status == START))
                    {
                        SetMAPColorFinish(x, y);
                        StartStop.Add(GetCubeVisual(x, y));
                    }
                    else {
                        SetMAPColorStart(x, y);
                        StartStop.Insert(0, GetCubeVisual(x, y));
                    }
                    break;
            }
        }
        private void SetStartFinish(int x, int y)
        {
            switch (GetCubeVisual(x, y).status)
            {
                case START:
                    SetMAPColorFree(x, y);
                    StartStop.Remove(GetCubeVisual(x, y));
                    break;
                case FINISH:
                    SetMAPColorFree(x, y);
                    StartStop.Remove(GetCubeVisual(x, y));
                    break;
                case FREE:
                    StartFinishChanged(x, y);
                    break;
            }
        }

        private void SetBlock(int x, int y)
        {
            switch (GetCubeVisual(x, y).status)
            {
                case BLOCK:
                    SetMAPColorFree(x, y);
                    break;
                case FREE:
                    SetMAPColorBlock(x, y);
                    break;
            }
        }

        public void SetMAPCubeState(int x, int y, int mouseButton)
        {
            switch (mouseButton)
            {
                case -1: // Left mouse button
                    SetBlock(x, y);
                    break;
                case -2: // Right mouse button
                    SetStartFinish(x, y);
                    break;
            }
        }

        public List<CubeStateInfo> CreateVisualTable()
        {
            Vector3 zeropos = transform.position;
            Quaternion zerorot = transform.rotation;
            int number = 0;

            for (int i = 0; i < MaxXcube; i++)
            {
                zeropos.z = 0;
                for (int j = 0; j < MaxYcube; j++)
                {
                    zeropos.z++;
                    GameObject cub = Instantiate(defaultCube, zeropos, zerorot);
                    CubeStateInfo cubeinfo = cub.AddComponent<CubeStateInfo>();

                    cubeinfo.SetCubeXY(i, j);
                    cubeinfo.SetFREEcube(FREE, MaterialDict[FREE]);

                    cub.transform.parent = this.transform;
                    mapCubeVisual.Add(cubeinfo);
                    number++;
                }
                zeropos.x++;
            }
            return mapCubeVisual;
        }

        private void Awake()
        {
            MaterialDict.Add(FREE, FreeCube);
            MaterialDict.Add(BLOCK, BlockCube);
            MaterialDict.Add(START, StartCube);
            MaterialDict.Add(FINISH, FinishCube);
            MaterialDict.Add(PATH, PathCube);

            CreateVisualTable();
        }

    }
}
