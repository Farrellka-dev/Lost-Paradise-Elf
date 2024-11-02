using System.Text.RegularExpressions;
using Content.Server.GameTicking;
using Content.Server.Station.Systems;
using Content.Server.Paper;
using Robust.Shared.Timing;
using Robust.Shared.GameObjects;
using Robust.Shared.IoC;

namespace Content.Server.PrinterSystem
{
    public sealed class Printer
    {
        [Dependency] private readonly StationSystem _station = default!;
        [Dependency] private readonly GameTicker _gameTicker = default!;
        [Dependency] private readonly IGameTiming _gameTiming = default!;
        [Dependency] private readonly IEntityManager _entityManager = default!;

        private static readonly Regex StationIdRegex = new(@".*-(\d+)$");

        public Printer()
        {
            IoCManager.InjectDependencies(this);
        }

        public void FillPaperData(PaperComponent paperComp, EntityUid entityUid)
        {
            string stationName = GetStationName(entityUid);
            string stationNumber = GetStationNumber(entityUid);
            string shiftTime = GetShiftDuration();
            string currentDate = DateTime.Now.AddYears(544).ToString("dd MMMM yyyy");

            paperComp.Content = paperComp.Content
                .Replace("{stationName}", stationName)
                .Replace("{stationNumber}", stationNumber)
                .Replace("{shiftTime}", shiftTime)
                .Replace("{currentDate}", currentDate);

            Dirty(paperComp);
        }

        private string GetStationName(EntityUid entityUid)
        {
            var stationId = _station.GetOwningStation(entityUid);

            if (stationId != null && _entityManager.TryGetComponent<MetaDataComponent>(stationId, out var meta))
            {
                return meta.EntityName;
            }

            return "Unknown Station";
        }

        private string GetStationNumber(EntityUid entityUid)
        {
            var stationId = _station.GetOwningStation(entityUid);

            if (stationId != null && _entityManager.TryGetComponent<MetaDataComponent>(stationId, out var meta))
            {
                var match = StationIdRegex.Match(meta.EntityName);
                return match.Success ? match.Groups[1].Value : "XX-???";
            }

            return "XX-???";
        }

        private string GetShiftDuration()
        {
            var stationTime = _gameTiming.CurTime.Subtract(_gameTicker.RoundStartTimeSpan);
            return stationTime.ToString(@"hh\:mm\:ss");
        }

        private void Dirty(IComponent component)
        {
            _entityManager.Dirty(component);
        }
    }
}
