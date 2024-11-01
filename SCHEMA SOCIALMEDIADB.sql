-- create database SocialMediaDB;

use SocialMediaDB;

-- # T A B L A S #

-- ## USUARIOS ##
CREATE TABLE Usuarios (
    UsuarioID INT PRIMARY KEY IDENTITY,
    NombreUsuario NVARCHAR(50) UNIQUE NOT NULL,
    Email NVARCHAR(100) UNIQUE NOT NULL,
    Contraseña NVARCHAR(255) NOT NULL,
    FotoPerfil NVARCHAR(255),
    Biografia NVARCHAR(MAX),
    Ubicacion NVARCHAR(100),
    Intereses NVARCHAR(255),
    EsPremium BIT DEFAULT 0,
    FechaRegistro DATETIME DEFAULT GETDATE()
);

-- ## ProveedorAutenticacion ##
CREATE TABLE ProveedorAutenticacion (
    ProveedorID INT PRIMARY KEY IDENTITY,
    NombreProveedor NVARCHAR(50) UNIQUE NOT NULL
);

-- ## AutenticacionSocial ##
CREATE TABLE AutenticacionSocial (
    AutenticacionID INT PRIMARY KEY IDENTITY,
    UsuarioID INT FOREIGN KEY REFERENCES Usuarios(UsuarioID),
    ProveedorID INT FOREIGN KEY REFERENCES ProveedorAutenticacion(ProveedorID),
    Token NVARCHAR(255) NOT NULL,
    FechaAutenticacion DATETIME DEFAULT GETDATE()
);

-- ## RecuperacionContrasena ##
CREATE TABLE RecuperacionContrasena (
    RecuperacionID INT PRIMARY KEY IDENTITY,
    UsuarioID INT FOREIGN KEY REFERENCES Usuarios(UsuarioID),
    Token NVARCHAR(255) NOT NULL,
    FechaSolicitud DATETIME DEFAULT GETDATE(),
    FechaExpiracion DATETIME
);

-- ## Amistades ##
CREATE TABLE Amistades (
    AmistadID INT PRIMARY KEY IDENTITY,
    UsuarioID INT FOREIGN KEY REFERENCES Usuarios(UsuarioID),
    AmigoID INT FOREIGN KEY REFERENCES Usuarios(UsuarioID),
    Estado NVARCHAR(50) CHECK (Estado IN ('Pendiente', 'Aceptado', 'Bloqueado')),
    FechaSolicitud DATETIME DEFAULT GETDATE(),
    FechaAceptacion DATETIME
);

-- ## Publicaciones ##
CREATE TABLE Publicaciones (
    PublicacionID INT PRIMARY KEY IDENTITY,
    UsuarioID INT FOREIGN KEY REFERENCES Usuarios(UsuarioID),
    Texto NVARCHAR(MAX),
    Imagen NVARCHAR(255),
    Enlace NVARCHAR(255),
    FechaPublicacion DATETIME DEFAULT GETDATE()
);

-- ## Comentarios ##
CREATE TABLE Comentarios (
    ComentarioID INT PRIMARY KEY IDENTITY,
    PublicacionID INT FOREIGN KEY REFERENCES Publicaciones(PublicacionID),
    UsuarioID INT FOREIGN KEY REFERENCES Usuarios(UsuarioID),
    Texto NVARCHAR(MAX),
    FechaComentario DATETIME DEFAULT GETDATE()
);

-- ## Reacciones ##
CREATE TABLE Reacciones (
    ReaccionID INT PRIMARY KEY IDENTITY,
    PublicacionID INT FOREIGN KEY REFERENCES Publicaciones(PublicacionID),
    UsuarioID INT FOREIGN KEY REFERENCES Usuarios(UsuarioID),
    TipoReaccion NVARCHAR(50) 
);

-- ## Mensajes ##
CREATE TABLE Mensajes (
    MensajeID INT PRIMARY KEY IDENTITY,
    EmisorID INT FOREIGN KEY REFERENCES Usuarios(UsuarioID),
    ReceptorID INT FOREIGN KEY REFERENCES Usuarios(UsuarioID),
    MensajeTexto NVARCHAR(MAX),
    FechaEnvio DATETIME DEFAULT GETDATE(),
    EsLeido BIT DEFAULT 0
);

-- ## Notificaciones ##
CREATE TABLE Notificaciones (
    NotificacionID INT PRIMARY KEY IDENTITY,
    UsuarioID INT FOREIGN KEY REFERENCES Usuarios(UsuarioID),
    Tipo NVARCHAR(50), -- Ej: 'Solicitud de Amistad', 'Comentario', 'Reaccion'
    Descripcion NVARCHAR(MAX),
    Fecha DATETIME DEFAULT GETDATE(),
    EsLeida BIT DEFAULT 0
);

-- ## Eventos ##
CREATE TABLE Eventos (
    EventoID INT PRIMARY KEY IDENTITY,
    UsuarioID INT FOREIGN KEY REFERENCES Usuarios(UsuarioID),
    Titulo NVARCHAR(255) NOT NULL,
    Descripcion NVARCHAR(MAX),
    FechaEvento DATETIME,
    Ubicacion NVARCHAR(255),
    FechaCreacion DATETIME DEFAULT GETDATE()
);

-- ## InvitadosEvento ##
CREATE TABLE InvitadosEvento (
    InvitadoID INT PRIMARY KEY IDENTITY,
    EventoID INT FOREIGN KEY REFERENCES Eventos(EventoID),
    UsuarioID INT FOREIGN KEY REFERENCES Usuarios(UsuarioID),
    Confirmacion NVARCHAR(50) CHECK (Confirmacion IN ('Asistiré', 'No asistiré', 'Pendiente'))
);

-- ## Pagos ##
CREATE TABLE Pagos (
    PagoID INT PRIMARY KEY IDENTITY,
    UsuarioID INT FOREIGN KEY REFERENCES Usuarios(UsuarioID),
    Monto DECIMAL(10, 2),
    FechaPago DATETIME DEFAULT GETDATE(),
    Estado NVARCHAR(50) CHECK (Estado IN ('Pendiente', 'Exitoso', 'Fallido'))
);

