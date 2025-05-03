using System;
using System.Collections.Generic;

namespace YottySuba.Database;

public partial class Board
{
    public int Id { get; set; }

    public string Code { get; set; } = null!;

    public string Name { get; set; } = null!;

    public string Description { get; set; } = null!;

    public bool AgeRestricted { get; set; }

    public bool IsReadonly { get; set; }

    public bool Enabled { get; set; }

    public DateTime Created { get; set; }

    public DateTime LastUpdated { get; set; }

    public List<string>? Extensions { get; set; }

    public int MaxActiveThreads { get; set; }

    public int MaxPostsInThread { get; set; }

    public long MaxFilesizeInBytes { get; set; }

    public virtual ICollection<Post> Posts { get; set; } = new List<Post>();
}
