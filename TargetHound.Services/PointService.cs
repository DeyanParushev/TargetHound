namespace TargetHound.Services
{
    using Microsoft.EntityFrameworkCore.Internal;
    using System.Linq;
    using System.Threading.Tasks;

    using TargetHound.Calcualtions;
    using TargetHound.Data;
    using TargetHound.Models;
    using TargetHound.Services.Interfaces;


    public class PointService : IPointService
    {
        private readonly TargetHoundContext dbContext;
        private readonly CoordinatesSetter coordinatesSetter;

        public PointService(TargetHoundContext dbContext, CoordinatesSetter coordinatesSetter)
        {
            this.dbContext = dbContext;
            this.coordinatesSetter = coordinatesSetter;
        }

        public async Task<bool> AddPoint(string boreholeId, double depth, double azimuth, double dip)
        {
            if (!this.dbContext.Boreholes.Any(x => x.Id == boreholeId))
            {
                return false;
            }

            Borehole borehole = this.dbContext.Boreholes.SingleOrDefault(x => x.Id == boreholeId);
            SurveyPoint lastStation = (SurveyPoint)borehole.SurveyPoints.TakeLast(1);
            SurveyPoint newStation = new SurveyPoint
            {
                Depth = depth,
                Azimuth = azimuth,
                Dip = dip,
            };

            this.coordinatesSetter.SetBottomStationUTMCoortinates(lastStation, newStation);

            borehole.SurveyPoints.Add(newStation);
            await this.dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ChangePointAzimuth(string pointId, double newAzimuth)
        {
            SurveyPoint station = this.dbContext.SurveyPoints.SingleOrDefault(x => x.Id == pointId);
            int stationIndex = station.Borehole.SurveyPoints.IndexOf(station);
           
            if (station == null)
            {
                return false;
            }
            station.Azimuth = newAzimuth;

            SurveyPoint previousStation = (SurveyPoint)station.Borehole.SurveyPoints[stationIndex - 1];
            this.coordinatesSetter.SetBottomStationUTMCoortinates(previousStation, station);
            await this.dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ChangePointDip(string pointId, double newDip)
        {
            SurveyPoint station = this.dbContext.SurveyPoints.SingleOrDefault(x => x.Id == pointId);
            int stationIndex = station.Borehole.SurveyPoints.IndexOf(station);

            if (station == null)
            {
                return false;
            }
            station.Dip = newDip;

            SurveyPoint previousStation = (SurveyPoint)station.Borehole.SurveyPoints[stationIndex - 1];
            this.coordinatesSetter.SetBottomStationUTMCoortinates(previousStation, station);
            await this.dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ChangePointDepth(string pointId, double newDepth)
        {
            SurveyPoint station = this.dbContext.SurveyPoints.SingleOrDefault(x => x.Id == pointId);
            int stationIndex = station.Borehole.SurveyPoints.IndexOf(station);

            if (station == null)
            {
                return false;
            }
            station.Depth = newDepth;

            SurveyPoint previousStation = (SurveyPoint)station.Borehole.SurveyPoints[stationIndex - 1];
            this.coordinatesSetter.SetBottomStationUTMCoortinates(previousStation, station);
            await this.dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeletePoint(string pointId)
        {
            SurveyPoint surveyPoint = this.dbContext.SurveyPoints.SingleOrDefault(x => x.Id == pointId);

            if(surveyPoint == null)
            {
                return false;
            }

            surveyPoint.IsDeleted = true;
            await this.dbContext.SaveChangesAsync();
            return true;
        }
    }
}
