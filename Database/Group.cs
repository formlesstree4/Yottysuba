using System;
using System.Collections.Generic;

namespace YottySuba.Database;

public partial class Group
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public List<int> Boards { get; set; } = null!;

    public int Order { get; set; }
}
