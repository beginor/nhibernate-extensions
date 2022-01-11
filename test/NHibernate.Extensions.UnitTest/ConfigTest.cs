using System.Reflection;
using Microsoft.Extensions.Configuration;
using NHibernate.Cfg;

namespace NHibernate.Extensions.UnitTest;

[TestFixture]
public class ConfigTest {

    private IConfiguration configuration;

    [OneTimeSetUp]
    public void OneTimeSetUp() {
        configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json")
            .Build();
    }

    [Test]
    public void _01_CanReadConfig() {
        Assert.NotNull(configuration);
        var nhibernate = configuration
            .GetSection("nhibernate").GetChildren();
        Console.WriteLine(nhibernate);

        foreach (var section in nhibernate) {
            var cfg = new Configuration();
            foreach (var pair in section.AsEnumerable(true)) {
                if (string.IsNullOrEmpty(pair.Value)) {
                    continue;
                }
                if (pair.Key.StartsWith("mapping:assembly")) {
                    var asm = pair.Value;
//                        if (!asm.EndsWith(".dll")) {
//                            asm += ".dll";
//                        }
                    cfg.AddAssembly(asm);
                }
                else if (pair.Key.StartsWith("mapping:file")) {
                    cfg.AddFile(pair.Value);
                }
                else if (pair.Key.StartsWith("mapping:resource")) {
                    var resAndAsm = pair.Value.Split(
                        ",",
                        StringSplitOptions.RemoveEmptyEntries
                    );
                    var res = resAndAsm[0].Trim();
                    var asm = resAndAsm[1].Trim();
                    if (!asm.EndsWith(".dll")) {
                        asm += ".dll";
                    }
                    cfg.AddResource(res, Assembly.LoadFrom(asm));
                }
                else {
                    cfg.SetProperty(pair.Key, pair.Value);
                }
            }
            var sf = cfg.BuildSessionFactory();
            Assert.IsNotNull(sf);
        }
    }

}
