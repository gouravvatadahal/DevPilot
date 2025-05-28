using Moq;
using Xunit;

namespace DevPilot.TDD.WS.Tests.Controllers
{
    public class FeaturesControllerTests
    {
        private readonly FeaturesController _controller;
        private readonly Mock<IFeatureManager> _featureManager;
        private readonly Mock<IViewService> _viewService;

        public FeaturesControllerTests()
        {
            _featureManager = new Mock<IFeatureManager>();
            _viewService = new Mock<IViewService>();
            _controller = new FeaturesController(_featureManager.Object, _viewService.Object);
        }

        [Fact]
        public void Index_SuccessfulNavigation_ShouldDisplayFeaturesList()
        {
            // Arrange
            var enabledFeatures = new List<string> { "NewFeature", "BetaFeature" };
            _featureManager.Setup(fm => fm.GetEnabledFeatures()).Returns(enabledFeatures);
            _viewService.Setup(vs => vs.GetFeaturesView(enabledFeatures)).Returns("Features view content");

            // Act
            var result = _controller.Index();

            // Assert
            Assert.IsType<ViewResult>(result);
            var viewResult = (ViewResult)result;
            Assert.Equal("Features view content", viewResult.ViewData.Model);
        }

        [Fact]
        public void Index_NoFeaturesAvailable_ShouldShowErrorMessage()
        {
            // Arrange
            _featureManager.Setup(fm => fm.GetEnabledFeatures()).Returns(new List<string> { "NewFeature" });
            _viewService.Setup(vs => vs.GetErrorView()).Returns("Something went wrong. Please try again later.");

            // Act
            var result = _controller.Index();

            // Assert
            Assert.IsType<ViewResult>(result);
            var viewResult = (ViewResult)result;
            Assert.Equal("Something went wrong. Please try again later.", viewResult.ViewData.Model);
        }

        [Fact]
        public void Index_NoFeaturesEnabled_ShouldShowNoAccessMessage()
        {
            // Arrange
            _featureManager.Setup(fm => fm.GetEnabledFeatures()).Returns(new List<string>());
            _viewService.Setup(vs => vs.GetNoAccessView()).Returns("No access to this feature.");

            // Act
            var result = _controller.Index();

            // Assert
            Assert.IsType<ViewResult>(result);
            var viewResult = (ViewResult)result;
            Assert.Equal("No access to this feature.", viewResult.ViewData.Model);
        }

        [Fact]
        public void Index_ErrorOccured_ShouldShowErrorMessage()
        {
            // Arrange
            _featureManager.Setup(fm => fm.GetEnabledFeatures()).Throws(new Exception("Test error"));
            _viewService.Setup(vs => vs.GetErrorView()).Returns("Something went wrong. Please try again later.");

            // Act
            var result = _controller.Index();

            // Assert
            Assert.IsType<ViewResult>(result);
            var viewResult = (ViewResult)result;
            Assert.Equal("Something went wrong. Please try again later.", viewResult.ViewData.Model);
        }
    }

    public class FeaturesController
    {
        private readonly IFeatureManager _featureManager;
        private readonly IViewService _viewService;

        public FeaturesController(IFeatureManager featureManager, IViewService viewService)
        {
            _featureManager = featureManager;
            _viewService = viewService;
        }

        public IActionResult Index()
        {
            try
            {
                var enabledFeatures = _featureManager.GetEnabledFeatures();
                if (enabledFeatures.Count > 1)
                {
                    return View(_viewService.GetFeaturesView(enabledFeatures));
                }
                else if (enabledFeatures.Count == 1)
                {
                    return View(_viewService.GetErrorView());
                }
                else
                {
                    return View(_viewService.GetNoAccessView());
                }
            }
            catch (Exception)
            {
                return View(_viewService.GetErrorView());
            }
        }
    }

    public interface IFeatureManager
    {
        List<string> GetEnabledFeatures();
    }

    public interface IViewService
    {
        string GetFeaturesView(List<string> features);
        string GetErrorView();
        string GetNoAccessView();
    }
}
