
const toggles = document.querySelectorAll('.accordion-toggle');

function closeAll() {
    toggles.forEach(btn => {
        btn.setAttribute('aria-expanded', 'false');
        const panel = document.getElementById(btn.getAttribute('aria-controls'));
        panel.hidden = true;
    });
}

toggles.forEach(btn => {
    btn.addEventListener('click', () => {
        const isOpen = btn.getAttribute('aria-expanded') === 'true';
        closeAll();
        if (!isOpen) {
            btn.setAttribute('aria-expanded', 'true');
            document.getElementById(btn.getAttribute('aria-controls')).hidden = false;
        }
    });
});


toggles.forEach(btn => {
    if (btn.getAttribute('aria-expanded') === 'true') {
        document.getElementById(btn.getAttribute('aria-controls')).hidden = false;
    }
});



document.addEventListener("DOMContentLoaded", function () {
    const trainingDropdown = document.querySelector(".nav-dropdown");
    const trainingToggle = document.getElementById("trainingToggle");

    if (!trainingDropdown || !trainingToggle) return;

    trainingToggle.addEventListener("click", function (e) {
        e.stopPropagation();
        trainingDropdown.classList.toggle("open");
    });

    document.addEventListener("click", function (e) {
        if (!trainingDropdown.contains(e.target)) {
            trainingDropdown.classList.remove("open");
        }
    });
});