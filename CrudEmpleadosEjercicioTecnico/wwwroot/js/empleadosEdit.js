var video = document.querySelector('video');

function captureCamera(callback) {
    navigator.mediaDevices.getUserMedia({ audio: true, video: true }).then(function (camera) {
        callback(camera);
    }).catch(function (error) {
        alert('Unable to capture your camera. Please check console logs.');
        console.error(error);
    });
}

function stopRecordingCallback() {
    video.src = video.srcObject = null;
    video.muted = false;
    video.volume = 1;
    video.src = URL.createObjectURL(recorder.getBlob());

    recorder.camera.stop();
    recorder.destroy();
    recorder = null;
}

var recorder; // globally accessible

document.getElementById('btn-start-recording').onclick = function () {
    this.disabled = true;
    captureCamera(function (camera) {
        video.muted = true;
        video.volume = 0;
        video.srcObject = camera;

        recorder = RecordRTC(camera, {
            type: 'video'
        });

        recorder.startRecording();

        // release camera on stopRecording
        recorder.camera = camera;

        document.getElementById('btn-stop-recording').disabled = false;
    });
};

document.getElementById('btn-stop-recording').onclick = function () {
    this.disabled = true;
    document.getElementById("btn-start-recording").disabled = false;

    recorder.stopRecording(stopRecordingCallback);

    const canvas = document.getElementById('canvas');

    var context = canvas.getContext('2d');
    context.drawImage(video, -40, 0, 270, 200);

    dataUrl = canvas.toDataURL();
    canvas.style.visibility = "hidden";

    var imagen = document.getElementById("imagenPerfil");
    imagen.src = dataUrl;

    var hidden = document.getElementById('hidden');
    hidden.value = canvas.toDataURL("image/jpg", 0.5);

    
};

document.getElementById('customFile').onchange = function (evt) {
    var tgt = evt.target || window.event.srcElement,
        files = tgt.files;

    // FileReader support
    if (FileReader && files && files.length) {
        var fr = new FileReader();
        fr.onload = function () {
            document.getElementById("imagenPerfil").src = fr.result;
        }
        fr.readAsDataURL(files[0]);
    }

    // Not supported
    else {
        // fallback -- perhaps submit the input to an iframe and temporarily store
        // them on the server until the user's session ends.
    }
}