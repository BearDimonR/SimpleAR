using System;
using SimpleAR.Examples.Chess.Scripts.Figures;
using SimpleAR.Examples.Chess.Scripts.Utils;
using UnityEngine;

namespace SimpleAR.Examples.Chess.Scripts
{
    public class VisualizationManager : MonoBehaviour
    {
        public GameObject pawnDarkPrefab;
        public GameObject pawnWhitePrefab;
        public GameObject rookDarkPrefab;
        public GameObject rookWhitePrefab;
        public GameObject knightDarkPrefab;
        public GameObject knightWhitePrefab;
        public GameObject queenDarkPrefab;
        public GameObject queenWhitePrefab;
        public GameObject kingDarkPrefab;
        public GameObject kingWhitePrefab;
        public GameObject bishopWhitePrefab;
        public GameObject bishopDarkPrefab;


        public GameObject selectorPrefab;
        public GameObject highlightPrefab;
        public Vector3 prefabScale;

        public GameObject boardInstance;

        [HideInInspector] public BoardManager boardManager;

        private GameObject _selectorPlane;

        public FloatCell CellSizes = new FloatCell(-1, -1);

        public void Awake()
        {
            prefabScale = boardInstance.transform.localScale;

            selectorPrefab.transform.localScale = prefabScale * 10f;
            highlightPrefab.transform.localScale = prefabScale * 10f;

            var size = boardInstance.GetComponent<BoxCollider>().size;
            CellSizes.x = size.x * prefabScale.x / (BoardManager.Size.x + 2);
            CellSizes.y = size.z * prefabScale.z / (BoardManager.Size.y + 2);

            _selectorPlane = Instantiate(selectorPrefab, Vector3.zero, Quaternion.identity);
            _selectorPlane.transform.parent = transform;
            _selectorPlane.SetActive(false);
        }

        public void Update()
        {
            var cell = boardManager.SelectedCell;
            if (cell.x < 0
                || cell.x >= BoardManager.Size.x
                || cell.y < 0
                || cell.y >= BoardManager.Size.y)
            {
                _selectorPlane.SetActive(false);
                return;
            }

            _selectorPlane.SetActive(true);
            _selectorPlane.transform.position = CellToPosition(cell.x, cell.y);
        }

        public void ClearHighlightCells()
        {
            foreach (var move in gameObject.GetComponentsInChildren<HighlightMove>()) Destroy(move.gameObject);
        }

        public void HighlightCells(bool[,] moves)
        {
            for (var i = 0; i < moves.GetLength(0); ++i)
            for (var j = 0; j < moves.GetLength(1); ++j)
            {
                if (!moves[i, j]) continue;
                var res = Instantiate(highlightPrefab, CellToPosition(i, j), Quaternion.identity);
                res.transform.parent = transform;
            }
        }

        public void MoveFigure(Figure selectedFigure, int x, int y)
        {
            selectedFigure.gameObject.transform.position = CellToPosition(x, y);
        }


        public GameObject InstantiateFigure(int x, int y, FigureType figure, bool colour)
        {
            GameObject prefab;
            switch (figure)
            {
                case FigureType.Bishop:
                    prefab = colour ? bishopWhitePrefab : bishopDarkPrefab;
                    break;
                case FigureType.King:
                    prefab = colour ? kingWhitePrefab : kingDarkPrefab;
                    break;
                case FigureType.Knight:
                    prefab = colour ? knightWhitePrefab : knightDarkPrefab;
                    break;
                case FigureType.Pawn:
                    prefab = colour ? pawnWhitePrefab : pawnDarkPrefab;
                    break;
                case FigureType.Queen:
                    prefab = colour ? queenWhitePrefab : queenDarkPrefab;
                    break;
                case FigureType.Rook:
                    prefab = colour ? rookWhitePrefab : rookDarkPrefab;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(figure), figure, "Wrong figure type!");
            }

            return InstantiateFigurePrefab(prefab, x, y);
        }

        private GameObject InstantiateFigurePrefab(GameObject prefab, int x, int y)
        {
            var res = Instantiate(prefab, CellToPosition(x, y), Quaternion.identity);
            res.transform.localScale = prefabScale;
            res.transform.parent = gameObject.transform;
            res.layer = LayerMask.NameToLayer("AR");
            return res;
        }

        public Vector3 CellToPosition(int x, int y)
        {
            if (x < 0 || y < 0) return Vector3.one;
            return new Vector3(x * CellSizes.x, 0.0001f, y * CellSizes.y) + gameObject.transform.position;
        }

        public IntCell PositionToCell(Vector3 pos)
        {
            var boardPos = pos - gameObject.transform.position;
            var cellX = (int) ((boardPos.x + CellSizes.x / 2) / CellSizes.x);
            var cellY = (int) ((boardPos.z + CellSizes.y / 2) / CellSizes.y);
            if (cellX < 0 || cellX >= 8 || cellY < 0 || cellY >= 8)
                return new IntCell(-1, -1);
            return new IntCell(cellX, cellY);
        }
    }
}