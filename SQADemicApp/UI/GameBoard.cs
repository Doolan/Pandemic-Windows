using System;
using System.Windows.Forms;
using SQADemicApp.BL;
using System.Collections.Generic;
using SQADemicApp.Players;

namespace SQADemicApp
{
    public partial class GameBoard : Form
    {
        public GameBoardModels BoardModel;
        readonly CharacterPane _characterPane;
        readonly PlayerPanel _playerForm;
        readonly EventCardForm _ecForm;
        public enum State { Dispatcher, Initializing, Move, Cure, Default, Airlift, GovGrant}
        public static State CurrentState;
        public enum Turnpart { Action, Draw, Infect };
        public static Turnpart CurrentTurnPart;
        public static int DispatcherMoveIndex;
        

        public GameBoard()
        {
            SetStyle(ControlStyles.SupportsTransparentBackColor, true);
            CurrentState = State.Initializing;
            string[] rolesDefault = { "Dispatcher", "Scientist" };
            BoardModel = new GameBoardModels(rolesDefault);
            _playerForm = new PlayerPanel(this);
            _characterPane = new CharacterPane(rolesDefault);
            _ecForm = new EventCardForm();
            InitializeComponent();
            _ecForm.Show();
            _characterPane.Show();
            _playerForm.Show();
            UpdatePlayerForm();
            UpdateCityButtons(true);
            

            CurrentState = State.Default;
            CurrentTurnPart = Turnpart.Action;
        }
        public GameBoard(string[] playerRoles)
        {

            SetStyle(ControlStyles.SupportsTransparentBackColor, true);
            BoardModel = new GameBoardModels(playerRoles);
            _playerForm = new PlayerPanel(this);
            _characterPane = new CharacterPane(playerRoles);
            _ecForm = new EventCardForm();
            InitializeComponent();
            _ecForm.Show();
            _characterPane.Show();
            _playerForm.Show();
            UpdatePlayerForm();
            UpdateCityButtons(true);
            CurrentState = State.Default;
            CurrentTurnPart = Turnpart.Action; 
        }

        private void City_Click(object sender, EventArgs e)
        {
            var pressed = sender as Button;
            var cityName = pressed.Text.Substring(3);
            switch (CurrentState)
            {
                case State.Airlift:
                    SpecialEventCardsBL.Airlift(GameBoardModels.GetCurrentPlayer(), Create.CityDictionary[cityName]);
                    break;
                case State.GovGrant:
                    SpecialEventCardsBL.GovernmentGrant(cityName);
                    break;
                case State.Dispatcher:
                    if (GameBoardModels.GetPlayerByIndex(DispatcherMoveIndex).DispatcherMovePlayer(new List<AbstractPlayer>(GameBoardModels.GetPlayers()), Create.CityDictionary[cityName]))
                    {
                        _characterPane.UpdatePlayerCity(DispatcherMoveIndex, cityName);
                        if (BoardModel.IncTurnCount())
                            CurrentTurnPart = Turnpart.Draw;
                        UpdatePlayerForm();
                        UpdateCityButtons(false);
                    }
                    else
                    {
                        MessageBox.Show("Invalid City", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    }
                    break;
                case State.Move:
                    if (GameBoardModels.GetCurrentPlayer().MovePlayer(Create.CityDictionary[cityName]))
                    {
                        _characterPane.UpdatePlayerCity(GameBoardModels.GetCurrentPlayerIndex(), cityName);
                        bool endofturn = BoardModel.IncTurnCount();
                        if (endofturn)
                            CurrentTurnPart = Turnpart.Draw;
                        UpdatePlayerForm();
                        UpdateCityButtons(false);
                    }
                    else
                    {
                        MessageBox.Show("Invalid City", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    }
                    break;
                default:
                    CityPageForm cpForm = new CityPageForm(Create.CityDictionary[cityName]);
                    cpForm.Show();
                    break;
            }
            CurrentState = State.Default;
        }

        public CharacterPane GetCharacterPane()
        {
            return _characterPane;
        }

        public static void UpdatePlayerButtonTextAccordingToIndex(CharacterPane pane, int index, string cityName)
        {
            pane.GetPlayerBtns()[index].Text = 
                "Player " + (index + 1) + "\n" + GameBoardModels.GetPlayerByIndex(index) + "\n" + cityName;
        }

        public void UpdatePlayerForm()
        {
            _playerForm.UpdateTurnProgressBoard(BoardModel.CurrentPlayerTurnCounter, GameBoardModels.GetCurrentPlayer().GetMaxTurnCount());
            _playerForm.UpdatePlayerHand(GameBoardModels.GetCurrentPlayer().HandStringList().ToArray());
            if (GameBoardModels.GetCurrentPlayer().GetType() == typeof(DispatcherPlayer))
            {
                _playerForm.AddDispatcherButton();
            }
            else
            {
                _playerForm.RemoveDispatcherButton();
            }
            if (CurrentTurnPart == Turnpart.Draw)
            {
                _playerForm.ShowDrawButton();
            }
            else if (CurrentTurnPart == Turnpart.Infect)
            {
                _playerForm.ShowInfectButton();
            }
            else
            {
                _playerForm.HideDrawInfectButton();
            }

            _characterPane.UpdateCurrentPlayer(GameBoardModels.GetCurrentPlayerIndex());
            _characterPane.UpdatePlayerCount(GameBoardModels.GetPlayerCount());

            _playerForm.UpdateCubeCounts(GameBoardModels.GetInfectionCubeCount(Color.Red), GameBoardModels.GetInfectionCubeCount(Color.Blue),
                GameBoardModels.GetInfectionCubeCount(Color.Black), GameBoardModels.GetInfectionCubeCount(Color.Yellow));
            _playerForm.UpdateCounters(GameBoardModels.InfectionRate, GameBoardModels.GetOutbreakMarker());
            _playerForm.updateCureStatus(GameBoardModels.GetCureStatus(Color.Red).ToString(), GameBoardModels.GetCureStatus(Color.Blue).ToString(),
                GameBoardModels.GetCureStatus(Color.Black).ToString(), GameBoardModels.GetCureStatus(Color.Yellow).ToString());
            _ecForm.UpdateEventCards();

        }

        private void button1_Click(object sender, EventArgs e)
        {
            var dp = new DiscardPile(false);
            dp.Show();
        }
        public void UpdateCityButtons(bool firstRun)
        {
            foreach (var control in this.Controls)
            {
                if (!(control is Button)) continue;
                var button = control as Button;
                var cityName = button.Text.Substring(3);
                try
                {
                    var city = Create.CityDictionary[cityName];
                    button.Text = string.Format("{0,2} " + city.Name, city.AllCubeCount());
                    if(firstRun)
                        button.Font = new System.Drawing.Font(button.Font.FontFamily, 5);
                }
                catch(KeyNotFoundException)
                {
                    // not a button that needs to be updated
                }
            }
        }
    }
}
