using System;
using System.Collections.Generic;

namespace YottySuba.Database;

public partial class Ban
{
    public byte[] Ip { get; set; } = null!;

    public string Reason { get; set; } = null!;

    public DateTime Created { get; set; }
}
