using System.Collections.Generic;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
using UnityEngine;

namespace SimpleAR.Examples.Chess.Scripts
{
    public class PlayerAgent : Agent
    {
        public MlBoardManager mlBoardManager;
        public bool isWhite;

        public float[] Figures;
        public List<float[]> Moves;


        public override void OnEpisodeBegin()
        {
            Figures = new float[64];
            Moves = new List<float[]>(16);
            for (int i = 0; i < 16; i++)
            {
                Moves.Add(new float[64]);
            }
        }

        // observations

        // 8*8 location - 64 array float
        // figures
        // 0.1
        // 0.2...

        // 8*8 moves - 64 array float
        // * 16 - for each figure
        // * 2 - for each player - drop for now
        // 0 - illegal move
        // 1 - legal move

        public override void CollectObservations(VectorSensor sensor)
        {
            //mlBoardManager.UpdateFigures(ref Figures);
            //mlBoardManager.UpdateMoves(ref Moves);
            // board as a list of observations
            sensor.AddObservation(Figures);
            foreach (var move in Moves)
            {
                sensor.AddObservation(move);
            }
        }

        // user input
        // get figure - selected figure
        // cell to move in

        public override void Heuristic(in ActionBuffers actionsOut)
        {
            var actions = actionsOut.DiscreteActions;
            var cell = mlBoardManager.SelectedCell;
            if (!Input.GetMouseButtonDown(0) || cell.X < 0
                || cell.X >= 8
                || cell.Y < 0
                || cell.Y >= 8)
            {
                actions[0] = -1;
                actions[1] = -1;
                actions[2] = -1;
                actions[2] = -1;
            }
            else
            {
                var figure = mlBoardManager.SelectedFigure;

                if (figure != null && (figure.Cell.X != cell.X || figure.Cell.Y != cell.Y))
                {
                    actions[0] = figure.Cell.X;
                    actions[1] = figure.Cell.Y;
                    actions[2] = cell.X;
                    actions[3] = cell.Y;
                }
                else
                {
                    mlBoardManager.SelectFigure(cell.X, cell.Y);
                    actions[0] = -1;
                    actions[1] = -1;
                    actions[2] = cell.X;
                    actions[3] = cell.Y;
                }
            }
        }


// input
        
        // Discrete Action
        // number - 1 - 16 - figure to move
        // number - 0-7 - cellX
        // number - 0-7 - cellY
        
        public override void OnActionReceived(ActionBuffers actions)
        {
            var a = actions.DiscreteActions;
            var fX = a[0];
            var fY = a[1];
            var cX = a[2];
            var cY = a[3];
            if (Check(fX) || Check(fY) || Check(cX) || Check(cY))
            {
                AddReward(-0.1f);
                return;
            }
            
            AddReward(0.1f);
            mlBoardManager.SelectedFigure = mlBoardManager.FigurePositions[fX, fY];
            if(mlBoardManager.SelectedFigure == null)
                AddReward(-0.5f);
            if(!mlBoardManager.MoveFigure(cX, cY))
                AddReward(-0.5f);
        }


        private bool Check(int v)
        {
            return v < 0 || v >= 8;
        }
    }
}