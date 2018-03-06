﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebApplication1.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Para entrar em contato com nossa equipe, enviar email ao endereço informado abaixo. Obrigado.";

            return View();
        }

        public ActionResult Download()
        {
            return View();
        }
    }
}