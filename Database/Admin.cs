using System;
using System.Collections.Generic;

namespace YottySuba.Database;

public partial class Admin
{
    public long Id { get; set; }

    public string Username { get; set; } = null!;

    public byte[] Password { get; set; } = null!;

    public byte[] Salt { get; set; } = null!;

    public DateTime Created { get; set; }

    public bool Disabled { get; set; }
}
