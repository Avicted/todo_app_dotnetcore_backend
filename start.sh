#!/bin/bash

set -xe
 
dotnet clean
dotnet restore
dotnet run --project "TodoApp.Web/TodoApp.Web.csproj"
