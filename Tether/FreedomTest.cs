﻿using System;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using SocksSharp;
using SocksSharp.Proxy;

namespace Tether
{
    internal class FreedomTest : ITest
    {
        private static readonly ProxySettings ProxySettings = new ProxySettings
        {
            Host = "127.0.0.1",
            Port = 9150
        };

        private static readonly ProxyClientHandler<Socks5> ProxyClientHandler =
            new ProxyClientHandler<Socks5>(ProxySettings);

        private static readonly HttpClient HttpClient = new HttpClient(ProxyClientHandler);

        private readonly IReportManager _reportManager;
        private readonly string _hostName;

        private bool _result;

        public FreedomTest(IReportManager reportManager)
        {
            _reportManager = reportManager;
            _hostName = Encoding.UTF8.GetString(Convert.FromBase64String("aHR0cHM6Ly9wb3JuaHViLmNvbQ=="));
        }

        public async Task<bool> Run()
        {
            _result = false;
            try
            {
                var stream = await HttpClient.GetStreamAsync(_hostName);
                _result = true;
            }
            catch (Exception e) when (e is HttpRequestException || e is ProxyException ||
                                      e is OperationCanceledException || e is IOException)
            {
            }

            return _result;
        }

        public void Report()
        {
            var (message, type) = _result ? ("OK", MessageType.Success) : ("Not working", MessageType.Failure);
            
            _reportManager.Print("Freedom: ", MessageType.Info);
            _reportManager.Print(message, type);
            _reportManager.PrintLine();
        }
    }
}