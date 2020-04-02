using System;
using System.Data.SqlClient;
using ComeFrexco.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ComeFrexco.Services
{
    public interface IConfig
    {
        void getControllerBase(ControllerBase controller);
        SqlConnection ConectarSqlServer(HttpRequest recuest);
        SqlConnection Login(Usuario usuario);
        Usuario ValidateToken(String Token);
        string GenerateToken(Usuario usuario);
        string Encriptar(string Texto);
        string DesEncriptar(string Texto);
        IActionResult getFauilureResponse(Exception ex);
    }
}
