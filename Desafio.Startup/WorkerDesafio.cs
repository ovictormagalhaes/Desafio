using Desafio.Core.Interface.Crawlers;
using Desafio.Core.Interface.Repositories;
using Desafio.Core.Interface.Services;
using Desafio.Infraestruture.Settings;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Desafio.Startup
{
    public class WorkerDesafio : BackgroundService
    {
        private readonly ILogger<WorkerDesafio> _logger;
        private readonly ILoremService _loremService;
        private readonly IMotherEffService _motherEffService;
        private readonly IFileRepository _fileRepository;
        private string filePath;
        private string textBuffer;
        private long bytesBuffer;
        private long bytesFile;
        private List<DateTime> iterations;

        public WorkerDesafio(ILogger<WorkerDesafio> logger, ILoremService loremService, IMotherEffService motherEffService, IFileRepository fileRepository)
        {
            _logger = logger;
            _loremService = loremService;
            _motherEffService = motherEffService;
            _fileRepository = fileRepository;
            filePath = Path.Combine(WorkerSettings.FilePath, $"lorem {DateTime.Now.Ticks}.txt");
            textBuffer = string.Empty;
            iterations = new List<DateTime>();
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var lorem = await _loremService.GetData();
            var motherEff = await _motherEffService.GetData(lorem.Text);            
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    if (bytesBuffer + motherEff.Bytes < WorkerSettings.MaxBufferBytes)
                    {
                        textBuffer += lorem.Text;
                        bytesBuffer += motherEff.Bytes;
                    }
                    else if (bytesFile > WorkerSettings.MAX_FILE_SIZE_BYTES)
                    {
                        int numberIterations = iterations.Count;
                        var totalTime = TimeSpan.FromTicks(iterations[numberIterations - 1].Ticks - iterations[0].Ticks).TotalSeconds;
                        var averageTime = TimeSpan.FromSeconds(totalTime / numberIterations).TotalSeconds;
                        var fileInfo = new FileInfo(filePath);
                        var fileName = fileInfo.FullName;
                        var fileSize = fileInfo.Length;
                        _logger.LogInformation("\n1 - Number Iterations: {0}\n2 - Total Time (seconds): {1}, Average Time (seconds): {2}\n3 - File Name: {3},  File Size (bytes): {4}", numberIterations, totalTime.ToString("0.##"), averageTime.ToString("0.###"), fileName, fileSize);
                        await StopAsync(stoppingToken);
                    }
                    else
                    {
                        iterations.Add(DateTime.Now);
                        await _fileRepository.Append(filePath, textBuffer);
                        bytesFile += bytesBuffer;
                        textBuffer = lorem.Text;
                        bytesBuffer = motherEff.Bytes;
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogWarning(ex.Message);
                }
            }
        }
    }
}
