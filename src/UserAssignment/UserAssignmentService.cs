using System;
using System.IO;
using System.Text;
using System.Text.Json;
using System.Collections;

namespace UserAssignment;
public class UserAssignmentService
{
    /// <summary>
    /// The accumulated weight for all entries.
    /// </summary>
    private double _totalWeight = 0.0; 
    private Random random = new Random();
    private string _filePath;
    public GroupAssignments GroupAssignments { get;set; } =  new GroupAssignments();
    public UserAssignmentService(string filePath)
    {
        _filePath = filePath;
    }
    
    /// <summary>
    /// Adds a new entry into the list of Groups for randomizing user assignment.
    /// </summary>
    /// <param name="name"></param>
    /// <param name="weight"></param>
    public void AddGroup(string name, double weight)
    {
        _totalWeight += (weight * 100);
        GroupAssignments.Groups.Add(new Groups
        {
            name = name,
            weight = _totalWeight
        });
    }
    /// <summary>
    /// Gets the group a user is in. If the user is not already in a group, they are dynamically assigned based off of the
    /// weighted amounts in the groupWeights.json file.
    /// </summary>
    /// <param name="newUserId"></param>
    /// <returns></returns>
    public string GetUserGroup(Guid newUserId)
    {
        // Get random number
        var randomNumber = random.NextDouble(0.0, _totalWeight);

        #region Group Assignment
        // Get the group to be assigned to
        Groups groupAssignment = default(Groups);
        foreach (var group in GroupAssignments.Groups)
        {
            if(group.weight >= randomNumber)
            {
                groupAssignment = group;
                break;
            }
        }
        // Assign the group and store it. 
        UserGroups assignedUser = new UserGroups
        {
            UserId = newUserId,
            AssignedGroup = groupAssignment
        };
        // Read current data
        var file = File.ReadAllText(_filePath);
        List<UserGroups> assignedUsers = JsonSerializer.Deserialize<List<UserGroups>>(file);

        // Search for duplicates
        UserGroups userAssigned = assignedUsers.Find(x => x.UserId == newUserId);

        if(userAssigned == default(UserGroups))   // Save new user if not found
        {
            assignedUsers.Add(assignedUser);
            var jsonString = JsonSerializer.Serialize<List<UserGroups>>(assignedUsers);
            File.WriteAllText(_filePath, jsonString);
            return assignedUser.AssignedGroup.name;
        } 
        else                                        // return current group
        {
            return userAssigned.AssignedGroup.name;
        }
        #endregion
    }
}
