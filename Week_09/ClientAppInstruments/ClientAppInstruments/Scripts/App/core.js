
function previewImage(event) {
    // Get a reference to the image element
    var image = document.querySelector("#instrumentImage");
    // Set its source to the file in the <input type=file element
    image.src = URL.createObjectURL(event.target.files[0]);
};

function previewAudio(event) {
    // Get a reference to the audio element
    var audio = document.querySelector("#instrumentAudio");
    // Set its source to the file in the <input type=file element
    audio.src = URL.createObjectURL(event.target.files[0]);
};
