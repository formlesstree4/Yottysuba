using System;
using System.Collections.Generic;

namespace YottySuba.Database;

public partial class Attachment
{
    public Guid Id { get; set; }

    public string Location { get; set; } = null!;

    public byte[] Hash { get; set; } = null!;

    public DateTime Created { get; set; }

    public bool Deleted { get; set; }

    public long Size { get; set; }
}
