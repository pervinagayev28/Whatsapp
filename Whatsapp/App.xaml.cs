using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using Whatsapp.DbContexts;
using Whatsapp.Services;

namespace Whatsapp
{
    public partial class App : Application
    {
        public static IServiceProvider? _serviceProvider;

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            _serviceProvider = new ServiceCollection()
                .AddDbContext<MyChatingAppContext>(options =>
                    options.UseSqlServer(Configuration.GetValue("ConnectionStrings", "SqlConnectionString")))
                .BuildServiceProvider();
        }
    }
}

