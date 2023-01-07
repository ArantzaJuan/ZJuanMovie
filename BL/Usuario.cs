using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL
{
    public class Usuario
    {
        public static ML.Result Login(ML.Usuario usuario)
        {
            ML.Result result = new ML.Result();
            try
            {
                using (DL_.ZjuanMovieContext context = new DL_.ZjuanMovieContext())
                {
                    var query = context.Usuarios.FromSqlRaw($"UsuarioGetByPass'{usuario.Password}'").AsEnumerable().FirstOrDefault();

                    if (query != null)
                    {

                        ML.Usuario usuarioobj = new ML.Usuario();

                        usuarioobj.IdUsuario = query.IdUsuario;
                        usuarioobj.Nombre = query.Nombre;
                        usuarioobj.ApellidoPaterno = query.ApellidoPaterno;
                        usuarioobj.ApellidoMaterno = query.ApellidoMaterno;
                        usuarioobj.Password = query.Password;
                        usuarioobj.UserName = query.UserName;

                        result.Object = usuarioobj;

                        result.Correct = true;
                    }
                    else
                    {
                        result.Correct = false;
                        result.ErrorMessage = "Hubo un error";
                    }
                }
            }
            catch (Exception ex)
            {
                result.Correct = false;
                result.ErrorMessage = ex.Message;
            }
            return result;
        }


        public static ML.Result GetAll()
        {
            ML.Result result = new ML.Result();
            try
            {
                using (DL_.ZjuanMovieContext context = new DL_.ZjuanMovieContext())
                {
                    var codigo = context.Usuarios.FromSqlRaw($"UsuarioGetAll").ToList();
                    result.Objects = new List<object>();
                    if (codigo != null)
                    {
                        foreach (var obj in codigo)
                        {
                            ML.Usuario empleado = new ML.Usuario();

                            empleado.IdUsuario = obj.IdUsuario;
                            empleado.Nombre = obj.Nombre;
                            empleado.ApellidoPaterno = obj.ApellidoPaterno;
                            empleado.ApellidoMaterno = obj.ApellidoMaterno;
                            empleado.UserName = obj.UserName;
                            empleado.Password = obj.Password;

                            result.Objects.Add(empleado);
                        }
                        result.Correct = true;
                    }
                    else
                    {
                        result.Correct = false;
                        result.ErrorMessage = "Hubo un error";
                    }
                    result.Correct = true;
                }
            }
            catch (Exception ex)
            {
                result.Correct = false;
                result.ErrorMessage = ex.Message;
            }
            return result;
        }
    }
}
