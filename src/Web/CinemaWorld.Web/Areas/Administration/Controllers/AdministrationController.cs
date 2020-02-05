﻿namespace CinemaWorld.Web.Areas.Administration.Controllers
{
    using CinemaWorld.Common;
    using CinemaWorld.Web.Controllers;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    [Authorize(Roles = GlobalConstants.AdministratorRoleName)]
    [Area("Administration")]
    public class AdministrationController : BaseController
    {
    }
}
