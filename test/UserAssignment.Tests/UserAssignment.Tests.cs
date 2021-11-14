using Xunit;
using System;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Collections.Generic;
using UserAssignment;

namespace UserAssignment.Tests;

public class UserAssignmentTests
{
    private GroupAssignments _groupAssignments;
    private string _dataFilePath;
    public UserAssignmentTests()
    {
        _dataFilePath = @"../../../assets/users.json";
        // Read in the grouped weights for testing        
        var jsonString = File.ReadAllText(@"../net6.0/assets/groupWeights.json");
        _groupAssignments = JsonSerializer.Deserialize<GroupAssignments>(jsonString);
    }
    /// <summary>
    /// Validate there is no extra values in the json payload to ensure the accumulated weights are
    /// not more than 100%
    /// </summary>
    [Fact]
    public void Add_Group_Test()
    {
        UserAssignmentService userAssignmentService = new UserAssignmentService(_dataFilePath);
        foreach(Groups group in _groupAssignments.Groups)
        {
           userAssignmentService.AddGroup(group.name, group.weight);
        }
        Assert.True(userAssignmentService.GroupAssignments.Groups.Max(x => x.weight) == 100);
    }
    /// <summary>
    /// Validate when assigning a user to a group. 
    /// </summary>
    [Fact]
    public void Add_New_User_To_Group_Test()
    {
        UserAssignmentService userAssignmentService = new UserAssignmentService(_dataFilePath);
        foreach(Groups group in _groupAssignments.Groups)
        {
           userAssignmentService.AddGroup(group.name, group.weight);
        }

        var groupName = userAssignmentService.GetUserGroup(Guid.NewGuid());
        Assert.True(!string.IsNullOrEmpty(groupName));
    }
    /// <summary>
    /// Validate when GetUserGroup is called a > 1 time the same group is returned and not added
    /// to the data store.
    /// </summary>
    [Fact]
    public void Add_Duplicate_User_To_Group_Test()
    {
        UserAssignmentService userAssignmentService = new UserAssignmentService(_dataFilePath);
        foreach(Groups group in _groupAssignments.Groups)
        {
           userAssignmentService.AddGroup(group.name, group.weight);
        }
        var guid = Guid.NewGuid();
        var groupName = userAssignmentService.GetUserGroup(guid);

        var file = File.ReadAllText(_dataFilePath);
        int countBefore = JsonSerializer.Deserialize<List<UserGroups>>(file).ToList().Count();

        var groupName1 = userAssignmentService.GetUserGroup(guid);

        file = File.ReadAllText(_dataFilePath);
        int countAfter = JsonSerializer.Deserialize<List<UserGroups>>(file).ToList().Count();
        Assert.True(countBefore == countAfter);
    }
}