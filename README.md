# BattleShipGame

A simple BattleShip game made in C# .Net Core.

## Instalation

Just check-out the code and open the BattleShipGame.sln on Visual Studio 2019 or above.

## How to play

After opening it on Visual Studio, make sure the BattleShipGameUI project is your default project and run it.

It will present you a console window with a 10x10 grid. Type a letter/number to shot. You can also just hit enter to fire a random shot.

It is defaulted to have 1 BattleShip (5 squares) and 2 Destroyers (4 squares).

When you finally sunk all ships, it will give you the number of shots that were necessary to succeed.

## Projects

### BattleShipGame

This is the core project. It contains all the logic and behaviours.

### BattleShipGame.Test

The tests relative to the core project.

### BattleShipGameUI

A simple console UI that consumes the core project and allow a user to play.