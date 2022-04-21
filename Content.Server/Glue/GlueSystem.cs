

using Content.Server.Sticky.Components;
using Content.Shared.Interaction;

namespace Content.Server.Glue;

public sealed class GlueSystem : EntitySystem
{
    public override void Initialize()
    {
        base.Initialize();

        SubscribeLocalEvent<GlueComponent, AfterInteractEvent>(OnAfterInteract);
    }

    private void OnAfterInteract(EntityUid uid, GlueComponent component, AfterInteractEvent args)
    {
        if (args.Target == null) return;
        var newComp = EntityManager.AddComponent<StickyComponent>(args.Target.Value);
        newComp.StickDelay = component.GlueStickDelay;
        newComp.UnstickDelay = component.GlueUnstickDelay;
    }
}
