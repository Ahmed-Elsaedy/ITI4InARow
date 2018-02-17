using ITI4InARow.Module.Client;
using ITI4InARow.Module.Core;
using System;
using System.Drawing;
using System.Windows.Forms;
using System.Linq;

namespace ITI4InARow.Game.Client2
{
    public partial class _ClientForm : Form
    {
        GameClient _GameClient;
        public _ClientForm()
        {
            InitializeComponent();
            CheckForIllegalCrossThreadCalls = false;
            SwitchFormMode(ClientFormMode.Disconnected);

            _GameClient = new GameClient();
            _GameClient.ClientStatusChanged += GameClient_ClientStatusChanged;
            _GameClient.ProfileUpdateMessage += GameClient_ProfileUpdateMessage;
            _GameClient.RoomUpdateMessage += GameClient_RoomUpdateMessage;
            _GameClient.GameUpdateMessage += GameClient_GameUpdateMessage;
            _lbRooms.SelectedIndexChanged += lbRooms_SelectedIndexChanged;

            var IPAddress = Helper.GetLocalIP().GetAddressBytes();
            _numIP1.Value = IPAddress[0];
            _numIP2.Value = IPAddress[1];
            _numIP3.Value = IPAddress[2];
            _numIP4.Value = IPAddress[3];
        }
        private void GameClient_ClientStatusChanged(object sender, ClientActionEventArgs e)
        {
            switch (e.Status)
            {
                case ClientStatus.ConnectionError:
                    MessageBox.Show("Connection Error");
                    break;
                case ClientStatus.ClientConnected:
                    SwitchFormMode(ClientFormMode.Connected);
                    break;
                case ClientStatus.ClientDisconnected:
                    SwitchFormMode(ClientFormMode.Disconnected);
                    break;
            }
        }
        private void _btnConnect_Click(object sender, EventArgs e)
        {
            var IPAddress = new byte[4];
            IPAddress[0] = (byte)(int)_numIP1.Value;
            IPAddress[1] = (byte)(int)_numIP2.Value;
            IPAddress[2] = (byte)(int)_numIP3.Value;
            IPAddress[3] = (byte)(int)_numIP4.Value;

            var Port = (int)_numPort.Value;
            _GameClient.ConnectClient(Helper.GetLocalIP().GetAddressBytes(), Port);

            ProfileUpdateMessage profile = new ProfileUpdateMessage();
            Random r = new Random();
            profile.PlayerName = "Player " + r.Next(100);
            profile.PlayerColor = Color.FromArgb(r.Next(0, 256), r.Next(0, 256), r.Next(0, 256)).ToArgb();
            _GameClient.SendMessageToServer(profile);
        }
        private void GameClient_ProfileUpdateMessage(object sender, ProfileUpdateMessage e)
        {
            txt_NickName.Text = e.PlayerName;
            btnColor.BackColor = Color.FromArgb(e.PlayerColor);
        }
        private void GameClient_RoomUpdateMessage(object sender, RoomUpdateMessage msg)
        {
            switch (msg.UpdateStatus)
            {
                case RoomUpdateStatus.NewRoomRequest:
                    SwitchFormMode(ClientFormMode.CreateRoom);
                    break;
                case RoomUpdateStatus.NewRoomRollback:
                    SwitchFormMode(ClientFormMode.Connected);
                    break;
            }
            _lbRooms.Items.Clear();
            foreach (RoomUpdateMessage message in _GameClient.RoomsUpdates)
                _lbRooms.Items.Add(message);
            _lbRooms.ClearSelected();
            UpdateButtonsState();
        }
        private void lbRooms_SelectedValueChanged(object sender, EventArgs e)
        {
            UpdateButtonsState();
        }
        private void lbRooms_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateButtonsState();
        }
        private void UpdateButtonsState()
        {
            if (_lbRooms.SelectedItems.Count > 0)
            {
                RoomUpdateMessage msg = (RoomUpdateMessage)_lbRooms.SelectedItem;
                _btnJoin.Enabled = _NumPass1.Enabled = msg.GetRoomStatus() == RoomStatus.Waiting;
                _btnView.Enabled = msg.GetRoomStatus() == RoomStatus.Gamming;
            }
            else
            {
                _btnJoin.Enabled = _NumPass1.Enabled = false;
                _btnView.Enabled = false;
            }
        }
        private void _btnJoin_Click(object sender, EventArgs e)
        {
            var msg = (RoomUpdateMessage)_lbRooms.SelectedItem;
            if (_NumPass1.Value == msg.RoomPass)
            {
                msg.UpdateStatus = RoomUpdateStatus.JoinRoomRequest;
                msg.Player2ID = _GameClient.PlayerID;
                _GameClient.SendMessageToServer(msg);
            }
            else
                MessageBox.Show("Wrong Room Password", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        private void _btnCreate_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(_txtName.Text))
            {
                RoomUpdateMessage myRoom = new RoomUpdateMessage();
                myRoom.UpdateStatus = RoomUpdateStatus.NewRoomRequest;
                myRoom.RoomName = _txtName.Text;
                myRoom.RoomPass = (int)_NumPass2.Value;
                myRoom.Player1ID = _GameClient.PlayerID;
                _GameClient.SendMessageToServer(myRoom);
            }
        }
        private void _btnCancel_Click(object sender, EventArgs e)
        {
            var msg = _GameClient.RoomsUpdates.Single(x => x.GetRoomStatus() == RoomStatus.Waiting && x.Player1ID == _GameClient.PlayerID);
            msg.UpdateStatus = RoomUpdateStatus.NewRoomRollback;
            msg.Player1ID = 0;
            _GameClient.SendMessageToServer(msg);
        }
        private void SwitchFormMode(ClientFormMode mode)
        {
            switch (mode)
            {
                case ClientFormMode.Connected:
                    _GBServerCnn.Enabled = false;
                    _GBPlayerProfile.Enabled = true;
                    _GBCreateRoom.Enabled = true;
                    _txtName.Enabled = true;
                    _txtName.Clear();
                    _NumPass2.Enabled = true;
                    _NumPass2.Value = 0;
                    _btnCreate.Enabled = true;
                    _btnCancel.Enabled = false;
                    _GBRooms.Enabled = true;
                    _GBGameOpt.Enabled = false;
                    UpdateButtonsState();
                    break;
                case ClientFormMode.Disconnected:
                    _GBServerCnn.Enabled = true;
                    _GBPlayerProfile.Enabled = false;
                    _GBRooms.Enabled = false;
                    _GBCreateRoom.Enabled = false;
                    _GBGameOpt.Enabled = false;
                    break;
                case ClientFormMode.CreateRoom:
                    _GBServerCnn.Enabled = false;
                    _GBPlayerProfile.Enabled = false;
                    _GBRooms.Enabled = false;
                    _GBCreateRoom.Enabled = true;
                    _txtName.Enabled = false;
                    _NumPass2.Enabled = false;
                    _btnCreate.Enabled = false;
                    _btnCancel.Enabled = true;
                    _GBGameOpt.Enabled = false;
                    break;
                case ClientFormMode.GameStarted:
                    _GBServerCnn.Enabled = false;
                    _GBPlayerProfile.Enabled = false;
                    _GBRooms.Enabled = false;
                    _GBCreateRoom.Enabled = false;
                    _GBGameOpt.Enabled = true;
                    _btnLeaveGame.Enabled = true;
                    _btnRematch.Enabled = false;
                    break;
                case ClientFormMode.GameEnded:
                    _GBServerCnn.Enabled = false;
                    _GBPlayerProfile.Enabled = false;
                    _GBRooms.Enabled = false;
                    _GBCreateRoom.Enabled = false;
                    _GBGameOpt.Enabled = true;
                    _btnLeaveGame.Enabled = true;
                    _btnRematch.Enabled = true;
                    break;
            }
            UpdateStatusBar(mode);
        }
        private void UpdateStatusBar(ClientFormMode mode)
        {
            switch (mode)
            {
                case ClientFormMode.Connected:
                    _statusBar.Text = "Connected - " + _GameClient.PlayerID;
                    break;
                case ClientFormMode.Disconnected:
                    _statusBar.Text = "Disconnected";
                    break;
                case ClientFormMode.CreateRoom:
                    _statusBar.Text = "waiting for another player...";
                    break;
                case ClientFormMode.GameStarted:
                    _statusBar.Text = _GameClient.GameUpdate.GetStatusString();
                    break;
                case ClientFormMode.GameEnded:
                    _statusBar.Text = "Game Ended";
                    break;
            }
        }
        private void GameClient_GameUpdateMessage(object sender, GameUpdateMessage e)
        {
            switch (e.UpdateStatus)
            {
                case GameUpdateStatus.GameStarted:
                    SwitchFormMode(ClientFormMode.GameStarted);
                    break;
                case GameUpdateStatus.PlayerMove:
                    DisplayBoard(e.GameBoard, Color.FromArgb(e.Player1Color), Color.FromArgb(e.Player2Color));
                    _statusBar.Text = _GameClient.GameUpdate.GetStatusString();
                    break;
                case GameUpdateStatus.PlayerWin:
                    MessageBox.Show("You Won");
                    break;
                case GameUpdateStatus.PlayerLose:
                    MessageBox.Show("You Lost");
                    break;
                case GameUpdateStatus.GameDraw:
                    MessageBox.Show("Game Draw");
                    break;
            }
        }
        private void GameSpot_MouseClick(object sender, MouseEventArgs e)
        {
            if (_GameClient.GameUpdate == null)
                return;
            if (_GameClient.GameUpdate.GetTurnID() == _GameClient.PlayerID)
            {
                int dropChoice = int.Parse(((GameSpot)sender).Tag.ToString());
                if (IsValidPlayerDrop(_GameClient.GameUpdate.GameBoard, dropChoice))
                {
                    DropChoice(_GameClient.GameUpdate.GameBoard, _GameClient.GameUpdate.TurnMove, dropChoice);
                    _GameClient.GameUpdate.UpdateStatus = GameUpdateStatus.PlayerMove;
                    _GameClient.SendMessageToServer(_GameClient.GameUpdate);
                }
            }
        }
        private void _btnLeaveGame_Click(object sender, EventArgs e)
        {

        }

        private void DisplayBoard(PlayerMove[,] board, Color xColor, Color oColor)
        {
            int rows = 6, columns = 7, i, ix, spotNum = 0;
            for (i = 1; i <= rows; i++)
            {
                for (ix = 1; ix <= columns; ix++)
                {
                    GameSpot spot = (GameSpot)_flpGameSurface.Controls[spotNum];
                    if (board[i, ix] == PlayerMove.X)
                        spot.SpotSelectedColor = xColor;
                    else if (board[i, ix] == PlayerMove.O)
                        spot.SpotSelectedColor = oColor;
                    if (board[i, ix] != PlayerMove.N)
                        spot.Invalidate();
                    spotNum++;
                }
            }
        }
        private bool IsValidPlayerDrop(PlayerMove[,] board, int dropChoice)
        {
            return dropChoice >= 1 && dropChoice <= 7
                && board[1, dropChoice] != PlayerMove.X && board[1, dropChoice] != PlayerMove.O;
        }
        private void DropChoice(PlayerMove[,] board, PlayerMove playerTag, int dropChoice)
        {
            int length, turn;
            length = 6;
            turn = 0;
            do
            {
                if (board[length, dropChoice] != PlayerMove.X && board[length, dropChoice] != PlayerMove.O)
                {
                    board[length, dropChoice] = playerTag;
                    turn = 1;
                }
                else
                    --length;
            } while (turn != 1);
        }
    }
    public enum ClientFormMode
    {
        Connected,
        Disconnected,
        CreateRoom,
        GameStarted,
        GameEnded
    }
}
