using Agoda.FileDownloaderSystem.DataAccess.Repository;
using Agoda.FileDownloaderSystem.DataObjects.Mappers;
using Agoda.FileDownloaderSystem.DataObjects.Settings;
using Agoda.FileDownloaderSystem.Domain.Interfaces;
using Agoda.FileDownloaderSystem.Utilities;
using Renci.SshNet;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Agoda.FileDownloaderSystem.Domain.Managers
{
    public class FileManager : IFileManager
    {
        private readonly IRepository<Entities.File> _repository;
        private readonly IRepository<Entities.Protocol> _protocolRepository;
        private readonly IRepository<Entities.Status> _statusRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly AppSettings _appSettings;
        public FileManager(IRepository<Entities.File> repository, IRepository<Entities.Protocol> protocolRepository, IRepository<Entities.Status> statusRepository,IUnitOfWork unitOfWork, AppSettings appSettings)
        {
            _repository = repository; 
            _protocolRepository = protocolRepository;
            _statusRepository = statusRepository;
            _unitOfWork = unitOfWork;
            _appSettings = appSettings;
        }

        public IEnumerable<DataObjects.Domain.File> GetAllFileDetails()
        {

            var result = (from m in _repository.Get()
                          join p in _protocolRepository.Get() on m.ProtocolId equals p.ProtocolId
                          join s in _statusRepository.Get() on m.StatusId equals s.StatusId
                          select new Entities.File
                          {
                              Destination = m.Destination,
                              DownloadEndedDate = m.DownloadEndedDate,
                              DownloadSpeed = m.DownloadSpeed,
                              DownloadStartedDate = m.DownloadStartedDate,
                              ElapsedTime = m.ElapsedTime,
                              FileId = m.FileId,
                              IsLargeData = m.IsLargeData,
                              IsSlow = m.IsSlow,
                              PercentageOfFailure = m.PercentageOfFailure,
                              ProtocolId = m.ProtocolId,
                              Protocol = new Entities.Protocol() { ProtocolId = p.ProtocolId, Name = p.Name},
                              Source = m.Source,
                              StatusId = m.StatusId,
                              Status = new Entities.Status() { StatusId = s.StatusId, Name = s.Name}
                          }).ToList();

            var list = result.Select(x => x.ToQueries());
            return list;
        }

        public async Task GetDataFromResponseAndWriteLocalDisk(string sourceUrl, WebResponse httpWebResponse,string protocol)
        {
            long totalSize = 0; 
            DateTime downloadStarted = DateTime.Now; 
            DateTime downloadEnded = DateTime.Now; 
            double elapsedTime = 0; 
            double downloadSpeed = 0;
            int PercentProgress = 0;
            try
            {
                Stream httpResponseStream = httpWebResponse.GetResponseStream();
                totalSize = httpWebResponse.ContentLength;
                int bufferSize = _appSettings.BufferSize;
                if (bufferSize > int.MaxValue) bufferSize = int.MaxValue;
                byte[] buffer = new byte[bufferSize];

                string fileName = GetFileNameFromUrl(sourceUrl);
                if (string.IsNullOrEmpty(fileName)) fileName = "example.txt";
                string path = _appSettings.DownloadedFileLocation + fileName;

                if (!Directory.Exists(_appSettings.DownloadedFileLocation)) Directory.CreateDirectory(_appSettings.DownloadedFileLocation);
                if (File.Exists(path)) fileName = Guid.NewGuid().ToString() + fileName;
                FileStream fileStream = File.Create(_appSettings.DownloadedFileLocation + fileName);
                int bytesRead;
                downloadStarted = DateTime.Now;
                while ((bytesRead = await httpResponseStream.ReadAsync(buffer, 0, bufferSize)) != 0)
                {
                    fileStream.Write(buffer, 0, bytesRead);
                    PercentProgress = Convert.ToInt32((fileStream.Length * 100) / totalSize);
                }
                downloadEnded = DateTime.Now;
                elapsedTime = (downloadEnded - downloadStarted).TotalSeconds; // Total time needed in seconds
                downloadSpeed = Utiles.ConvertBytesToMegabytes(totalSize) / elapsedTime;  // Calculation for Mbps
                
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally 
            {
                var entity = DomainObjectToEntity(sourceUrl, protocol, totalSize, _appSettings.DownloadedFileLocation, downloadStarted, downloadEnded, elapsedTime, downloadSpeed, PercentProgress);
                _repository.Add(entity);
                _unitOfWork.Commit();
            }
        }

        public async Task GetDataFromResponseAndWriteLocalDisk(string sourceUrl, SftpClient sftp, string protocol)
        {
            long totalSize = 0;
            DateTime downloadStarted = DateTime.Now;
            DateTime downloadEnded = DateTime.Now;
            double elapsedTime = 0;
            double downloadSpeed = 0;
            int PercentProgress = 0;
            try
            {
                string fileName = GetFileNameFromUrl(sourceUrl);
                string path = _appSettings.DownloadedFileLocation + fileName;
                if (!Directory.Exists(_appSettings.DownloadedFileLocation)) Directory.CreateDirectory(_appSettings.DownloadedFileLocation);
                if (File.Exists(path)) fileName = Guid.NewGuid().ToString() + fileName;
                FileStream fileStream = File.Create(_appSettings.DownloadedFileLocation + fileName);
                int bufferSize = _appSettings.BufferSize;
                if (bufferSize > int.MaxValue) bufferSize = int.MaxValue;
                byte[] buffer = new byte[bufferSize];
                int bytesRead;
                var sftpFileStream = sftp.OpenRead(path);
                totalSize = sftpFileStream.Length;
                downloadStarted = DateTime.Now;
                while ((bytesRead = await sftpFileStream.ReadAsync(buffer, 0, bufferSize)) != 0)
                {
                    fileStream.Write(buffer, 0, bytesRead);
                    PercentProgress = Convert.ToInt32((fileStream.Length * 100) / sftpFileStream.Length);
                }
                downloadEnded = DateTime.Now;
                elapsedTime = (downloadEnded - downloadStarted).TotalSeconds; // Total time needed in seconds
                downloadSpeed = Utiles.ConvertBytesToMegabytes(totalSize) / elapsedTime;  // Calculation for Mbps
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally 
            {
                var entity = DomainObjectToEntity(sourceUrl, protocol, totalSize, _appSettings.DownloadedFileLocation, downloadStarted, downloadEnded, elapsedTime, downloadSpeed, PercentProgress);
                _repository.Add(entity);
                _unitOfWork.Commit();
            }
        }

        public string GetFileNameFromUrl(string url)
        {
            if (!Uri.TryCreate(url, UriKind.Absolute, out Uri uri))
                uri = new Uri(url);
            return Path.GetFileName(uri.LocalPath);
        }

        public string GetProtocolFromSource(string source)
        {
            string protocol = string.Empty;
            Uri address = new Uri(source);
            if (address.Scheme == Uri.UriSchemeFtp) protocol = Uri.UriSchemeFtp;
            else if (address.Scheme == Uri.UriSchemeHttp) protocol = Uri.UriSchemeHttp;
            else if (address.Scheme == Uri.UriSchemeHttps) protocol = Uri.UriSchemeHttps;
            else if (address.Scheme == "sftp") protocol = "sftp";
            else if (address.Scheme == Uri.UriSchemeNetTcp) protocol = Uri.UriSchemeNetTcp;
            else if (address.Scheme == Uri.UriSchemeNetPipe) protocol = Uri.UriSchemeNetPipe;
            return protocol;
        }

        private Entities.File DomainObjectToEntity(string sourceUrl, string protocol, long totalSize, string path, DateTime downloadStarted, DateTime downloadEnded, double elapsedTime, double downloadSpeed,int percentProgress)
        {
            return FileMapper.ToCommand(new DataObjects.Domain.File
            {
                Source = sourceUrl,
                Destination = path,
                DownloadStartedDate = downloadStarted,
                DownloadEndedDate = downloadEnded,
                IsLargeData = totalSize > _appSettings.VolumeOfData ? "true" : "false",
                IsSlow = downloadSpeed > _appSettings.VelocityOfData ? "false" : "true",
                Protocol = protocol,
                ProtocolId = DataObjects.Enums.Protocol.GetKey(protocol),
                StatusId = percentProgress == 100 ?  DataObjects.Enums.Status.GetKey(DataObjects.Enums.Status.Completed) : DataObjects.Enums.Status.GetKey(DataObjects.Enums.Status.Failed),
                PercentageOfFailure = percentProgress == 100 ? 0 : percentProgress,
                DownloadSpeed = downloadSpeed,
                ElapsedTime = elapsedTime
            });
        }
    }
}
