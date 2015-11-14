using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
// new...
using System.Threading.Tasks;

namespace ClientAppInstruments.Controllers
{
    public class InstrumentsController : Controller
    {
        // App's manager class reference
        Manager m = new Manager();

        // GET: Instruments
        public async Task<ActionResult> Index()
        {
            // Attention - Get all items
            // Processing of this request will pause while it's waiting for the response
            var instruments = await m.GetInstrumentsAsync();

            // We just need the collection that's inside the response package
            return View(instruments.Collection);
        }

        // GET: Instruments/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            // Attention - Get one item
            // Processing will pause while it's waiting
            var instrument = await m.GetInstrumentByIdAsync(id.GetValueOrDefault());

            if (instrument == null)
            {
                return HttpNotFound();
            }
            else
            {
                // We just need the item that's inside the response package
                return View(instrument.Item);
            }
        }

        // GET: Instruments/Details/5/Photo
        [Route("instruments/details/{id}/photo")]
        public async Task<ActionResult> DetailsPhoto(int? id)
        {
            // Attention - Get one item's media
            // Processing will pause while it's waiting
            var instrument = await m.GetInstrumentPhotoByIdAsync(id.GetValueOrDefault());

            if (instrument == null)
            {
                return HttpNotFound();
            }
            else
            {
                // Return the media item; must custom-configure the response
                return File(instrument.Content.ReadAsByteArrayAsync().Result,
                    instrument.Content.Headers.ContentType.MediaType);
            }
        }

        // GET: Instruments/Details/5/SoundClip
        [Route("instruments/details/{id}/soundclip")]
        public async Task<ActionResult> DetailsSoundClip(int? id)
        {
            // Attention - Get one item's media
            // Processing will pause while it's waiting
            var instrument = await m.GetInstrumentSoundClipByIdAsync(id.GetValueOrDefault());

            if (instrument == null)
            {
                return HttpNotFound();
            }
            else
            {
                // Return the media item; must custom-configure the response
                return File(instrument.Content.ReadAsByteArrayAsync().Result,
                    instrument.Content.Headers.ContentType.MediaType);
            }
        }

        // GET: Instruments/Create
        public ActionResult Create()
        {
            // Attention - Show the "add new" form
            return View();
        }

        // POST: Instruments/Create
        [HttpPost]
        public async Task<ActionResult> Create(InstrumentAdd newItem)
        {
            // Attention - Add a new item
            // The form accepts data and an optional image upload

            // Validate the input
            if (!ModelState.IsValid) { return View(); }

            // Process the input
            var addedItem = await m.AddInstrument(newItem);

            if (addedItem == null)
            {
                return View(newItem);
            }
            else
            {
                return RedirectToAction("details", new { id = addedItem.Item.Id });
            }
        }

        // GET: Instruments/EditPhoto/5
        public async Task<ActionResult> EditPhoto(int? id)
        {
            // Validate the requested item
            var item = await m.GetInstrumentByIdAsync(id.GetValueOrDefault());

            if (item == null)
            {
                return HttpNotFound();
            }
            else
            {
                // Create an edit form object

                var description = string.Format("{0} {1} {2}",
                    item.Item.ManufacturerName,
                    item.Item.InstrumentName,
                    item.Item.Category);
                var form = new InstrumentEditPhoto(id.Value, description, item.Item.PhotoMediaLength);

                return View(form);
            }
        }

        // POST: Instruments/EditPhoto/5
        [HttpPost]
        public async Task<ActionResult> EditPhoto(int id, InstrumentEditPhoto newItem)
        {
            // Validate the requested item
            if (!ModelState.IsValid | id != newItem.Id)
            {
                // There was a problem, so re-display

                var form = new InstrumentEditPhoto(id, newItem.Description, newItem.MediaSize);

                ModelState.AddModelError("modelState", "There was an error. The incoming data is invalid.");

                return View(form);
            }

            // Attempt to do the update

            if (newItem.PhotoUpload != null)
            {
                await m.SetPhoto(id, newItem.PhotoUpload);
            }

            return RedirectToAction("details", new { id = id });
        }

        // GET: Instruments/EditSoundClip/5
        public async Task<ActionResult> EditSoundClip(int? id)
        {
            // Validate the requested item
            var item = await m.GetInstrumentByIdAsync(id.GetValueOrDefault());

            if (item == null)
            {
                return HttpNotFound();
            }
            else
            {
                // Create an edit form object

                var description = string.Format("{0} {1} {2}",
                    item.Item.ManufacturerName,
                    item.Item.InstrumentName,
                    item.Item.Category);
                var form = new InstrumentEditSoundClip(id.Value, description, item.Item.SoundClipMediaLength);

                return View(form);
            }
        }

        // POST: Instruments/EditSoundClip/5
        [HttpPost]
        public async Task<ActionResult> EditSoundClip(int id, InstrumentEditSoundClip newItem)
        {
            // Validate the requested item
            if (!ModelState.IsValid | id != newItem.Id)
            {
                // There was a problem, so re-display

                var form = new InstrumentEditSoundClip(id, newItem.Description, newItem.MediaSize);

                ModelState.AddModelError("modelState", "There was an error. The incoming data is invalid.");

                return View(form);
            }

            // Attempt to do the update

            if (newItem.SoundClipUpload != null)
            {
                await m.SetSoundClip(id, newItem.SoundClipUpload);
            }

            return RedirectToAction("details", new { id = id });
        }

    }

}
