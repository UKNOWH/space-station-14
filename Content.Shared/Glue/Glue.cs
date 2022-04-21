namespace Content.Shared.Glue;

[RegisterComponent]
public sealed class GlueComponent : Component
{
    [DataField("stickDelay")]
    [ViewVariables(VVAccess.ReadWrite)]
    public TimeSpan GlueStickDelay = TimeSpan.Zero;

    [DataField("unstickDelay")]
    [ViewVariables(VVAccess.ReadWrite)]
    public TimeSpan GlueUnstickDelay = TimeSpan.Zero;

    [DataField("applyDelay")]
    [ViewVariables(VVAccess.ReadWrite)]
    public TimeSpan ApplyDelay = TimeSpan.Zero;
}
