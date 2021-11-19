using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace AStar
{
    public class StartTest : MonoBehaviour
    {
        [SerializeField] private GameObject mapVisual;
        private MapVisual mv;
        private Pathfinder finderPath;
        private List<CubeStateInfo> newbestPath = new List<CubeStateInfo>();

        private void Start()
        {
            GameObject mapCubeVisual = Instantiate(mapVisual, transform.position, transform.rotation);
            mv = mapCubeVisual.GetComponent<MapVisual>();
            finderPath = new Pathfinder(mv.mapCubeVisual, mv.GetMaxX(), mv.GetMaxY());
            mv.GetComponent<ClickFinder>().OnMapVisualClickAction += OnMapClickChanged;
        }

        public void OnMapClickChanged(int x, int y, int mouseButton) => OnFinderPath(x, y, mouseButton);

        private void ShowBestPath() {
            if (newbestPath.Count > 0)
            {
                for (int i = 1; i < newbestPath.Count - 1; i++)
                {
                    mv.SetMAPColorPath(newbestPath[i].x, newbestPath[i].y);
                }
            }
        }

        private void ClearOldBestPath(){
            if (newbestPath.Count > 0)
            {
                for (int i = 1; i < newbestPath.Count - 1; i++)
                {
                    mv.SetMAPColorFree(newbestPath[i].x, newbestPath[i].y);
                }
                newbestPath.Clear();
            }
        }

        private void OnFinderPath(int x, int y, int mouseButton) {
            ClearOldBestPath();
            mv.SetMAPCubeState(x, y, mouseButton);

            if (mv.StartStop.Count == 2) {

                CubeStateInfo start = mv.StartStop.Find(a => a.status == mv.GetStart());
                CubeStateInfo finish = mv.StartStop.Find(a => a.status == mv.GetFinish());

                newbestPath = finderPath.FindPath(start, finish);
                ShowBestPath();
            }
        }
    }
}