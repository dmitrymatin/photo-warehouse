initialiseCategoriesMenu('#categoriesMenuToggle', '#expMenu');
localiseDateString();


function initialiseCategoriesMenu(menuToggleSelector, expandableMenuSelector) {
    const menuToggle = document.querySelector(menuToggleSelector);
    const menu = document.querySelector(expandableMenuSelector);

    menuToggle?.addEventListener('click', () => {
        const menuToggleCoords = menuToggle.getBoundingClientRect();
        //console.dir(menuToggleCoords);
        const x = menuToggleCoords.left;
        const y = menuToggleCoords.bottom;


        menu.style.left = `${x}px`;
        menu.style.top = `${y}px`;
        menu.classList.toggle('closed');
    });

    window.addEventListener('resize', () => {
        menu.classList.add('closed');
    });
}

function localiseDateString() {
    const dateFields = document.querySelectorAll(".local-date-time");
    dateFields.forEach(df => {
        const value = df.innerText;
        if (value) {
            const localDate = new Date(value);
            df.innerText = localDate.toLocaleString();
        }
    });
}
