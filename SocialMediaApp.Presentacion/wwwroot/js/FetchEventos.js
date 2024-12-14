const EVENTOS_API = "http://localhost:5142/api/EventoControllerAPI/obtenerTodosLosEventos";

/**
 * cargarEventos(): Se utiliza para cargar los eventos dentro en el dropdown.
 */
async function cargarEventos() {
    const dropdownMenu = document.getElementById("eventosDropdown");

    // Asegúrate de agregar el botón "Nuevo Evento" primero
    if (!document.getElementById("nuevoEventoButton")) {
        const nuevoEventoButton = document.createElement('li');
        nuevoEventoButton.innerHTML = `
            <button type="button" class="btn btn-lg btn-primary w-100" data-bs-toggle="modal" data-bs-target="#eventFormModal" id="nuevoEventoButton">Nuevo Evento</button>`;
        dropdownMenu.appendChild(nuevoEventoButton);

        // Lógica para el evento del botón "Nuevo Evento"
        nuevoEventoButton.addEventListener("click", () => {
            console.log("Nuevo Evento clicked");
            // Aquí puedes abrir un modal o realizar alguna acción
        });
    }

    try {
        const response = await fetch(EVENTOS_API, { method: 'GET' });
        if (!response.ok) throw new Error("Error al hacer fetch de eventos");

        const data = await response.json();
        const eventos = data || []; // Asegura que eventos esté inicializado

        // Limpia todos los eventos excepto el botón "Nuevo Evento"
        dropdownMenu.innerHTML = dropdownMenu.firstElementChild.outerHTML;

        // Agrega eventos al dropdown
        if (eventos.length === 0) {
            const emptyItem = document.createElement('li');
            emptyItem.innerHTML = `
                <span class="dropdown-item text-muted">No hay eventos disponibles</span>
            `;
            dropdownMenu.appendChild(emptyItem);
        } else {
            eventos.forEach((evento, index) => {
                const { Titulo, Descripcion, FechaEvento, Ubicacion, EsLeida } = evento;

                const itemClass = EsLeida ? "" : "fw-bold text-primary";

                const li = document.createElement('li');
                li.innerHTML = `
                    <a href="#" class="dropdown-item ${itemClass}" data-index="${index}" data-bs-toggle="modal" data-bs-target="#notificationModal">
                        <strong>${Titulo}</strong>
                        <button type="button" class="btn btn-sm btn-primary ms-2">✏</button>
                        <p class="mb-0 text-muted">${Descripcion || 'No description'}</p>
                        <small class="text-muted">${Ubicacion}</small>
                        <br><small>${new Date(FechaEvento).toLocaleString()}</small>
                    </a>
                `;
                dropdownMenu.appendChild(li);
            });
        }
    } catch (error) {
        console.error("Error al cargar eventos", error);
        dropdownMenu.innerHTML = `
            ${dropdownMenu.firstElementChild.outerHTML}
            <li><span class="dropdown-item text-danger">Error al cargar los eventos</span></li>
        `;
    }
}

// Ejecutar la función al cargar la página
document.addEventListener("DOMContentLoaded", cargarEventos);

// ➡️ GESTIONAR EL FORMULARIO DE ENVÍO ⬅️
document.addEventListener("DOMContentLoaded", function () {
    const form = document.getElementById("eventForm");
    if (form) {
        form.addEventListener("submit", function (event) {
            event.preventDefault();

            const formData = new FormData(form);

            // Validate form data before sending
            const titulo = formData.get("Titulo");
            const descripcion = formData.get("Descripcion");
            const fechaEvento = formData.get("FechaEvento");
            const ubicacion = formData.get("Ubicacion");
            const usuariosInvitar = formData.getAll("usuariosInvitar[]");

            // Basic validation
            if (
                !titulo ||
                !descripcion ||
                !fechaEvento ||
                !ubicacion ||
                usuariosInvitar.length === 0
            ) {
                alert("Por favor, complete todos los campos requeridos.");
                return;
            }

            const usuarioIdsInt = usuariosInvitar.map((id) => parseInt(id, 10));

            const dataToSend = {
                usuarioId: 1,
                titulo: titulo,
                descripcion: descripcion,
                fechaEvento: fechaEvento,
                ubicacion: ubicacion,
                usuarioIds:
                    usuariosInvitar.length > 0
                        ? usuariosInvitar.map((id) => parseInt(id, 10))
                        : [],
            };

            console.log("Datos a enviar:", JSON.stringify(dataToSend, null, 2));

            fetch("http://localhost:5142/api/EventoControllerAPI/crear", {
                method: "POST",
                headers: {
                    "Content-Type": "application/json",
                },
                body: JSON.stringify(dataToSend),
            })
                .then(async (response) => {
                    if (!response.ok) {
                        const errorData = await response.json();
                        throw new Error(
                            `Error: ${response.status}. ${JSON.stringify(
                                errorData.errors
                            )}`
                        );
                    }
                    return response.json();
                })
                .then((data) => {
                    console.log("Evento creado con éxito:", data);
                    alert("Evento creado exitosamente!");
                    $("#eventFormModal").modal("hide");

                    // Optional: Refresh events or update UI
                    if (typeof cargarEventos === "function") {
                        cargarEventos();
                    }

                    // Reset form
                    form.reset();
                })
                .catch((error) => {
                    console.error("Error al crear el evento:", error);
                    alert(`Hubo un error al crear el evento: ${error.message}`);
                });
        });
    } else {
        console.error("Formulario no encontrado");
    }
});