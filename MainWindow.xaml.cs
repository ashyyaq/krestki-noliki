using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace TicTacToeGame
{
    public partial class MainWindow : Window
    {
        private bool playerTurn = true;
        private bool gameEnded = false;
        private string[] board = new string[9];
        private Grid mainGrid;

        public MainWindow()
        {
            InitializeComponent();
            InitializeGame();
        }

        private void InitializeGame()
        {
            for (int i = 0; i < 9; i++)
            {
                board[i] = "";
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            int index = Grid.GetRow(button) * 3 + Grid.GetColumn(button);

            if (!gameEnded && board[index] == "")
            {
                if (playerTurn)
                {
                    PlayerMove(index);
                }

                if (!gameEnded)
                {
                    ComputerMove();
                }
            }
        }

        private void PlayerMove(int index)
        {
            board[index] = "X";
            UpdateButton(index);
            playerTurn = false;
            CheckGameStatus();
        }

        private void ComputerMove()
        {
            int randomIndex;
            do
            {
                randomIndex = new Random().Next(0, 9);
            } while (board[randomIndex] != "");

            board[randomIndex] = "O";
            UpdateButton(randomIndex);
            playerTurn = true;
            CheckGameStatus();
        }

        private void UpdateButton(int index)
        {
            Button button = FindButtonByIndex(index, mainGrid);
            button.Content = board[index];
        }

        private Button FindButtonByIndex(int index, Grid grid)
        {
            foreach (var child in grid.Children)
            {
                if (child is Button button && Grid.GetRow(button) * 3 + Grid.GetColumn(button) == index)
                {
                    return button;
                }
            }
            return null;
        }

        private void CheckGameStatus()
        {
            string[] lines = {
                board[0] + board[1] + board[2],
                board[3] + board[4] + board[5],
                board[6] + board[7] + board[8],
                board[0] + board[3] + board[6],
                board[1] + board[4] + board[7],
                board[2] + board[5] + board[8],
                board[0] + board[4] + board[8],
                board[2] + board[4] + board[6]
            };

            foreach (var line in lines)
            {
                if (line == "XXX")
                {
                    txtResult.Text = "Crosses win!";
                    gameEnded = true;
                    DisableButtons();
                    return;
                }
                else if (line == "OOO")
                {
                    txtResult.Text = "Noughts win!";
                    gameEnded = true;
                    DisableButtons();
                    return;
                }
            }

            if (!board.Contains(""))
            {
                txtResult.Text = "It's a draw!";
                gameEnded = true;
                DisableButtons();
                return;
            }
        }

        private void DisableButtons()
        {
            foreach (var child in mainGrid.Children)
            {
                if (child is Button button)
                {
                    button.IsEnabled = false;
                }
            }
        }

        private void NewGame_Click(object sender, RoutedEventArgs e)
        {
            InitializeGame();
            foreach (var child in mainGrid.Children)
            {
                if (child is Button button)
                {
                    button.Content = "";
                    button.IsEnabled = true;
                }
            }
            txtResult.Text = "";
            gameEnded = false;
            playerTurn = true;
        }
    }
}