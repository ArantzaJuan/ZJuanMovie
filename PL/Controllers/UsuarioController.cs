using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace PL.Controllers
{
    public class UsuarioController : Controller
    {

        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(ML.Usuario usuario)
        {
            ML.Result result = new ML.Result();
            ML.Usuario usuarioObj = new ML.Usuario();
            result = BL.Usuario.Login(usuario);
            if (result.Correct)
            {
                usuarioObj = (ML.Usuario)result.Object;
                if (usuario.Password == usuarioObj.Password && usuario.UserName == usuarioObj.UserName)
                {
                    return RedirectToAction("GetAll", "Usuario");
                }
                else
                {
                    ViewBag.Message = "Contraseña y/o usuario incorrecto";
                    return PartialView("ModalLogin");
                }

            }
            else
            {
                ViewBag.Message = "Contraseña y/o usuario incorrecto";
                return PartialView("ModalLogin");
            }
            return View();
        }
        [HttpGet]
        public ActionResult GetAll()
        {
            ML.Usuario usuario = new ML.Usuario();
            ML.Result result = new ML.Result();
            result = BL.Usuario.GetAll();

            if (result.Correct)
            {
                usuario.Usuarios = result.Objects;
                return View(usuario);
            }
            else
            {
                ViewBag.Message = "Algo salio mal";
                return View(usuario);
            }
        }
    }
}
