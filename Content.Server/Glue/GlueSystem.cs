
using Content.Server.DoAfter;
using Content.Server.Popups;
using Content.Server.Sticky.Components;
using Content.Shared.Examine;
using Content.Shared.Interaction;
using Robust.Shared.Player;

namespace Content.Server.Glue;

public sealed class GlueSystem : EntitySystem
{
    [Dependency] private readonly DoAfterSystem _doAfterSystem = default!;
    [Dependency] private readonly PopupSystem _popupSystem = default!;
    public override void Initialize()
    {
        base.Initialize();

        SubscribeLocalEvent<GlueComponent, AfterInteractEvent>(OnAfterInteract);
        SubscribeLocalEvent<GlueComponent, ActivateInWorldEvent>(OnActivate);
        SubscribeLocalEvent<GlueDoAfterComplete>(OnDoAfterComplete);
        SubscribeLocalEvent<GlueDoAfterCancelled>(OnDoAfterCancelled);
        SubscribeLocalEvent<GlueComponent, ExaminedEvent>(OnGlueExamined);
    }

    private void OnGlueExamined(EntityUid uid, GlueComponent component, ExaminedEvent args)
    {
        if (args.IsInDetailsRange)
        {
            args.PushMarkup(
                Loc.GetString(
                    "flash-component-examine-detail-count",
                    ("count", component.UsesLeft),
                    ("markupCountColor", "green")
                )
            );
        }
    }

    private void OnActivate(EntityUid uid, GlueComponent component, ActivateInWorldEvent args)
    {
        args.Handled = true;
    }

    private sealed class GlueDoAfterComplete : EntityEventArgs
    {
        public readonly EntityUid User;
        public readonly EntityUid Target;
        public readonly GlueComponent Component;

        public GlueDoAfterComplete(EntityUid user, EntityUid target, GlueComponent component)
        {
            Target = target;
            Component = component;
            User = user;
        }
    }

    private void OnDoAfterComplete(GlueDoAfterComplete ev)
    {
        var newComp = EntityManager.AddComponent<StickyComponent>(ev.Target);
        newComp.StickDelay = ev.Component.GlueStickDelay;
        newComp.UnstickDelay = ev.Component.GlueUnstickDelay;
        ev.Component.UsesLeft --;
        ev.Component.IsApplying = false;
        var msg = Loc.GetString("glue-applied") + ev.Target;
        _popupSystem.PopupEntity(msg, ev.User, Filter.Entities(ev.User));
    }

    private sealed class GlueDoAfterCancelled : EntityEventArgs
    {
        public readonly GlueComponent Component;

        public GlueDoAfterCancelled(GlueComponent component)
        {
            Component = component;
        }
    }

    private void OnDoAfterCancelled(GlueDoAfterCancelled ev)
    {
        ev.Component.IsApplying = false;
    }
    private void OnAfterInteract(EntityUid uid, GlueComponent component, AfterInteractEvent args)
    {
        if (component.IsApplying || args.Target == null || EntityManager.HasComponent<StickyComponent>(args.Target)) return;
        if (component.UsesLeft == 0)
        {
            var msg = Loc.GetString("glue-out");
            _popupSystem.PopupEntity(msg, args.User, Filter.Entities(args.User));
            return;
        }
        component.IsApplying = true;

        var doAfterEventArgs = new DoAfterEventArgs(args.User, component.ApplyDelay, default, args.Target)
        {
                BreakOnTargetMove = true,
                BreakOnUserMove = true,
                BreakOnDamage = true,
                BreakOnStun = true,
                NeedHand = true,
                BroadcastFinishedEvent = new GlueDoAfterComplete(args.User,args.Target!.Value, component),
                BroadcastCancelledEvent = new GlueDoAfterCancelled(component),
        };
        _doAfterSystem.DoAfter(doAfterEventArgs);
    }
}
