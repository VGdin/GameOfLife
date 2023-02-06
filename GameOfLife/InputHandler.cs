using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace GameOfLife
{
    internal sealed class InputHandler
    {
        public enum AvailableActions
        {
            /* Selection Controls */
            SelectionMoveUp,
            SelectionMoveDown,
            SelectionMoveLeft,
            SelectionMoveRight,
            SelectionMoveTo,

            /* Camera Movement */
            CameraMoveUp,
            CameraMoveDown,
            CameraMoveLeft,
            CameraMoveRight,
            CameraMoveTo,
            CameraZoomIn,
            CameraZoomOut,

            /* Came Controls */
            IncreaseSpeed,
            DecreaseSpeed,
            Pause,
            ClearBoard,
            FlipValueAtPos,

            /* MISC */
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
            { Keys.OemPeriod,  AvailableActions.EnterEditMode},
            { Keys.P,  AvailableActions.Pause},
            { Keys.C,  AvailableActions.ClearBoard},
            { Keys.Space,  AvailableActions.FlipValueAtPos}
        };

        private static readonly RegexOptions regexOptions = RegexOptions.Compiled | RegexOptions.IgnoreCase;
        private static readonly Dictionary<string, (AvailableActions, Regex)> _commandMap = new Dictionary<string, (AvailableActions, Regex)>()
        {
            { "EXIT",       (AvailableActions.ExitEditMode,     new Regex(@"^(EXIT)\s*$", regexOptions)) },
            { "CLEAR",      (AvailableActions.ClearBoard,       new Regex(@"^(CLEAR)\s*$", regexOptions)) },
            { "CENTER",     (AvailableActions.CameraMoveTo,     new Regex(@"^(CENTER)\s*$", regexOptions)) },
            { "GOTO",       (AvailableActions.SelectionMoveTo,  new Regex(@"^(GOTO)\s+(\d+)\s+(\d+)\s*$", regexOptions)) },
            { "LOAD",       (AvailableActions.Load,             new Regex(@"^(LOAD)\s+([\w\.\\]+)\s*$", regexOptions)) },
        };

        public bool EditMode { get; private set; } = false;
        public string CurrentCommand { get; private set; } = "";

        private readonly Object _inputLock = new();
        private readonly GameState _gameState;
        private readonly Camera _camera;

        public InputHandler(GameState gameState, Camera camera)
        {
            _gameState = gameState;
            _camera = camera;
        }

        public void HandleInput(object sender, InputKeyEventArgs e)
        {
            lock (_inputLock)
            {
                if (EditMode)
                {
                    return;
                }

                if (_keyMap.TryGetValue(e.Key, out AvailableActions action))
                {
                    HandleAction(action);
                }
            }
        }

        public void HandleText(object sender, TextInputEventArgs e)
        {
            lock (_inputLock)
            {
                if (!EditMode)
                {
                    return;
                }
                switch (e.Key)
                {
                    case Keys.Back:
                        CurrentCommand = CurrentCommand.Length == 0 ? "" : CurrentCommand.Substring(0, CurrentCommand.Length - 1);
                        break;
                    case Keys.Escape:
                        HandleAction(AvailableActions.ExitEditMode);
                        break;
                    case Keys.Enter:
                        //TODO: Tellback
                        string firstWord = CurrentCommand.ToUpper().Split(" ")[0];
                        if (_commandMap.TryGetValue(firstWord, out (AvailableActions action, Regex regex) command))
                        {
                            Match match = command.regex.Match(CurrentCommand);
                            if (match.Success)
                            {
                                HandleAction(command.action, match.Groups);
                            }
                        }
                        CurrentCommand = "";
                        break;
                    default:
                        if (Char.IsAscii(e.Character))
                        {
                            CurrentCommand += e.Character;
                        }
                        break;
                }
            }
        }

        public void HandleAction(AvailableActions action)
        {
            HandleAction(action, null);
        }

        private void HandleAction(AvailableActions action, GroupCollection args)
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
                case AvailableActions.SelectionMoveTo when args != null:
                    _gameState.MoveCurrentSelectionTo((Convert.ToInt32(args[2].Value), (Convert.ToInt32(args[3].Value))));
                    break;
                case AvailableActions.IncreaseSpeed:
                    _gameState.IncreaseUpdateRate();
                    break;
                case AvailableActions.DecreaseSpeed:
                    _gameState.DecreaseUpdateRate();
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
                case AvailableActions.CameraMoveTo:
                    _camera.MoveCameraTo(Util.TupleToVector2(_gameState.CurrentSelection));
                    break;
                case AvailableActions.CameraZoomIn:
                    _camera.ZoomCamera(ZoomActions.ZoomIn);
                    break;
                case AvailableActions.CameraZoomOut:
                    _camera.ZoomCamera(ZoomActions.ZoomOut);
                    break;
                case AvailableActions.EnterEditMode:
                    EditMode = true;
                    break;
                case AvailableActions.ExitEditMode:
                    EditMode = false;
                    break;
                case AvailableActions.Load:
                    _gameState.LoadFile(args[2].Value);
                    break;
                case AvailableActions.Save: //TODO:
                default:
                    throw new NotImplementedException("Action " + action + "not implemented");
            }
        }

    }
}
