﻿namespace Idology.Space.Core.Commands.SelectLocation;

public record SelectLocationCommand(int X, int Y, bool Cycle) : ISpaceCommand;
