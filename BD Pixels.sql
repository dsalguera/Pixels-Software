create database pixels

create table registro(
    id_registro int primary key not null auto_increment,
    nombre varchar(100),
    correo varchar(100),
    telefono varchar(20),
    cedula varchar(50),
    direccion varchar(150),
    descripcion varchar(200),
    total varchar(50),
    archivo varchar(300)
);

create table compra(
    id_compra int primary key not null auto_increment,
    id_registro int, foreign key (id_registro) references registro(id_registro),
    monto double,
    efectivo double,
    cambio double
);

create table detalle_compra(
    id_detalle_compra int primary key not null auto_increment,
    id_compra int, foreign key (id_compra) references compra(id_compra),
    montos varchar(100),
    descripcion varchar(100)
);

/*Aqui comienza todo lo importante*/

create table material(
    id_material int primary key not null auto_increment,
    nombre varchar(100),
    precio double,
    clicks int
);

insert into material(nombre,precio)values('Lona',18);
insert into material(nombre,precio)values('Adhesivo-Laminado',20);
insert into material(nombre,precio)values('Adhesivo',20);

create table lata(
    id_lata int primary key not null auto_increment,
    precio int,
    comision double,
    flete int,
    tamano int,
    metros int,
    multiplicador double
);

insert into lata(precio, comision, flete, tamano, metros, multiplicador)values(93229,0,5000,122,20,2.5);

create table estructura(
    id_estructura int primary key not null auto_increment,
    cantidad int,
    descripcion varchar(100),
    precio int,
    imagen varchar(300)
);

insert into estructura(cantidad,descripcion,precio,imagen)values(1,'Sierra',2000,'PNG');

create table detalle_estructura(
    id_detalle_estructura int primary key not null auto_increment,
    costo_compra int,
    flete int,
    mano_obra int,
    costo_total double
);

insert into detalle_estructura(costo_compra,flete,mano_obra,costo_total)values(0,0,0,0);

/*Edicion*/

create table parametros_laminantes(
    id_param_lam int primary key not null auto_increment,
    id_material int, foreign key (id_material) references material(id_material),
    precio int,
    flete int,
    tamano int,
    altura int,
    porc_venta double,
    metros int, 
    mult double
);

/*Fin edicion*/

/*insert into magnetico(precio, flete, tamano, porc_venta, metros)values(115029,5000,62,368,30);*/

create table variable(
    id_variable int primary key not null auto_increment,
    instalacion int,
    diseno int,
    corte int,
    dolar_venta int,
    dolar_compra int,
    decimales int,
    sumaExtra int,
    impuesto double,
    ultG varchar(20),
    ultGPVC varchar(20),
    dAuto varchar(10)
);

insert into variable(instalacion,diseno,corte,dolar_venta,dolar_compra,decimales,sumaExtra,impuesto,ultG,ultGPVC,dAuto)
values(1000,1000,1000,619,606,2,1000,13,'','','');

create table imagen(
    id_imagen int primary key not null auto_increment,
    img_materiales varchar(300),
    img_pvc varchar(300)
);

insert into imagen(img_materiales, img_pvc)values('1','2');

create table papeleria(
    id_papeleria int primary key not null auto_increment,
    nombre varchar(50),
    base double,
    altura double,
    precio double,
    medida varchar(5)
);

insert into papeleria(nombre,base,altura,precio,medida)values('Carta - Grueso (1)',8.5,11,200,'pulg');
insert into papeleria(nombre,base,altura,precio,medida)values('Carta - Grueso (2)',8.5,11,200,'pulg');
insert into papeleria(nombre,base,altura,precio,medida)values('Carta - Grueso (3)',8.5,11,300,'pulg');
insert into papeleria(nombre,base,altura,precio,medida)values('Carta - Grueso (4)',8.5,11,350,'pulg');


insert into papeleria(nombre,base,altura,precio,medida)values('A4 - Grueso (1)',8.26,11.69,100,'cm');
insert into papeleria(nombre,base,altura,precio,medida)values('A4 - Grueso (2)',8.26,11.69,200,'cm');
insert into papeleria(nombre,base,altura,precio,medida)values('A4 - Grueso (3)',8.26,11.69,300,'cm');
insert into papeleria(nombre,base,altura,precio,medida)values('A4 - Grueso (4)',8.26,11.69,350,'cm');


insert into papeleria(nombre,base,altura,precio,medida)values('Tabloide - Grueso (1)',11,17,350,'pulg');
insert into papeleria(nombre,base,altura,precio,medida)values('Tabloide - Grueso (2)',11,17,350,'pulg');
insert into papeleria(nombre,base,altura,precio,medida)values('Tabloide - Grueso (3)',11,17,350,'pulg');
insert into papeleria(nombre,base,altura,precio,medida)values('Tabloide - Grueso (4)',11,17,500,'pulg');


insert into papeleria(nombre,base,altura,precio,medida)values('Arch B - Grueso (1)',12,18,200,'pulg');
insert into papeleria(nombre,base,altura,precio,medida)values('Arch B - Grueso (2)',12,18,350,'pulg');
insert into papeleria(nombre,base,altura,precio,medida)values('Arch B - Grueso (3)',12,18,350,'pulg');
insert into papeleria(nombre,base,altura,precio,medida)values('Arch B - Grueso (4)',12,18,500,'pulg');

insert into papeleria(nombre,base,altura,precio,medida)values('Mini-Banner - Grueso (4)',12,47,1000,'pulg');

create table caja(
    id_caja int primary key not null auto_increment,
    monto_arranque double,
    hora int,
    minutos int
);

insert into caja(monto_arranque, hora, minutos)values(13000, 18, 30);

create table ingresos(
    id_ingreso int primary key not null auto_increment,
    fecha date,
    ingreso int,
    descripcion varchar(100)
);

create table egresos(
    id_egreso int primary key not null auto_increment,
    fecha date,
    egreso int,
    descripcion varchar(100)
);

create table bitacora(
    id_bitacora int primary key not null auto_increment,
    fecha date,
    descripcion varchar(300)
);

/*

Codigo por si se resetean los privilegios

create user 'root'@'%' identified by '';
grant all privileges on *.* to 'root'@'%' with grant option;
flush privileges;

*/















