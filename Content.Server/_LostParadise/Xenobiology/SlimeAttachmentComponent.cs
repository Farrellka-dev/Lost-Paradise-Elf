using Robust.Shared.GameObjects;
using Robust.Shared.IoC;
using Robust.Shared.Timing;
using Content.Shared.Damage;

namespace Content.Server._LostParadise.Xenobiology
{
    [RegisterComponent]
    public sealed partial class SlimeAttachmentComponent : Component
    {
        private float _cellularDamageRate = 0.5f;
        private bool _isAttached = false;
        private EntityUid _target = EntityUid.Invalid;

        [Dependency] private readonly IGameTiming _gameTiming = default!;
        private IEntityManager _entityManager = default!;

        public void Initialize()
        {
            IoCManager.InjectDependencies(this);
        }

        public void AttachToTarget(EntityUid target)
        {
            _target = target;
            _isAttached = true;

            if (_entityManager.TryGetComponent<TransformComponent>(_target, out var targetTransform) &&
                _entityManager.TryGetComponent<TransformComponent>(Owner, out var ownerTransform))
            {
                ownerTransform.AttachParent(targetTransform);
            }

            Owner.SpawnTimer(1000, TickDamage);
        }

        private void TickDamage()
        {
            if (_isAttached && _target != EntityUid.Invalid)
            {
                // if (_entityManager.TryGetComponent(_target, out DamageableComponent damage))
                // {
                //     damage.TakeDamage(DamageType.Cellular, _cellularDamageRate);
                //     if (damage.Health <= 0)
                //     {
                        _isAttached = false;
                        Owner.SpawnTimer(5000, GrowToAdult);
                //     }
                //     else
                //     {
                //         Owner.SpawnTimer(1000, TickDamage);
                //     }
                // }
            }
        }

        private void GrowToAdult()
        {
            _entityManager.SpawnEntity("LPPMobAdultSlimeGrey", _entityManager.GetComponent<TransformComponent>(Owner).Coordinates);  // Spawn the adult slime
            _entityManager.DeleteEntity(Owner);  // Delete the current entity
        }

    }
}
