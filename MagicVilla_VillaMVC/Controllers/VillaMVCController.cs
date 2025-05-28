using MagicVilla_VillaMVC.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace MagicVilla_VillaMVC.Controllers
{
    public class VillaMVCController : Controller
    {
        private string url = "https://localhost:7286/api/VillaAPI/";
        private readonly HttpClient client= new HttpClient();

        [HttpGet]
        public IActionResult Index()
        {
            List<VillaDTO> villa = new List<VillaDTO>();
            HttpResponseMessage response = client.GetAsync(url).Result;
            if (response.IsSuccessStatusCode)
            {
                var apiResponse = response.Content.ReadAsStringAsync().Result;
                // convert json to normal data
                var data = JsonConvert.DeserializeObject<List<VillaDTO>>(apiResponse);
                if (data != null)
                {
                    villa = data;
                }
            }
            return View(villa);
        }

        [HttpGet]
        public IActionResult Create()
        {
          
            return View();
        }

        [HttpPost]
        public IActionResult Create(VillaDTO villa)
        {
            string data= JsonConvert.SerializeObject(villa);
            StringContent content = new StringContent(data, System.Text.Encoding.UTF8, "application/json");
            HttpResponseMessage response = client.PostAsync(url, content).Result;
            if (response.IsSuccessStatusCode)
            {
                TempData["success"] = "Villa created successfully";
                return RedirectToAction("Index");
            }
            else
            {
                ModelState.AddModelError("Error", "Error while creating Villa");
            }
            return View();
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            VillaDTO villa = new VillaDTO();
            HttpResponseMessage response = client.GetAsync(url + id).Result;
            if (response.IsSuccessStatusCode)
            {
                var apiResponse = response.Content.ReadAsStringAsync().Result;
                // convert json to normal data
                var data = JsonConvert.DeserializeObject<VillaDTO>(apiResponse);
                if (data != null)
                {
                    villa = data;
                }
            }
            return View(villa);
        }

        [HttpPost]
        public IActionResult Edit(VillaDTO villa)
        {
            string data = JsonConvert.SerializeObject(villa);
            StringContent content = new StringContent(data, System.Text.Encoding.UTF8, "application/json");
            HttpResponseMessage response = client.PutAsync(url + villa.Id, content).Result;
            if (response.IsSuccessStatusCode)
            {
                TempData["success"] = "Villa updated successfully";
                return RedirectToAction("Index");
            }
            else
            {
                ModelState.AddModelError("Error", "Error while editing Villa");
            }
            return View();
        }

        [HttpGet]
        public IActionResult Details(int id)
        {
            VillaDTO villa = new VillaDTO();
            HttpResponseMessage response = client.GetAsync(url + id).Result;
            if (response.IsSuccessStatusCode)
            {
                var apiResponse = response.Content.ReadAsStringAsync().Result;
                // convert json to normal data
                var data = JsonConvert.DeserializeObject<VillaDTO>(apiResponse);
                if (data != null)
                {
                    villa = data;
                }
            }
            return View(villa);
        }


        [HttpGet]
        public IActionResult Delete(int id)
        {
            VillaDTO villa = new VillaDTO();
            HttpResponseMessage response = client.GetAsync(url + id).Result;
            if (response.IsSuccessStatusCode)
            {
                var apiResponse = response.Content.ReadAsStringAsync().Result;
                // convert json to normal data
                var data = JsonConvert.DeserializeObject<VillaDTO>(apiResponse);
                if (data != null)
                {
                    villa = data;
                }
            }
            return View(villa);
        }

        [HttpPost, ActionName("Delete")]
        public IActionResult DeleteConfirmed(int id)
        {
            HttpResponseMessage response = client.DeleteAsync(url + id).Result;
            if (response.IsSuccessStatusCode)
            {
                TempData["success"] = "Villa deleted successfully";
                return RedirectToAction("Index");
            }
            else
            {
                ModelState.AddModelError("Error", "Error while deleting Villa");
            }
            return View();
        }
    }
}
