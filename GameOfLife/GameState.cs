using GameOfLifeLib;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System.Diagnostics;

namespace GameOfLife
{
    public enum SelectionDirection
    {
        Up, Down, Left, Right
    }

    /// <summary>
    /// Class to keep track of the game and data relating to it, current selection & updaterate etc.
    /// </summary>
    internal sealed class GameState
    {
        public (int x, int y) CurrentSelection { get; private set; } = (Config.Instance.GameSize.widht / 2, Config.Instance.GameSize.height / 2);
        public bool Paused { get; set; } = true;
        public float UpdateRate { get; private set; }
        public float UpdateCap { get; private set; } = Config.Instance.DefaultUpdateRate;

        private readonly IGameOfLife _state;

        public GameState()
        {
            _state = GameFactory.CreateGameOfLifeOptimized((uint)Config.Instance.GameSize.widht, (uint)Config.Instance.GameSize.height);
            CurrentSelection = ((int)_state.Grid.Width / 2, (int)_state.Grid.Height / 2);
        }

        private double latestUpdate = 0;
        private int _latest_index = 0;
        private readonly float[] _latest_times = new float[10];
        private readonly Stopwatch _stopwatch = new();
        public void Update(GameTime gameTime)
        {
            // Check if enough time has elapsed
            latestUpdate += gameTime.ElapsedGameTime.TotalSeconds;
            if (!Paused && latestUpdate > UpdateCap)
            {
                latestUpdate = 0;

                // Time a step
                _stopwatch.Restart();
                _state.Step();
                _stopwatch.Stop();


                /// Calc average update speed
                UpdateRate -= _latest_times[_latest_index] / _latest_times.Length;
                _latest_times[_latest_index] = _stopwatch.ElapsedMilliseconds / 1000f;
                UpdateRate += _latest_times[_latest_index] / _latest_times.Length;
                if (++_latest_index >= _latest_times.Length)
                {
                    _latest_index = 0;
                }
            }
        }

        public void Reset()
        {
            _state.Clear();
        }

        public void FlipAtSelection()
        {
            bool current = _state.Grid.GetAt((uint)CurrentSelection.x, (uint)CurrentSelection.y);
            if (current)
            {
                _state.Grid.ClearAt((uint)CurrentSelection.x, (uint)CurrentSelection.y);
            }
            else
            {
                _state.Grid.SetAt((uint)CurrentSelection.x, (uint)CurrentSelection.y);
            }
        }

        public void IncreaseUpdateRate()
        {
            UpdateCap /= 1.5f;
        }

        public void DecreaseUpdateRate()
        {
            UpdateCap *= 1.5f;
        }

        public ISet<(uint x, uint y)> getAllActiveCells()
        {
            return _state.AllActiveCells;
        }

        public void MoveCurrentSelectionTo((int x, int y) newSelection)
        {
            if (WithinBounds(newSelection))
            {
                CurrentSelection = newSelection;
            }
        }

        public void MoveCurrentSelection(SelectionDirection direction)
        {
            CurrentSelection = GetNewSelection(direction);
        }

        public void LoadFile(string path)
        {
            IGameFileReader gameFileReader = FileHandlerFactory.CreateFileReader(path);
            _state.Load(gameFileReader, (uint)CurrentSelection.x, (uint)CurrentSelection.y);
        }

        private (int, int) GetNewSelection(SelectionDirection direction) => direction switch
        {
            SelectionDirection.Up when CurrentSelection.y == 0 => (CurrentSelection.x, (int)_state.Grid.Height - 1),
            SelectionDirection.Up => (CurrentSelection.x, CurrentSelection.y - 1),

            SelectionDirection.Down when CurrentSelection.y == _state.Grid.Height - 1 => (CurrentSelection.x, 0),
            SelectionDirection.Down => (CurrentSelection.x, CurrentSelection.y + 1),

            SelectionDirection.Right when CurrentSelection.x == _state.Grid.Width - 1 => (0, CurrentSelection.y),
            SelectionDirection.Right => (CurrentSelection.x + 1, CurrentSelection.y),

            SelectionDirection.Left when CurrentSelection.x == 0 => ((int)_state.Grid.Width - 1, CurrentSelection.y),
            SelectionDirection.Left => (CurrentSelection.x - 1, CurrentSelection.y),
            _ => CurrentSelection
        };

        private bool WithinBounds((int x, int y) pos)
        {
            return (pos.x >= 0 && pos.y >= 0 && pos.x < _state.Grid.Width && pos.y < _state.Grid.Height);
        }
    }
}
