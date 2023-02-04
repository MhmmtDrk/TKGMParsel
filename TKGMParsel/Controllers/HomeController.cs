using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Diagnostics;
using TKGMParsel.Business.Repositories;
using TKGMParsel.Data.Cache;
using TKGMParsel.Data.Entities;
using TKGMParsel.Models;

namespace TKGMParsel.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        WebRepository<City> _repoCity;
        WebRepository<District> _repoDistrict;
        WebRepository<Street> _repoStreet;
        WebRepository<Parcel> _repoParcel;
        private readonly DataCacheRedis _dataCacheRedis;
        public HomeController(ILogger<HomeController> logger, WebRepository<City> repoCity, WebRepository<District> repoDistrict, WebRepository<Street> repoStreet, WebRepository<Parcel> repoParcel, DataCacheRedis dataCacheRedis)
        {
            _logger = logger;
            _repoCity = repoCity;
            _repoDistrict = repoDistrict;
            _repoStreet = repoStreet;
            _repoParcel = repoParcel;
            _dataCacheRedis = dataCacheRedis;
        }

        public IActionResult Index()
        {
            try
            {
                var FindCity = _repoCity.GetAll().FirstOrDefault();
                if (FindCity == null)
                {
                    var Model = GetResponseJSON("https://cbsservis.tkgm.gov.tr/megsiswebapi.v3/api/idariYapi/ilListe").Result;
                    var ModelJson = JsonConvert.DeserializeObject<ParselModel>(Model);
                    List<City> ModelList = new List<City>();
                    foreach (var item in ModelJson.features.Select(x => x.properties))
                    {

                        City CityModel = new City();
                        CityModel.Name = item.text;
                        CityModel.TKGMValue = item.id;
                        ModelList.Add(CityModel);

                    }
                    _repoCity.CreateAll(ModelList);
                }
                ViewBag.Cities = new SelectList(_repoCity.GetAll().OrderBy(i => i.TKGMValue), "TKGMValue", "Name");
                return View();
            }
            catch (Exception Ex)
            {
                return Error();
            }

        }
        [HttpPost]
        public JsonResult GetDistrict(int cityVal)
        {
            try
            {
                var FindDistrict = _repoDistrict.GetByCityValDistrictList(cityVal).FirstOrDefault();
                if (FindDistrict == null)
                {
                    var Model = GetResponseJSON($"https://cbsservis.tkgm.gov.tr/megsiswebapi.v3/api//idariYapi/ilceListe/{cityVal}").Result;
                    var ModelJson = JsonConvert.DeserializeObject<ParselModel>(Model);
                    var FindCityId = _repoCity.GetAll().Where(x => x.TKGMValue == cityVal).Select(x => x.Id).FirstOrDefault();
                    List<District> ModelList = new List<District>();
                    foreach (var item in ModelJson.features.Select(x => x.properties))
                    {
                        District DistrictModel = new District();
                        DistrictModel.Name = item.text;
                        DistrictModel.TKGMValue = item.id;
                        DistrictModel.TKGMCityValue = cityVal;
                       
                        ModelList.Add(DistrictModel);

                    }
                    _repoDistrict.CreateAll(ModelList);
                }

                var Result = _repoDistrict.GetAll().Where(x => x.City.TKGMValue == cityVal);
                return Json(Result);
            }
            catch (Exception ex)
            {
                return Json("Error");
            }

            //var ServiceLogList = db.ServiceLogs.ToList();
            //return View(ServiceLogList);
        }
        [HttpPost]
        public JsonResult GetStreet(int districtVal)
        {
            try
            {
                var FindStreet = _repoStreet.GetByDistrictValStreetList(districtVal).FirstOrDefault();
                if (FindStreet == null)
                {
                    var Model = GetResponseJSON($"https://cbsservis.tkgm.gov.tr/megsiswebapi.v3/api/idariYapi/mahalleListe/{districtVal}").Result;
                    var ModelJson = JsonConvert.DeserializeObject<ParselModel>(Model);
                    var FindDistrictId = _repoDistrict.GetAll().Where(x => x.TKGMValue == districtVal).Select(x => x.Id).FirstOrDefault();
                    List<Street> ModelList = new List<Street>();
                    foreach (var item in ModelJson.features.Select(x => x.properties))
                    {
                        Street StreetModel = new Street();
                        StreetModel.Name = item.text;
                        StreetModel.TKGMValue = item.id;
                        StreetModel.TKGMDistrictValue = districtVal;
                        StreetModel.DistrictId = FindDistrictId;
                        ModelList.Add(StreetModel);
                    }
                    _repoStreet.CreateAll(ModelList);
                }

                var Result = _repoStreet.GetAll().Where(x => x.District.TKGMValue == districtVal);

                return Json(Result);
            }
            catch (Exception ex)
            {
                return Json("Error");
            }

            //var ServiceLogList = db.ServiceLogs.ToList();
            //return View(ServiceLogList);
        }
        [HttpPost]
        public JsonResult GetData(GetDataModel model)
        {
            try
            {
                var cacheKey = $"Parsel_{model.StreetVal}_{model.AdaVal}_{model.ParcelVal}";
                var cacheParcel = _dataCacheRedis.Get<Parcel>(cacheKey);
                if (cacheParcel == null)
                {
                    var findParcel = _repoStreet.GetByStreetParsel(model.StreetVal, model.AdaVal.ToString(), model.ParcelVal.ToString());
                    if (findParcel == null)
                    {
                        var Model = GetResponseJSON($"https://cbsservis.tkgm.gov.tr/megsiswebapi.v3/api/parsel/{model.StreetVal}/{model.AdaVal}/{model.ParcelVal}").Result;
                        if (Model == "")
                        {
                            return Json(null);
                        }

                        var ModelJson = JsonConvert.DeserializeObject<ParselDataModel>(Model);
                        Parcel pModel = new Parcel();
                        pModel.ilceAd = ModelJson.properties.ilceAd;
                        pModel.ilId = ModelJson.properties.ilId;
                        pModel.durum = ModelJson.properties.durum;
                        pModel.parselId = ModelJson.properties.parselId;
                        pModel.zeminKmdurum = ModelJson.properties.zeminKmdurum;
                        pModel.zeminId = ModelJson.properties.zeminId;
                        pModel.parselNo = ModelJson.properties.parselNo;
                        pModel.nitelik = ModelJson.properties.nitelik;
                        pModel.mahalleAd = ModelJson.properties.mahalleAd;
                        pModel.gittigiParselListe = ModelJson.properties.gittigiParselListe;
                        pModel.gittigiParselSebep = ModelJson.properties.gittigiParselSebep;
                        pModel.alan = ModelJson.properties.alan;
                        pModel.adaNo = ModelJson.properties.adaNo;
                        pModel.ilceId = ModelJson.properties.ilceId;
                        pModel.ilAd = ModelJson.properties.ilAd;
                        pModel.mahalleId = ModelJson.properties.mahalleId;
                        pModel.pafta = ModelJson.properties.pafta;
                        pModel.mevkii = ModelJson.properties.mevkii;

                        _dataCacheRedis.Set(cacheKey, pModel);
                        _repoParcel.Create(pModel);

                        return Json(pModel);
                    }
                    else
                    {
                        _dataCacheRedis.Set(cacheKey, findParcel);
                        return Json(findParcel);
                    }
                }
                else
                {
                    return Json(cacheParcel);
                }

            }
            catch (Exception Ex)
            {
                return Json("Error");
            }




        }
        public async Task<string> GetResponseJSON(string url)
        {
            using (System.Net.Http.HttpClient client = new System.Net.Http.HttpClient())
            {
                client.BaseAddress = new Uri(url);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                System.Net.Http.HttpResponseMessage response = client.GetAsync(url).Result;
                if (response.IsSuccessStatusCode)
                {
                    var data = await response.Content.ReadAsStringAsync();
                    return data;
                }
                return "";
            }
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