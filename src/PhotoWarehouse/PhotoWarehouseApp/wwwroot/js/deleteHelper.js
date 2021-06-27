markElementAsDeleted('.photoItemDeleteButton', '.itemStatus');

function markElementAsDeleted(deleteButtonSelector, statusElementSelector) {
    const existingPhotoItems = document.querySelectorAll(deleteButtonSelector);
    existingPhotoItems.forEach((pi) => {
        pi.addEventListener('click', (event) => {
            const clickedButton = event.target;

            const parent = clickedButton.parentElement;
            const photoItemStatusInput = parent.querySelector(statusElementSelector);
            photoItemStatusInput.value = "Deleted";

            parent.style.display = "none";
        });
    });
}