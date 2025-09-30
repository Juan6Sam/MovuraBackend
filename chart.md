erDiagram
      
"dbo.Roles" {
    int id_rol "PK"
          nvarchar(100) nombre ""
          nvarchar(250) descripcion ""
          
}
"dbo.Estatus" {
    int id_estatus "PK"
          nvarchar(50) nombre ""
          nvarchar(50) modulo ""
          nvarchar(200) descripcion ""
          
}
"dbo.Marcas" {
    int id_marca "PK"
          nvarchar(50) nombre ""
          nvarchar(200) descripcion ""
          
}
"dbo.Usuarios" {
    int id_usuario "PK"
          nvarchar(100) nombre ""
          nvarchar(100) apellido ""
          nvarchar(250) correo ""
          nvarchar(50) telefono ""
          int id_rol "FK"
          int id_estatus "FK"
          bit first_login ""
          datetime2 fecha_registro ""
          
}
"dbo.Contrasenas" {
    int id_contrasena "PK"
          int id_usuario "FK"
          nvarchar(512) hash ""
          nvarchar(128) salt ""
          datetime2 fecha_creacion ""
          bit activa ""
          
}
"dbo.Comercios" {
    int id_comercio "PK"
          nvarchar(200) nombre ""
          nvarchar(300) direccion ""
          nvarchar(20) tipo_cortesia ""
          decimal valor ""
          int id_estatus "FK"
          datetime2 fecha_alta ""
          
}
"dbo.ComercioEmails" {
    int id_rel "PK"
          int id_comercio "FK"
          nvarchar(250) correo ""
          
}
"dbo.Accesos" {
    int id_acceso "PK"
          int id_usuario "FK"
          datetime2 hora_entrada ""
          datetime2 hora_salida ""
          int id_estatus "FK"
          
}
"dbo.Tickets" {
    int id_ticket "PK"
          int id_usuario "FK"
          int id_acceso "FK"
          datetime2 fecha_emision ""
          int id_estatus "FK"
          
}
"dbo.Excesos" {
    int id_exceso "PK"
          int id_acceso "FK"
          datetime2 hora_inicio ""
          datetime2 hora_fin ""
          decimal monto ""
          
}
"dbo.CodigosQR" {
    int id_qr "PK"
          uniqueidentifier codigo ""
          int id_usuario "FK"
          int id_comercio "FK"
          int id_acceso "FK"
          datetime2 fecha_creacion ""
          datetime2 fecha_expiracion ""
          int id_estatus "FK"
          
}
"dbo.Pagos" {
    int id_pago "PK"
          int id_ticket "FK"
          int id_usuario "FK"
          int id_acceso "FK"
          decimal monto ""
          nvarchar(50) metodo_pago ""
          datetime2 fecha_pago ""
          int id_estatus "FK"
          
}
"dbo.Transacciones" {
    int id_transaccion "PK"
          int id_pago "FK"
          int id_acceso "FK"
          int id_qr "FK"
          nvarchar(50) tipo ""
          datetime2 fecha_transaccion ""
          decimal monto ""
          int id_estatus "FK"
          int id_usuario "FK"
          
}
"dbo.Tarjetas" {
    int id_tarjeta "PK"
          int id_usuario "FK"
          nvarchar(20) numero_enmascarado ""
          nvarchar(7) expiracion ""
          nvarchar(200) token_seguro ""
          datetime2 fecha_registro ""
          nvarchar(50) marca ""
          
}
"dbo.Logs" {
    int id_log "PK"
          int id_usuario "FK"
          nvarchar(200) accion ""
          nvarchar(1000) descripcion ""
          datetime2 fecha ""
          nvarchar(50) ip_address ""
          
}
"dbo.Notificaciones" {
    int id_notificacion "PK"
          int id_usuario "FK"
          nvarchar(50) canal ""
          nvarchar(1000) mensaje ""
          datetime2 fecha_envio ""
          nvarchar(50) estatus_envio ""
          
}
      "dbo.Usuarios" ||--|{ "dbo.Roles": "id_rol"
"dbo.Usuarios" ||--|{ "dbo.Estatus": "id_estatus"
"dbo.Contrasenas" ||--|{ "dbo.Usuarios": "id_usuario"
"dbo.Comercios" ||--|{ "dbo.Estatus": "id_estatus"
"dbo.ComercioEmails" ||--|{ "dbo.Comercios": "id_comercio"
"dbo.Accesos" |o--|{ "dbo.Usuarios": "id_usuario"
"dbo.Accesos" ||--|{ "dbo.Estatus": "id_estatus"
"dbo.Tickets" |o--|{ "dbo.Usuarios": "id_usuario"
"dbo.Tickets" |o--|{ "dbo.Accesos": "id_acceso"
"dbo.Tickets" ||--|{ "dbo.Estatus": "id_estatus"
"dbo.Excesos" ||--|{ "dbo.Accesos": "id_acceso"
"dbo.CodigosQR" |o--|{ "dbo.Usuarios": "id_usuario"
"dbo.CodigosQR" |o--|{ "dbo.Comercios": "id_comercio"
"dbo.CodigosQR" |o--|{ "dbo.Accesos": "id_acceso"
"dbo.CodigosQR" ||--|{ "dbo.Estatus": "id_estatus"
"dbo.Pagos" ||--|{ "dbo.Tickets": "id_ticket"
"dbo.Pagos" |o--|{ "dbo.Usuarios": "id_usuario"
"dbo.Pagos" |o--|{ "dbo.Accesos": "id_acceso"
"dbo.Pagos" ||--|{ "dbo.Estatus": "id_estatus"
"dbo.Transacciones" |o--|{ "dbo.Pagos": "id_pago"
"dbo.Transacciones" |o--|{ "dbo.Accesos": "id_acceso"
"dbo.Transacciones" |o--|{ "dbo.CodigosQR": "id_qr"
"dbo.Transacciones" ||--|{ "dbo.Estatus": "id_estatus"
"dbo.Transacciones" ||--|{ "dbo.Usuarios": "id_usuario"
"dbo.Tarjetas" ||--|{ "dbo.Usuarios": "id_usuario"
"dbo.Logs" |o--|{ "dbo.Usuarios": "id_usuario"
"dbo.Notificaciones" |o--|{ "dbo.Usuarios": "id_usuario"
