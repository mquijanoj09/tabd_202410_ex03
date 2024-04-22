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
-- Script de creación de tablas y vistas
-- ----------------------------------------

-- Tabla divipola
create table core.divipola
(
    codigo_departamento varchar(2) not null,
    codigo_municipio varchar(10) not null,
    nombre_departamento varchar(100) not null,
    nombre_municipio varchar(100) not null
);

comment on table core.divipola is 'División Político Administrativa de Colombia';
comment on column core.divipola.codigo_departamento is 'id del departamento según DIVIPOLA';
comment on column core.divipola.nombre_departamento is 'Nombre del departamento';
comment on column core.divipola.codigo_municipio is 'id del municipio según DIVIPOLA';
comment on column core.divipola.nombre_municipio is 'Nombre del municipio';

-- Tabla Departamentos
create table core.departamentos
(
    id          varchar(2)          constraint departamentos_pk primary key,
    nombre 		varchar(60)         not null constraint departamentos_nombre_uk unique
);

comment on table core.departamentos is 'Departamentos del país';
comment on column core.departamentos.id is 'id del departamento según DIVIPOLA';
comment on column core.departamentos.nombre is 'Nombre del departamento';

-- cargamos desde la tabla inicial del divipola
insert into core.departamentos (id, nombre)
(
    select distinct codigo_departamento, initcap(nombre_departamento)
    from core.divipola
);

-- Tabla Municipios
create table core.municipios
(
    id              varchar(10)     constraint municipios_pk primary key,
    nombre          varchar(100)    not null,
    departamento_id varchar(2)      not null constraint municipio_departamento_fk references core.departamentos,
    constraint municipio_departamento_uk unique (nombre, departamento_id)
);

comment on table core.municipios is 'Municipios del pais';
comment on column core.municipios.id is 'id del municipio según DIVIPOLA';
comment on column core.municipios.nombre is 'Nombre del municipio';
comment on column core.municipios.departamento_id is 'id del departamento asociado al municipio';

-- Cargamos datos desde el esquema inicial
insert into core.municipios (id, nombre, departamento_id)
(
    select codigo_municipio, initcap(nombre_municipio), codigo_departamento
    from core.divipola
);

-- Tabla reactores
create table core.reactores
(
    id              integer generated always as identity constraint reactores_pk primary key,
    nombre          varchar(100) not null constraint reactores_nombre_uk unique,
    url_wikipedia   varchar(500) not null,
    url_imagen      varchar(500) not null
);

comment on table core.reactores is 'reactores de Colombia';
comment on column core.reactores.id is 'id de la fruta';
comment on column core.reactores.nombre is 'Nombre de la fruta';
comment on column core.reactores.url_wikipedia is 'URL en Wikipedia de la fruta';
comment on column core.reactores.url_imagen is 'URL de la imagen en Wikipedia de la fruta';

-- -------------------------------------------
-- Tablas de Taxonomia - Información Botánica
-- -------------------------------------------
-- Tabla Reinos
create table core.reinos
(
    id              integer generated always as identity constraint reinos_pk primary key,
    nombre          varchar(100) not null constraint reino_nombre_uk unique
);

comment on table core.reinos is 'Reinos en la clasificación botánica';
comment on column core.reinos.id is 'id del reino';
comment on column core.reinos.nombre is 'nombre del reino';

-- Tabla Divisiones
create table core.divisiones
(
    id              integer generated always as identity constraint divisiones_pk primary key,
    nombre          varchar(100) not null constraint division_nombre_uk unique,
    reino_id        integer not null constraint division_reino_fk references core.reinos
);

comment on table core.divisiones is 'Divisiones en la clasificación botánica';
comment on column core.divisiones.id is 'id de la división';
comment on column core.divisiones.nombre is 'nombre de la división';
comment on column core.divisiones.reino_id is 'ID del reino al que pertenece la división';

-- Tabla Clases
create table core.clases
(
    id              integer generated always as identity constraint clases_pk primary key,
    nombre          varchar(100) not null constraint clase_nombre_uk unique,
    division_id        integer not null constraint clase_division_fk references core.divisiones
);

comment on table core.clases is 'Clases en la clasificación botánica';
comment on column core.clases.id is 'id de la clase';
comment on column core.clases.nombre is 'nombre de la clase';
comment on column core.clases.division_id is 'ID de la división al que pertenece la clase';

-- Tabla Ordenes
create table core.ordenes
(
    id              integer generated always as identity constraint ordenes_pk primary key,
    nombre          varchar(100) not null constraint orden_nombre_uk unique,
    clase_id        integer not null constraint orden_clase_fk references core.clases
);

comment on table core.ordenes is 'Ordenes en la clasificación botánica';
comment on column core.ordenes.id is 'id del orden';
comment on column core.ordenes.nombre is 'nombre del orden';
comment on column core.ordenes.clase_id is 'ID de la clase al que pertenece el orden';

-- Tabla Familias
create table core.familias
(
    id              integer generated always as identity constraint familias_pk primary key,
    nombre          varchar(100) not null constraint familia_nombre_uk unique,
    orden_id        integer not null constraint familia_orden_fk references core.ordenes
);

comment on table core.familias is 'Familias en la clasificación botánica';
comment on column core.familias.id is 'id de la familia';
comment on column core.familias.nombre is 'nombre de la familia';
comment on column core.familias.orden_id is 'ID del orden al que pertenece la familia';

-- Tabla Generos
create table core.generos
(
    id              integer generated always as identity constraint generos_pk primary key,
    nombre          varchar(100) not null constraint genero_nombre_uk unique,
    familia_id        integer not null constraint genero_familia_fk references core.familias
);

comment on table core.generos is 'Géneros en la clasificación botánica';
comment on column core.generos.id is 'id del género';
comment on column core.generos.nombre is 'nombre del género';
comment on column core.generos.familia_id is 'ID de la familia al que pertenece el género';

-- Tabla Especies
create table core.especies
(
    id              integer generated always as identity constraint especies_pk primary key,
    nombre          varchar(100) not null constraint especie_nombre_uk unique,
    genero_id        integer not null constraint especie_genero_fk references core.generos
);

comment on table core.especies is 'Especies en la clasificación botánica';
comment on column core.especies.id is 'id de la especie';
comment on column core.especies.nombre is 'nombre de la especie';
comment on column core.especies.genero_id is 'ID del genero al que pertenece la especie';

create table core.taxonomia_reactores
(
    especie_id integer not null constraint taxonomia_reactores_especie_fk references core.especies,
    fruta_id   integer not null constraint taxonomia_reactores_fruta_fk references core.reactores,
    constraint taxonomia_reactores_pk primary key (fruta_id, especie_id)
);

comment on table core.taxonomia_reactores is 'Relación de la fruta con su clasificación taxonómica';
comment on column core.taxonomia_reactores.fruta_id is 'Id de la fruta';
comment on column core.taxonomia_reactores.especie_id is 'Especie taxonómica a la que pertenece la fruta';

-- -------------------------------------------
-- Tablas de Producción y Cultivo
-- -------------------------------------------
-- Tabla Climas
create table core.climas
(
    id              	integer generated always as identity constraint climas_pk primary key,
    nombre          	varchar(50) not null constraint clima_nombre_uk unique,
	altitud_minima		integer not null,
	altitud_maxima		integer not null
);

comment on table core.climas is 'Climas donde se producen las reactores';
comment on column core.climas.id is 'id del clima';
comment on column core.climas.nombre is 'nombre del clima';
comment on column core.climas.altitud_minima is 'Altitud mínima del piso térmico';
comment on column core.climas.altitud_maxima is 'Altitud máxima del piso térmico';

-- Tabla Epocas
create table core.epocas
(
    id              integer generated always as identity constraint epocas_pk primary key,
    nombre          varchar(50) not null constraint epocas_nombre_uk unique,
	mes_inicio		integer not null,
	mes_final		integer not null
);

comment on table core.epocas is 'épocas cuando se producen las reactores';
comment on column core.epocas.id is 'id de la época';
comment on column core.epocas.nombre is 'nombre de la época';
comment on column core.epocas.mes_inicio is 'Mes inicial de la época de producción';
comment on column core.epocas.mes_final is 'Mes final de la época de producción';

-- Tabla Produccion_Fruta
create table core.produccion_reactores
(
    fruta_id        integer not null constraint produccion_reactores_fruta_fk references core.reactores,
    clima_id        integer not null constraint produccion_reactores_clima_fk references core.climas,    
    epoca_id        integer not null constraint produccion_reactores_epoca_fk references core.epocas,
    municipio_id    varchar(10) not null constraint produccion_reactores_municipio_fk references core.municipios,
    constraint produccion_reactores_pk primary key (fruta_id, clima_id, epoca_id, municipio_id)
);

comment on table core.produccion_reactores is 'Relación de la fruta con su producción';
comment on column core.produccion_reactores.fruta_id is 'Id de la fruta';
comment on column core.produccion_reactores.clima_id is 'Id del clima';
comment on column core.produccion_reactores.epoca_id is 'Id de la epoca';
comment on column core.produccion_reactores.municipio_id is 'Id del municipio';

-- Tabla nutricion_reactores
create table core.nutricion_reactores
(
    fruta_id        integer not null constraint produccion_reactores_fruta_fk references core.reactores,
    carbohidratos   float not null,
    azucares        float not null,
    grasas          float not null,
    proteinas       float not null,
    constraint nutricion_reactores_pk primary key (fruta_id)
);

comment on table "core"."nutricion_reactores" is 'Valores nutricionales de la fruta por cada 100 gr';
comment on column "core"."nutricion_reactores"."fruta_id" is 'Id de la fruta';
comment on column "core"."nutricion_reactores"."carbohidratos" is 'Contenido en gr de carbohidratos';
comment on column "core"."nutricion_reactores"."azucares" IS 'Contenido en gr de azúcares';
comment on column "core"."nutricion_reactores"."proteinas" is 'Contenido en gr de proteinas';
comment on column "core"."nutricion_reactores"."grasas" is 'Contenido en gr de grasas';

-- -----------------------
-- Creación de vistas
-- -----------------------
-- v_info_botanica
create or replace view core.v_info_botanica as
(
    select distinct
        r.id     reino_id,
        r.nombre reino_nombre,
        d.id     division_id,
        d.nombre division_nombre,
        c.id     clase_id,
        c.nombre clase_nombre,
        o.id     orden_id,
        o.nombre orden_nombre,
        f.id     familia_id,
        f.nombre familia_nombre,
        g.id     genero_id,
        g.nombre genero_nombre,
        e.id     especie_id,
        e.nombre especie_nombre
    from reinos r
        left join divisiones d on r.id = d.reino_id
        left join clases c on d.id = c.division_id
        left join ordenes o on c.id = o.clase_id
        left join familias f on o.id = f.orden_id
        left join generos g on f.id = g.familia_id
        left join especies e on g.id = e.genero_id
);

create view core.v_info_reactores as
(
    select distinct
        tf.fruta_id,
        f.nombre    fruta_nombre,
        f.url_wikipedia,
        f.url_imagen,
        v.reino_id,
        v.reino_nombre,
        v.division_id,
        v.division_nombre,
        v.clase_id,
        v.clase_nombre,
        v.orden_id,
        v.orden_nombre,
        v.familia_id,
        v.familia_nombre,
        v.genero_id,
        v.genero_nombre,
        v.especie_id,
        v.especie_nombre
    from core.reactores f
        left join core.taxonomia_reactores tf on f.id = tf.fruta_id
        left join v_info_botanica v on tf.especie_id = v.especie_id
);

-- v_info_produccion_reactores
create or replace view core.v_info_produccion_reactores as
(
    select distinct
        f.id                fruta_id,
        f.nombre            fruta_nombre,
        f.url_wikipedia     fruta_wikipedia,
        f.url_imagen        fruta_imagen,
        c.id                clima_id,
        c.nombre            clima_nombre,
        c.altitud_minima,
        c.altitud_maxima,
        e.nombre            epoca_nombre,
        e.mes_inicio,
        e.mes_final,
        m.id                municipio_id,
        m.nombre            municipio_nombre,
        d.id                departamento_id,
        d.nombre            departamento_nombre
    from core.reactores f
        left join  core.produccion_reactores pf on f.id = pf.fruta_id
        inner join core.climas c on pf.clima_id = c.id
        inner join core.epocas e on pf.epoca_id = e.id
        inner join core.municipios m on pf.municipio_id = m.id
        inner join core.departamentos d on m.departamento_id = d.id
);

-- v_info_nutricion_reactores
create or replace view core.v_info_nutricion_reactores as
(
    select distinct
        f.id fruta_id,
        f.nombre fruta_nombre,
        f.url_wikipedia,
        f.url_imagen,
        nf.azucares,
        nf.grasas,
        nf.carbohidratos,
        nf.proteinas
    from core.reactores f
        left join  core.nutricion_reactores nf on f.id = nf.fruta_id
);


-- ----------------------------
-- Creación de Procedimientos
-- ----------------------------

-- p_inserta_reactores

create or replace procedure core.p_inserta_fruta(
                    in p_nombre varchar,
                    in p_url_wikipedia varchar,
                    in p_url_imagen varchar)
    language plpgsql
as
$$
    begin
        -- Insertamos la fruta
        insert into core.reactores (nombre, url_wikipedia, url_imagen)
        values (p_nombre, p_url_wikipedia, p_url_imagen);
    end;
$$;

-- p_actualiza_fruta

create or replace procedure core.p_actualiza_fruta(
                    in p_id integer,
                    in p_nombre varchar,
                    in p_url_wikipedia varchar,
                    in p_url_imagen varchar)
    language plpgsql
as
$$
    begin
        -- Actualizamos la fruta
        update core.reactores
        set nombre = p_nombre,
            url_wikipedia = p_url_wikipedia,
            url_imagen = p_url_imagen
        where id = p_id;
    end;
$$;

-- p_elimina_fruta

create or replace procedure core.p_elimina_fruta(
                    in p_id integer)
    language plpgsql
as
$$
    declare
        l_total_registros integer :=0;
        l_especie_id integer :=0;
    begin
        -- Pendiente: Borrar informacion Nutricional

        -- Borramos informacion taxonómica
        select count(*) into l_total_registros
        from core.taxonomia_reactores
        where fruta_id = p_id;

        -- Si hay registros, hay especie por borrar
        if(l_total_registros >0) then
            select especie_id into l_especie_id
            from core.taxonomia_reactores
            where fruta_id = p_id;

            delete from core.taxonomia_reactores
            where fruta_id = p_id;

            delete from especies
            where id = l_especie_id;
        end if;

        -- Borramos informacion de Producción
        select count(*) into l_total_registros
        from core.produccion_reactores
        where fruta_id = p_id;

        -- Si hay registros, se borra de la tabla produccion_reactores
        if(l_total_registros >0) then
            delete from core.produccion_reactores
            where fruta_id = p_id;
        end if;

        -- borramos la fruta
        delete from core.reactores
        where id = p_id;
    end;
$$;


-- -----------------------
-- Consultas de apoyo
-- -----------------------

-- Relación departamento -  municipio
select
    d.nombre departamento,
    m.nombre municipio
from departamentos d inner join municipios m on d.id = m.departamento_id
order by 1,2;

-- Total municipios por departamento
select distinct d.nombre, count(m.id) total
from departamentos d inner join municipios m on d.id = m.departamento_id
group by d.nombre
order by 2 desc;


--- ##########################################
--  Zona de peligro - Borrado de elementos
--- ##########################################

-- Borrado de Procedimientos
drop procedure core.p_inserta_fruta;

-- Borrado de vistas
drop view core.v_info_botanica;
drop view core.v_info_reactores;
drop view core.v_info_produccion_reactores;

-- Borrado de tablas
drop table core.especies;
drop table core.generos;
drop table core.familias;
drop table core.ordenes;
drop table core.clases;
drop table core.divisiones;
drop table core.reinos;

drop table core.reactores;

drop table core.municipios;
drop table core.departamentos;