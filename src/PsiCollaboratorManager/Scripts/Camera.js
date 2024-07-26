
window.onload = async () => {
    var videoWidth = 320;
    var videoHeight = 320;
    var videoTag = document.getElementById('theVideo');
    var canvasTag = document.getElementById('theCanvas');
    var btnCapture = document.getElementById("btnCapture");
    var btnDefaultImage = document.getElementById("btnDefaultImage");
    let profileImage = document.getElementById("SectionHeaderPicture");
    videoTag.setAttribute('width', videoWidth);
    videoTag.setAttribute('height', videoHeight);
    canvasTag.setAttribute('width', videoWidth);
    canvasTag.setAttribute('height', videoHeight);

    async function getMediaStream() {
        try {
            const stream = await navigator.mediaDevices.getUserMedia({
                audio: true, video: { width: videoWidth, height: videoHeight }
            });
            return stream;
        }
        catch (ex) {
            return null;
        }
    }

    const stream = await getMediaStream();
    videoTag.srcObject = stream;

    var canvasContext = canvasTag.getContext('2d');
    btnCapture.addEventListener("click", () => {
        if(videoTag.srcObject == null){
            SetDefaultImage();
            return;
        }
        canvasContext.drawImage(videoTag, 0, 0, videoWidth, videoHeight);
        let data = canvasTag.toDataURL();
        profileImage.src = data;
        canvasTag.toBlob((Blob) => {

            let file = new File([Blob], "fileName.jpg", { type: "image/jpeg" });
            let container = new DataTransfer();
            container.items.add(file);
            let file_input = document.getElementById("file_input");
            file_input.files = container.files;
        });
    });

    btnDefaultImage.addEventListener("click", () => {
        SetDefaultImage();
    });
    function SetDefaultImage() {
        let img = new Image();
        img.src = "../../Images/NoPhotoImg.png";
        profileImage.src = "../../Images/NoPhotoImg.png";
        img.onload = (() => {
            canvasContext.drawImage(img, 0, 0, videoWidth, videoWidth);
            canvasTag.toBlob(Blob => {
                let file = new File([Blob], "fileName.jpg", { type: "image/jpeg" });
                let container = new DataTransfer();
                container.items.add(file);
                let file_input = document.getElementById("file_input");
                file_input.files = container.files;
            });
        });
    }
};

