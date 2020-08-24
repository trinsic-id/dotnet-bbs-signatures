using System.Reflection;

using Android.App;
using Android.OS;
using Java.Lang;
using Xamarin.Android.NUnitLite;

namespace Hyperledger.Ursa.BbsSignatures.Tests.Android
{
    [Activity(Label = "BbsSignatures.Tests.Android", MainLauncher = true)]
    public class MainActivity : TestSuiteActivity
    {
        protected override void OnCreate(Bundle bundle)
        {
            JavaSystem.LoadLibrary("bbs");

            // tests can be inside the main assembly
            AddTest(Assembly.GetExecutingAssembly());
            // or in any reference assemblies
            // AddTest (typeof (Your.Library.TestClass).Assembly);

            // Once you called base.OnCreate(), you cannot add more assemblies.
            base.OnCreate(bundle);
        }
    }
}
