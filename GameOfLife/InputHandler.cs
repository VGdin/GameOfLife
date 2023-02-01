using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace GameOfLife


{
    public sealed class InputHandler
    {
        public static readonly string AvailableCommandsString = "Move (HJKL), Toggle Value (Space)\n" +
            "(P)ause, (I)ncrease or (D)ecrease speed, (C)lear";

        private static readonly Dictionary<Keys, AvailableActions> _keyMap = new Dictionary<Keys, AvailableActions>()
        {
            /* Movements */
            { Keys.Left,  AvailableActions.MoveLeft},
            { Keys.H,  AvailableActions.MoveLeft},
            { Keys.Down,  AvailableActions.MoveDown},
            { Keys.J,  AvailableActions.MoveDown},
            { Keys.Up,  AvailableActions.MoveUp},
            { Keys.K,  AvailableActions.MoveUp},
            { Keys.Right,  AvailableActions.MoveRight},
            { Keys.L,  AvailableActions.MoveRight},

            /* Update Rate */
            { Keys.I,  AvailableActions.IncreaseSpeed},
            { Keys.OemPlus,  AvailableActions.IncreaseSpeed},
            { Keys.D,  AvailableActions.DecreaseSpeed},
            { Keys.OemMinus,  AvailableActions.DecreaseSpeed},

            /* MISC */
            { Keys.E,  AvailableActions.EnterEditMode},
            { Keys.P,  AvailableActions.Pause},
            { Keys.C,  AvailableActions.ClearBoard},
            { Keys.Space,  AvailableActions.FlipValueAtPos}
        };

        private static readonly Dictionary<string, (AvailableActions, Regex)> _commandMap = new Dictionary<string, (AvailableActions, Regex)>()
        {

            { "exit", (AvailableActions.ExitEditMode, new Regex(@"^exit\s+$", RegexOptions.Compiled | RegexOptions.IgnoreCase)) },

            /* Saving and Loading*/
        };

        public string CurrentCommand { get; private set; }

        private readonly GameState _gameState;

        public InputHandler(GameState gameState)
        {
            _gameState = gameState;
        }

        public void HandleInput(object sender, InputKeyEventArgs e)
        {
            if (_gameState.EditMode)
            {
                return;
            }

            AvailableActions action;
            if (_keyMap.TryGetValue(e.Key, out action))
            {
                _gameState.HandleAction(action);
            }
        }

        public void HandleText(object sender, TextInputEventArgs e)
        {
            if (!_gameState.EditMode)
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
                        _gameState.HandleAction(command.action, CurrentCommand.Substring(firstWord.Length,CurrentCommand.Length));
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
    }
}
