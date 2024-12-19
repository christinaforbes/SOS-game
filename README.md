# SOS Game GUI Application (November 2024)

## About
A digital adaptation of the paper-and-pencil game SOS, developed as a semester-long project for my Foundations of Software Engineering course.

Note: the "SosGame" folder contains the final version of the game.

![sos-game-finished](https://github.com/user-attachments/assets/48033d4d-d921-4cf6-a4fa-eeeac2187dba)

![sos-game-recorded](https://github.com/user-attachments/assets/c40307c5-d2f9-42b0-b7ed-0a7b0210866c)

## Game Options

| Option | Description |
| ------ | ----------- |
| Game mode | Select either a simple game or a general game <br/> See below for the differences between game modes |
| Board size | Enter the board's length/width value (number of squares) <br/> Valid board sizes are 3-10 (inclusive) |
| Record game | Check to record the game's moves to a file |
| Replay game | Select for an automated replay of the most recently recorded game |
| New game | Start a new game of the selected game mode and board size | 

## How to Play

* Note: The blue player always makes the first move, regardless of game mode.
* To make a move, select either "S" or "O" and then select an unoccupied square in which to place the letter.

### Simple Game Mode
* Players always alternate turns after each move.
* If a move forms the sequence "S-O-S" (horizontally, vertically, or diagonally), then the game ends and the player who completed the sequence wins the game.
* If the board is filled and neither player formed the sequence, then the game is a draw.

### General Game Mode
* Players usually alternate turns after each move.
* If a move forms the sequence "S-O-S" (horizontally, vertically, or diagonally), then the player who completed the sequence scores a point for every sequence created by the move and then makes another move. This continues until the player makes a move that does not form the sequence.
* When the board is filled, the player who scored the most points (completed the most sequences) wins the game.
* If neither player formed the sequence or if both players score the same number of points, then the game is a draw.
