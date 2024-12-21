class ImageUploadHandler {
    constructor() {
        this.form = document.getElementById('uploadForm');
        this.imageInput = document.getElementById('imageInput');
        this.preview = document.getElementById('preview');
        this.previewPlaceholder = document.getElementById('preview-placeholder');
        this.fileInputLabel = document.querySelector('.file-input-label');
        this.apiUrl = '/api/upload'; // Cambia esto según tu API

        this.initializeEventListeners();
    }

    initializeEventListeners() {
        // Manejo de la vista previa de la imagen
        this.imageInput.addEventListener('change', (e) => this.handleImagePreview(e));

        // Manejo del envío del formulario
        this.form.addEventListener('submit', (e) => this.handleFormSubmit(e));

        // Manejo de drag and drop
        const dropZone = document.querySelector('.preview-container');
        dropZone.addEventListener('dragover', (e) => this.handleDragOver(e));
        dropZone.addEventListener('drop', (e) => this.handleDrop(e));
    }

    handleImagePreview(event) {
        const file = event.target.files[0];
        if (file) {
            this.displayImagePreview(file);
        }
    }

    displayImagePreview(file) {
        // Validar que sea una imagen
        if (!file.type.startsWith('image/')) {
            this.showError('Por favor selecciona un archivo de imagen válido');
            return;
        }

        // Validar tamaño (máximo 5MB)
        if (file.size > 5 * 1024 * 1024) {
            this.showError('La imagen no debe superar los 5MB');
            return;
        }

        const reader = new FileReader();
        reader.onload = (e) => {
            this.preview.src = e.target.result;
            this.preview.style.display = 'block';
            this.previewPlaceholder.style.display = 'none';
            this.fileInputLabel.innerHTML = `<span class="upload-icon">📁</span>${file.name}`;
        };
        reader.readAsDataURL(file);
    }

    async handleFormSubmit(event) {
        event.preventDefault();

        const file = this.imageInput.files[0];
        if (!file) {
            this.showError('Por favor selecciona una imagen');
            return;
        }

        try {
            this.showLoading();
            await this.uploadImage(file);
            this.showSuccess('Imagen subida exitosamente');
            this.resetForm();
        } catch (error) {
            this.showError('Error al subir la imagen: ' + error.message);
        } finally {
            this.hideLoading();
        }
    }

    async uploadImage(file) {
        const formData = new FormData();
        formData.append('image', file);

        const response = await fetch(this.apiUrl, {
            method: 'POST',
            body: formData
        });

        if (!response.ok) {
            throw new Error(`Error ${response.status}: ${response.statusText}`);
        }

        return await response.json();
    }

    handleDragOver(event) {
        event.preventDefault();
        event.stopPropagation();
        event.currentTarget.classList.add('dragover');
    }

    handleDrop(event) {
        event.preventDefault();
        event.stopPropagation();
        event.currentTarget.classList.remove('dragover');

        const file = event.dataTransfer.files[0];
        if (file) {
            this.imageInput.files = event.dataTransfer.files;
            this.displayImagePreview(file);
        }
    }

    showLoading() {
        const button = this.form.querySelector('button[type="submit"]');
        button.disabled = true;
        button.innerHTML = 'Subiendo...';
    }

    hideLoading() {
        const button = this.form.querySelector('button[type="submit"]');
        button.disabled = false;
        button.innerHTML = 'Guardar Imagen';
    }

    showError(message) {
        // Crear y mostrar mensaje de error
        const errorDiv = document.createElement('div');
        errorDiv.className = 'error-message';
        errorDiv.style.color = '#e53e3e';
        errorDiv.style.padding = '10px';
        errorDiv.style.marginTop = '10px';
        errorDiv.style.borderRadius = '4px';
        errorDiv.style.backgroundColor = '#fff5f5';
        errorDiv.textContent = message;

        // Insertar mensaje después del formulario
        this.form.parentNode.insertBefore(errorDiv, this.form.nextSibling);

        // Remover después de 3 segundos
        setTimeout(() => errorDiv.remove(), 3000);
    }

    showSuccess(message) {
        // Crear y mostrar mensaje de éxito
        const successDiv = document.createElement('div');
        successDiv.className = 'success-message';
        successDiv.style.color = '#38a169';
        successDiv.style.padding = '10px';
        successDiv.style.marginTop = '10px';
        successDiv.style.borderRadius = '4px';
        successDiv.style.backgroundColor = '#f0fff4';
        successDiv.textContent = message;

        // Insertar mensaje después del formulario
        this.form.parentNode.insertBefore(successDiv, this.form.nextSibling);

        // Remover después de 3 segundos
        setTimeout(() => successDiv.remove(), 3000);
    }

    resetForm() {
        this.form.reset();
        this.preview.style.display = 'none';
        this.previewPlaceholder.style.display = 'block';
        this.fileInputLabel.innerHTML = '<span class="upload-icon">📁</span>Seleccionar archivo';
    }
}

// Inicializar el manejador cuando el DOM esté listo
document.addEventListener('DOMContentLoaded', () => {
    const uploadHandler = new ImageUploadHandler();
});