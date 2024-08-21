using ImagekitUpload.Context;
using ImagekitUpload.Models;
using ImagekitUpload.Services;
using Microsoft.AspNetCore.Mvc;

namespace ImagekitUpload.Controllers;

public class PetController : Controller
{
    private readonly ApplicationContext _context;
    private readonly IImagekitAPIService _imagekit;

    public PetController(ApplicationContext context, IImagekitAPIService imagekit)
    {
        _context = context;
        _imagekit = imagekit;
    }

    [HttpGet("pets")]
    public ViewResult Pets()
    {
        List<Pet> pets = _context.Pets.ToList();
        return View("Pets", pets);
    }

    [HttpGet("pets/new")]
    public ViewResult NewPet()
    {
        return View("NewPet");
    }

    [HttpPost("pets/create")]
    public async Task<IActionResult> CreatePet(PetForm form)
    {
        if (!ModelState.IsValid)
        {
            return View("NewPet");
        }

        string imageUrl = await _imagekit.UploadImageAsync(form.PetImage!);

        Pet newPet = new()
        {
            PetName = form.PetName,
            PetImageUrl = imageUrl,
            PetImageDescription = form.PetImageDescription
        };

        _context.Pets.Add(newPet);
        _context.SaveChanges();
        return RedirectToAction("Pets");
    }
}
