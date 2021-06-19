const existingPhotoItems = document.querySelectorAll('.photoItemDeleteButton');
existingPhotoItems.forEach((pi) => {
    pi.addEventListener('click', (event) => {
        const clickedButton = event.target;

        const parent = clickedButton.parentElement;
        const photoItemStatusInput = parent.querySelector('.itemStatus');
        photoItemStatusInput.value = "Deleted";

        parent.style.display = "none";
    });
});