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

        public void GetDataFromResponseAndWriteLocalDisk(string sourceUrl, WebResponse httpWebResponse,string protocol)
        {
            try 
            {
                Stream httpResponseStream = httpWebResponse.GetResponseStream();
                var totalSize = httpWebResponse.ContentLength;
                int bufferSize = _appSettings.BufferSize;
                if (bufferSize > int.MaxValue) bufferSize = int.MaxValue;
                byte[] buffer = new byte[bufferSize];

                string fileName = GetFileNameFromUrl(sourceUrl);
                if (string.IsNullOrEmpty(fileName)) fileName = "example.txt";
                string path = _appSettings.DownloadedFileLocation + fileName;
                if (System.IO.File.Exists(path)) fileName = System.Guid.NewGuid().ToString() + fileName;
                FileStream fileStream = System.IO.File.Create(_appSettings.DownloadedFileLocation + fileName);
                int bytesRead;
                int PercentProgress = 0;
                DateTime downloadStarted = DateTime.Now;
                while ((bytesRead = httpResponseStream.Read(buffer, 0, bufferSize)) != 0)
                {
                    fileStream.Write(buffer, 0, bytesRead);
                    PercentProgress = Convert.ToInt32((fileStream.Length * 100) / totalSize);
                }
                DateTime downloadEnded = DateTime.Now;
                var elapsedTime = (downloadEnded - downloadStarted).TotalSeconds; // Total time needed in seconds
                var downloadSpeed = Utiles.ConvertBytesToMegabytes(totalSize) / elapsedTime;  // Calculation for Mbps
                var entity = DomainObjectToEntity(sourceUrl, protocol, totalSize, path, downloadStarted, downloadEnded, elapsedTime, downloadSpeed);
                _repository.Add(entity);
                _unitOfWork.Commit();
            }
            catch (Exception ex)
            { 

            }
        }

        public void GetDataFromResponseAndWriteLocalDisk(string sourceUrl, SftpClient sftp, string protocol)
        {
            try 
            {
                string fileName = GetFileNameFromUrl(sourceUrl);
                string path = _appSettings.DownloadedFileLocation + fileName;
                if (System.IO.File.Exists(path)) fileName = System.Guid.NewGuid().ToString() + fileName;
                FileStream fileStream = System.IO.File.Create(_appSettings.DownloadedFileLocation + fileName);
                int bufferSize = _appSettings.BufferSize;
                if (bufferSize > int.MaxValue) bufferSize = int.MaxValue;
                byte[] buffer = new byte[bufferSize];
                int bytesRead;
                int PercentProgress = 0;
                var sftpFileStream = sftp.OpenRead(path);
                var totalSize = sftpFileStream.Length;
                DateTime downloadStarted = DateTime.Now;
                while ((bytesRead = sftpFileStream.Read(buffer, 0, bufferSize)) != 0)
                {
                    fileStream.Write(buffer, 0, bytesRead);
                    PercentProgress = Convert.ToInt32((fileStream.Length * 100) / sftpFileStream.Length);
                }
                DateTime downloadEnded = DateTime.Now;
                var elapsedTime = (downloadEnded - downloadStarted).TotalSeconds; // Total time needed in seconds
                var downloadSpeed = Utiles.ConvertBytesToMegabytes(totalSize) / elapsedTime;  // Calculation for Mbps
                var entity = DomainObjectToEntity(sourceUrl, protocol, totalSize, path, downloadStarted, downloadEnded, elapsedTime, downloadSpeed);
                _repository.Add(entity);
                _unitOfWork.Commit();
            }
            catch (Exception ex) 
            {

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

        private Entities.File DomainObjectToEntity(string sourceUrl, string protocol, long totalSize, string path, DateTime downloadStarted, DateTime downloadEnded, double elapsedTime, double downloadSpeed)
        {
            return FileMapper.ToCommand(new DataObjects.Domain.File
            {
                Source = sourceUrl,
                Destination = path,
                DownloadStartedDate = downloadStarted,
                DownloadEndedDate = downloadEnded,
                IsLargeData = totalSize > _appSettings.VolumeOfData ? true : false,
                IsSlow = downloadSpeed > _appSettings.VelocityOfData ? false : true,
                Protocol = protocol,
                ProtocolId = DataObjects.Enums.Protocol.GetKey(protocol),
                StatusId = DataObjects.Enums.Status.GetKey(DataObjects.Enums.Status.Completed),
                PercentageOfFailure = 0,
                DownloadSpeed = downloadSpeed,
                ElapsedTime = elapsedTime
            });
        }
    }
}
