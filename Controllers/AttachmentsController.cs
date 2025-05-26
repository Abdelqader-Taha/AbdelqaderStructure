using System.ComponentModel.DataAnnotations;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OrphanSystem.ActionFilters;
using OrphanSystem.Helpers;
using OrphanSystem.Services;

namespace OrphanSystem.Controllers;

[Authorize]
public class AttachmentsController : BaseController
{
    private readonly IAttachmentsService _attachmentsService;

    public AttachmentsController(IAttachmentsService attachmentsService)
    {
        _attachmentsService = attachmentsService;
    }

    [HttpPost]
    public async Task<ActionResult<string>> Upload([Required] IFormFile file) =>
        Ok(new Response<string>(await _attachmentsService.Upload(file), null, 200));
}