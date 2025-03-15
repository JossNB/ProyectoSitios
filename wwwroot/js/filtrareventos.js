function abrirModal() {
    document.getElementById("modalFecha").style.display = "flex";
    
    // Inicializar Flatpickr para ambos campos de fecha
    flatpickr("#fechaInicio", {
        inline: true,  // Hace que el calendario siempre esté visible
        allowInput: true  // Permite ingresar texto además de seleccionar la fecha
    });

    flatpickr("#fechaFin", {
        inline: true,  // Hace que el calendario siempre esté visible
        allowInput: true  // Permite ingresar texto además de seleccionar la fecha
    });
}

function cerrarModal() {
    document.getElementById("modalFecha").style.display = "none";
}

function aplicarFechas() {
    let inicio = document.getElementById("fechaInicio").value;
    let fin = document.getElementById("fechaFin").value;

    if (inicio && fin) {
        document.getElementById("fechaInput").value = `${inicio} - ${fin}`;
        cerrarModal();
    } else {
        alert("Seleccione ambas fechas.");
    }
}
