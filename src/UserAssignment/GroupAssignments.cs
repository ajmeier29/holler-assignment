using System.Linq;
using System.Collections;

namespace UserAssignment;
public class GroupAssignments
{
    public List<Groups> Groups { get; set; } = new List<Groups>();
}

public class Groups
{
    public string name { get; set;}
    public double weight { get; set;}
}
