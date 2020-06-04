/*
*
* �ļ���    ��Program     
* ����ʱ��  : 2020-05-21 11:44
* ��ϵ����  : QQ:779149549 
* ������Ⱥ  : QQȺ:874752819
* �ʼ���ϵ  : zhouhaogg789@outlook.com
* ��Ƶ�̳�  : https://space.bilibili.com/32497462
* ���͵�ַ  : https://www.cnblogs.com/zh7791/
* ��Ŀ��ַ  : https://github.com/HenJigg/WPF-Xamarin-Blazor-Examples
* ��Ŀ˵��  : �������д��������Դ���ʹ��,��ֹ������Ϊ���۱���ĿԴ����
*/


namespace Consumption.Api
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Consumption.EFCore.Orm;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using Microsoft.Extensions.Logging;
    using Microsoft.OpenApi.Writers;
    using NLog.Web;

    /// <summary>
    /// 
    /// </summary>
    public class Program
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="args"></param>
        public static void Main(string[] args)
        {
            var logger = NLogBuilder.ConfigureNLog("nlog.config").GetCurrentClassLogger();
            try
            {
                var host = CreateHostBuilder(args).Build();
                //���μ��ز�����������
                using (var scope = host.Services.CreateScope())
                {
                    var serivces = scope.ServiceProvider;
                    var context = serivces.GetRequiredService<ConsumptionContext>();
                    ConsumptionHelper.InitSampleDataAsync(context).Wait();
                }
                host.Run();
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Stopped program because of exception");
                throw;
            }
            finally
            {
                NLog.LogManager.Shutdown();
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>()
                     .ConfigureLogging(logging =>
                     {
                         logging.ClearProviders();
                         logging.SetMinimumLevel(Microsoft.Extensions.Logging.LogLevel.Trace);
                     }).UseNLog();
                });
    }
}