using GameOfLifeLib;
using Microsoft.Xna.Framework;
using System;

namespace GameOfLife
{
    public enum AvailableActions
    {
        MoveUp,
        MoveDown,
        MoveLeft,
        MoveRight,
        IncreaseSpeed,
        DecreaseSpeed,
        Pause,
        ClearBoard,
        FlipValueAtPos,
        EnterEditMode,
        ExitEditMode,
        Save,
        Load
    }

    public partial class GameState
    {
        private readonly IGameOfLife<bool> _state;
        public (int x, int y) CurrentSelection { get; private set; }
        public (int width, int height) Size { get; private set; }
        public bool Paused { get; private set; } = true;
        public bool EditMode { get; private set; } = false;
        public float UpdateRate { get; private set; }

        public GameState((int w, int h) size, float updateRate) : this(size.w, size.h, updateRate)
        {
        }

        public GameState(int width, int height, float updateRate)
        {
            _state = GameOfLifeFactory.CreateGameOfLife((uint)width, (uint)height);
            Size = (width, height);
            CurrentSelection = (width / 2, height / 2);
            UpdateRate = updateRate;
        }

        private double latestUpdate = 0;
        public void Update(GameTime gameTime)
        {
            // Check if enough time has elapsed
            latestUpdate += gameTime.ElapsedGameTime.TotalSeconds;
            if (!Paused && latestUpdate > UpdateRate)
            {
                latestUpdate = 0;
                _state.Step();
            }
        }

        public void HandleAction(AvailableActions action)
        {
            HandleAction(action, "");
        }

        public void HandleAction(AvailableActions action, string args)
        {
            switch (action)
            {
                case AvailableActions.MoveUp:
                case AvailableActions.MoveDown:
                case AvailableActions.MoveRight:
                case AvailableActions.MoveLeft:
                    CurrentSelection = GetNewSelection(action);
                    break;
                case AvailableActions.IncreaseSpeed:
                    UpdateRate /= 2;
                    break;
                case AvailableActions.DecreaseSpeed:
                    UpdateRate *= 2;
                    break;
                case AvailableActions.Pause:
                    Paused = !Paused;
                    break;
                case AvailableActions.ClearBoard:
                    _state.Clear();
                    break;
                case AvailableActions.FlipValueAtPos:
                    bool current = _state.Grid.GetAt((uint)CurrentSelection.x, (uint)CurrentSelection.y);
                    _state.Grid.SetAt((uint)CurrentSelection.x, (uint)CurrentSelection.y, !current);
                    break;
                case AvailableActions.EnterEditMode:
                    EditMode = true;
                    break;
                case AvailableActions.ExitEditMode:
                    EditMode = true;
                    break;
                case AvailableActions.Save:
                case AvailableActions.Load:
                default:
                    throw new NotImplementedException("Action " + action + "not implemented");
            }
        }

        public delegate void ActionForACell(bool cell, int x, int y);
        public void DoActionForACell(ActionForACell action)
        {
            ICellGridIterator<bool> enumerator = _state.Grid.GetEnumerator();
            while (enumerator.MoveNext())
            {
                action(enumerator.Current, (int)enumerator.X, (int)enumerator.Y);
            };
        }



        private (int, int) GetNewSelection(AvailableActions action) => action switch
        {
            AvailableActions.MoveUp when CurrentSelection.y == 0 => (CurrentSelection.x, Size.height - 1),
            AvailableActions.MoveUp => (CurrentSelection.x, CurrentSelection.y - 1),

            AvailableActions.MoveDown when CurrentSelection.y == Size.height - 1 => (CurrentSelection.x, 0),
            AvailableActions.MoveDown => (CurrentSelection.x, CurrentSelection.y + 1),

            AvailableActions.MoveRight when CurrentSelection.x == Size.width - 1 => (0, CurrentSelection.y),
            AvailableActions.MoveRight => (CurrentSelection.x + 1, CurrentSelection.y),

            AvailableActions.MoveLeft when CurrentSelection.x == 0 => (Size.width - 1, CurrentSelection.y),
            AvailableActions.MoveLeft => (CurrentSelection.x - 1, CurrentSelection.y),
            _ => CurrentSelection
        };
    }
}
