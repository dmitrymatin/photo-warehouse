const menuToggle = document.querySelector('#categoriesMenuToggle');
const menu = document.querySelector('#expMenu');

menuToggle.addEventListener('click', () => {
    const menuToggleCoords = menuToggle.getBoundingClientRect();
    console.dir(menuToggleCoords);
    const x = menuToggleCoords.left;
    const y = menuToggleCoords.bottom;


    menu.style.left = `${x}px`;
    menu.style.top = `${y}px`;
    menu.classList.toggle('closed');
});

window.addEventListener('resize', () => {
    menu.classList.add('closed');
});
