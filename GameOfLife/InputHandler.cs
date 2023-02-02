using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text.RegularExpressions;

namespace GameOfLife


{
    public sealed class InputHandler
    {
        public enum AvailableActions
        {
            SelectionMoveUp,
            SelectionMoveDown,
            SelectionMoveLeft,
            SelectionMoveRight,
            CameraMoveUp,
            CameraMoveDown,
            CameraMoveLeft,
            CameraMoveRight,
            CameraZoomIn,
            CameraZoomOut,
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

        private static readonly Dictionary<Keys, AvailableActions> _keyMap = new Dictionary<Keys, AvailableActions>()
        {
            /* Movements */
            { Keys.Left,  AvailableActions.SelectionMoveLeft},
            { Keys.Down,  AvailableActions.SelectionMoveDown},
            { Keys.Up,  AvailableActions.SelectionMoveUp},
            { Keys.Right,  AvailableActions.SelectionMoveRight},

            /* Update Rate */
            { Keys.I,  AvailableActions.IncreaseSpeed},
            { Keys.O,  AvailableActions.DecreaseSpeed},

            /* Camera */
            { Keys.H,  AvailableActions.CameraMoveLeft},
            { Keys.J,  AvailableActions.CameraMoveDown},
            { Keys.K,  AvailableActions.CameraMoveUp},
            { Keys.L,  AvailableActions.CameraMoveRight},
            { Keys.Y,  AvailableActions.CameraZoomIn},
            { Keys.U,  AvailableActions.CameraZoomOut},

            /* MISC */
            { Keys.E,  AvailableActions.EnterEditMode},
            { Keys.P,  AvailableActions.Pause},
            { Keys.C,  AvailableActions.ClearBoard},
            { Keys.Space,  AvailableActions.FlipValueAtPos}
        };

        private static readonly Dictionary<string, (AvailableActions, Regex)> _commandMap = new Dictionary<string, (AvailableActions, Regex)>()
        {
            { "exit", (AvailableActions.ExitEditMode, new Regex(@"^exit\s+$", RegexOptions.Compiled | RegexOptions.IgnoreCase)) },
        };

        public bool EditMode { get; private set; } = false;
        public string CurrentCommand { get; private set; }

        private readonly GameState _gameState;
        private readonly Camera _camera;

        public InputHandler(GameState gameState, Camera camera)
        {
            _gameState = gameState;
            _camera = camera;
        }

        public void HandleInput(object sender, InputKeyEventArgs e)
        {
            if (EditMode)
            {
                return;
            }

            AvailableActions action;
            if (_keyMap.TryGetValue(e.Key, out action))
            {
                HandleAction(action);
            }
        }

        public void HandleText(object sender, TextInputEventArgs e)
        {
            if (EditMode)
            {
                return;
            }
            switch (e.Key)
            {
                case Keys.Enter:
                    string firstWord = CurrentCommand.Split(" ")[0];
                    (AvailableActions action, Regex regex) command;
                    if (_commandMap.TryGetValue(firstWord, out command))
                    {
                        HandleAction(command.action, CurrentCommand.Substring(firstWord.Length, CurrentCommand.Length));
                    }
                    break;
                default:
                    if (Char.IsAsciiLetterOrDigit(e.Character))
                    {
                        CurrentCommand += e.Character;
                    }
                    break;
            }
        }

        public void HandleAction(AvailableActions action)
        {
            HandleAction(action, "");
        }

        private void HandleAction(AvailableActions action, string args)
        {
            switch (action)
            {
                case AvailableActions.SelectionMoveUp:
                    _gameState.MoveCurrentSelection(SelectionDirection.Up);
                    break;
                case AvailableActions.SelectionMoveDown:
                    _gameState.MoveCurrentSelection(SelectionDirection.Down);
                    break;
                case AvailableActions.SelectionMoveRight:
                    _gameState.MoveCurrentSelection(SelectionDirection.Right);
                    break;
                case AvailableActions.SelectionMoveLeft:
                    _gameState.MoveCurrentSelection(SelectionDirection.Left);
                    break;
                case AvailableActions.IncreaseSpeed:
                    _gameState.increaseUpdateRate();
                    break;
                case AvailableActions.DecreaseSpeed:
                    _gameState.decreaseUpdateRate();
                    break;
                case AvailableActions.Pause:
                    _gameState.Paused = !_gameState.Paused;
                    break;
                case AvailableActions.ClearBoard:
                    _gameState.Reset();
                    break;
                case AvailableActions.FlipValueAtPos:
                    _gameState.FlipAtSelection();
                    break;
                case AvailableActions.CameraMoveUp:
                    _camera.MoveCamera(CameraDirections.Up);
                    break;
                case AvailableActions.CameraMoveDown:
                    _camera.MoveCamera(CameraDirections.Down);
                    break;
                case AvailableActions.CameraMoveRight:
                    _camera.MoveCamera(CameraDirections.Right);
                    break;
                case AvailableActions.CameraMoveLeft:
                    _camera.MoveCamera(CameraDirections.Left);
                    break;
                case AvailableActions.CameraZoomIn:
                    _camera.ZoomCamera(ZoomActions.ZoomIn);
                    break;
                case AvailableActions.CameraZoomOut:
                    _camera.ZoomCamera(ZoomActions.ZoomOut);
                    break;
                case AvailableActions.EnterEditMode:
                    EditMode= true;
                    break;
                case AvailableActions.ExitEditMode:
                    EditMode= false;
                    break;
                case AvailableActions.Save:
                case AvailableActions.Load:
                default:
                    throw new NotImplementedException("Action " + action + "not implemented");
            }
        }

    }
}
