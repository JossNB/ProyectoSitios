document.addEventListener("DOMContentLoaded", function () {
    const modal = document.getElementById("modalAsientos");
    const btnSeleccionarSector = document.querySelector(".seleccionar-sector");
    const btnCerrarModal = document.getElementById("cerrarModal");
    const btnConfirmarAsiento = document.getElementById("confirmarAsiento");

    // Verificar que los elementos del DOM existen
    if (!modal || !btnSeleccionarSector || !btnCerrarModal || !btnConfirmarAsiento) {
        console.error("Uno o más elementos del DOM no fueron encontrados.");
        return;
    }

    // Abrir modal al hacer clic en "Seleccionar Sector"
    btnSeleccionarSector.addEventListener("click", function () {
        modal.style.display = "flex";
    });

    // Cerrar modal
    btnCerrarModal.addEventListener("click", function () {
        modal.style.display = "none";
    });

    // Confirmar asiento seleccionado
    btnConfirmarAsiento.addEventListener("click", function () {
        const asientoSeleccionado = document.querySelector(".asiento.seleccionado");
        if (asientoSeleccionado) {
            const sectorId = asientoSeleccionado.closest(".sector").dataset.sector;
            const numeroAsiento = asientoSeleccionado.dataset.asiento;
            alert(`Asiento seleccionado: Sector ${sectorId}, Asiento ${numeroAsiento}`);
            modal.style.display = "none";
        } else {
            alert("Por favor, selecciona un asiento.");
        }
    });

    // Delegación de eventos para manejar clics en los asientos
    const sectoresContainer = document.querySelector(".sectores");
    if (sectoresContainer) {
        sectoresContainer.addEventListener("click", function (event) {
            const asiento = event.target.closest(".asiento");
            if (asiento) {
                // Deseleccionar todos los asientos
                document.querySelectorAll(".asiento").forEach(a => a.classList.remove("seleccionado"));
                // Seleccionar el asiento clickeado
                asiento.classList.add("seleccionado");
            }
        });
    } else {
        console.error("El contenedor de sectores no fue encontrado.");
    }
});