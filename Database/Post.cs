using System;
using System.Collections.Generic;

namespace YottySuba.Database;

public partial class Post
{
    public long Id { get; set; }

    public int Board { get; set; }

    public string Name { get; set; } = null!;

    public string Subject { get; set; } = null!;

    public byte[] DeletePassword { get; set; } = null!;

    public Guid? File { get; set; }

    public DateTime Created { get; set; }

    public byte[]? IpV4 { get; set; }

    public byte[]? IpV6 { get; set; }

    public string Message { get; set; } = null!;

    public bool IsStart { get; set; }

    public DateTime LastUpdated { get; set; }

    public long? Parent { get; set; }

    public virtual Board BoardNavigation { get; set; } = null!;

    public virtual ICollection<Post> InverseParentNavigation { get; set; } = new List<Post>();

    public virtual Post? ParentNavigation { get; set; }
}
