using System;
using System.Web;
using System.Web.Mvc;
using Microsoft.FeatureManagement;

namespace DevPilot.TAD.Tests.Controllers.Models
{
    public class TestFeatureManager : IFeatureManager
    {
        private readonly bool _isEnabled;
        private readonly Exception _exception;

        public TestFeatureManager(bool isEnabled = true, Exception exception = null)
        {
            _isEnabled = isEnabled;
            _exception = exception;
        }

        public Task<bool> IsEnabledAsync(string featureName)
        {
            if (_exception != null)
            {
                throw _exception;
            }
            return Task.FromResult(_isEnabled);
        }
    }

    public class TestHttpContext : HttpContextBase
    {
        private readonly HttpSessionStateBase _session;

        public TestHttpContext(HttpSessionStateBase session)
        {
            _session = session;
        }

        public override HttpSessionStateBase Session => _session;
    }

    public class TestSession : HttpSessionStateBase
    {
        private readonly object _featureManager;

        public TestSession(object featureManager)
        {
            _featureManager = featureManager;
        }

        public override object this[string name]
        {
            get => name == "FeatureManager" ? _featureManager : null;
            set { }
        }
    }

    public class TestControllerContext : ControllerContext
    {
        public TestControllerContext(HttpContextBase httpContext, RouteData routeData, Controller controller)
            : base(httpContext, routeData, controller)
        {
        }
    }
}
