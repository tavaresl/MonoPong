#!/usr/bin/env bash

export PATH=/usr/local/share/dotnet/x64:$PATH
export DOTNET_ROOT=/usr/local/share/dotnet/x64

dotnet mgcb-editor ./MonoGame.Core/Content/Content.mgcb
