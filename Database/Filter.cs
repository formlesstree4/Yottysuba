using System;
using System.Collections.Generic;

namespace YottySuba.Database;

public partial class Filter
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string Description { get; set; } = null!;

    public string Expression { get; set; } = null!;

    public List<int>? Boards { get; set; }

    public bool Active { get; set; }

    public DateTime Created { get; set; }
    
    public string Replace { get; set; } = null!;
}
