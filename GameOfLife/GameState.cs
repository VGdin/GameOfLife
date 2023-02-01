using GameOfLifeLib;
using Microsoft.Xna.Framework;
using System;

namespace GameOfLife
{

    public enum SelectionDirection
    {
        Up, Down, Left, Right
    }
    public partial class GameState
    {
        public (int x, int y) CurrentSelection { get; private set; }
        public bool Paused { get; set; } = true;
        public bool EditMode { get; private set; } = false;
        public float UpdateRate { get; private set; }

        private readonly IGameOfLife<bool> _state;
        private (int width, int height) Size;

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

        public ICellGridIEnumerator<bool> GetCellEnumerator()
        {
            return _state.Grid.GetEnumerator();
        }

        public void Reset()
        {
            _state.Clear();
        }
        public void FlipAtSelection()
        {
            bool current = _state.Grid.GetAt((uint)CurrentSelection.x, (uint)CurrentSelection.y);
            _state.Grid.SetAt((uint)CurrentSelection.x, (uint)CurrentSelection.y, !current);
        }

        public void increaseUpdateRate()
        {
            UpdateRate /= 1.5f;
        }
        public void decreaseUpdateRate()
        {
            UpdateRate *= 1.5f;
        }

        public void MoveCurrentSelection(SelectionDirection direction)
        {
            CurrentSelection = GetNewSelection(direction);
        }

        private (int, int) GetNewSelection(SelectionDirection direction) => direction switch
        {
            SelectionDirection.Up when CurrentSelection.y == 0 => (CurrentSelection.x, Size.height - 1),
            SelectionDirection.Up => (CurrentSelection.x, CurrentSelection.y - 1),

            SelectionDirection.Down when CurrentSelection.y == Size.height - 1 => (CurrentSelection.x, 0),
            SelectionDirection.Down => (CurrentSelection.x, CurrentSelection.y + 1),

            SelectionDirection.Right when CurrentSelection.x == Size.width - 1 => (0, CurrentSelection.y),
            SelectionDirection.Right => (CurrentSelection.x + 1, CurrentSelection.y),

            SelectionDirection.Left when CurrentSelection.x == 0 => (Size.width - 1, CurrentSelection.y),
            SelectionDirection.Left => (CurrentSelection.x - 1, CurrentSelection.y),
            _ => CurrentSelection
        };
    }
}
