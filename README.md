# Holler Assignment

## Description

This class library follows the requriements provided to me by Holler.io for my programming assessment. 

## UserAssignmentService

The UserAssignmentService adds a user to a group based upon the groupWeights.json config file. 

## UserAssginmentService.Tests

The tests will validate that groupWeights.json will never be > 100%, while ensuring that UserAssignmentService.GetUserGroup works as expected.

## Testing
`dotnet test ./test/UserAssignment.Tests/UserAssignment.Tests.csproj`