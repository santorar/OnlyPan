using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OnlyPan.Models;
using OnlyPan.Models.ViewModels;

namespace OnlyPan.Services;

public class UserServices
{
  private Usuario _user { get; set; }

  public UserServices(Usuario user)
  {
    _user = user;
  }

}