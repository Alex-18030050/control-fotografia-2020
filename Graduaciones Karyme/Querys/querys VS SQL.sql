select * from AUDITORIAS

select * from INSTITUCIONES_ACADEMICAS 
select * from GRUPOS where GR_Id =1
select * from ALUMNOS 

SELECT IA.IA_Nombre, G.GR_Especialidad ,A.AL_Nombre FROM ALUMNOS A inner join GRUPOS G ON AL_Id = G.GR_Id inner join  NOTA_DETALLE ND ON AL_Id = ND.ND_IdAlumno inner join INSTITUCIONES_ACADEMICAS IA On G.GR_IdIA = IA.IA_Id where GR_Especialidad = '" + grupo + "' and G.GR_IdIA = " + valorid.ToString() + " order by G.GR_Especialidad
SELECT * FROM ALUMNOS  inner join GRUPOS  ON AL_Id = GR_Id  where GR_Especialidad = '03 QUIMICO-BIOLOGO' and GR_IdIA = 1 order by GR_Especialidad
SELECT * FROM PAQUETES where PAQ_Estatus=1
SELECT * FROM SERVICIOS where Estatus = 1
SELECT * FROM EXTRAS

SELECT * FROM PAQUETES where PAQ_Nombre = @paquete
SELECT * FROM SERVICIOS where Nombre = @servicio
SELECT * FROM EXTRAS where EX_Nombre = @extra

SELECT * FROM NOTA_DETALLE
UPDATE NOTA_DETALLE SET ND_Id=@ND_Id, ND_Fecha=@ND_Fecha, ND_Anticipo=@ND_Anticipo, ND_Descripcion=@ND_Descripcion, ND_IdAlumno=@ND_IdAlumno, ND_PrecioTotal=@ND_PrecioTotal,ND_Debe=@ND_Debe

SELECT * FROM ALUMNOS WHERE AL_Nombre = ' " " '

INSERT INTO NOTA_DETALLE VALUES(@ND_Id, @ND_Fecha, @ND_Anticipo, @ND_Descripcion,@ND_IdAlumno, @ND_PrecioTotal,@ND_Debe)
