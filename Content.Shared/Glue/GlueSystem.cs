using Content.Shared.Interaction;


namespace Content.Shared.Glue;

public sealed class GlueSystem : EntitySystem
{
    public override void Initialize()
    {
        base.Initialize();

        SubscribeLocalEvent<GlueComponent, AfterInteractEvent>(AfterInteract);
    }

    private void AfterInteract(EntityUid uid, GlueComponent component, AfterInteractEvent args)
    {
        var target = args.Target;

        ComponentAdd;
    }
}
