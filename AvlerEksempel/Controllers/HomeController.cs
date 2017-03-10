using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AvlerEksempel.Models;
using AvlerEksempel.Factories;

namespace AvlerEksempel.Controllers
{
    public class HomeController : Controller
    {
        BreederFactory breederFac = new BreederFactory();
        ProductFactory productFac = new ProductFactory();
        ProductBreederFactory productBreederFac = new ProductBreederFactory();

        public ActionResult Index()
        {
            // Henter listen af forbindelser mellem produkter og avlere
            List<ProductBreederViewModel> allProductBreeders = GetAllProductBreeders();
            // returnere listen
            return View(allProductBreeders);
        }

        public List<ProductBreederViewModel> GetAllProductBreeders()
        {
            // Opretter en tom liste, der skal indeholde alle avler forbindelserne
            List<ProductBreederViewModel> all = new List<ProductBreederViewModel>();

            // Liste alle produkter ud, dette gør vi fordi vi skal bruge alle produkter,
            // for at kunne forbinde et produkt med de avlere der 'avler' produktet
            foreach (Product product in productFac.GetAll())
            {
                // Opretter en ViewModel, denne indeholder en reference til Product og en Liste af Breeder 
                ProductBreederViewModel bvm = new ProductBreederViewModel();
                // Sætter som startpunkt produktet til vores ViewModel
                bvm.Product = product;
                // Derefter opretter vi en tom liste af breeders, den skal indeholde de avlere som 'avler' dette produkt
                List<Breeder> breeders = new List<Breeder>();

                // For at vi kan finde de avlere der avler dette produkt, skal vi her bruge ProductBreeder tabellen.
                // ProduktBreeder tabellen indeholder en række pr. forbindelse en avler har til et produkt
                // Så ved at hente alle de felter i denne tabel, der matcher det produktet, kan vi tilføje forbindelsen til listen 'all' på linje 26
                List<ProductBreeder> productBreederConnections = productBreederFac.GetBy("ProductID", product.ID);
                // Vi looper så igennem den liste vi har hentet
                foreach (ProductBreeder item in productBreederConnections)
                {
                    // Og tilføjer den Breeder der matcher produktets ID
                    breeders.Add(breederFac.Get(item.BreederID));
                }

                // Så sætter vi ViewModellens reference for Breeders til den liste vi har lavet over Breeders der avler et specifikt produkt
                bvm.Breeders = breeders;

                // Og til sidst tilføjer vi ViewModellen til listen all som indeholder alle referencer mellem produkter og avlere
                all.Add(bvm);
            }

            return all;
        }

        public ActionResult Breeders()
        {
            List<Breeder> allBreeders = breederFac.GetAll();
            return View(allBreeders);
        }

        public ActionResult AddBreederConnection(int id = 0)
        {
            ViewBag.Breeder = breederFac.Get(id);

            List<Product> allProducts = productFac.GetAll();
            List<ProductBreeder> productBreedersByBreederID = productBreederFac.GetBy("BreederID", id);
            BreederConnectionViewModel pdvm = new BreederConnectionViewModel();
            pdvm.Products = allProducts;
            pdvm.ProductBreeders = productBreedersByBreederID;
            return View(pdvm);
        }

        [HttpPost]
        public ActionResult AddBreederConnectionSubmit(int breederID, List<int> productIDs)
        {
            for (int i = 0; i < productIDs.Count; i++)
            {
                ProductBreeder productBreederToAdd = new ProductBreeder();
                productBreederToAdd.BreederID = breederID;
                productBreederToAdd.ProductID = productIDs[i];

                productBreederFac.Add(productBreederToAdd);
            }
            return RedirectToAction("Breeders");
        }

        public ActionResult DeleteBreederConnection(int id = 0)
        {
            ViewBag.Breeder = breederFac.Get(id);

            List<Product> allProducts = productFac.GetAll();
            List<ProductBreeder> productBreedersByBreederID = productBreederFac.GetBy("BreederID", id);
            BreederConnectionViewModel pdvm = new BreederConnectionViewModel();
            pdvm.Products = allProducts;
            pdvm.ProductBreeders = productBreedersByBreederID;
            return View(pdvm);
        }

        [HttpPost]
        public ActionResult DeleteBreederConnectionSubmit(int breederID, List<int> productBreederIDs)
        {
            for (int i = 0; i < productBreederIDs.Count; i++)
            {
                productBreederFac.Delete(productBreederIDs[i]);
            }
            return RedirectToAction("Breeders");
        }





    }
}
