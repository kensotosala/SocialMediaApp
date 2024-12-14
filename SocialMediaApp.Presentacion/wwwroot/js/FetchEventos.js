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
                    <a href="#" class="dropdown-item ${itemClass}" data-index="${index}">
                        <strong>${Titulo}</strong>
                        <button type="button" class="btn btn-sm btn-primary ms-2">✏</button>
                        <p class="mb-0 text-muted">${Descripcion || 'No description'}</p>
                        <small class="text-muted">${Ubicacion}</small>
                        <br><small>${new Date(FechaEvento).toLocaleString()}</small>
                    </a>
                `;
                dropdownMenu.appendChild(li);

                // Agregar el evento de clic para abrir el modal
                li.querySelector('a').addEventListener('click', (event) => {
                    event.preventDefault();
                    openEventModal(evento);
                });
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

function openEventModal(evento) {
    const { Titulo, Descripcion, FechaEvento, Ubicacion, EventoId } = evento;

    document.getElementById('eventTitle').textContent = Titulo || 'Sin título';
    document.getElementById('eventDescription').textContent = Descripcion || 'Sin descripción';
    document.getElementById('eventDate').textContent = new Date(FechaEvento).toLocaleString();
    document.getElementById('eventLocation').textContent = Ubicacion || 'Sin ubicación';

    // Limpiar la lista de usuarios invitados antes de llenarla
    const listaUsuarios = document.getElementById('invitedUserList');
    listaUsuarios.innerHTML = '';

    // Hacer la solicitud para obtener los usuarios invitados
    fetch(`http://localhost:5142/api/InvitarEventoControllerAPI/ObtenerInvitados/${EventoId}`)
        .then(response => response.json())  // Convertir la respuesta en formato JSON
        .then(usuariosInvitados => {
            // Verificar si hay usuarios invitados
            if (usuariosInvitados && usuariosInvitados.length > 0) {
                // Iterar sobre la lista de usuarios invitados y agregarlos al modal
                usuariosInvitados.forEach(usuario => {
                    const li = document.createElement('li');
                    li.textContent = `Usuario ID: ${usuario.UsuarioId} - Confirmación: ${usuario.Confirmacion || 'No disponible'}`;
                    listaUsuarios.appendChild(li);
                });
            } else {
                // Si no hay usuarios invitados, mostrar mensaje
                const li = document.createElement('li');
                li.textContent = 'No hay usuarios invitados.';
                listaUsuarios.appendChild(li);
            }
        })
        .catch(error => {
            console.error('Error al obtener los usuarios invitados:', error);
            const li = document.createElement('li');
            li.textContent = 'Error al cargar los usuarios invitados.';
            listaUsuarios.appendChild(li);
        });

    // Mostrar el modal
    const modal = new bootstrap.Modal(document.getElementById('eventModal'));
    modal.show();
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