using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace GameOfLife
{
    /// <summary>
    /// Class to handle all of the input, either by text or normal keypresses
    /// </summary>
    internal sealed class InputHandler
    {
        public enum Modes
        {
            Normal,
            Input,
            Visual
        }
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

            /* MODES */
            InputMode,
            NormalMode,
            VisualMode,

            /* MISC */
            Save,
            Load
        }

        private static readonly Dictionary<Keys, AvailableActions> _keyMap = new()
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
            { Keys.OemPeriod,  AvailableActions.InputMode},
            { Keys.P,  AvailableActions.Pause},
            { Keys.C,  AvailableActions.ClearBoard},
            { Keys.Space,  AvailableActions.FlipValueAtPos}
        };

        private static readonly RegexOptions regexOptions = RegexOptions.Compiled | RegexOptions.IgnoreCase;
        private static readonly Dictionary<string, (AvailableActions, Regex)> _commandMap = new()
        {
            { "EXIT",       (AvailableActions.NormalMode,       new Regex(@"^(EXIT)\s*$", regexOptions)) },
            { "PAUSE",      (AvailableActions.Pause,            new Regex(@"^(PAUSE)\s*$", regexOptions)) },
            { "CLEAR",      (AvailableActions.ClearBoard,       new Regex(@"^(CLEAR)\s*$", regexOptions)) },
            { "CENTER",     (AvailableActions.CameraMoveTo,     new Regex(@"^(CENTER)\s*$", regexOptions)) },
            { "GOTO",       (AvailableActions.SelectionMoveTo,  new Regex(@"^(GOTO)\s+(\d+)\s+(\d+)\s*$", regexOptions)) },
            { "LOAD",       (AvailableActions.Load,             new Regex(@"^(LOAD)\s+([\w\.\\]+)\s*$", regexOptions)) },
        };

        public string CurrentCommand { get; private set; } = "";
        public Modes CurrentMode
        {
            get { return _currentMode; }
            private set
            {
                _currentMode = value;
                CurrentCommand = "";
                hasEnteredInputMode = false;
            }
        }

        private Modes _currentMode = Modes.Normal;
        private readonly LinkedList<string> _commandHistory = new();
        private LinkedListNode<string> _commandHistoryNode = null;
        private bool hasEnteredInputMode = false;

        private readonly GameWindow _window;
        private readonly GameState _gameState;
        private readonly Camera _camera;

        public InputHandler(GameWindow window, GameState gameState, Camera camera)
        {
            _window = window;
            _gameState = gameState;
            _camera = camera;
        }

        public void HandleInput(object sender, InputKeyEventArgs e)
        {
            switch (CurrentMode)
            {
                case Modes.Normal:
                    AvailableActions action;
                    if (_keyMap.TryGetValue(e.Key, out action))
                    {
                        HandleAction(action);
                    }
                    break;

                case Modes.Input:
                    HandleConsoleInput(e.Key);
                    break;

                case Modes.Visual:
                default:
                    break;
            }
        }

        public void HandleText(object sender, TextInputEventArgs e)
        {
            //HACK: return first time so char to enter input mode is not also added.
            if (!hasEnteredInputMode)
            {
                hasEnteredInputMode = true;
                return;
            }
            if (CurrentMode == Modes.Input && !Char.IsControl(e.Character))
            {
                CurrentCommand += e.Character;
            }
        }

        private void HandleConsoleInput(Keys key)
        {
            switch (key)
            {
                case Keys.Back:
                    CurrentCommand = CurrentCommand.Length == 0 ? "" : CurrentCommand[..^1];
                    break;

                case Keys.Escape:
                    HandleAction(AvailableActions.NormalMode);
                    break;

                case Keys.Up:
                    _commandHistoryNode = _commandHistoryNode?.Next ?? _commandHistory.First;
                    CurrentCommand = _commandHistoryNode?.Value ?? "";
                    break;

                case Keys.Down:
                    _commandHistoryNode = _commandHistoryNode?.Previous ?? null;
                    CurrentCommand = _commandHistoryNode?.Value ?? "";
                    break;

                case Keys.Enter:
                    //TODO: Add tellback if wrong form etc.
                    string firstWord = CurrentCommand.ToUpper().Split(" ")[0];
                    if (_commandMap.TryGetValue(firstWord, out (AvailableActions action, Regex regex) command))
                    {
                        Match match = command.regex.Match(CurrentCommand);
                        if (match.Success)
                        {
                            HandleAction(command.action, match.Groups);
                        }
                    }
                    _commandHistory.AddFirst(CurrentCommand);
                    CurrentCommand = "";
                    break;

                default:
                    break;
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
                case AvailableActions.NormalMode:
                    CurrentMode = Modes.Normal;
                    _window.TextInput -= HandleText;
                    break;
                case AvailableActions.InputMode:
                    CurrentMode = Modes.Input;
                    _window.TextInput += HandleText;
                    break;
                case AvailableActions.VisualMode:
                    CurrentMode = Modes.Visual;
                    _window.TextInput -= HandleText;
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
