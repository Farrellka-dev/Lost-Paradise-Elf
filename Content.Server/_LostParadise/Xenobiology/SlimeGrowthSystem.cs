// using Robust.Shared.GameObjects;
// using Content.Shared.GameObjects.EntitySystems;

// namespace Content.Server.Slime
// {
//     public class SlimeGrowthSystem : EntitySystem
//     {
//         public override void Initialize()
//         {
//             base.Initialize();
//             SubscribeLocalEvent<SlimeAttachmentComponent, StartCollideEvent>(OnCollide);
//         }

//         private void OnCollide(EntityUid uid, SlimeAttachmentComponent component, StartCollideEvent args)
//         {
//             if (args.OtherEntity.HasComponent<MonkeyComponent>())
//             {
//                 component.AttachToTarget(args.OtherEntity);
//             }
//         }
//     }
// }
