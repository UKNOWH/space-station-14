using Content.Shared.Whitelist;

namespace Content.Server.Glue;

[RegisterComponent]
public sealed class GlueComponent : Component
{
    [DataField("usesLeft")]
    [ViewVariables(VVAccess.ReadWrite)]
    public int UsesLeft = 10;

    [DataField("stickDelay")]
    [ViewVariables(VVAccess.ReadWrite)]
    public TimeSpan GlueStickDelay = TimeSpan.Zero;

    [DataField("unstickDelay")]
    [ViewVariables(VVAccess.ReadWrite)]
    public TimeSpan GlueUnstickDelay = TimeSpan.Zero;

    [DataField("applyDelay")]
    [ViewVariables(VVAccess.ReadWrite)]
    public float ApplyDelay = 0f;

    [DataField("isApplying")]
    public bool IsApplying = false;

    [DataField("whitelist")]
    [ViewVariables(VVAccess.ReadWrite)]
    public EntityWhitelist? Whitelist;


}
