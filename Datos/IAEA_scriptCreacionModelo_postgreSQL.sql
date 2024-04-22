-- Parcial #03 TABD
-- Curso de Tópicos Avanzados de base de datos
-- Miguel Quijano Jaramillo - miguel.quijano@upb.edu.co

-- Proyecto:  IAEA_Reactores_Nucleares_Investigacion
-- Motor de Base de datos: PostgreSQL 16.x

-- ***********************************
-- Abastecimiento de imagen en Docker
-- ***********************************
 
-- Descargar la imagen
docker pull postgres:latest

-- Crear el contenedor
docker run --name postgres-reactores -e POSTGRES_PASSWORD=unaClav3 -d -p 5432:5432 postgres:latest

-- ****************************************
-- Creación de base de datos y usuarios
-- ****************************************

-- Con usuario Root:

-- crear el esquema la base de datos
create database reactores_db;

-- Conectarse a la base de datos
\c reactores_db;

-- Creamos un esquema para almacenar todo el modelo de datos del dominio
create schema core;
create schema auth;
create schema localizacion;
create schema auditoria;

-- crear el usuario con el que se implementará la creación del modelo
create user reactores_app with encrypted password 'unaClav3';

-- asignación de privilegios para el usuario
grant connect on database reactores_db to reactores_app;
grant create on database reactores_db to reactores_app;
grant create, usage on schema core to reactores_app;
alter user reactores_app set search_path to core;


-- crear el usuario con el que se conectará la aplicación
create user reactores_usr with encrypted password 'unaClav3';

-- asignación de privilegios para el usuario
grant connect on database reactores_db to reactores_usr;
grant usage on schema core, localizacion to reactores_usr;
alter default privileges for user reactores_app in schema core grant insert, update, delete, select on tables to reactores_usr;
alter default privileges for user reactores_app in schema core grant execute on routines TO reactores_usr;
alter user reactores_usr set search_path to core;

-- ----------------------------------------
-- Script de creación de tablas
-- ----------------------------------------

-- Tabla Paises
create table core.Paises
(
    id      int not null primary key,
    nombre  varchar(50) not null
);

comment on table Paises is 'Tabla de países';
comment on column Paises.id is 'ID del país';
comment on column Paises.nombre is 'Nombre del país';

-- Tabla Ciudades
create table core.Ciudades
(
    id          int not null primary key,
    pais_id     int not null,
    nombre      varchar(50) not null,
    foreign key (pais_id) references Paises(id)
);

comment on table Ciudades is 'Tabla de ciudades';
comment on column Ciudades.id is 'ID de la ciudad';
comment on column Ciudades.pais_id is 'ID del país al que pertenece la ciudad';
comment on column Ciudades.nombre is 'Nombre de la ciudad';

-- Tabla Tipos
create table core.Tipos
(
    id      int not null primary key,
    tipo    varchar(50) not null
);

comment on table Tipos is 'Tabla de tipos';
comment on column Tipos.id is 'ID del tipo';
comment on column Tipos.tipo is 'Nombre del tipo';

-- Tabla Reactores
create table core.Reactores
(
    id          int not null primary key,
    cuidad_id   int not null,
    tipo_id     int not null,
    nombre      varchar(50) not null,
    potencia    float not null,
    estado      varchar(50) not null,
    fecha       timestamp not null,
    foreign key (cuidad_id) references Ciudades(id),
    foreign key (tipo_id) references Tipos(id)
);

comment on table Reactores is 'Tabla de reactores';
comment on column Reactores.id is 'ID del reactor';
comment on column Reactores.cuidad_id is 'ID de la ciudad donde se encuentra el reactor';
comment on column Reactores.tipo_id is 'ID del tipo de reactor';
comment on column Reactores.nombre is 'Nombre del reactor';
comment on column Reactores.potencia is 'Potencia del reactor';
comment on column Reactores.estado is 'Estado actual del reactor';
comment on column Reactores.fecha is 'Fecha de registro del reactor';


-- ----------------------------------------
-- Script para insertar datos
-- ----------------------------------------

-- Insertar datos en la tabla Paises
insert into core.Paises (id, nombre) values (1, 'Colombia');
insert into core.Paises (id, nombre) values (2, 'Estados Unidos');
insert into core.Paises (id, nombre) values (3, 'Francia');
insert into core.Paises (id, nombre) values (4, 'Alemania');
insert into core.Paises (id, nombre) values (5, 'Japón');

-- Insertar datos en la tabla Ciudades
insert into core.Ciudades (id, pais_id, nombre) values (1, 1, 'Medellín');
insert into core.Ciudades (id, pais_id, nombre) values (2, 1, 'Bogotá');
insert into core.Ciudades (id, pais_id, nombre) values (3, 2, 'New York');
insert into core.Ciudades (id, pais_id, nombre) values (4, 2, 'Los Angeles');
insert into core.Ciudades (id, pais_id, nombre) values (5, 3, 'Paris');
insert into core.Ciudades (id, pais_id, nombre) values (6, 3, 'Lyon');
insert into core.Ciudades (id, pais_id, nombre) values (7, 4, 'Berlín');
insert into core.Ciudades (id, pais_id, nombre) values (8, 4, 'Munich');
insert into core.Ciudades (id, pais_id, nombre) values (9, 5, 'Tokio');

-- Insertar datos en la tabla Tipos
insert into core.Tipos (id, tipo) values (1, 'Nuclear');
insert into core.Tipos (id, tipo) values (2, 'Fusión');
insert into core.Tipos (id, tipo) values (3, 'Fisión');

-- Insertar datos en la tabla Reactores
insert into core.Reactores (id, cuidad_id, tipo_id, nombre, potencia, estado, fecha) values (1, 1, 1, 'Medellín 1', 1000, 'Activo', '2021-01-01');
insert into core.Reactores (id, cuidad_id, tipo_id, nombre, potencia, estado, fecha) values (2, 2, 2, 'Bogotá 1', 2000, 'Activo', '2021-01-01');
insert into core.Reactores (id, cuidad_id, tipo_id, nombre, potencia, estado, fecha) values (3, 3, 3, 'New York 1', 3000, 'Activo', '2021-01-01');
insert into core.Reactores (id, cuidad_id, tipo_id, nombre, potencia, estado, fecha) values (4, 4, 1, 'Los Angeles 1', 4000, 'Activo', '2021-01-01');
insert into core.Reactores (id, cuidad_id, tipo_id, nombre, potencia, estado, fecha) values (5, 5, 2, 'Paris 1', 5000, 'Activo', '2021-01-01');
insert into core.Reactores (id, cuidad_id, tipo_id, nombre, potencia, estado, fecha) values (6, 6, 3, 'Lyon 1', 6000, 'Activo', '2021-01-01');
insert into core.Reactores (id, cuidad_id, tipo_id, nombre, potencia, estado, fecha) values (7, 7, 1, 'Berlín 1', 7000, 'Activo', '2021-01-01');
insert into core.Reactores (id, cuidad_id, tipo_id, nombre, potencia, estado, fecha) values (8, 8, 2, 'Munich 1', 8000, 'Activo', '2021-01-01');
insert into core.Reactores (id, cuidad_id, tipo_id, nombre, potencia, estado, fecha) values (9, 9, 3, 'Tokio 1', 9000, 'Activo', '2021-01-01');
