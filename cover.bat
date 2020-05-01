echo off
REM generate a coverage report locally
REM if this does not work, first do a
REM dotnet tool install --global coverlet.console

dotnet build

coverlet .\tests\bin\Debug\netcoreapp3.1\NuKeeper.ProjectReader.Tests.dll --target "dotnet" --targetargs "test --no-build"