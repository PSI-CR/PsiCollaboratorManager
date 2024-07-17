class Carousel {
    constructor() {
        this.CurrentIndex = 0;
        this.ImagesCount = 0;
        this.CarouselId = "";
        this.images = [];
        this.dots = [];
    }
    Next() {
        let scope = this;
        if (scope.CurrentIndex + 1 >= scope.ImagesCount) scope.CurrentIndex = -1;
        scope.CurrentIndex++;
        scope.DisplayImage(scope.CurrentIndex);

    }
    Previous() {
        let scope = this;
        if (scope.CurrentIndex - 1 < 0) scope.CurrentIndex = scope.ImagesCount;
        scope.CurrentIndex--;
        scope.DisplayImage(scope.CurrentIndex);
    }
    SetCurrent(index) {
        let scope = this;
        if (index < 0) return;
        if (index >= scope.ImagesCount) return;
        scope.CurrentIndex = index;
        scope.DisplayImage(scope.CurrentIndex);
    }
    Slide() {
        let scope = this;
        scope.DisplayImage(scope.CurrentIndex);
        if (scope.CurrentIndex + 1 >= scope.ImagesCount) scope.CurrentIndex = -1;
        scope.CurrentIndex++;
        setTimeout(scope.Slide.bind(this), 4000);
    }
    DisplayImage(imageIndex) {
        let scope = this;
        if (imageIndex < 0 || imageIndex > scope.ImagesCount) return;
        scope.images.forEach((image, index) => {
            image.style.display = "none";
            if (index == imageIndex) image.style.display = "block";
        });
        scope.dots.forEach((dot, index) => {
            dot.classList.remove("activeDot");
            if (index == imageIndex) dot.classList.add("activeDot");
        });
    }
    SetImages() {
        let scope = this;
        scope.images = Array.from(document.getElementsByClassName("carousel-image-container"));
        scope.dots = Array.from(document.getElementsByClassName("dot"));
    }
    LoadEvents() {
        let scope = this;
        let previousButton = document.getElementById("PreviousButton" + scope.CarouselId);
        previousButton.addEventListener("click", () => {
            scope.Previous();
        });
        let nextButton = document.getElementById("NextButton" + scope.CarouselId);
        nextButton.addEventListener("click", () => {
            scope.Next();
        });
        $(".dot").on("click", (e) => {
            let dotId = e.target.id;
            let index = document.getElementById(dotId).getAttribute("indexTarget");
            scope.SetCurrent(index);
        });
    }
    LoadCarousel(id, imagesUrl) {
        let scope = this;
        scope.CarouselId = id;
        $.ajax({
            url: imagesUrl,
            type: 'GET',
            dataType: 'json',
            success: function (data) {
                let carousel = $('#' + id);
                let carouselContent = '<div class="carousel-container">';
                let dotsContent = '<div class="dots">';
                data.forEach((img, index) => {
                    try {
                        let byteCharacters = atob(img.replace(/\s/g, ''));
                        let byteNumbers = new Array(byteCharacters.length);
                        for (let i = 0; i < byteCharacters.length; i++) {
                            byteNumbers[i] = byteCharacters.charCodeAt(i);
                        }
                        let byteArray = new Uint8Array(byteNumbers);
                        let blob = new Blob([byteArray], { type: 'image/png' });

                        let imageUrl = URL.createObjectURL(blob);
                        carouselContent += '<div class="carousel-image-container"><img class="carousel-image" src="' + imageUrl + '"></div>';
                        dotsContent += '<span class="dot" id="dot' + index + id + '" indexTarget=" '+ index +'"></span> ';
                        scope.ImagesCount = index + 1;
                        console.log("ImagesCount: " + scope.ImagesCount);
                    }
                    catch (e) {
                        console.error('Error decoding base64:', e);
                    }
                });
                carouselContent += '</div>';
                dotsContent += '</div>';

                carousel.append(carouselContent);
                carousel.append('<a class="prev" id="PreviousButton' + id + '">❮</a>');
                carousel.append('<a class="next" id="NextButton' + id + '">❯</a>');
                carousel.append(dotsContent);

                scope.LoadEvents();
                scope.SetImages();
                scope.Slide();
            },
            error: function (error) {
                console.error('Error al obtener las imágenes: ', error);
            }
        });
    }
}
function CreateCarousel(imagesId, imagesUrl) {
    let carousel = new Carousel();
    carousel.LoadCarousel(imagesId, imagesUrl);
}