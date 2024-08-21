using CipatBarzel.DAL;
using CipatBarzel.Models;
using CipatBarzel.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;


namespace CipatBarzel.Controllers
{
    public class HomeController : Controller
    {
        public static Dictionary<string, CancellationTokenSource> ThreatMap = new();

        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        //פתיחת עמוד שמציג לי את כל הטילי יירוט
        public IActionResult DefenceAmmunition()
        {
            List<DefenceAmmunition> defenceammunition = Data.Get.DefenceAmmunitions.ToList();
            return View(defenceammunition);
        }

        //יצירת עמוד של הוספת טילי יירוט
        public IActionResult CreateDefenceAmmunition()
        {
            return View(new DefenceAmmunition());
        }

        //הוספת טילי יירוט
        [HttpPost]
        public IActionResult CreateDefenceAmmunition(DefenceAmmunition defenceAmmunition)
        {
            Data.Get.DefenceAmmunitions.Add(defenceAmmunition);
            Data.Get.SaveChanges();
            return RedirectToAction("DefenceAmmunition");
        }


        //פונקציה שמעדכן את המלאי של הטילי יירוט
        public IActionResult updateDefenceAmmunition(int dfid, int amount)
        {
            DefenceAmmunition? da = Data.Get.DefenceAmmunitions.Find(dfid);
            da.Amount = amount;
            Data.Get.SaveChanges();
            return RedirectToAction("DefenceAmmunition");

        }

        //פתיחת עמוד שמציג לי את כל האיומים
        public IActionResult Threat()
        {
            List<Threat> threat = Data.Get.Threats.
                Include(t => t.TerrorOrg).
                Include(t => t.Type).
                ToList();
            return View(threat);
        }

        public IActionResult CreateThreat()
        {

            List<ThreatAmmunition>? ta = Data.Get.ThreatAmmunitions.ToList();
            List<TerrorOrg>? orgList = Data.Get.TerrorOrgs.ToList();

            CreateThreatViewModel model = new CreateThreatViewModel
            {
                Types = ta.Select(t => new SelectListItem { Value = t.Id.ToString(), Text = t.Name }).ToList(),
                TerrorOrgs = orgList.Select(t => new SelectListItem { Value = t.Id.ToString(), Text = t.Name }).ToList()
            };

            return View(model);
        }


        [HttpPost]
        public IActionResult CreateThreat(CreateThreatViewModel model)
        {
            TerrorOrg? org = Data.Get.TerrorOrgs.Find(model.OrgId);
            ThreatAmmunition? threatType = Data.Get.ThreatAmmunitions.Find(model.ThreatTypeId);

            if (org != null && threatType != null)
            {
                Threat newThreat = new Threat
                {
                    TerrorOrg = org,
                    Type = threatType,
                };

                Data.Get.Threats.Add(newThreat);
                Data.Get.SaveChanges();

       
            }
            return RedirectToAction(nameof(Threat));
        }

        public IActionResult Launch(int Id)
        {
            Threat? t = Data.Get.Threats.Find(Id);
            if (t == null)
            {
                return NotFound();
            }
            t.Status = Utils.ThreatStatus.active;
            t.FireTime = DateTime.Now;
            Data.Get.SaveChanges();

			// create cancelation token
			CancellationTokenSource cts = new();
			// create & run task
			Task task = Task.Run(async () =>
			{
				// print status every 2 seconds
				int timer = t.ResponceTime;
				while (!cts.IsCancellationRequested && timer > 0)
				{
					Console.WriteLine($"{t.Id} threat is {timer} seconds away");
					await Task.Delay(2000);
					timer -= 2;
				}
                if (cts.IsCancellationRequested)
                {
                    t.Status = Utils.ThreatStatus.failed;
                    
                }
                else
                {
                    t.Status = Utils.ThreatStatus.succeeded;
				    cts.Cancel();
                }
			
				ThreatMap.Remove(t.Id.ToString());
				Data.Get.SaveChanges();
			}, cts.Token);

			// save the threat in the dictionary
			ThreatMap[t.Id.ToString()] = cts;

			return RedirectToAction(nameof(Threat));
			
        }

        public IActionResult DeffenceArea()
        {
            return View(Data.Get.Threats
                .Include(t => t.Type)
                .Include(t => t.TerrorOrg)
                .ToList()
                .Where(t => t.Status != Utils.ThreatStatus.inActive));
        }

        public IActionResult Intercept(int tid, int did)
        {
            // למצא את האיום
            Threat? t = Data.Get.Threats.Find(tid);
            // למצא הגנה
            DefenceAmmunition? da = Data.Get.DefenceAmmunitions.Find(did);
            //לוודא ששניהם קיימים
            if (t == null || da == null)
            {
                return NotFound();
            }
            if (da.Amount < 1)
            {
                return BadRequest($"{da.Name} אזל מהמלאי תחמושת ההגנה");
            }
            //לבטל את הטאסק ולמחוק את הדקשנרי 
            ThreatMap[tid.ToString()].Cancel();
            ThreatMap.Remove(tid.ToString());
            // הפחתת כמות המירטים
            --da.Amount;


            Data.Get.SaveChanges();
            Thread.Sleep(10000);
        

            return RedirectToAction(nameof(DeffenceArea));

        }















		public IActionResult Index()
        {

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
