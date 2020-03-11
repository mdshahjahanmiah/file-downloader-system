using System;
using System.Collections.Generic;
using System.Text;

namespace Agoda.FileDownloaderSystem.DataObjects.Mappers
{
    public static class FileMapper
    {
        public static Entities.File ToCommand(this DataObjects.Domain.File model)
        {
            return new Entities.File() 
            { 
                FileId = model.FileId,
                Source = model.Source,
                Destination = model.Destination,
                DownloadStartedDate = model.DownloadStartedDate,
                DownloadEndedDate = model.DownloadEndedDate,
                IsLargeData = Convert.ToBoolean(model.IsLargeData),
                IsSlow = Convert.ToBoolean(model.IsSlow),
                PercentageOfFailure = model.PercentageOfFailure,
                DownloadSpeed = model.DownloadSpeed,
                ElapsedTime = model.ElapsedTime,
                ProtocolId = model.ProtocolId,
                StatusId = model.StatusId
            };
        }
        public static Domain.File ToQueries(this Entities.File model)
        {
            return new Domain.File()
            {
                FileId = model.FileId,
                Source = model.Source,
                Destination = model.Destination,
                DownloadStartedDate = model.DownloadStartedDate,
                DownloadEndedDate = model.DownloadEndedDate,
                IsLargeData = Convert.ToString(model.IsLargeData),
                IsSlow = Convert.ToString(model.IsSlow),
                PercentageOfFailure = model.PercentageOfFailure,
                DownloadSpeed = model.DownloadSpeed,
                ElapsedTime = model.ElapsedTime,
                ProtocolId = model.ProtocolId,
                StatusId = model.StatusId,
                Protocol = model.Protocol.Name,
                Status = model.Status.Name
            };
        }

    }
}
