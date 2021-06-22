const menuToggle = document.querySelector('#categoriesMenuToggle');
menuToggle.addEventListener('click', () => {
    const menu = document.querySelector('#expMenu');
    menu.classList.toggle('closed');
});
