using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewComponents;
using ViewComponentTest.Services;

namespace ViewComponentTest.Components
{
    public class TimerDepInjViewComponent
    {
        ITimeService timeService;
        public TimerDepInjViewComponent(ITimeService service)
        {
            timeService = service;
        }
        public IViewComponentResult Invoke()
        {
            return new HtmlContentViewComponentResult(new HtmlString($"<p>Текущее время:<b>{timeService.GetTime()}</b></p>"));
        }
    }
}
